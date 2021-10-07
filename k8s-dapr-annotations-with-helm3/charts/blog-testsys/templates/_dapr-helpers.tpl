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