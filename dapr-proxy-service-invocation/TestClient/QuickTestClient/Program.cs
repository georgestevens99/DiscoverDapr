using ExploreDapr.SDK.WebAPI1HttpProxy;
using ExploreDapr.Shared.CSharpDTOs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Copyright © 2022 Solid Value Software, LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
namespace QuickTestClient
{
    internal class Program
    { 
        // 12/19/22 All below started from the grpc-pubsub-azservicebus in the DiscoverDapr repo
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

        // 12/20/22  TODO Push down into Startup.cs.  Or push the QTC code down into a class.

        // Below code idea came from
        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-7.0#use-ihttpclientfactory-in-a-console-app

            var host = new HostBuilder()
                .ConfigureLogging((hostContext, logging) =>
                    {
                        // 12-23-22  TODO update below from the original 3.1 to .NET 6+
                        //// 7/28/21 Below prevents Info and Trace logs from HttpClient from cluttering up console.
                        //logging..ClearProviders();
                        //// 7/28/21 Below makes no difference.  Info and Trace logs get displayed on console.
                        //logging.SetMinimumLevel(LogLevel.Error);
                    })
                .ConfigureServices(services => 
                    {
                        services.AddHttpClient();
                        services.AddTransient<WebAPI1TestsControllerProxy>();
                    })
                .Build();

            // 12/23/22 Below commented out lines are for direct testing of ServiceA via native gRPC rather than Dapr.
            // The below USING disposes of the channel when execution goes outside the current set of curly braces.
            //using GrpcChannel serviceAChannel = GrpcChannelFactory.MakeGrpcChannel(GrpcSettings.ServiceAClientHttpAddressString);

            // Make the protobuf client side proxy to allow calling protobuf rpc methods on ServiceADemo.
            //SvcADemo.SvcADemoClient serviceADemoProxy = new SvcADemo.SvcADemoClient(serviceAChannel);                        
            //ServiceA.ServiceAClient serviceAProxy = new ServiceA.ServiceAClient();  // This needs a grpc channel.

