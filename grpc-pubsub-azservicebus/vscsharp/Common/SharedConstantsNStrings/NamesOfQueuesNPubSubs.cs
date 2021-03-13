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

namespace SharedConstantsNStrings
{
    public class NamesOfQueuesNPubSubs
    {
        // PubSubKinds
        public const string DaprPubSubKind = "daprpubsub";
       
        // Topic Names
        public const string ServiceADemoEvents1Topic = "svcADemoEvents1";
        public const string ServiceADemoEvents2Topic = "svcADemoEvents2";

        // PubSub Building Block components.
        
        // Redis Streams pubsub that comes with dapr.
        public const string PubSubDefaultDaprComponent = "pubsub"; 

        // Custom asb topic component containing connection string in yaml.
        public const string PubSubAzServiceBusComponent = "svs-pubsub-asbtopic";
        
        // Custom Azure Service Bus component using Environment Variable Secret Store for key info.
        public const string PubSubAzServiceBusEnvVarSecretsComponent = "svs-pubsub-envvarsecrets-asbtopic";
    }
}
