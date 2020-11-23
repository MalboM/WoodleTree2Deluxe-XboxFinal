Unity SaveData plugin extension for PS4.

NOTE: For convenience this source zip is distributed inside a Unity package, before building you should move it outside of your project's assets folder to avoid the files within this source being treated as Unity assets.
This is an example native plugin for Unity that provides a wrapper to SCE's SaveData library and functionality. Full native (C++) and managed (C#) source code is provided.
Please note that this example project should not be expected to be fully TRC compliant. Planning for TRC compliance should be done early in your project to minimize risk.
The source code is provided as is : you are free to extend / specialize / amend as your project requires. 
The Unity for PlayStation Development Team will also welcome submissions / suggestions for alterations to source code if they have general ongoing utility for other developers.

Difference to the original SaveGame plugin
------------------------------------------
- Methods can be called asynchronously or synchronously.
- There is only one callback now where all asynchronous results are returned. This occurs on a separate C# thread. 
- The save/load/delete dialog flow is now written as a C# Unity coroutine. Before it was an internal state machine written in C++. It is not possible to modify the dialog flow using C# code. 
- Developers can now use methods to directly mount and unmount save data. Most methods provided by the Sony SaveData library now have C# wrappers so developers can have low level control of the save data system.
- The system provides a custom file operation callback so developers can now save multiple files inside a mounted save data. Developer can also create directories and use general .NET file IO methods.
- The system provides methods to read/write save icons. The sample app demonstrates one method to grab a screenshot and save it as part of mounted savedata.
- The methods required to backup and restore SaveData have also been implemented.

Source Code
-----------
The source code for the SonyPS4SaveData.dll and SaveData.prx are provided in a zip file in \Assets\Plugins\PS4\SaveData_Source.zip. Use this code to rebuild the managed and native plugins if you require to change them or if you want to use them on an early SDK version. The pre-built versions in the sample app are always compiled using the same SDK as the latest player release supports.

Setting up the Sample App
-------------------------
After importing the Unity package there are couple of project settings that need to be set correctly. These should only really be used when importing the package into an empty project. Otherwise you might overwrite your own applications Player settings.
Use the custom ‘SCE Publishing Utils->Set Publish Settings For PS4’ menu option within the Editor to initialise the Player Settings. 
WARNING: The next option will overwrite the current \ProjectSettings\InputManager.asset file so don’t use this in your own Unity projects.
Use custom ‘SCE Publishing Utils->Set Input Manager’ Editor menu option. This is used so the GamePad code can pick up input from up to four local users in the sample app. 
Open the build settings and set the ‘SaveDataSample’ scene as Scene 0, switch to PS4 platform and build/run as normal.

Using Your Own Title ID
-----------------------
You may wish to use your own title id and trophy pack in the sample. This may require you to update the sample in a number of places. 

Documentation and Intellisense
------------------------------
An XML file called SonyPS4SaveData.XML is provided alongside the SonyPS4SaveData.dll in the \Assets\SonyAssemblies. This contains the documentation for the C# API which is picked up by Visual Studio. The C# source code solution will generate this file if you build the dll from source. 

Request/Response API
--------------------
The new SaveData plugin uses a pattern of Request/Response objects. Each API call passes in a Request object that contains the attributes to use for the method, and a Response object which will be filled out with the results. 

Multiple Local User support
---------------------------
Each request object contains a UserId property. This is the local user ID that makes the request. Some methods require this to fetch or perform certain actions where the identity of the user is required. For example fetching a user list of saves.

Asynchronous and Synchronous Calls
----------------------------------

All Request objects contain an Async property. Use this to specify if the call should be asynchronous or synchronous. Synchronous calls will block until the results are returned. Asynchronous calls will return immediately, and once results are available a callback (declared in the script) is called containing the Response object.
When the call is asynchronous, methods will return a request ID. This can be used to track or abort requests.
NOTE: Once a request has started to be sent (i.e. reached the top of the internal queue of requests) it is no longer possible to abort them.
Errors occurring inside methods are generated as exceptions.

Asynchronous Callback

---------------------
To handle asynchronous requests a callback must be declared like so:

Sony.PS4.SaveData.Main.OnAsyncEvent += OnAsyncEvent;

private void OnAsyncEvent(Sony.PS4.SaveData.SaveDataCallbackEvent callbackEvent)
{
    try
    {
        // Process callbackEvent.Response object here 
    }
    catch (Sony.PS4.SaveData.SaveDataException e)
    {
    }
    catch (Exception e)
    {
    }
}

The SaveDataCallbackEvent contains the response object, return code, enums for function type.

It’s important to add the exception handling. The OnAsyncEvent is called on a separate C# thread. There is no need to update the system in a MonoBehaviour’s Update method. 
If you don’t catch these exceptions, then the thread itself will catch them and log them to the console output but you won’t see any exception display in the Unity app as the exception is not occurring on the main Mono thread. 
This means you might not be immediately aware an exception has occurred. You need to catch errors in the Unity script and handle them in a safe way. 
Be careful not to cause this thread to stall for a long time or stop it in any way, as without this thread there will be nothing to fetch the data from the native plugin and call the OnAsyncEvent callback (and stop the system from returning any asynchronous responses). 
Don't make synchronous calls from this thread as that may cause the system to stall indefinitely. 

Exception Handling
------------------
All methods can generate exceptions if an error occurs. These generally occur due to incorrect parameters being used. There is a specific SaveDataException which contains details about an errors generated. This can include a SceErrorCode, which can be searched for on DevNet or the PS4 Error Code Viewer tool installed with your SDK. If the error originates from the C++ native plugin it will also include a C++ filename and line number.

Response Return Code
--------------------
The response object contains a ReturnCode which might contain an error code if less than 0, or a return code dependent on the type of request method used.
A Response object can also contain additional error information.