            bool keepRunning = true;
            while (keepRunning)
            {
                Console.WriteLine("Input w-echo testscontroller a-message nIterations delaySec ENTER to execute WebAPI1.EchoMsg().");
                Console.WriteLine("OR the following cmds: w-echo ServiceA...., and others TBD.");
                Console.WriteLine("OR the following cmds: a-pubevent, a-pubeventmulti, others TBD.");
                Console.WriteLine("     OR Press ENTER to quit.");

                string? userInput = Console.ReadLine();
                if (string.IsNullOrEmpty(userInput))
                {
                    keepRunning = false;
                }

                if (keepRunning)
                {
                    string resultsMsg;
                    ParsedUserInput cmdNArgs = ParseUserInput(userInput);

                    // 12/23/22 Commands supported.  Args 3,4, and 5 are optional.

                    //  Cmd   Arg1       Arg2                         Arg3                        Arg4                        Arg5

                    //w-echo testscontroller user-supplied-message  user-supplied-niterations   user-supplied-delay-milliSec  throw

                    //w-echo ServiceA user-supplied-message  user-supplied-niterations   user-supplied-delay-milliSec         throw

                    //w-echo ServiceB user-supplied-message  user-supplied-niterations   user-supplied-delay-milliSec         throw

                    int nIterations;
                    int delayMilliSec;

                    switch (cmdNArgs.Cmd)
                    {
                        // Run a basic test to verify the service specified in Arg1 is operational.
                        // This has the WebAPI1 (hence the "w-" prefix) invoke the specified service, i.e. a controller
                        // within the WebAPI1, or another service via Dapr Service Invocation (Dapr SI).
                        case "w-echo":
                            nIterations = MakeIntegerArgValue(cmdNArgs.Arg3, 1);
                            delayMilliSec = MakeIntegerArgValue(cmdNArgs.Arg4, 0);
                            EchoRequestDto request;
                            
                            WebAPI1TestsControllerProxy webAPI1 = 
                                                host.Services.GetRequiredService<WebAPI1TestsControllerProxy>();
                            switch (cmdNArgs.Arg1)
                            { 
                                case "testscontroller":
                                    request = new EchoRequestDto
                                    {
                                        DataToEcho = cmdNArgs.Arg2,
                                        ThrowException = cmdNArgs.Arg5 //"throw" or anything else
                                    };                                    

                                    for (int i = 1; i < nIterations + 1; i++)
                                    {
                                        EchoResponseDto response = await webAPI1.EchoMessageAsync(request);

                                        DisplayCurrentIterationNumber(nIterations, i);
                                        resultsMsg = $"webAPI1:  ResponseMsg = {response.ResponseMsg}\n";
                                        resultsMsg += $"ResponseDetails = \n\t{response.ResponseDetails}\n";
                                        Console.WriteLine(resultsMsg);
                                        await Task.Delay(delayMilliSec);
                                    }
                                    Console.WriteLine($"***Done with user input={userInput}\n");  
                                    break;

                                case "ServiceA":
                                    request = new EchoRequestDto
                                    {
                                        DataToEcho = cmdNArgs.Arg2,
                                        ThrowException = cmdNArgs.Arg5 //"throw" or anything else
                                    };
                                    
                                    for (int i = 1; i < nIterations + 1; i++)
                                    {                                        
                                        EchoResponseDto response = await webAPI1.EchoMessageOnServiceAAsync(request);
                                        
                                        DisplayCurrentIterationNumber(nIterations, i);
                                        resultsMsg = $"webAPI1:  ResponseMsg = {response.ResponseMsg}\n";
                                        resultsMsg += $"ResponseDetails = \n\t{response.ResponseDetails}\n";
                                        Console.WriteLine(resultsMsg);
                                        await Task.Delay(delayMilliSec);
                                    }
                                    Console.WriteLine($"***Done with user input={userInput}\n");
                                    break;

                                default:
                                    Console.WriteLine($"Unrecognized 'service to call' arg. cmd. userInput = {userInput}. Try again!\n");
                                    break;
                            }// End switch.

                            break; 

                        default:
                            Console.WriteLine($"Unrecognized cmd. userInput = {userInput}. Try again!");
                            break;
                    } // End switch.
                }
                else
                {
                    Console.WriteLine($"QuickTestClient quitting on user pressing Enter with no other input.");
                }
            } // End while keep running.

            Console.WriteLine("QuickTestClient EXITING!");
            await Task.Delay(1000);
        }

        private static string ComposeEventPayload(int i, string uiEventPayload)
        {
            string senderSequenceNumber = i.ToString("D9");
            string adjustedEventPayload = senderSequenceNumber + ", " + uiEventPayload;
            return adjustedEventPayload;
        }

        private class ParsedUserInput
        {
            public string Cmd { get; set; } = string.Empty;
            public string Arg1 { get; set; } = string.Empty;
            public string Arg2 { get; set; } = string.Empty;
            public string Arg3 { get; set; } = string.Empty;
            public string Arg4 { get; set; } = string.Empty;
            public string Arg5 { get; set; } = string.Empty;
            public string Arg6 { get; set; } = string.Empty;
        }

        private static ParsedUserInput ParseUserInput(string userInput)
        {
            string[] inputItems = userInput.Split(' ');

            ParsedUserInput cmdNArgs = new ParsedUserInput();
            try
            {
                cmdNArgs.Cmd = inputItems[0];
                cmdNArgs.Arg1 = inputItems[1];
                cmdNArgs.Arg2 = inputItems[2];
                cmdNArgs.Arg3 = inputItems[3];
                cmdNArgs.Arg4 = inputItems[4];
                cmdNArgs.Arg5 = inputItems[5];
                cmdNArgs.Arg6 = inputItems[6];
            }
            catch (Exception ex)
            {
            }
            return cmdNArgs;
        }

        private static int MakeIntegerArgValue(string uICommandArg, int parseFailureReturnValue)
        {
            if (!Int32.TryParse(uICommandArg, out int returnValue))
            {
                returnValue = parseFailureReturnValue;
            }
            return returnValue;
        }

        private static void DisplayCurrentIterationNumber(int nIterations, int i)
        {
            Console.WriteLine($"QTC6: Ran command # {i.ToString("D9")} of {nIterations.ToString("D9")} commands to execute.");
        }
    }
}