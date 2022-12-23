using Ardalis.GuardClauses;
using Grpc.Core;

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
namespace ExploreDapr.Services.ServiceA.Services
{
    public class ServiceATests : Tests.TestsBase
    {
        private readonly ILogger<ServiceATests> m_Logger;
        public ServiceATests(ILogger<ServiceATests> logger)
        {
            m_Logger = logger;
        }
        private readonly string m_ThisClassName = nameof(ServiceATests);
        public override Task<EchoResponse> EchoMessage(EchoRequest request, ServerCallContext context)
        {
            string classNameDotMethod = m_ThisClassName + "." + nameof(EchoMessage);
            EchoResponse response = new EchoResponse();

            // Test uncaught exceptions.  They need to display nicely in the UI.
            //throw new ArgumentOutOfRangeException("Testing uncaught exceptions.");
            try
            {
                m_Logger.LogInformation($"***{classNameDotMethod}(): ENTERED.");

                Guard.Against.Null(request, nameof(request));
                Guard.Against.Null(request.DataToEcho, nameof(request.DataToEcho));

                response.ResponseMsg = "'dataToEcho' = : " + request.DataToEcho;
                response.ResponseDetails = $"{classNameDotMethod} all OK.";

                if (request.ThrowException == "throwInA")
                {
                    throw new NullReferenceException($"{classNameDotMethod}(): ThrowException found in request Dto");
                }
            }
            catch (Exception ex)
            {
                response.ResponseDetails = $"***Error: {classNameDotMethod} had exception. Msg={ex.Message}";
                m_Logger.LogWarning(response.ResponseDetails);
            }

            return Task.FromResult(response);
        }
    }
}
