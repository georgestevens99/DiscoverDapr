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
namespace ExploreDapr.iFX.Configuration.ConstantsNEnums
{
    public class ServiceEndpoints
    {
        // For tests via a browser.
        public const string WebAPI1SelfHostedEndPt = "http://localhost:5130/api/"; // QTC requires a "/".  
              
        // For the WebAPI1 TestsController service operations.
        public const string WebAPI1SelfHostedTestsBaseUrl = WebAPI1SelfHostedEndPt + "tests/";

        // DaprGrpcPort Addresses used in the args to the dapr run command and in the K8s dapr annotations.
        // Note in the preceeding, only the port number is used.  However for Service Invocation the below string is used.

        // Key info to understand Service invocation:
        //
        // The Dapr sidecar's API gRPC server listens to the app on the dapr-grpc-port (or dapr-http-port)
        // The app listens to the Dapr sidecar on the app-port.  This distinction is key.

        // Used in Service Invocation when WebAPI1 wants to invoke another service
        // via the Dapr sidecar whose dapr-grpc-port is 50001.  WebAPI1 sends requests to Dapr on this port.
        public const string WebAPI1DaprGrpcPortAddress = "http://localhost:50001";

        // Used in Service Invocation when ServiceA wants to invoke another service
        // via the Dapr sidecar whose dapr-grpc-port is 50002.  ServiceA sends requests to Dapr on this port,
        // but not demonstrated in the current solution which only has ServiceA receiving requests from
        // WebAPI1 via Dapr.
        public const string ServiceADaprGrpcPortAddress = "http://localhost:50002";
    }
}