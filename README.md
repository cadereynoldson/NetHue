# NetHue
A C# package for managment of Phillips Hue devices.
- Based off of the [Hue Clip API V2](https://developers.meethue.com/develop/hue-api-v2/api-reference/#)
- Implementation is currently geared towards basic interaction with the API, such as getting lights, updating the colors of lights, setting scenes, etc.
- Programmatic creation of scenes, rooms, and so on is currently on hold.
    - The Hue App provides a UI for working with these visual functionalities, consider the current state of this project as an "add on" for interacting with the API programmatically. 

## Base Concepts
- **Controllers:** Handle fetching information about your Hue ecosystem from the configured HueBridge. A controller will (soon to be) exist for each endpoint in the [Hue Clip API V2](https://developers.meethue.com/develop/hue-api-v2/api-reference/#), allowing for programmatic access to Hue resources. 
- **Models:** Store the information returned from the API. Some fields are renamed due to what I can tell to be as *odd* choices of names. 
- **Repositories:** As a user of the package, there should be no direct interation with the repository(s) defined in this package. Handles basic HTTP calls to a Hue bridge. 

## Getting Set-Up
Package was designed to be as easy to get started with as possible. 
1. Follow the according steps to find the IP of your HueBridge, and create a client key for access to your Hue Bridge's API - [Setup Instructions](https://developers.meethue.com/develop/hue-api-v2/getting-started/)
2. Once you have these values, you're almost there! Create a JSON file titled whatever you like, we will call it: `config.json`. This file will contain the information you just fetched about your Hue bridge in the following schema:
    ```json
    {
        "ip": "YOUR_BRIDGE_IP",
        "appKey": "YOUR_SECRET_APPKEY"
    }
    ``` 
3. And thats it! Use this created file to create a `HueConfiguration` object by manually parsing the IP and Application Key, or just use `HueConfiguration.FromJson("config.json")`. All of the controllers for this package take a `HueConfiguration` object as a constructor parameter, or you can provide the path to this file and they'll handle the parsing themselves. 

## Basic Examples
- Example of getting all the lights connected to your Hue bridge:
    ```csharp
    var controller = new HueLightController("config.json");
    var lights = await controller.GetLights();
    ```

- Example getting all the lights of a room configured on your Hue bridge:
    ```csharp
    var config = new HueConfiguration("config.json");
    var lightController = new HueLightController(config);
    var roomController = new HueRoomController(config);

    var room = await roomController.GetRooms().First();
    var lights = await lightController.GetLights(room);
    ```

- Example setting a scene: 
    ```csharp
    var controller = new HueSceneController("config.json");
    var scene = controller.GetScenes().First();

    // SetScene mutates passed in scene object, no need to catch it.
    await controller.SetScene(scene);
    ```

- Example changing the *current* brightness of a scene: 
    ```csharp
    var controller = new HueSceneController("config.json");
    var scene = controller.GetScenes().First();

    // Set scene to 50 percent brightness.
    await controller.SetSceneBrightness(scene, 50);
    ```