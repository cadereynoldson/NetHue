# NetHue
A C# package for managment of Phillips Hue devices.
- Based off of the [Hue Clip API V2](https://developers.meethue.com/develop/hue-api-v2/api-reference/#)
- Implementation is currently geared towards basic interaction with the API, such as getting lights, updating the colors of lights, setting scenes, etc.
- Programmatic creation of scenes, rooms, and so on is currently on hold.
    - The Hue App provides a UI for working with these visual functionalities, consider the current state of this project as an "add on" for interacting with the API programmatically. 

### License
Shield: [![CC BY-NC-SA 4.0][cc-by-nc-sa-shield]][cc-by-nc-sa]

This work is licensed under a
[Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License][cc-by-nc-sa].

[![CC BY-NC-SA 4.0][cc-by-nc-sa-image]][cc-by-nc-sa]

[cc-by-nc-sa]: http://creativecommons.org/licenses/by-nc-sa/4.0/
[cc-by-nc-sa-image]: https://licensebuttons.net/l/by-nc-sa/4.0/88x31.png
[cc-by-nc-sa-shield]: https://img.shields.io/badge/License-CC%20BY--NC--SA%204.0-lightgrey.svg

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

#### NOTE: Support for HTTPS certificate validation is still a TODO, will be present in first minor release 

## Basic Examples
- Example of getting all the lights connected to your Hue bridge & setting them to a random color:
    ```csharp
    var controller = new HueLightController("config.json");
    var lights = await controller.GetLights();

    foreach (var light in lights)
    {
        await controller.UpdateLightState
        (
            light, 
            new HueLightStateBuilder()
                .Color(RgbColor.Random(light.CieColorGamut));
        )
    }
    ```

- Example getting all the lights of a room configured on your Hue bridge:
    ```csharp
    var config = new HueConfiguration("config.json");
    var lightController = new HueLightController(config);
    var roomController = new HueRoomController(config);

    var room = (await roomController.GetRooms()).First();
    var lights = await lightController.GetLights(room);
    ```

- Example setting a scene: 
    ```csharp
    var controller = new HueSceneController("config.json");
    var scene = (await controller.GetScenes()).First();

    await controller.SetScene(scene);
    ```

- Example changing the *current* brightness of a scene: 
    ```csharp
    var controller = new HueSceneController("config.json");
    var scene = (await controller.GetScenes()).First();

    // Set scene to 50 percent brightness.
    await controller.SetSceneBrightness(scene, 50);
    ```

- Example changing the brightness of the lights of a zone/room: 
    ```csharp
    var config = new HueConfiguration("config.json");
    var roomController = new HueRoomController(config);
    var lightController = new HueLightController(config);
    var room = (await roomController.GetRooms()).First();

    // Set scene to 50 percent brightness.
    await controller.SetSceneBrightness(scene, 50);
    ```

- Example resource state management:
    ```csharp
    // This is a slightly more "advanced" example, geared towards 
    // demonstrating how we can keep our models up to date when other
    // applications interact with them (e.g. the mobile Hue app)

    var config = new HueConfiguration("config.json");
    var eventRepository = new HueEventRepository(config);

    // Currently supported "managable" resources are lights, and scenes. Let's keep track of them :)
    var sceneController = new HueSceneController(config);
    var lightController = new HueLightController(config);

    var scenes = await sceneController.GetScenes();
    var lights = await lightController.GetLights();

    // Create a hue resource manager with these resources
    var resourceManager = new HueResourceManager()
        .Manage(scenes)
        .Manage(lights);
    
    // Start an event stream, this will run in the background of your application 
    // and keep the HueResource models you fetched up to date
    // If your application has a cancellation token, supply it here for a smooth shut down!
    // Parameter is optional though, and not supplied in this example. 
    eventRepository.StartEventStream(resourceManager); 
    ```
