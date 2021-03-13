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

using ConfigHelpers;
using Dapr.Client;

namespace ServiceHelpers
{
    // TODO Make this a proper singleton? Or set it up for DI?
    public class DaprClientProxyFactory
    {
        // The DaprProxy is used to make calls to other daperized services and bindings
        private static DaprClient m_DaprProxy;
        public static DaprClient MakeDaprProxy()
        {
            if (m_DaprProxy == null)
            {
                DaprClient daprProxy = new DaprClientBuilder()
                    .UseJsonSerializationOptions(JsonConfig.MakeJsonSerializerOptions())
                    .Build();
                m_DaprProxy = daprProxy;
            }

            return m_DaprProxy;
        }
    }
}
