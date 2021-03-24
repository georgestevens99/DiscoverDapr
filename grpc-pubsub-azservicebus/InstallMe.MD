# grpc-pubsub-azservicebus Installation Instructions
This will be written on Wed - Th 3/24/21 - 3/25/21.

## Prerequisites
Three prerequisites need to be fulfilled to use this code sample.
1. The free Microsoft Visual Studio Community IDE (integrated development environment) needs to be installed on *each machine* that will run one of the services in the code sample.

2. You need to have a Microsoft Azure cloud subscription.  Free introductory subscriptions are available.

3. Dapr needs to be installed on *each machine* that will run one of the services in the code sample. The code sample will work with any version of Dapr that is compatible with v1.0

**Visual Studio Community** -- Get the free Visual Studio Community IDE at this link:  https://visualstudio.microsoft.com/free-developer-offers/
* Note that you must **get Visual Studio Community and NOT Visual Studio Code**.  It takes a substantial amount of work to get the code sample to run using Visual Studio Code.  Don't bother!
  
**Azure subscription** -- Get a free Azure subscription at one of these links:
1. For Developers -- This subscription has some extra developer oriented features: https://visualstudio.microsoft.com/dev-essentials/
2. For general use: https://azure.microsoft.com/en-us/free/ 
3. For Students -- This subscription does NOT require a credit card, unlike the 2 above.  But you must prove you are a student: https://azure.microsoft.com/en-us/free/students/

**Dapr** -- Install Dapr on *each machine* that runs one of the services in the code sample by following the instructions at this link: https://docs.dapr.io/getting-started/
* You will only need to do the first 2 steps, i.e. 1. Install the Cli and 2. Initialize Dapr.  
* The code sample does NOT require that Docker be installed.  Therefore you can use the command **dapr init --slim** rather than the standard **dapr init** command.  However, if you want to explore the full features of Dapr beyond just this code sample, go ahead and follow the instructions for initializing for use WITH Docker.
1. Install the Cli:  https://docs.dapr.io/getting-started/install-dapr-cli/
2. Install Dapr with or without Docker: 
    * Initialize Dapr for use WITHOUT Docker:  https://docs.dapr.io/operations/hosting/self-hosted/self-hosted-no-docker/
    * OR
    * Initialize Dapr for use WITH Docker (the normal configuration):  https://docs.dapr.io/getting-started/install-dapr-selfhost/  
      * If you are using Windows for your development system I recommend installing WSL2 Backend (from Microsoft) that really smooths the process of using Docker.  WSL means Windows System for Linux.  It emulates a Linux machine to make Docker work smoother.  See the Docker Desktop for Windows documentation at https://docs.docker.com/docker-for-windows/install/ for links on how to install WSL2. 

## Deploy Assets
Now that you have all the prerequisites installed you need to obtain and setup the code and scripts for the grpc-pubsub-azservicebus code.
1. Clone this repo (https://github.com/georgestevens99/DiscoverDapr) to the machine (your "test" machine) upon which you intend to run this code sample.
    * Start Visual Studio Community on your "test" machine.
      * In the Visual Studio start up menu click on the "Clone a repository" selection.  Then fill out the input boxes on the screen.  Use https://github.com/georgestevens99/DiscoverDapr for the "Repository location" input box.
      * It may take a few minutes for the GitHub repo to be downloaded to your machine.
      * When the download is complete, in Visual Studio (VS) open the grpc-pubsub-azservicebus.sln.  Double clicking on it usually works.
      * Once the solution is open then build the solution by clicking on Rebuild in the Build menu in the menubar at the top of the VS display.
      * The build should work, and if it does then you can proceed with the remainder of these installation instructions.
        * If the build has errors then you must fix them before proceeding.  Try asking on Dapr Discord at https://discord.com/invite/ptHhX6jc34
        *  
2. Deploy repo files to required places outside the repo

  
## Create Azure Assets
## Run the Code


