# grpc-pubsub-azservicebus RunMe
Completed Wednesday 3/31/21.

This document contains a *conceptual overview* of this Visual Studio solution, plus *instructions to run* the 2 services and the test client with the commands found in DiscoverDapr\grpc-pubsub-azservicebus\vscsharp\StartRunServicesAndUI.txt

## Conceptual Overview of the grpc-pubsub-azservicebus VS Solution

This solution consists of the components, and their interactions, shown in the below Logical Diagram of the software. The **ultimate overall goal** of this solution is to be able to start ServiceA, ServiceB, and the QuickTestClient.  Then, with all 3 running, allow the user to issue commands to the QuickTestClient UI that cause ServiceA to publish events to Pub/Sub component (implemented via an external Azure Service Bus Topic) which is subscribed to by ServiceB.  ServiceB will then receive each event published to the Topic and display the event content on its Console UI.  Likewise, the QuickTestClient and ServiceA also have Console UIs which display the traffic going through each of them.  

![image](https://user-images.githubusercontent.com/6517661/112858075-86198d80-907f-11eb-8980-83a456cef69f.png)

While running ServiceA and ServiceB it is also beneficial to have your Azure Service Bus Topic (the Pub/Sub component) open in the Azure Portal.  This allows you to verify that events have indeed been published, plus monitor any errors that may occur.  For example when you have not yet created at least one subscription for the Topic and try to publish to that Topic, the publish will fail.  However, that failure can only be detected in the Azure Portal which will show a "User Error". And in the Portal you can use the Service Bus Explorer to view and manipulate messages residing in each Topic.

Note:
1. The QuickTestClient UI has a **command that publishes *multiple events***, plus a separate **command that publishes a *single event***.  Please see DiscoverDapr\grpc-pubsub-azservicebus\vscsharp\StartRunServicesAndUI.txt for the syntax of these UI commands.
2. Only Dapr "self-host" mode is used in the below scenarios, i.e. a service and its Dapr side car run in processes (no containers).  Kubernetes is NOT used, although it could be if you want to do the work required for Kubernetes hosting and putting each service in a container as well.

### The Dapr-Mesh
At this point it is useful to define the concept of a Dapr-Mesh.  The term Dapr-Mesh is shorthand for a "A Dapr Mesh-of-Services". A Mesh-of-Services is a collection of services that can collaborate with each other.  The Implementation Diagram below shows a generalized Dapr-Mesh, which *uses Dapr* to facilitate the service's collaboration:

![image](https://user-images.githubusercontent.com/6517661/112873279-6a1de800-908f-11eb-8296-42b0b90ab376.png)

As you can see, a Dapr-Mesh is "A collection of collaborating services all within a 'nearby network neighborhood'".  This quote, plus all diagrams in this RunMe.MD, are from the PowerPoint slide deck "Thinking Outside the Dapr-Mesh with Dapr, gRPC" in this repo at https://github.com/georgestevens99/DiscoverDapr/blob/main/grpc-pubsub-azservicebus/Thinking%20Outside%20the%20Dapr-Mesh%20with%20Dapr%2C%20gRPC.pptx.  This was part of a presentation I gave at the Dapr Community Meeting on 3/23/21.  It is available on YouTube at https://www.youtube.com/watch?v=PXXYxJLyT9Y.

Note that a Dapr-Mesh is very different from Service Meshes like Linkerd, Istio, etc. in 2 ways:
1. The Service Meshes operate the Network Level while Dapr works at the Application Level.  This is shown above with Dapr using HTTP and gRPC communications to do its job.
2. Dapr provides capabilities (via its Building Blocks and Components) to services using it that are far beyond the capabilities offered by Service Meshes.  For example, Dapr provides capabilities like State Storage, Pub/Sub, Bindings and Triggers, Virtual Actors, and more.  These capabilities are broad "runtime" capabilities that also include facilitating services collaborating with each other.  Please see this link for Dapr capabilities https://docs.dapr.io/concepts/overview/. 

### Usage Scenarios for this Code Sample
The **typical usage scenario** is to host ServiceA, ServiceB, their sidecars, and the QuickTestClient on the *same machine*, and run all 5 processes together, all at once, for each testing session.  Here, both ServiceA and ServiceB reside in the ***SAME Dapr-Mesh***, i.e. each will have its own private Dapr sidecar, interconnected to all other sidecars by gRPC connections.  Below is an Implementation Diagram of this "all on same machine" usage scenario.  Note its relationship with the Logical Diagram at the start of this document.  Also note that the **QuickTestClient is not a part of any Dapr-Mesh** since it has no Dapr sidecar.  Finally, note that the Az SB Pub/Sub component is an **external service** that runs in the Azure Cloud and thus is not part of any of the Dapr-Meshes shown below.

![image](https://user-images.githubusercontent.com/6517661/112875080-b9fdae80-9091-11eb-8d1d-a4fba6224e17.png)

However, **one does not have to run all services together, all at once**.  Instead, running only ServiceA (plus its Dapr sidecar) and the QuickTestClient is an excellent way to test just the capability of publishing events that will be stored in the Azure Service Bus Topic until a subscriber process (ServiceB) is started later on.  This gives one the ability to use the Azure Service Bus Explorer in the Azure Portal to examine the content of messages published.  This can be helpful when debugging.  In this scenario you can exit ServiceA and the QuickTestClient, then later start ServiceB and its Dapr sidecar running all by themselves.  Here, ServiceB will immediately read all the messages from the Topic since the code is set up to subscribe to that Topic.  The items processed are visible on the ServiceB console UI.

The above "typical usage scenario" is the way this Visual Studio solution is setup when you follow the directions in InstallMe.MD.  It is ready to run as a single Dapr-Mesh.

Another useful, and quite interesting, hosting scenario has **the 2 services** (plus their Dapr sidecars) **running on separate machines**.  This creates ***2 separate Dapr-Meshes***, one on each machine (still in "self-host" mode on each machine). ServiceA (with its Dapr sidecar) and the QuickTestClient run on one machine, with ServiceA in Dapr-MeshA.  And ServiceB (with its Dapr side car) running on another machine in Dapr-MeshB.  This Multi-Dapr-Mesh scenario is shown in the following Implementation Diagram:

![image](https://user-images.githubusercontent.com/6517661/112878408-e74c5b80-9095-11eb-8f1e-813a701a7d66.png)

For a dive into the details of the Multi-Dapr-Mesh *Mesh-of-Dapr-Meshes* scenario please refer to the PowerPoint slide deck "Thinking Outside the Dapr-Mesh with Dapr, gRPC" in this repo at https://github.com/georgestevens99/DiscoverDapr/blob/main/grpc-pubsub-azservicebus/Thinking%20Outside%20the%20Dapr-Mesh%20with%20Dapr%2C%20gRPC.pptx. Look through the slides so that you understand what a Dapr-Mesh is, what a Mesh-of-Dapr-Meshes is, and their **benefits and limitations** (both are really important!), because that is what is happening in this hosting scenario.  

It is quite easy to set up the Multi-Dapr-Mesh (Mesh-of-Dapr-Meshes) hosting scenario for this Visual Studio solution as follows:
1. Use InstallMe.MD to install ALL the software on 2 machines: Machine A and Machine B. This will simulate 2 geographically separate systems, each of which will host a separate Dapr-Mesh in self-host mode.  And **this requires no code changes!**  
2. Then on Machine A start ServiceA and the QuickTestClient.  Then on MachineB start ServiceB.
3. Then issue commands to the QuickTestClient UI and you will see the results displayed in the serviceB debug console on Machine B.
4. Note that Machine A and Machine B can be anywhere on the planet, so long as they both can successfully connect to the same Azure Service Bus Topic (located externally in some Azure Cloud somewhere).

Now that you have an overview of where the services are hosted and a general understanding of what is going on, here are a few more details on how to run the code in this solution.

## How to Run the Code

Sometimes you will want to run all processes at once:  ServiceA, ServiceB, their side cars, and the QuickTestClient UI.  At other times you will want to run only a few of these.  This is accomplished by Actions. The details of these Actions can be found in  DiscoverDapr\grpc-pubsub-azservicebus\vscsharp\StartRunServicesAndUI.txt.  It contains the following 5 multi-step Actions:
1. ACTION -- Prepare the Visual Studio solution for Test Runs.
2. ACTION -- Start ServiceA and its Dapr sidecar.
3. ACTION -- Start ServiceB and its Dapr sidecar.
4. ACTION -- Start the QuickTestClient.
5. ACTION -- Input commands into the QuickTestClient command window to Test the system.

Many of the steps in each of the above actions require text to be typed into one or more Command Prompt windows.  To save typing and the time required to fix typing errors the StartRunServicesAndUI.txt file is formatted that you can copy a command line from that file, and then paste it into a Command Prompt window.  That is why it is a separate text file, so you can open it on each system when you are doing test runs.

I have found that the Windows Terminal Command Prompt window is much, much more efficient to use that the Windows Command Prompt window. Here is the link to install Windows Terminal for free: https://www.microsoft.com/en-us/p/windows-terminal/9n0dx20hk701?activetab=pivot:overviewtab

Finally, be sure to use the QuickTestClient **a-pubeventmulti** command to load test and stress test the system.  Try running EventCounts of tens of thousands of of events, or more.  And try running thousands or even tens of thousands of events with a 0 millisecond delay.  Do these break your system?  Do these cause events to "backup" in your Azure Service Bus Topic when its "Message Count" is viewed in the portal.  Doing load and stress testing will both show you where you need to strengthen the software, and build your confidence in it as well.

Good luck with your exploration of Dapr, ASP.NET Core gRPC servcies, and the Azure Service Bus.  If you get stuck try asking a question on Dapr Discord at https://discord.com/invite/ptHhX6jc34.
