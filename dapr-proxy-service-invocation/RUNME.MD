## How to Run the Code
Completed 1/2/23.

You will need to run all 3 of the process at once to use this demo.  That is WebAPI1, ServiceA, and the QuickTestClient.  Thus you will need a Command Window for each of the 2 services (WebAPI1 and ServiceA) in order to run them in the appropriate directory using the Dapr Run command that supplies a Dapr sidecar to the service being run.  On the other hand, the Quick Test Client supplies its own Command Window when it is started since it is a Console App.

Please see the document StartAndRunServices.txt in the RunSelfHosted solution folder for instructions on how to start each of the above services. This text file contains the text for the Command Window commands required to change to the proper directory for a service's code, and then run the service via Dapr Run.  Just copy the command text from StartAndRunServices.txt and then paste it into the appropriate service's Command Window

The StartAndRunServices.txt document also states how to start the QuickTestClient, plus contains a couple of commands you can input to the QuickTestClient UI to run tests.

Please see the QuickTestClient code for the meanings of each command and its arguments.



I've found that the Windows Terminal Command Prompt window is much, much more efficient to use that the Windows Command Prompt window. Here is the link to install Windows Terminal for free: https://www.microsoft.com/en-us/p/windows-terminal/9n0dx20hk701?activetab=pivot:overviewtab

Finally, be sure to try out the QuickTestClient's "n-Iterations" argument" to load test and stress test the system.  Try running n-iterations of thousands or tens of thousands iterations.  You will be amazed at how fast it is.  And try running thousands or even tens of thousands of events with a 0 millisecond delay.  Do these break your system?  How long does it take the system as a whole, end-to-end, to process 1000 events, i.e. the throughput?  Doing load and stress testing will both show you where you need to strengthen the software, and build your confidence in it as well.

Good luck with your exploration of Dapr, ASP.NET Core gRPC servcies, and the Azure Service Bus.  If you get stuck try asking a question on Dapr Discord at https://discord.com/invite/ptHhX6jc34.
