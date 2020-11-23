Unity NPToolkit2 plugin extension for PS4.

NOTE: For convenience this source zip is distributed inside a Unity package, before building you should move it outside of your project's assets folder to avoid the files within this source being treated as Unity assets.

This is an example native plugin for Unity that provides a wrapper to SCE's NPToolkit libraries and functionality. Full native (C++) and managed (C#) source code is provided.
Please note that this example project should not be expected to be fully TRC compliant. Planning for TRC compliance should be done early in your project to minimize risk.
The source code is provided as is : you are free to extend / specialize / amend as your project requires. 
The Unity for PlayStation Development Team will also welcome submissions / suggestions for alterations to source code if they have general ongoing utility for other developers.

Note any future changes to the Sony libSceNpToolkit2.prx API, when SDK updates are released, may require changes to the managed C# API. 

Major Difference to NpToolkit 1
-------------------------------
- All methods that require network access can be called asynchronously or synchronously. There is no ‘busy’ state anymore so requests can be queued up. Methods can also be called from separate C# threads. See ‘Asynchronous and Synchronous Calls’ below for details.

- There is only one callback now where all asynchronous results are returned. This occurs on a separate C# thread. There is no longer any need to update the NpToolkit2 system in a MonoBehaviour’s Update method. See ‘Asynchronous Callback’ below for details.

- Make sure the ‘Modules’ list in the Publishing Settings contains libSceNpToolkit2.prx. Also make sure any reference to the old libSceNpToolkit.prx is removed as it's not possible to have both the old and new NpToolkit libraries included at the same time.

- The push notification tickboxes in the publishing settings are only used for the original NpToolkit. The new system is setup via script code by configuring a set of push notifications in the Sony.NP.InitToolkit.serverPushNotifications class before the toolkit is initialized. 
 
- The sample app now supports up to four gamepads. It is now possible to have multiple local users signed into PSN. The sample app shows a list of local users and highlights the current local user. The current local user is chosen based on which gamepad last produced any input.

- The C# API has been kept as close to the C++ Sony NpToolkit API as possible. This means the Sony documentation on DevNet for NpToolkit is very relevant to the way the C# API works and is a good source of information.

- NpToolkit2 is only available for PS4, and does not function on other platforms (including the Editor).

Server Push Notifications
-------------------------

serverPushNotifications is no longer set using a the ServerPushNotifications structure. They must now be set using SetPushNotificationsFlags along with a set a flags. For example.

	init.SetPushNotificationsFlags(Sony.NP.PushNotificationsFlags.NewGameDataMessage | Sony.NP.PushNotificationsFlags.NewInGameMessage |
											Sony.NP.PushNotificationsFlags.NewInvitation | Sony.NP.PushNotificationsFlags.UpdateBlockedUsersList |
											Sony.NP.PushNotificationsFlags.UpdateFriendPresence | Sony.NP.PushNotificationsFlags.UpdateFriendsList);

This will enable all push notifications. If push notifications are enabled for services your product doesn't support it can result in various errors occurring  when making NPToolkit2 requests.

Source Code
-----------
The source code for the SonyNp.dll and UnityNpToolkit2.prx are provided in a zip file in \Assets\Plugins\PS4\UnityNpToolkit2_Source.zip. Use this code to rebuild the managed and native plugins if you require to change them or if you want to use them on an early SDK version. The pre-built versions in the sample app are always compiled using the same SDK as the latest player release supports.

Setting up the Sample App
-------------------------
After importing the Unity package there are couple of project settings that need to be set correctly. These should only really be used when importing the package into an empty project. Otherwise you might overwrite your own applications Player settings.

Use the custom ‘SCE Publishing Utils->Set Publish Settings For PS4’ menu option within the Editor to initialise the Player Settings. This will setup the TitleId etc for the Unity sample app. This will also swap the default libSceNpToolkit.prx in the ‘Modules’ list with the new libSceNpToolkit2.prx

WARNING: The next option will overwrite the current \ProjectSettings\InputManager.asset file so don’t use this in your own Unity projects.

Use custom ‘SCE Publishing Utils->Set Input Manager’ Editor menu option. This is used so the GamePad code can pickup input from up to four local users in the sample app. 

Open the build settings and set the ‘NpToolkitSample’ scene as Scene 0, switch to PS4 platform and build/run as normal.

Using Your Own Title ID
-----------------------
You may wish to use your own title id and trophy pack in the sample. This may require you to update the sample in a number of places. 

You will need to set the correct push notifications for your title. See above. 

If you don’t have the TUS service you will need to comment out ENABLE_TUS_LOGGING define at the top of SonyNpMain.cs, otherwise the sample app will attempt to use it and will fail if your product doesn’t have the TUS service. 

Lastly there are many other places in the sample app that assume access to specific services or configurations that might not work with your own Title ID depending on how your product is configured.

