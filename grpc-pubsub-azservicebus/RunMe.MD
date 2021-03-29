# grpc-pubsub-azservicebus RunMe
UNDER CONSTRUCTION -- Estimated completion date Monday 3/29/21.  This is still quite rough, in the early stage of being written.

Contains instructions to run the 2 services and the test client with commands found in DiscoverDapr\grpc-pubsub-azservicebus\vscsharp\StartRunServicesAndUI.txt

## Overview of the grpc-pubsub-azservicebus VS solution

The **ultimate overall goal** of this solution is to be able to start serviceA, serviceB, and the QuickTestClient.  Then with all 3 running, allow the user to issue commands to the QuickTestClient UI that cause ServiceA to publish events to an Azure Service Bus Topic which is subscribed to by serviceB.  serviceB will then receive each event published to the Topic and display the event content on its debug UI.  Likewise, the QuickTestClient and serviceA also have UIs which display the traffic going through each of them.  This flow of processing is shown in the below logical diagram of the software:

![image](https://user-images.githubusercontent.com/6517661/112858075-86198d80-907f-11eb-8980-83a456cef69f.png)

It is also beneficial while running serviceA and serviceB to have your Azure Service Bus Topic open in the Azure Portal.  This will allow you to verify that events have indeed been published, plus monitor any errors that may occur.  For example when you have not yet created at least one subscription for the Topic and try to publish to that Topic, the publish will fail.  However, that failure can only be detected in the Azure Portal which will show a "User Error".

Note:
1. The QuickTestClient has a command that **publishes multiple events**, plus a separate **command that publishes a single event**.
2. Only local hosting of a service and its Dapr side car in processes (no containers) is used in the below scenarios.  Kubernetes is NOT used, although it could be if you want to do the work required for Kubernetes hosting and putting each service in a container as well.

At this point it is useful to define the concept of a Dapr-Mesh.  The term Dapr-Mesh is shorthand for a "A Dapr-Mesh of Services".  The Implementation Diagram below shows a generalized Dapr-Mesh:

![image](https://user-images.githubusercontent.com/6517661/112873279-6a1de800-908f-11eb-8296-42b0b90ab376.png)

As you can see, a Dapr-Mesh is "A collection of collaborating services all using Dapr sidecars within a “nearby network neighborhood”".  This quote and all diagrams in this RunMe.MD are from the PowerPoint slide deck in this repo, at DiscoverDapr\grpc-pubsub-azservicebus\Thinking Outside the Dapr-Mesh with Dapr, gRPC.pptx.  This was part of a presentation I gave at the Dapr Community Meeting on 3/23/21.

The **typical usage scenario** is to host serviceA, serviceB, and the QuickTestClient on the *same machine*, and run all 3 processes at once for each testing session.  Here, both serviceA and serviceB reside in the ***SAME Dapr-Mesh***, i.e. each will have its own private Dapr sidecar, which are interconnected by gRPC connections.  Below is an Implementation Diagram of this "all on same machine" usage scenario.  Note its relationship with the Logical Diagram at the start of this document.

![image](https://user-images.githubusercontent.com/6517661/112875080-b9fdae80-9091-11eb-8d1d-a4fba6224e17.png)

However, **one does not have to run all three services at once**.  Instead, running only serviceA (plus its Dapr sidecar) and the QuickTestClient is an excellent way to test the capability of publishing events that will be stored in the Azure Service Bus Topic until a subscriber process (ServiceB) is started later on.  This gives one the ability to use the Azure Service Bus Explorer in the Azure Portal to examine the content of messages published.  This can be helpful when debugging.  In this scenario you can exit serviceA and the QuickTestClient, then later start serviceB and its Dapr sidecar running all by themselves.  Here, serviceB will immediately read all the messages from the Topic since the code is set up to subscribe to that Topic.  The items processed are visible on the serviceB console UI.

The above "typical usage scenario" is the way this Visual Studio solution is setup when you follow the directions in InstallMe.MD.  It is ready to run as a single Dapr-Mesh.

Another quite interesting hosting scenario has **the 2 services** (plus their Dapr sidecars) **running on separate machines**.  This creates ***2 separate Dapr-Meshes***, one on each machine. ServiceA (with its Dapr sidecar) and the QuickTestClient run on one machine, with ServiceA in Dapr-MeshA.  And ServiceB (with its Dapr side car) runing on another machine in Dapr-MeshB.  This Multi-Dapr-Mesh scenario is shown in the below Implementation Diagram:

![image](https://user-images.githubusercontent.com/6517661/112878208-a3f1ed00-9095-11eb-875e-b14983d55fed.png)

For a dive into a *Mesh-of-Dapr-Meshes* please refer to the PowerPoint presentation "Thinking Outside the Dapr-Mesh" that is part of the DiscoverDapr\grpc-pubsub-azservicebus repo you cloned, as follows: In Visual Studio switch to the folder view.  Then open the grpc-pubsub-azservicebus and you will see this PowerPoint slide deck.  Look through the slides so that you understand what a Dapr-Mesh is, and what a Mesh-of-Dapr-Meshes is because that is what is happening in this hosting scenario.  

Set up the Multi-Dapr-Mesh hosting scenario as follows:
1. Use InstallMe.MD to install ALL the software on 2 machines!  Machine A and Machine B. This will simulate 2 geographically separate systems, each of which will host a separate Dapr-Mesh.  
2. Then on Machine A start serviceA and the QuickTestClient.  Then on MachineB start serviceB.
3. Then issue commands to the QuickTestClient UI and you will see the results displayed in the serviceB debug console on Machine B.
4. Note that Machine A and Machine B can be anywhere, so long as they both can successfully connect to the same Azure Service Bus Topic!

So, now that you have an overview of where the services are hosted and a general understanding of what is going on, here are a few more details on how to run the code in this solution.

## Actions to prepare the Visual Studio solution for running serviceA, serviceB, and the QuickTestClient

You will need to perform a small set of actions at the start of each session where you run one or more services or the QuickTestClient, as follows:
1. Start Visual Studio.  Then open the DiscoverDapr folder on your machine that you cloned from GitHub.
2. In the Visual Studio Solution Explorer, navigate to the grpc-pubsub-azservicebus\vscsharp folder.
3. Open the grpc-pubsub-azservicebus solution by double clicking on the .sln file icon shown in the Solution Explorer.
4. It is best to rebuild the entire solution before running any of the above services, just to prevent wasting time
   by having to chase down inconsistences.  Before each test run open the VS Build menu in the menu bar at the
   top of the display, and click Rebuild Solution.
5. Now you are ready to do any, or all, of the Actions described below.

## Overview of run scenarios
Sometimes you will want to run all three processes at once:  serviceA, serviceB, and the QuickTestClient UI.  At other times you will want to run only one or two of these.  The below 4 Actions are separated out into Actions for running serviceA, Actions for running serviceB, and Actions for running the QuickTestClient.  Each of these 4 Actions may be performed independently of the other Actions.  However, for full end-to-end functionality of of the Pub/Sub example it is best to perform thee actions in the order presented below.

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
