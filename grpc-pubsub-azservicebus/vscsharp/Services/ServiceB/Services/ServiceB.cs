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
using Dapr.Client.Autogen.Grpc.v1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using SharedConstantsNStrings;
using System;
using System.Threading.Tasks;

namespace ServiceB.Services
{
    public class ServiceB : AppCallback.AppCallbackBase
    {
        private readonly ILogger<ServiceB> m_Logger;

        public ServiceB(ILogger<ServiceB> logger)
        {
            m_Logger = logger;
        }

        public override Task<ListTopicSubscriptionsResponse> ListTopicSubscriptions(Empty request, ServerCallContext context)
        {
            var result = new ListTopicSubscriptionsResponse();

            try
            {
                //result.Subscriptions.Add(new TopicSubscription
                //{
                //    // Used with supplied default Redis pubsub component.
                //    PubsubName = NamesOfQueuesNPubSubs.PubSubDefaultDaprComponent,
                //    Topic = NamesOfQueuesNPubSubs.ServiceADemoEvents1Topic
                //});

                result.Subscriptions.Add(new TopicSubscription
                {
                    PubsubName = NamesOfQueuesNPubSubs.PubSubAzServiceBusComponent,
                    Topic = NamesOfQueuesNPubSubs.ServiceADemoEvents1Topic
                });

                //result.Subscriptions.Add(new TopicSubscription
                //{
                //    PubsubName = NamesOfQueuesNPubSubs.PubSubAzServiceBusEnvVarSecretsComponent,
                //    Topic = NamesOfQueuesNPubSubs.ServiceADemoEvents1Topic
                //});
            }
            catch (Exception ex)
            {
                m_Logger.LogError($"** ServiceB.ListTopicSubscriptions() threw exception ex={ex}");
                throw;
            }

            // TODO  Add subscriptions for NamesOfQueuesNPubSubs.ServiceADemoEvents2Topic

            return Task.FromResult(result);
        }

