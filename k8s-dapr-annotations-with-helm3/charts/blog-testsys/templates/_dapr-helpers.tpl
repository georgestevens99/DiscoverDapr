{{/*
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
*/}}
{{/*
Dapr annotations
#Note that the app-id annotation MUST be in the deployment that invokes this Named Template.
#This is due to not knowing how to pass in the app-id string, e.g. {{ include "app-servicea.fullname" . | quote }}, into this template.
*/}}
{{- define "dapr.annotations" -}}
dapr.io/enabled: {{ .Values.daprAnnotations.enabled | quote }}
dapr.io/app-port: {{ .Values.daprAnnotations.appPort | quote }}
dapr.io/app-protocol: {{ .Values.daprAnnotations.appProtocol | quote }}
dapr.io/config: "{{ .Release.Name  }}-config"
dapr.io/log-level: {{ .Values.daprAnnotations.logLevel | quote }}
{{- end }}