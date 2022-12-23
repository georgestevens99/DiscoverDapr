using Ardalis.GuardClauses;
using ExploreDapr.iFX.Configuration.ConstantsNEnums;
using ExploreDapr.Services.ServiceA;
using ExploreDapr.Services.WebAPI1.Interface;
using ExploreDapr.Shared.CSharpDTOs;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;

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
namespace ExploreDapr.Services.WebAPI1.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // 12/19/22 The "api" was added since I am using that in my AKS ingress.
    public class TestsController : Controller, ITestsController
    {
        private readonly string m_ThisClassName = nameof(TestsController);

        private const string ServiceOpCompletedOK = "ServiceOp call completed OK";

        private readonly ILogger<TestsController> m_Logger;
        public TestsController(ILogger<TestsController> logger)
        {
            m_Logger = logger;
        }        

        #region Aliveness Test of this TestController        

        [HttpGet]
        //The URL is -- http://localhost:somePort#/tests (i.e. TestsController with the last word removed.).
        // When WebAPI1 is running one can use the above URL to test that it is alive in a browser.  Quick!
        public EchoResponseDto Get()
        {
            string methodDotClassName = m_ThisClassName + "." + nameof(Get);
            m_Logger.LogInformation($"***{methodDotClassName}(): ENTERED.");

            EchoResponseDto reply = new EchoResponseDto();
            reply.ResponseMsg = $"Testing GET from {methodDotClassName}. Testing. Testing.";
            return reply;
        }
        #endregion Aliveness Test of this Test Controller   

        #region Implementation of ITestsController  
        [HttpPost(nameof(EchoMessageAsync))]
        public async Task<EchoResponseDto> EchoMessageAsync(EchoRequestDto request)
        {
            string classNameDotMethod = m_ThisClassName + "." + nameof(EchoMessageAsync);
            EchoResponseDto response = new EchoResponseDto();

            // Test uncaught exceptions.  They need to display nicely in the UI.
            //throw new ArgumentOutOfRangeException("Testing uncaught exceptions.");
            try
            {
                m_Logger.LogInformation($"***{classNameDotMethod}(): ENTERED.");

                Guard.Against.Null(request, nameof(request));
                Guard.Against.Null(request.DataToEcho, nameof(request.DataToEcho));

                string responseMsg =
                    $"\tHello from {classNameDotMethod}():" +
                    $"\n\tDataToEcho={request.DataToEcho}";

                string resultsMsg = $"{classNameDotMethod} response =\n{responseMsg}";
                response.ResponseMsg = resultsMsg;
                response.ResponseMsg += " -- " + ServiceOpCompletedOK;

                //  Test the basic functionality of exception handling in this service.
                if (request.ThrowException == "throw")
                {
                    throw new NullReferenceException($"{classNameDotMethod}(): ThrowException found in request Dto");
                }
            }
            catch (Exception ex)
            {
                response.ResponseDetails = $"***Error: {classNameDotMethod}() had EXCEPTION={ex}";
                m_Logger.LogWarning(response.ResponseDetails);
            }            
            
            return response;
        }
        [HttpPost(nameof(EchoMessageOnServiceAAsync))]
        public async Task<EchoResponseDto> EchoMessageOnServiceAAsync(EchoRequestDto request)
        {
            string classNameDotMethod = m_ThisClassName + "." + nameof(EchoMessageOnServiceAAsync);
            EchoResponseDto responseDto = new EchoResponseDto();

            // Test uncaught exceptions.  They need to display nicely in the UI.
            //throw new ArgumentOutOfRangeException("Testing uncaught exceptions.");
            try
            {
                m_Logger.LogInformation($"***{classNameDotMethod}(): ENTERED.");

                Guard.Against.Null(request, nameof(request));
                Guard.Against.Null(request.DataToEcho, nameof(request.DataToEcho));

                // Use Dapr Service Invocation to invoke ServiceA.EchoMessage().  This includes converting
                // the POCO request DTO to protobuf that is used by ServiceA.

                // A grpc metadata object is used to tell the Dapr sidecar the name of the service to invoke.
                // This name is the name used in the --app-id argument in the dapr run command or same named K8s
                // annotation.
                var serviceInvokeGrpcMetadata = new Metadata
                {
                    { DaprInfo.DaprAppIdLabel, DaprInfo.ServiceADaprAppId }
                };

                // Key info to understand Dapr proxy Service invocation:
                //
                // The Dapr sidecar's API gRPC server listens to its app on the dapr-grpc-port (or dapr-http-port)
                // The app listens to the Dapr sidecar for requests on the app-port, for example Invoke ServiceATests.
                // This distinction is key.  Also note the app-port is also the port that receives requests from
                // the outside, i.e. non-Dapr generated requests from different clients).

                // An app doing Service Invocation tells the Dapr sidecar to invoke a method on another service.
                // That requires this app code to communicate with the Dapr sidecar over grpc on the
                // sidecar's dapr-grpc-port as noted above, i.e. the port used in the --dapr-grpc-port argument of the
                // dapr run command or the same named annotation in K8s. That is the purpose of the below grpcChannel.
                using var grpcChannel = GrpcChannel.ForAddress(ServiceEndpoints.WebAPI1DaprGrpcPortAddress);

                // Create a client (aka proxy) that knows about the various methods to call on the grpc
                // service via the channel.                
                var grpcClient = new Tests.TestsClient(grpcChannel);

                // From the incomming EchoRequestDto (a .NET POCO) create the EchoRequest args (in protobuf)
                // required for the ServiceA.EchoMessageAsync() call.
                EchoRequest echoRequest = new EchoRequest 
                { 
                    DataToEcho = request.DataToEcho,
                    ThrowException=request.ThrowException
                };

                EchoResponse echoResponse = await grpcClient.EchoMessageAsync(echoRequest, serviceInvokeGrpcMetadata);

                // Return the echoResponse data to the caller of this WebAPI1 method in a POCO DTO, rather than
                // in the protobuf form that is returned above in EchoResponse.
                string responseMsg =
                    $"\tHello from {classNameDotMethod}():" +
                    $"\n\tDataToEcho={request.DataToEcho}" +
                    $"\n\tServiceA-DataEchoed={echoResponse.ResponseMsg}" +
                    $"\n\tServiceA-ResponseDetails={echoResponse.ResponseDetails}";

                string resultsMsg = $"{classNameDotMethod} response =\n{responseMsg}";
                responseDto.ResponseMsg = resultsMsg;
                responseDto.ResponseMsg += " -- " + ServiceOpCompletedOK;

                //  Test the basic functionality of exception handling in this service.
                if (request.ThrowException == "throw")
                {
                    throw new NullReferenceException($"{classNameDotMethod}(): ThrowException found in request Dto");
                }
            }
            catch (Exception ex)
            {
                responseDto.ResponseDetails = $"***Error: {classNameDotMethod}() had EXCEPTION={ex}";
                m_Logger.LogWarning(responseDto.ResponseDetails);
            }
            
            return responseDto;

            #endregion Implementation of ITestsController  
        }
    }
}
