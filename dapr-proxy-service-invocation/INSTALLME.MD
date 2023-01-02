# dapr-proxy-service-invocation Installation Instructions
Completed 1/2/23.

## Prerequisites
You need to fulfill 3 prerequisites to use this code sample.
1. Microsoft Visual Studio IDE (integrated development environment) needs to be **installed on *your "test" machine*** that will run the services in the code sample.  The Microsoft **Visual Studio Community** edition is free.

2. Docker Desktop needs to be **installed on *your "test" machine*** to support the Dapr container and others as well.  Docker is free for individuals and small organizations.

3. **Dapr** needs to be **installed on *your "test" machine*** that will run the services in the code sample. The code sample will work with any version of Dapr that is compatible with v1.9.2 Dapr runtime and v1.9.0 Dapr CLI.  Dapr is free of charge.

**Visual Studio Community** -- Get the free Visual Studio Community IDE at this link:  https://visualstudio.microsoft.com/free-developer-offers/
* Note that you must **get Visual Studio Community and NOT Visual Studio Code**.  It takes a substantial amount of work to get the code sample to run using Visual Studio Code.  Don't bother now. Rather save VS Code for later if you want to use it.

**Docker Desktop** --  Install the free version of Docker, including Docker Desktop, on **your "test" machine** that runs the services in the code sample.  Just follow the instructions at https://docs.docker.com/get-docker/

**Dapr** -- Install Dapr on **your "test" machine** that runs the services in the code sample by following the instructions at this link: https://docs.dapr.io/getting-started/
* You will only need to do the first 2 steps, i.e. 1. Install the Cli and 2. Initialize Dapr.  
* The code sample requires that Docker be installed.  Therefore DO NOT use the command **dapr init --slim**, rather **use the standard dapr init command instead**.  
    
To install Dapr:
1. Install the Dapr Cli (command language interface):  https://docs.dapr.io/getting-started/install-dapr-cli/
2. Install the Dapr Runtime (i.e. the executable) as follows:
    * Initialize Dapr for use WITH Docker (the normal configuration):  https://docs.dapr.io/getting-started/install-dapr-selfhost/
    * If you are using Windows for your "test" system I recommend installing WSL2 Backend (from Microsoft) that really smooths the process of using Docker.  WSL means Windows System for Linux.  It emulates a Linux machine to make Docker work smoother.  See the Docker Desktop for Windows documentation at https://docs.docker.com/docker-for-windows/install/ for links on how to install WSL2. With WSL2 you **do not** need to install a Linux distro in order to run services using Dapr!  Having only WSL2 and Docker is all you need.

You are done with the prequisites step!

## Get and Deploy GitHub Repo Assets
Now that you have all the prerequisites installed you need to obtain and setup the assets (the code, files, scripts, etc) for the dapr-proxy-service-invocation VS solution from the GitHub repository.
1. **Copy the VS solution from GitHub to your "test" machine** -- Clone this repo (https://github.com/georgestevens99/DiscoverDapr) to the machine (your "test" machine) upon which you intend to run this code sample, as follows:
    * Start Visual Studio IDE on your "test" machine.
    * Download the repo, i.e. Clone it from GitHub to your "test" machine.
      * In the Visual Studio start up menu click on the "Clone a repository" selection.  Then fill out the input boxes on the screen as follows.
        * For the "Repository location" input box use https://github.com/georgestevens99/DiscoverDapr.
        * For the "Path" input box insert the file path on your machine where you want the DiscoverDapr repo to reside. This path is know as the **LocalRepoPathPrefix** in this document so **record it somewhere**.  You will need to use this path name later.
      * It may take a few minutes for the GitHub repo to be downloaded to your machine.
    * Open the solution and build it.
      * When the download is complete, in Visual Studio (VS) open the dapr-proxy-service-invocation.sln.  Double clicking on the .sln file icon will open the solution.
      * Once the solution is open in VS, build the solution by opening the Build menu in the menu bar at the top of the display and click on Rebuild.
      * The build should work, and if it does then you can proceed with the remainder of these installation instructions.
        * If the build has errors then you must fix them before proceeding.  Try asking on Dapr Discord at https://discord.com/invite/ptHhX6jc34 if you cannot figure out by yourself how to get the solution to build properly.
       
2.**Update the LocalRepoPath path names in the StartRunServices.txt file** in Visual Studio.  Commands in this file are used when you actually manually run services in the VS solution via Command Prompt windows.
  * You can see StartRunServices.txt in the VS Solution Explorer. Open StartRunServices.txt in Visual Studio.
  * Create the "LocalRepoPath" string by concatenating the "LocalRepoPathPrefix" you saved above with the string "DiscoverDapr" and save the entire path in a temporary text file for use below.  This is the full path to the DiscoverDapr repo on your system.
  * Next, in StartRunServices.txt replace the string "LocalRepoPath"" with the actual full path string you saved above.  Replacement must happen in the 2 lines in StartRunServices.txt shown below:
    * cd **LocalRepoPath**\DiscoverDapr\dapr-proxy-service-invocation\Services\ServiceA
    * cd **LocalRepoPath**\DiscoverDapr\dapr-proxy-service-invocation\Services\ServiceB  
  * When you are done with the string replacement be sure to do a manual test to make sure the above cd commands actually work in a Command Prompt window, as follows:
    *  Open a Command Prompt window.  
      * Using the Windows Terminal Command Prompt is much, much easier to deal with so install Windows Terminal if you do not already have it!  Here is the link to install Windows Terminal for free: https://www.microsoft.com/en-us/p/windows-terminal/9n0dx20hk701?activetab=pivot:overviewtab
    *  Copy the above string containing the full path name into the Command Prompt window and press enter.  
      *  If it does not execute the "cd" command you will have to figure out why and fix it.  This "cd" command MUST work in ordder to run the services.
  *  When both of the newly adjusted "cd" commands in the StartRunServices.txt file have been proven to work without a doubt, SAVE the file.
  *  NOTE -- This technique of using copy/paste to transfer command strings from the text file into a Command Window is used throughout RUNME.MD to avoid manually typing long command strings into the Command Window and the inveitable errors.
 
You are done with the GitHub Repo Assets step!

**All done with INSTALLME!  To run the code see RUNME.MD**
