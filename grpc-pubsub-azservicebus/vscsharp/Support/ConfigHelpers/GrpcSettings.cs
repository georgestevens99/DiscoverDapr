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

namespace ConfigHelpers
{
    public class GrpcSettings
    {
        // The port number (e.g. 5001) must match the port of the gRPC server's Properties\launchSettings.json file.
        // TODO LATER Put the port numbers elsewhere, in a config file or?
        public const string ServiceAClientHttpAddressString = "https://localhost:5001";
        public const string ServiceBClientHttpAddressString = "https://localhost:5002";
    }
}
