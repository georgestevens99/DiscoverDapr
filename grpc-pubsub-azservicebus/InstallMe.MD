# grpc-pubsub-azservicebus Installation Instructions
Completed Thursday 4/1/21.

## Prerequisites
You need to fulfill three prerequisites to use this code sample.
1. Microsoft Visual Studio IDE (integrated development environment) needs to be **installed on *each machine*** that will run one of the services in the code sample.  The Microsoft **Visual Studio Community** edition is free.

2. You need to have a Microsoft **Azure cloud subscription**.  Free introductory subscriptions are available.

3. **Dapr** needs to be **installed on *each machine*** that will run one of the services in the code sample. The code sample will work with any version of Dapr that is compatible with v1.0.1 Dapr runtime and v1.0.0 Dapr CLI.  Dapr is free of charge.

**Visual Studio Community** -- Get the free Visual Studio Community IDE at this link:  https://visualstudio.microsoft.com/free-developer-offers/
* Note that you must **get Visual Studio Community and NOT Visual Studio Code**.  It takes a substantial amount of work to get the code sample to run using Visual Studio Code.  Don't bother now. Rather save VS Code for later if you want to use it.
  
**Azure subscription** -- Get a free Azure subscription at one of these links:
1. For Developers -- This subscription has some extra developer oriented features: https://visualstudio.microsoft.com/dev-essentials/
2. For general use: https://azure.microsoft.com/en-us/free/ 
3. For Students -- This subscription does NOT require a credit card, unlike the 2 above.  But you must prove you are a student: https://azure.microsoft.com/en-us/free/students/

**Dapr** -- Install Dapr on *each machine* that runs one of the services in the code sample by following the instructions at this link: https://docs.dapr.io/getting-started/
* You will only need to do the first 2 steps, i.e. 1. Install the Cli and 2. Initialize Dapr.  
* The code sample does NOT require that Docker be installed.  Therefore you can use the command **dapr init --slim** rather than the standard **dapr init** command.  
    * However, if you want to explore the full features of Dapr beyond just this code sample, go ahead and follow the instructions for initializing for use WITH Docker and use **dapr init**.  This requires that you install Docker Desktop if it is not already installed on your "test" machine.
    
To install Dapr:
1. Install the Dapr Cli (command language interface):  https://docs.dapr.io/getting-started/install-dapr-cli/
2. Install the Dapr Runtime (i.e. the executable) as follows.  Please **make a note of the full pathname of the directory** on your local machine where Dapr is installed.  This is known as the **DaprRuntimePathPrefix** in this document.  It is used later in this document, and perhaps in other documents as well. 

    * Initialize Dapr for use WITHOUT Docker:  https://docs.dapr.io/operations/hosting/self-hosted/self-hosted-no-docker/
    * OR
    * Initialize Dapr for use WITH Docker (the normal configuration):  https://docs.dapr.io/getting-started/install-dapr-selfhost/  
      * If you are using Windows for your development system I recommend installing WSL2 Backend (from Microsoft) that really smooths the process of using Docker.  WSL means Windows System for Linux.  It emulates a Linux machine to make Docker work smoother.  See the Docker Desktop for Windows documentation at https://docs.docker.com/docker-for-windows/install/ for links on how to install WSL2. With WSL2 you do not need to install a Linux distro in order to run services using Dapr!  Having only WSL2 and Docker is all you need.

You are done with the prequisites step!

