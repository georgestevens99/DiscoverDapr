# grpc-pubsub-azservicebus RunMe
UNDER CONSTRUCTION -- This will be written on Sat 3/27/21.

Contains the instructions to run the 2 services and test client with the commands found in StartRunServicesAndUI.txt
## Overview

The **ultimate overall goal** of this solution is to be able to start serviceA, serviceB, and the QuickTestClient.  Then with all 3 running, issue commands to the QuickTestClient UI that cause ServiceA to publish events to an Azure Service Bus Topic which is subscribed to by serviceB.  serviceB will then receive each event published to the Topic and display the event content on its debug UI.  Likewise, the QuickTestClient and serviceA also have UIs which display the traffic going thru each of them.

It is also beneficial while running serviceA and serviceB to have your Azure Service Bus Topic open in the Azure Portal.  This will allow you to verify that events have indeed been published, plus monitor any errors that may occur.  For example when you have not yet created at least one subscription for the Topic and try to publish to that Topic, the publish will fail.  However, that failure can only be detected in the Azure Portal which will show a "User Error".

Note:
	1. the QuickTestClient has a command that publishes multiple events, plus a separate command that publishes a single event.
	2. Only local hosting of a service and its Dapr side car in processes (no containers) is used in the below scenarios.  Kubernetes is NOT used, although it could be if you want to do the work required for Kubernetes hosting and putting the services in containers as well.

The typical usage scenario is to host serviceA, serviceB, and the QuickTestClient on the same machine, and run all 3 processes at once for each testing session.  Here, both serviceA and serviceB will reside in the SAME Dapr-Mesh, i.e. each will have its own private Dapr sidecar, which are interconnected by gRPC connections using

However, one does not have to run all three services at once.  Running only serviceA and the QuickTestClient is an excellent way to test the capability of publishing events that will be stored in the Azure Service Bus Topic until a subscriber process is started.  This gives one the ability to use the Azure Service Bus Explorer in the Azure Portal to examine the content of messages published.  In this scenario you can exit serviceA and the QuickTestClient, then later start serviceB running all by itself.  Here, serviceB will immediately read all the messages from the Topic since the code is set up to subscribe to that Topic.  The items processed are visible on the serviceB console UI.

Another quite interesting hosting scenario has serviceA and the QuickTestClient running on separate machines, as follows:
1. Use InstallMe.MD to install ALL the software on 2 machines!  Machine A and Machine B. This will simulate 2 geographically separate systems, each of which will host a separate Dapr-Mesh.

## Actions to prepare the Visual Studio solution for running serviceA, serviceB, and the QuickTestClient

You will need to perform a small set of actions at the start of each session where you run one or more services or the QuickTestClient, as follows:
1. Start Visual Studio.  Then open the DiscoverDapr folder on your machine that you cloned from GitHub.
2. In the Visual Studio Solution Explorer, navigate to the grpc-pubsub-azservicebus\vscsharp folder.
3. Open the grpc-pubsub-azservicebus solution by double clicking on the .sln file icon shown in the Solution Explorer.
4. It is best to rebuild the entire solution before running any of the above services, just to prevent wasting time
   by having to chase down inconsistences.  Before each test run open the VS Build menu in the menu bar at the
   top of the display, and click Rebuild Solution.
5. Now you are ready to do any, or all, of the Actions describe below.

## Overview of run scenarios
Sometimes you will want to run all three processes at once:  serviceA, serviceB, and the QuickTestClient UI.  At other times you will want to run only one or two of these.  The below Actions are separated out into Actions for running serviceA, Actions for running serviceB, and Actions for running the QuickTestClient.  Each of these 3 Actions can be performed independently of each other.  However, for full end-to-end functionality of pub

From StartRunServicesAndUI.ext

