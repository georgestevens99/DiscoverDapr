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
    public class DaprInfo
    {
        // General Dapr Things

        // Used in the grpc metadata required for Dapr Service Invocation.
        // The key for the --app-id argument to dapr for use in a key-value-pair.
        public const string DaprAppIdLabel = "dapr-app-id";

        //Service Names -- The used in the --app-id argument for the dapr run command and K8s dapr annotations.
        public const string ServiceADaprAppId = "ServiceA";
    }
}
