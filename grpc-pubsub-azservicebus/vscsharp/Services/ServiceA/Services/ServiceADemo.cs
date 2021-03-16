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

using Dapr.Client;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ServiceHelpers;
using SharedConstantsNStrings;
using System;
using System.Threading.Tasks;

namespace ServiceA.Services
{
    public class ServiceADemo : SvcADemo.SvcADemoBase
    {
        private readonly DaprClient m_DaprProxy;
        private readonly ILogger<ServiceADemo> m_Logger;

        public ServiceADemo(ILogger<ServiceADemo> logger)
        {
            m_Logger = logger;
            m_DaprProxy = DaprClientProxyFactory.MakeDaprProxy();
        }

        #region Service Operations for ServiceADemo, aka the ServiceADemo API
        public override Task<SvcABftReply> DoBasicFunctionalTest(SvcABftRequest request, ServerCallContext context)
        {
            m_Logger.LogInformation("** ServiceADemo.DoBasicFunctionalTest() entered.");
            SvcABftReply reply =
                new SvcABftReply
                {
                    Message = $"\tHello from ServiceADemo.DoBasicFunctionalTest():\n\tTestData1 = {request.TestData1}\n\tTestData2 = {request.TestData2}"
                };
            m_Logger.LogInformation("** ServiceADemo.DoBasicFunctionalTest() returning.");
            
            return Task.FromResult(reply);
        }

        public override async Task<SvcAStringReply> PublishEvent(SvcAPublishEventRequest request, ServerCallContext context)
        {
            SvcAStringReply reply;
            switch (request.PubSubKind)
            {
                case NamesOfQueuesNPubSubs.DaprPubSubKind:
                    reply = await PublishEventViaDapr(request);
                    break;

                default:
                    reply = new SvcAStringReply();
                    reply.Message = $"** ServiceADemo.PublishEvent(): Error -- Cannot publish. Unknown request. PubSubKind = {request.PubSubKind}";
                    break;
            }
            return reply;
        }
        #endregion Service Operations for ServiceADemo, aka the ServiceADemo API
        

        #region Publish to Topic using Dapr

        private async Task<SvcAStringReply> PublishEventViaDapr(SvcAPublishEventRequest request)
        {
            SvcAStringReply reply = new SvcAStringReply();
            reply.Message = "** ServiceADemo.PublishEventViaDapr(): Publish succeeded.";

            try
            {
                m_Logger.LogInformation(
                    $"** ServiceADemo.PublishEventViaDapr(): PubsubName={request.PubSubName}, TopicName={request.TopicName}, Payload={request.EventPayload}");

                // For display in a running demo.
                Console.WriteLine(
                    $"** ServiceADemo publish to Topic={request.TopicName}:  {request.EventPayload}");

                await m_DaprProxy.PublishEventAsync<string>(request.PubSubName, request.TopicName, request.EventPayload);
            }
            catch (System.Exception ex)
            {
                reply.Message = $"** ServiceADemo.PublishEventViaDapr() Error -- Caught exception = {ex}";
            }

            // For error detection in a running  demo.
            if (reply.Message.Contains("Error"))
            {
                Console.WriteLine(reply.Message);
            }
            return reply;
        }

        #endregion Publish to Topic using Dapr
    }
}