Actions to prepare the Visual Studio solution for running serviceA, serviceB, and the QuickTestClient.
1. Start Visual Studio.  Then open the DiscoverDapr folder on your machine that you cloned from GitHub.
2. In the Visual Studio Solution Explorer, navigate to the grpc-pubsub-azservicebus\vscsharp folder.
3. Open the grpc-pubsub-azservicebus solution by double clicking on the .sln file icon shown in the Solution Explorer.
4. It is best to rebuild the entire solution before running any of the above services, just to prevent wasting time
   by having to chase down inconsistences.  Before each test run open the VS Build menu in the menu bar at the
   top of the display, and click Rebuild Solution.
5. Now you are ready to do any, or all, of the following ACTIONS.




ACTIONS to start and run serviceA and its Dapr sidecar.
	Dapr starts serviceA via dotnet run (with implicit "." for current directory).

Open a Windows Terminal Command Prompt window or Windows Command Prompt window.  Input the following commands. Best to do
this one-by-one at first by copying a single command line from this file.  Paste it in to the Command Prompt window.  Press Enter.
NOTE you need a SEPARATE COMMAND PROMPT WINDOW for each service and the QuickTestClient!

cd LocalRepoPathPrefix\DiscoverDapr\grpc-pubsub-azservicebus\Services\ServiceA
dapr run --app-id serviceA dotnet run




ACTIONS to start and run serviceB and its Dapr sidecar.
	Dapr starts serviceB via dotnet run (with implicit "." for current directory).


Open a 2nd separate Windows Terminal Command Prompt window or Windows Command Prompt window.  Input the following commands. 
Best to do this one-by-one at first by copying a single command line from this file. Then Paste it in to the Command Prompt window.
Press Enter.  NOTE you need a SEPARATE COMMAND PROMPT WINDOW for each service and the QuickTestClient!

cd LocalRepoPathPrefix\DiscoverDapr\grpc-pubsub-azservicebus\Services\ServiceB
dapr run --app-id serviceB --app-port 5050 --app-protocol grpc dotnet run




ACTIONS to start and run the QuickTestClient. NOTE you need a SEPARATE COMMAND PROMPT WINDOW for each service and the QuickTestClient!

1. In the Visual Studio Solution Explorer, navigate to the grpc-pubsub-azservicebus\vscsharp folder.
2. Open the grpc-pubsub-azservicebus solution by double clicking on the .sln file icon.
3. Find the QuickTestClient project and hover your mouse cursor on it.
   a. Right click this project symbol to make a context menu pop up.  
   b. Hover your mouse cursor over the Debug context menu item.  
   c. Click on Start New Instance and the Quick Test Client will start and display its own Command Prompt window.




ACTIONS to input commands into the QuickTestClient command window that will cause it to publish an event to ServiceA which will enqueue one or more items into your Azure Service Bus svcADemoEvent1 Topic.

Note:  The payload must always be a string since this is a minimal implementation no DTOs are used! Also note that the QuickTestClient
       automagically prepends a 9 character "SenderSequeuceNumber" to each payload string.  This allows the output
	   of the receiving service to be inspected to find missing messages, a BAD BUG!  But, the SenderSequenceNumber is only
	   prepended to the multi-event publish command.

1. The below QuickTestClient UI command publishes a SINGLE message, WITHOUT a SenderSequenceNumber. Best to do
this by copying a single UI command line from this file.  Paste it in to the Command Prompt window.  Press Enter.

	a-pubevent daprpubsub svs-pubsub-asbtopic svcADemoEvent1 payload=tttttt



2. The below QuickTestClient UI command publishes MULTIPLE messages, WITH a SenderSequenceNumber. Best to do
this by copying a single UI command line from this file.  Paste it in to the Command Prompt window.  Press Enter.

	a-pubeventmulti daprpubsub svs-pubsub-asbtopic svcADemoEvent1 payload=tttttt 10 1000

	The two numbers at the right end of the command line are 10 for EventCount to publish, and
	1000 for NumberOfMillisecondsToDelay between event publications.

Please see the source code of the QuickTestClient for the meaning of all other command arguments.  However, with the
way this solution is set up these args CANNOT BE CHANGED:  daprpubsub svs-pubsub-asbtopic svcADemoEvent1.  Changing
them in any way is a breaking change and will cause an error.
