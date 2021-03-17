// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Copyright © 2021 Solid Value Software, LLC

using System;
using System.Threading.Tasks;
using ConfigHelpers;
using Grpc.Net.Client;
using ServiceA;
using ServiceHelpers;

namespace QuickTestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // The below USING disposes of the channel when execution goes outside the current set of curly braces.
            using GrpcChannel serviceAChannel = GrpcChannelFactory.MakeGrpcChannel(GrpcSettings.ServiceAClientHttpAddressString);
            
            // Make the protobuf client side proxy to allow calling protobuf rpc methods on ServiceADemo.
            SvcADemo.SvcADemoClient serviceADemoProxy = new SvcADemo.SvcADemoClient(serviceAChannel);

            bool keepRunning = true;
            while (keepRunning)
            {
                Console.WriteLine("Input a-bft arg1 arg2 ENTER to execute ServiceA.DoBasicFunctionalTestAsync(message).");
                Console.WriteLine("Or the following cmds: a-pubevent, a-pubeventmulti, others TBD.");
                Console.WriteLine("     OR Press ENTER to quit.");

                string userInput = Console.ReadLine();
                if (string.IsNullOrEmpty(userInput))
                {
                    keepRunning = false;
                }

                if (keepRunning)
                {
                    SvcAPublishEventRequest request;
                    SvcAStringReply pubEventReply = new SvcAStringReply();
                    pubEventReply.Message = "No results returned from the service!  Input Error?";
                    ParsedUserInput cmdNArgs = ParseUserInput(userInput);
                    string resultsMsg;
                    switch (cmdNArgs.Cmd)
                    {
                        // Run a "Basic Functional Test" to verify ServiceA is operational.
                        case "a-bft":
                            SvcABftRequest svcABftRequest =
                                new SvcABftRequest
                                {
                                    TestData1 = $"SvcADemo.SvcADemoClient request: Cmd={cmdNArgs.Cmd} Arg1={cmdNArgs.Arg1}, Arg2={cmdNArgs.Arg2}.",
                                    TestData2 = $"Test data, test data, test data, ...."
                                };
                            SvcABftReply svcABftReply = await serviceADemoProxy.DoBasicFunctionalTestAsync(svcABftRequest);

                            resultsMsg = $"SvcADemo.DoBasicFunctionalTestAsync() response =\n{svcABftReply.Message}\n";
                            Console.WriteLine(resultsMsg);
                            break;

                        // Publish a single event.
                        case "a-pubevent": // Usage: a-pubevent  pubsubkind  pubsubname  topicname  somepayloadstring

                            // Example:   a-pubevent netcodeconnstr pubsub svcADemoEvents1 payload=tttttt

                            // The only pubsubkind supported in this sample is daprpubsub
                            // The only pubsubname supported in this sample is svs-pubsub-asbtopic, although it is easy
                            // for YOU to add the code to ServiceB to support the default dapr redis pubsubname = pubsub.
                            
                            request = new SvcAPublishEventRequest();
                            request.PubSubKind = cmdNArgs.Arg1;
                            request.PubSubName = cmdNArgs.Arg2;
                            request.TopicName = cmdNArgs.Arg3;
                            request.EventPayload = cmdNArgs.Arg4;
                            
                            // TODO -- ALWAYS USE the ASYNC version of serviceAProxy.PublishEventViaDapr as shown below.
                            // TODO -- Using the non-async version results in GetAwaiter() error.
                            pubEventReply = await serviceADemoProxy.PublishEventAsync(request);

                            resultsMsg = $"Service response =\n  {pubEventReply.Message}\n";
                            Console.WriteLine(resultsMsg);
                            break;
                            
                        // Publish a stream of multiple events
                        case "a-pubeventmulti": 
                            
                            // Usage: a-pubeventmulti  pubsubkind  pubsubname  topicname  somepayloadstring nevents delaymsec

                            // Example:   a-pubeventmulti daprpubsub svs-pubsub-asbtopic svcADemoEvents1 payload=tttttt 100 20
                            // Above example will send 100 events (nevents = 100) with a time delay of 20 millisec between
                            // each send (delaymsec = 20);

                            // The only pubsubkind supported in this sample is daprpubsub
                            // The only pubsubname supported in this sample is svs-pubsub-asbtopic, although it is easy
                            // for YOU to add the code to ServiceB to support the default dapr redis pubsubname = pubsub.
                            
                            request = new SvcAPublishEventRequest();
                            request.PubSubKind = cmdNArgs.Arg1;
                            request.PubSubName = cmdNArgs.Arg2;
                            request.TopicName = cmdNArgs.Arg3;

                            int nEvents;
                            bool isParsed = Int32.TryParse(cmdNArgs.Arg5, out nEvents);
                            if (!isParsed)
                            {
                                Console.WriteLine("\n** ERROR: Input value 'nevents' is <= 0. Please try again.");
                            }
                            int delayMsec;
                            Int32.TryParse(cmdNArgs.Arg6, out delayMsec);

                            for (int i = 1; i < nEvents+1; i++)
                            {
                                request.EventPayload = ComposeEventPayload(i, cmdNArgs.Arg4);
                                
                                pubEventReply = await serviceADemoProxy.PublishEventAsync(request);

                                Console.WriteLine($"Sent Message number {i} of {nEvents} messages to send.\n\t\t\t\t\tMsg = {request.EventPayload}");
                                
                                // Don't clutter up the display unless there are errors.
                                if (pubEventReply.Message.Contains("Error"))
                                {
                                    resultsMsg = $"Service response = {pubEventReply.Message}\n";
                                    Console.WriteLine(resultsMsg);
                                }
                                await Task.Delay(delayMsec);
                            }
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
    }
}