## Get and Deploy GitHub Repo Assets
Now that you have all the prerequisites installed you need to obtain and setup the assets (the code, files, scripts, etc) for the grpc-pubsub-azservicebus VS solution from the GitHub repository.
1. **Copy the VS solution from GitHub to your machine** -- Clone this repo (https://github.com/georgestevens99/DiscoverDapr) to the machine (your "test" machine) upon which you intend to run this code sample, as follows:
    * Start Visual Studio IDE on your "test" machine.
    * Download the repo, i.e. Clone it from GitHub to your "test" machine.
      * In the Visual Studio start up menu click on the "Clone a repository" selection.  Then fill out the input boxes on the screen as follows.
        * For the "Repository location" input box use https://github.com/georgestevens99/DiscoverDapr.
        * For the "Path" input box insert the file path on your machine where you want the DiscoverDapr repo to reside. This path is know as the **LocalRepoPathPrefix** in this document so **record it somewhere**.  You will need to use this path name later.
      * It may take a few minutes for the GitHub repo to be downloaded to your machine.
    * Open the solution and build it.
      * When the download is complete, in Visual Studio (VS) open the grpc-pubsub-azservicebus.sln.  Double clicking on the .sln file icon will open the solution.
      * Once the solution is open in VS, build the solution by opening the Build menu in the menu bar at the top of the display and click on Rebuild.
      * The build should work, and if it does then you can proceed with the remainder of these installation instructions.
        * If the build has errors then you must fix them before proceeding.  Try asking on Dapr Discord at https://discord.com/invite/ptHhX6jc34 if you cannot figure out by yourself how to get the solution to build properly.
       
2. **Deploy repo files** to required places outside the repo as follows:
    * Copy the Azure Service Bus Pub/Sub Dapr component .yaml file from the local solution to the .dapr\components directory on your "test" machine.
      * In your local copy of the DiscoverDapr repo at **LocalRepoPathPrefix**, copy LocalRepoPathPrefix\DiscoverDapr\grpc-pubsub-azservicebus\components\svs-pubsub-asbtopic.yaml to your machine's DaprRuntimePathPrefix\\.dapr\components directory.
        * **DaprRuntimePathPrefix** is the directory on your machine where your **dapr init** command placed the Dapr runtime executables and components when you installed Dapr above. 
    * If you **ARE NOT using Docker**, i.e. you installed Dapr with dapr init --slim, then do the following 2 items that manipulate yaml files so as to prevent dapr from generating fatal errors when it encounters components that require containers running in Docker plus to prevent your display being cluttered with non-fatal errors when dapr tries to write to the tracing port.  If you **ARE using Docker** *do NOT do the following two items* that manipulate yaml files.
      * **ARE NOT using Docker** -- With the exception of svs-pubsub-asbtopic.yaml, remove all other yaml files from your machine's DaprRuntimePathPrefix\\.dapr\components directory.  I do this by making another directory called DaprRuntimePathPrefix\\.dapr\componentsOffline.  Then move all items in DaprRuntimePathPrefix\\.dapr\components directory to DaprRuntimePathPrefix\\.dapr\componentsOffline except leaving only svs-pubsub-asbtopic.yaml in DaprRuntimePathPrefix\\.dapr\components directory.  This prevents fatal errors when dapr tries to use a component that depends on a container running in Docker.  For example when using the default pubsub.yaml component, or the default statestore.yaml component, or other components depending upon containers running in Docker.
      * **ARE NOT using Docker** -- In DaprRuntimePathPrefix\\.dapr\config.yaml change samplingRate: "1" to samplingRate: "0".  This prevents the display from being cluttered with non-fatal errors when dapr tries to write tracing data every second or so.

3. **Update the LocalRepoPathPrefix path names in the StartRunServicesAndUI.txt file** in Visual Studio.  Commands in this file are used when you actually run services in the VS solution via Command Prompt windows.
  * You can see StartRunServicesAndUI.txt in the VS Solution Explorer. Open StartRunServicesAndUI.txt in Visual Studio.
  * Replace the string LocalRepoPathPrefix with the actual full path string you saved above.  Do this in the 2 lines in StartRunServicesAndUI.txt shown below:
    * cd **LocalRepoPathPrefix**\DiscoverDapr\grpc-pubsub-azservicebus\Services\ServiceA
    * cd **LocalRepoPathPrefix**\DiscoverDapr\grpc-pubsub-azservicebus\Services\ServiceB  
  * When you are done with the string replacement be sure to do a test to make sure the above cd commands actually work in a Command Prompt window, as follows:
    *  Open a Command Prompt window.  
      * Using the Windows Terminal Command Prompt is much, much easier to deal with so install Windows Terminal if you do not already have it!  Here is the link to install Windows Terminal for free: https://www.microsoft.com/en-us/p/windows-terminal/9n0dx20hk701?activetab=pivot:overviewtab
    *  Copy the above string containing the full path name into the Command Prompt window and press enter.  
      *  If it does not execute the "cd" command you will have to figure out why and fix it.  This "cd" command MUST work.
  *  When both of the newly adjusted "cd" commands in the StartRunServicesAndUI.txt file have been proven to work without a doubt, SAVE the file.
 
You are done with the GitHub Repo Assets step!
  
## Create Azure Assets
Creating the Azure Assets required by the grpc-pubsub-azservicebus solution is fairly easy.  There is only a single Azure Asset required -- An Azure Service Bus Topic with one Subscription.  The Topic and Subscription must be part of an Azure Service Bus Namespace, a "logical container" for Queues and Topics.  And the Azure Service Bus Namespace must be within an Azure Resource Group, another "logical container" used to organize Azure Assets of all kinds.

1. Follow the instructions in the following link to Azure Documentation to create all the above Azure Assets.  Make sure you follow the instructions step by step and create all the necessary assets with the names as specified below.  If you do not do this, the code sample will not work. https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quickstart-topics-subscriptions-portal

Required Naming for the Azure Assets, including the required case:
* Use any name for the Resource Group and Service Bus Namespace.
* The Topic MUST be named "**svcADemoEvent1**". This name is "hard coded" in the solution's code.  You'll find it in the "NamesOfQueuesNPubSubs.cs" file located in Common.SharedConstantsNStrings.
* The Subscription MUST be named "**ServiceB**". This name comes from the dapr run command, namely the --app-id argument, i.e. --app-id ServiceB, that is in the StartRunServicesAndUI.txt file.

2. Copy the Azure Service Bus Namespace "connection string" from the Azure Portal and paste it into the svs-pubsub-asbtopic.yaml that resides in  DaprRuntimePathPrefix\.dapr\components.  Here is how to do that:
* In the Azure Portal navigate to your Azure Service Bus Namespace.
* Then click on Settings/Shared Access Policies.
* Then click on RootManageSharedAccessKey which will display the Primary and Secondary Key and Primary and Secondary Connection Strings.
* Click on the Copy icon of the Primary Connection String to make a copy of the connection string.  
* Then paste the copy of the connecction string into some temporary local text file (NoteBook works well for this) on your local dev system.
* Open your local DaprRuntimePathPrefix\\.dapr\components\svs-pubsub-asbtopic.yaml in a text editor.  You can right click on the yaml file and choose Edit.
* Paste the Primary Connection String that you copied from the Azure Portal into the local DaprRuntimePathPrefix\.dapr\components\svs-pubsub-asbtopic.yaml file as follows:
  * Replace the contents of the field labeled "value" as shown below:

  metadata:
  
   \- name: connectionString 
   
     value: Endpoint=sb://YourNamespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=YourSharedAccessKey  
  
  * Above, paste so as to replace all to the right of "value: " with your connection string, leaving 1 space between ':' and the connection string.
  
* Save the DaprRuntimePathPrefix\\.dapr\components\svs-pubsub-asbtopic.yaml file.

* Now run the yaml file through a yaml validator.  For example copy the yaml and then paste it into https://codebeautify.org/yaml-validator.  If there are errors the validator will tell you.  This saves much time compared to trying to debug incorrect yaml via running code.  When the yaml is validated copy the yaml from the validator back into the yaml file and then save it.
  * Note that from my experience a >= 1 space indent is required for each level of nesting in the yaml.  Without this even yaml that has been validated OK with the above yaml validator will still cause runtime errors.  Also use ONLY SPACES, and not tabs for indents.  Tabs cause errors.

You are done with the Azure Assets step!

**All done with InstallMe!  To run the code see RunMe.MD**
