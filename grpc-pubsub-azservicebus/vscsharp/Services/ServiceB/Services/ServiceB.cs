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

using Dapr.AppCallback.Autogen.Grpc.v1;
using Dapr.Client;
using Dapr.Client.Autogen.Grpc.v1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ServiceHelpers;
using SharedConstantsNStrings;
using System;
using System.Threading.Tasks;
using Google.Protobuf;

namespace ServiceB.Services
{
    public class ServiceB : AppCallback.AppCallbackBase
    {
        private readonly ILogger<ServiceB> m_Logger;
        private readonly DaprClient m_DaprProxy;

        public ServiceB(ILogger<ServiceB> logger)
        {
            m_Logger = logger;
            m_DaprProxy = DaprClientProxyFactory.MakeDaprProxy();
        }

        // FYI Here where ServiceB tells Dapr which Pubsub subscriptions it can handle.
        public override Task<ListTopicSubscriptionsResponse> ListTopicSubscriptions(Empty request, ServerCallContext context)
        {
            var result = new ListTopicSubscriptionsResponse();
            try
            {
                TopicSubscription subscr;

                subscr = new TopicSubscription
                {
                    PubsubName = NamesOfQueuesNPubSubs.PubSubAzServiceBusComponent,
                    Topic = NamesOfQueuesNPubSubs.ServiceADemoEvent1Topic
                };
                result.Subscriptions.Add(subscr);
                // For display in a running demo.
                Console.WriteLine($" ** ServiceB: Subscribed to Topic={subscr.Topic} for PubsubName={subscr.PubsubName}");


                // For Dapr default Redis pubsub component.  Also needs event processing code in OnTopicEvent() below.
                subscr = new TopicSubscription()  
                {
                    PubsubName = NamesOfQueuesNPubSubs.PubSubDefaultDaprComponent,
                    Topic = NamesOfQueuesNPubSubs.ServiceADemoEvent1Topic
                };
                result.Subscriptions.Add(subscr);
                // For display in a running demo.
                Console.WriteLine($" ** ServiceB: Subscribed to Topic={subscr.Topic} for PubsubName={subscr.PubsubName}");
            }
            catch (Exception ex)
            {
                m_Logger.LogError($"** ServiceB.ListTopicSubscriptions() threw exception ex={ex}");
                throw;
            }

            // TODO If required, add subscription for NamesOfQueuesNPubSubs.ServiceADemoEvents2Topic

            return Task.FromResult(result);
        }

        public override async Task<TopicEventResponse> OnTopicEvent(TopicEventRequest request, ServerCallContext context)
        {
            m_Logger.LogInformation($" ** ServiceB: Topic dispatcher entered. Topic={request.Topic}, PubsubName={request.PubsubName}");

            switch (request.Topic)
            {
                case NamesOfQueuesNPubSubs.ServiceADemoEvent1Topic:
                    try
                    {
                        string payloadString = request.Data.ToStringUtf8();
                        m_Logger.LogInformation($" ** ServiceB: {request.Topic} subscr. Dispatching event containing: {payloadString}.");
                        
                        // For display in a running demo.
                        Console.WriteLine($" ** ServiceB: {request.Topic} subscr. Dispatching event containing: {payloadString}");
                        
                        // Below, ServiceB processes the event locally here in ServiceB.
                        bool isSuccess = await ProcessServiceADemoEvent1(payloadString);

                        
                        // ANOTHER OPTION -- ServiceB could also use Dapr SERVICE INVOCATION to call some other
                        // service within this Dapr-Mesh to process this event, rather than process the event in
                        // THIS ServiceB.  This is sketched below but NOT IMPLEMENTED so as to keep the code minimal.

                        //HelloRequest helloRequest = new HelloRequest();
                        //await m_DaprProxy.InvokeMethodGrpcAsync<HelloRequest>("SomeService", "SomeServiceOp", helloRequest);

                    }
                    catch (Exception ex)
                    {
                        m_Logger.LogError($" ** ServiceB: Exception when dispatching event for Topic={request.Topic}. ex = {ex}");
                        throw;
                    }
                    break;

                // TODO If required, add subscription for NamesOfQueuesNPubSubs.ServiceADemoEvents2Topic and
                // process the events below.
                case NamesOfQueuesNPubSubs.ServiceADemoEvent2Topic:
                    try
                    {
                        string payloadString = request.Data.ToStringUtf8();
                        m_Logger.LogInformation($" ** ServiceB: {request.Topic} subscr. Dispatching event containing: {payloadString}.");

                        // For display in a running demo.
                        Console.WriteLine($" ** ServiceB: {request.Topic} subscr. Dispatching event containing: {payloadString}");
                    }
                    catch (Exception ex)
                    {
                        m_Logger.LogError($" ** ServiceB: Exception when dispatching event for Topic={request.Topic}. ex = {ex}");
                        throw;
                    }
                    break;

                default:
                    m_Logger.LogError($" ** ServiceB: ERROR. Received event for unsupported Topic={request.Topic}.");
                    break;
            }

            // TODO Determine the circumstances required to return Retry or Drop.
            TopicEventResponse topicResponse = new TopicEventResponse();
            // The other enum members are Retry, Drop.
            topicResponse.Status = TopicEventResponse.Types.TopicEventResponseStatus.Success;

            return await Task.FromResult(topicResponse);

            // More Info -- From Dapr SDK appcallback.proto
            /*
             // TopicEventResponse is response from app on published message
            message TopicEventResponse {
             // TopicEventResponseStatus allows apps to have finer control over handling of the message.
                enum TopicEventResponseStatus {
                    // SUCCESS is the default behavior: message is acknowledged and not retried or logged.
                    SUCCESS = 0;
                    // RETRY status signals Dapr to retry the message as part of an expected scenario (no warning is logged).
                    RETRY = 1;
                    // DROP status signals Dapr to drop the message as part of an unexpected scenario (warning is logged).
                    DROP = 2;
              //  }
             */
            //return base.OnTopicEvent(request, context);
        }

        // The ServiceB handler for the ServiceADemoEvent1.  Note this code could be in a separate
        // ServiceBImplementation class (but with a nicer name).
        private async Task<bool> ProcessServiceADemoEvent1(string eventPayload)
        {
            // NOTE -- One can use a longer delay here to cause the number of items in the Topic to increase
            // by making the delay here longer than the delay between sending messages in the Quick Test Client
            const int DelayMillisec = 10;
            await Task.Delay(DelayMillisec);

            return true;
        }

        // FYI Here is the top level handler for incoming Service Invocation requests.
        public override Task<InvokeResponse> OnInvoke(InvokeRequest request, ServerCallContext context)
        {
            return base.OnInvoke(request, context);
        }

        // FYI Here where ServiceB tells Dapr which Binding Events it can handle.
        public override Task<ListInputBindingsResponse> ListInputBindings(Empty request, ServerCallContext context)
        {
            return base.ListInputBindings(request, context);
        }
        // FYI Here is the top level handler for incoming Binding Events from Dapr Binding and Trigger Components.
        public override Task<BindingEventResponse> OnBindingEvent(BindingEventRequest request, ServerCallContext context)
        {
            return base.OnBindingEvent(request, context);
        }
    }
}