Documentation and Intellisense
------------------------------
An XML file called SonyNP.XML is provided alongside the SonyNP.dll in the \Assets\SonyAssemblies. This contains the documentation for the C# API which is picked up by Visual Studio. The C# source code solution will generate this file if you build the dll from source. 

Request/Response API
--------------------
The new NpToolkit uses a pattern of Request/Response objects. Each API call passes in a Request object that contains the attributes to use for the method, and a Response object which will be filled out with the results. 

Multiple Local User support
---------------------------
Each request object contains a UserId property. This is the local user ID that makes the request. This is only relevant for some methods. Some methods require this to fetch or perform certain actions where the identity of the user is required. For example fetching a list of friends.

Asynchronous and Synchronous Calls
----------------------------------
All Request objects contain an Async property. Use this to specify if the call should be asynchronous or synchronous. Synchronous calls will block until the results are returned. Asynchronous calls will return immediately, and once results are available a callback (declared in the script) is called containing the Response object.

When the call is asynchronous, methods will return a request ID. This can be used to track or abort requests.

NOTE: Once a request has started to be sent (i.e. reached the top of the internal queue of requests) it is no longer possible to abort them.

Errors occurring inside methods are generated as exceptions.

Asynchronous Callback
---------------------
To handle asynchronous requests a callback must be declared like so:

Sony.NP.Main.OnAsyncEvent += OnAsyncEvent;

private void OnAsyncEvent(Sony.NP.NpCallbackEvent callbackEvent)
{
    try
    {
        // Process callbackEvent.Response object here 
    }
    catch ( Sony.NP.NpToolkitException e)
    {
    }
    catch (Exception e)
    {
    }
}

The NpCallbackEvent contains the response object, return code, enums for service type and function type. It also contains any server error info.

It’s important to add the exception handling. The OnAsyncEvent is called on a separate C# thread. There is no need to update the system in a MonoBehaviour’s Update method. If you don’t catch these exceptions, then the thread itself will catch them and log them to the console output but you won’t see any exception display in the Unity app as the exception is not occurring on the main Mono thread. This means you might not be immediately aware an exception has occurred. You need to catch errors in the Unity script and handle them in a safe way. Be careful not to cause this thread to stall for a long time or stop it in any way, as without this thread there will be nothing to fetch the data from the native plugin and call the OnAsyncEvent callback (and stop the system from returning any asynchronous responses). Don't make synchronous calls from this thread as that may cause the system to stall indefinitely. 

The sample app has code to show how to handle the response objects in a MonoBehaviour’s Update if you don’t want to handle them on a separate thread, as you may not want to deal with the issues of multi-threaded programming. At the top of SonyNpMain.cs there is a define called USE_ASYNC_HANDLING. Disable this to see how the sample handles responses on the main thread. The OnAsyncEvent is still called on a separate thread but the results are added to a queue. Each frame one response is popped off the queue in the MonoBehaviour’s Update, and then processed. This is a very simple producer/consumer pattern.

Exception Handling
------------------
All methods can generate exceptions if an error occurs. These generally occur due to incorrect parameters being used. There is a specific NpToolkitException which contains details about an errors generated. This can include a SceErrorCode, which can be searched for on DevNet or the PS4 Error Code Viewer tool installed with your SDK. If the error originates from the C++ native plugin it will also include a C++ filename and line number.

Response Return Code and Server Error
-------------------------------------
The response object contains a ReturnCode which might contain an error code if less than 0, or a return code dependent on the type of request method used.

A Response object can also contain additional error information in a ServerError property object. Use HasServerError property to see if a server error has occurred, then you will need to handle the error.

This server error may also contain a timer value in WebApiNextAvailableTime. If this is not 0 then it means the application has many more requests than the PSN servers allow and the app must wait for the number of seconds specified in WebApiNextAvailableTime until another call can be made. See DevNet on how to handle and managed the network load the application places on the PSN servers. See here https://ps4.scedev.net/technotes/view/109/1 and check the TRC document for details.

PlayMode Tests
--------------

There are two steps required to enable PlayMode tests for the sample app. 

1) Open the 'Test Runner' window 'Window->Test Runner' and select the PlayMode tab. Then click the button to enable PlayMode tests. This will require a restart of the editor.
2) Open the sample app test config window under 'SCE Publishing Utils->Configure PlayMode Tests'.
   a) In this window select the tests you want to enable
   b) Wait for the scripts to compile and the tests will show in the Test Runner's PlayMode window.
3) Select 'Run all in player (PS4)' to run the tests.

The results of the tests will be displayed on the PS4 as well as the PlayMode window.

For more details on the 'Test Runner' see the Unity editor documentation. 

For the Sample App it is safe to enable 'All' test as the Sample App has all of the PSN features enabled. 

If you want to try these tests in your own app you should only enable tests your Title Id will be able to run. If you attempt to run tests your app is entitled to they are mostly likely throw an error as those API calls aren't authorised for your title id.

You should also be aware you must have a signed-in PSN user for these tests to work.
