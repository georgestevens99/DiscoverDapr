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
    public class NamesOfEnvVariables
    {
        public const string AzTenantId = "AZURE_TENANT_ID";
        public const string AzClientId = "AZURE_CLIENT_ID"; // Client is aka App, ClientId == AppId.
        public const string AzClientSecret = "AZURE_CLIENT_SECRET";
        public const string AzClientCertPath = "AZURE_CLIENT_CERT_PATH";
        public const string AzClientCertThumbPrint = "AZURE_CLIENT_CERT_THUMBPRINT";

        public const string AsbNsFqdn = "ASBNS_FQDN";
        public const string AsbNsConnString = "ASBNS_CONN_STRING";

        public const string AzKeyVaultName = "KEY_VAULT_NAME";

    }
}
