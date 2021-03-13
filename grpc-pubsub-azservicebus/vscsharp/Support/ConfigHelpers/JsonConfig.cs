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

using System.Text.Json;

namespace ConfigHelpers
{
    public static class JsonConfig
    {
        private static JsonSerializerOptions m_JsonOptions;
        public static JsonSerializerOptions MakeJsonSerializerOptions()
        {
            if (m_JsonOptions == null)
            {
                JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true,
                };
                m_JsonOptions = jsonOptions;
            }

            return m_JsonOptions;
        }
    }
}
