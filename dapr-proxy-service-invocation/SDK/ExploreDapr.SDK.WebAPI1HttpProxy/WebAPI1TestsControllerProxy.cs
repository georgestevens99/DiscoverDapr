using ExploreDapr.iFX.Configuration.ConstantsNEnums;
using ExploreDapr.Services.WebAPI1.Interface;
using ExploreDapr.Shared.CSharpDTOs;
using Newtonsoft.Json;
using System.Net.Http.Json;

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
namespace ExploreDapr.SDK.WebAPI1HttpProxy
{
    public class WebAPI1TestsControllerProxy : ITestsController
    { 
        private readonly IHttpClientFactory m_HttpClientFactory;
        
        // Cannot directly instantiate HttpClientFactory since it is part of the ASP.Net Core middleware pipeline.
        // Therefore one must use DI to get HttpClientFactory.  And that requires the use of DI to generate
        // this WebAPI1TestsControllerProxy class.  See QuickTestClient.Program.cs (near the top of the file)
        // for how that DI is setup and how the instance of the HttpClient is obtained from the HttpClientFactory.
        
        public WebAPI1TestsControllerProxy(IHttpClientFactory httpClientFactory)
        {
            m_HttpClientFactory = httpClientFactory;
        } 

        #region Implementation of ITestsController   
        public async Task<EchoResponseDto> EchoMessageAsync(EchoRequestDto echoRequestDto)
        {
            // Construct the URL -- NOTE the TestName part of the URL is the Service Contract's operation name.
            // Therefore CHANGES to the SERVICE CONTRACT CODE WILL cause changes to the URL!!
 
            string testNameUrl = nameof(WebAPI1TestsControllerProxy.EchoMessageAsync);
            string fullTestUrl = MakeFullUrl(ref testNameUrl);
 
            // 12/20/22 Below works!
            //fullTestUrl = "http://localhost:5130/api/tests/echomessage";
            
            HttpResponseMessage response = await SendHttpRequest(echoRequestDto, fullTestUrl);
            
            // The goal of the below code is to bubble all info, both success and failure, up to the
            // QTC display so as to speed up the debugging process by showing exactly what went wrong.
            // This can save lots of time, especially when working in K8s where config changes are
            // a common cause of bugs, rather that only code as when running Self Hosted.  This is
            // done throughout.
            string httpStatusMsg = $"Http Status Code= {response.StatusCode.ToString()}";
            EchoResponseDto? reply = null; 
            string exceptionMessage = string.Empty;
            try
            { 
                reply = await response.Content.ReadFromJsonAsync<EchoResponseDto>();
            }
            catch(Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            if(reply == null)
            { 
                reply = new EchoResponseDto();
                reply.ResponseMsg = "No response EchoResponseDto was returned.";
            }
            reply.ResponseDetails += $". HttpStatusMsg={httpStatusMsg}" + $". ExceptionMessage={exceptionMessage}";

            return reply;
        }

        public async Task<EchoResponseDto> EchoMessageOnServiceAAsync(EchoRequestDto echoRequestDto)
        {
            string testNameUrl = nameof(WebAPI1TestsControllerProxy.EchoMessageOnServiceAAsync);
            string fullTestUrl = MakeFullUrl(ref testNameUrl);
            
            // 12/21/22 Below works!
            //fullTestUrl = "http://localhost:5130/api/tests/echomessageonservicea";

            HttpResponseMessage response = await SendHttpRequest(echoRequestDto, fullTestUrl);

            string httpStatusMsg = $"Http Status Code= {response.StatusCode.ToString()}";
            EchoResponseDto? reply = null;
            string exceptionMessage = string.Empty;
            try
            {
                reply = await response.Content.ReadFromJsonAsync<EchoResponseDto>();
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            if (reply == null)
            {
                reply = new EchoResponseDto();
                reply.ResponseMsg = "No response EchoResponseDto was returned.";
            }
            reply.ResponseDetails += $". HttpStatusMsg={httpStatusMsg}" + $". ExceptionMessage={exceptionMessage}";

            return reply;
        }

        #endregion Implementation of ITestsController  


        #region HTTP Request/Response Oriented Helper Methods
        // 12/20/22 In a real system, the code in this area should go into a base class, or a separate class that is DId.
        private async Task<HttpResponseMessage> SendHttpRequest<T>(T requestDto, string fullTestUrl) where T : class
        {
            // Below is used only to get JSON strings for Postman via a breakpoint here.
            string json = JsonConvert.SerializeObject(requestDto);
                        
            HttpClient httpClient = m_HttpClientFactory.CreateClient();
            //httpClient.Timeout = TimeSpan.FromSeconds(600);  // Is this useful?
            
            // Below uses System.Text.Json to serialize the DTO.            
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(fullTestUrl, requestDto);
               
            return response;
        }

        private static string MakeFullUrl(ref string testNameUrl)
        { 
            string completeTestUrl = ServiceEndpoints.WebAPI1SelfHostedTestsBaseUrl + testNameUrl;
            completeTestUrl = completeTestUrl.ToLower();
            
            return completeTestUrl;       
        }

        #endregion HTTP Request/Response Oriented Helper Methods
    }
}