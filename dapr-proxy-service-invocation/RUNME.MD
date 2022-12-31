## How to Run the Code

You will need to run all 3 of the process at once to use this demo.  That is WebAPI1, ServiceA, and the QuickTestclient.

Please see the document StartAndRunServices.txt in the RunSelfHosted solution folder for instructions on how to start each of the above services.

The above document also contains a couple of commands you can input to the QuickTestClient UI to run tests.

Please see the QuickTestClient code for the meanings of each command and its arguments.



I've found that the Windows Terminal Command Prompt window is much, much more efficient to use that the Windows Command Prompt window. Here is the link to install Windows Terminal for free: https://www.microsoft.com/en-us/p/windows-terminal/9n0dx20hk701?activetab=pivot:overviewtab

Finally, be sure to use the QuickTestClient's "n-Iterations" argument" to load test and stress test the system.  Try running n-iterations of thousands or tens of thousands iterations.  You will be amazed at how fast it is.  And try running thousands or even tens of thousands of events with a 0 millisecond delay.  Do these break your system?  Do these cause events to "backup" in your Azure Service Bus Topic when its "Message Count" is viewed in the portal. How long does it take the system as a whole, end-to-end, to process 1000 events, i.e. the throughput?  Doing load and stress testing will both show you where you need to strengthen the software, and build your confidence in it as well.

Good luck with your exploration of Dapr, ASP.NET Core gRPC servcies, and the Azure Service Bus.  If you get stuck try asking a question on Dapr Discord at https://discord.com/invite/ptHhX6jc34.

