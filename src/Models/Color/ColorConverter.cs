using System.Drawing;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Newtonsoft.Json.Serialization;

namespace Hue; 

public class ColorConverter
{
    private static double CieCrossProduct(CieColor p1, CieColor p2)
    {
        return p1.X * p2.Y - p1.Y * p2.X;
    } 

    private static double CieDistance(CieColor p1, CieColor p2)
    {
        var dx = p1.X - p2.X;
        var dy = p1.Y - p2.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private static bool CieInGamut(CieColor cie, CieColorGamut gamut)
    {
        var v1 = new CieColor(gamut.Green.X - gamut.Red.X, gamut.Green.Y - gamut.Red.Y);
        var v2 = new CieColor(gamut.Blue.X - gamut.Red.X, gamut.Blue.Y - gamut.Red.Y);

        var q = new CieColor(cie.X - gamut.Red.X, cie.Y - gamut.Red.Y);
        var s = CieCrossProduct(q, v2) / CieCrossProduct(v1, v2);
        var t = CieCrossProduct(v1, q) / CieCrossProduct(v2, v2);

        return (s >= 0.0) && (t >= 0.0) && (s + t <= 1.0);
    }

    private static CieColor ClosestCieToCieLine(CieColor cie, CieColor p1, CieColor p2)
    {
        var ap = new CieColor(cie.X - p1.X, cie.Y - p1.Y);
        var ab = new CieColor(p2.X - p1.X, p2.Y - cie.Y);
        var ab2 = ab.X * ab.X + ab.Y * ab.Y;
        var abAb = ap.X * ab.X + ap.Y * ab.Y;
        var t = abAb / ab2; 
        
        if (t < 0.0)
        {
            t = 0.0;
        }
        else if (t > 1.0)
        {
            t = 1.0;
        }

        return new CieColor(p1.X + ab.X * t, p1.Y + ab.Y * t);
    }

    private static CieColor ClosestCieInGamut(CieColor cie, CieColorGamut gamut)
    {
        // Color is unreproducible, find the closest cie on each line in the CIE 1931 'triangle'.
        var pAb = ClosestCieToCieLine(cie, gamut.Red, gamut.Green);
        var pAc = ClosestCieToCieLine(cie, gamut.Blue, gamut.Red);
        var pBc = ClosestCieToCieLine(cie, gamut.Green, gamut.Blue);

        // Get the distances per cie and see which cie is closer to our Point.
        var dAb = CieDistance(cie, pAb);
        var dAc = CieDistance(cie, pAc);
        var dBc = CieDistance(cie, pBc);

        var lowest = dAb;
        var closestPoint = pAb;

        if (dAc < lowest)
        {
            lowest = dAc;
            closestPoint = pAc;
        }

        if (dBc < lowest)
        {
            closestPoint = pBc;
        }

        // Return a CIE value which is within reach of the lamp
        return new CieColor(closestPoint.X, closestPoint.Y);
    } 
    
    public static CieColor CieFromRgb(RgbColor rgb, CieColorGamut gamut)
    {
        double r = rgb.R / 255.0;
        double g = rgb.G / 255.0;
        double b = rgb.B / 255.0;

        double rLinear = (r > 0.04045) ? Math.Pow((r + 0.055) / (1.0 + 0.055), 2.4) : (r / 12.92);
        double gLinear = (g > 0.04045) ? Math.Pow((g + 0.055) / (1.0 + 0.055), 2.4) : (g / 12.92);
        double bLinear = (b > 0.04045) ? Math.Pow((b + 0.055) / (1.0 + 0.055), 2.4) : (b / 12.92);

        double x = rLinear * 0.664511 + gLinear * 0.154324 + bLinear * 0.162028;
        double y = rLinear * 0.283881 + gLinear * 0.668433 + bLinear * 0.047685;
        double z = rLinear * 0.000088 + gLinear * 0.072310 + bLinear * 0.986039;

        double cx = x / (x + y + z);
        double cy = y / (x + y + z);

        var color = new CieColor(cx, cy);
        
        if (!CieInGamut(color, gamut))
        {
            color = ClosestCieInGamut(color, gamut);
        }

        return color; 
    }

    public static RgbColor RgbFromCieAndBrightness(CieColor cie, CieColorGamut gamut, double brightness = 1)
    {
        var color = cie;
        if (!CieInGamut(cie, gamut))
        {
            color = ClosestCieInGamut(cie, gamut);
        }

        double y = brightness;
        double x = (y / color.Y) * color.X;
        double z = (y / color.Y) * (1 - color.X - color.Y);


        double r = x * 1.656492 - y * 0.354851 - z * 0.255038;
        double g = -x * 0.707196 + y * 1.655397 + z * 0.036152;
        double b = x * 0.051713 - y * 0.121364 + z * 1.011530;

        // Apply reverse gamma correction
        r = (r <= 0.0031308) ? (12.92 * r) : ((1.0 + 0.055) * Math.Pow(r, (1.0 / 2.4)) - 0.055);
        g = (g <= 0.0031308) ? (12.92 * g) : ((1.0 + 0.055) * Math.Pow(g, (1.0 / 2.4)) - 0.055);
        b = (b <= 0.0031308) ? (12.92 * b) : ((1.0 + 0.055) * Math.Pow(b, (1.0 / 2.4)) - 0.055);

        // Bring all negative components to zero
        r = Math.Max(0, r);
        g = Math.Max(0, g);
        b = Math.Max(0, b);

        // If one component is greater than 1, weight components by that value
        double maxComponent = Math.Max(r, Math.Max(g, b));
        if (maxComponent > 1)
        {
            r /= maxComponent;
            g /= maxComponent;
            b /= maxComponent;
        }

        // Scale to the 0-255 range and convert to integers
        int red = (int)(r * 255);
        int green = (int)(g * 255);
        int blue = (int)(b * 255);

        return new RgbColor(red, green, blue);
    }
}