        public override async Task<TopicEventResponse> OnTopicEvent(TopicEventRequest request, ServerCallContext context)
        {
            m_Logger.LogInformation($" ** ServiceB: Topic dispatcher entered. Topic={request.Topic}, PubsubName={request.PubsubName}");

            switch (request.Topic)
            {
                case NamesOfQueuesNPubSubs.ServiceADemoEvents1Topic:
                    try
                    {
                        string payloadString = request.Data.ToStringUtf8();
                        m_Logger.LogInformation($" ** ServiceB: {NamesOfQueuesNPubSubs.ServiceADemoEvents1Topic} subscr dispatching event. Payload = {payloadString}.");
                        // Does not display on console.
                        //Console.WriteLine($" ** ServiceB: {NamesOfQueuesNPubSubs.ServiceADemoEvents1Topic} subscr dispatching event.");
                    }
                    catch (Exception ex)
                    {
                        m_Logger.LogError($" ** ServiceB: Exception when dispatching event for Topic={request.Topic}. ex = {ex}");
                        throw;
                    }
                    break;

                // TODO  Add subscriptions for NamesOfQueuesNPubSubs.ServiceADemoEvents2Topic
                case NamesOfQueuesNPubSubs.ServiceADemoEvents2Topic:
                    try
                    {
                        string payloadString = request.Data.ToStringUtf8();
                        m_Logger.LogInformation($" ** ServiceB: {NamesOfQueuesNPubSubs.ServiceADemoEvents2Topic} subscr dispatching event.subscr dispatching event. Payload = {payloadString}.");
                        // Does not display on console
                        //Console.WriteLine($" ** ServiceB: {NamesOfQueuesNPubSubs.ServiceADemoEvents2Topic} subscr dispatching event.");
                    }
                    catch (Exception ex)
                    {
                        m_Logger.LogError($" ** ServiceB: Exception when dispatching event for Topic={request.Topic}. ex = {ex}");
                        throw;
                    }
                    break;

                default:
                    m_Logger.LogError($" ** ServiceB: ERROR Received event for unsupported Topic={request.Topic}.");
                    break;
            }

            // TODO Use a switch statement.
            //if (request.PubsubName == NamesOfQueuesNPubSubs.PubSubDefaultDaprComponent)
            //{
            //    m_Logger.LogInformation($" ** ServiceB: PubsubName={request.PubsubName} event handler entered.");

            //    if (request.Topic == NamesOfQueuesNPubSubs.ServiceADemoEvents1Topic)
            //    {
            //        try
            //        {
            //            m_Logger.LogInformation($" ** ServiceB: {NamesOfQueuesNPubSubs.ServiceADemoEvents1Topic} subscr received event.");
            //        }
            //        catch (Exception ex)
            //        {
            //            m_Logger.LogError($" ** ServiceB exception while processing for {NamesOfQueuesNPubSubs.PubSubDefaultDaprComponent} threw ex = {ex}");
            //            throw;
            //        }
            //    }
            //    else
            //    { 
            //        m_Logger.LogError($" ** ServiceB: Unrecognized Topic encountered = {request.Topic}");
            //    }
            //}
            //else if (request.PubsubName == NamesOfQueuesNPubSubs.PubSubAzServiceBusComponent)
            //{
            //    m_Logger.LogInformation($" ** ServiceB: PubsubName={request.PubsubName} event handler entered.");

            //    if (request.Topic == NamesOfQueuesNPubSubs.ServiceADemoEvents1Topic)
            //    {
            //        try
            //        {
            //            m_Logger.LogInformation($" ** ServiceB: {NamesOfQueuesNPubSubs.ServiceADemoEvents1Topic} subscr received event.");
            //        }
            //        catch (Exception ex)
            //        {
            //            m_Logger.LogInformation($" ** ServiceB exception while processing for {NamesOfQueuesNPubSubs.PubSubAzServiceBusComponent} threw ex = {ex}");
            //            throw;
            //        }
            //    }
            //    else
            //    {
            //        m_Logger.LogError($" ** ServiceB: Unrecognized Topic encountered = {request.Topic}");
            //    }
            //}
            //else if (request.PubsubName == NamesOfQueuesNPubSubs.PubSubAzServiceBusEnvVarSecretsComponent)
            //{
            //    m_Logger.LogInformation($" ** ServiceB: PubsubName={request.PubsubName} event handler entered.");

            //    if (request.Topic == NamesOfQueuesNPubSubs.ServiceADemoEvents1Topic)
            //    {
            //        try
            //        {
            //            m_Logger.LogInformation($" ** ServiceB: {NamesOfQueuesNPubSubs.ServiceADemoEvents1Topic} subscr received event.");
            //        }
            //        catch (Exception ex)
            //        {
            //            m_Logger.LogError($" ** ServiceB exception while processing for {NamesOfQueuesNPubSubs.PubSubAzServiceBusEnvVarSecretsComponent} threw ex = {ex}");
            //            throw;
            //        }
            //    }
            //    else
            //    {
            //        m_Logger.LogError($" ** ServiceB: Unrecognized Topic encountered = {request.Topic}");
            //    }
            //}


            // TODO Determine the circumstances to return Retry and Drop.  Do like I did in MethodInAfDf.
            TopicEventResponse topicResponse = new TopicEventResponse();
            topicResponse.Status = TopicEventResponse.Types.TopicEventResponseStatus.Success; // Other enum members are Retry, Drop.
            
            return await Task.FromResult(topicResponse);

            // INFO Below is original code from sample. The default value of TopicEventResponse is Success.
            //return await Task.FromResult(default(TopicEventResponse));

            //From appcallback.proto
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

        public override Task<InvokeResponse> OnInvoke(InvokeRequest request, ServerCallContext context)
        {
            return base.OnInvoke(request, context);
        }

        public override Task<ListInputBindingsResponse> ListInputBindings(Empty request, ServerCallContext context)
        {
            return base.ListInputBindings(request, context);
        }

        public override Task<BindingEventResponse> OnBindingEvent(BindingEventRequest request, ServerCallContext context)
        {
            return base.OnBindingEvent(request, context);
        }
    }
}
