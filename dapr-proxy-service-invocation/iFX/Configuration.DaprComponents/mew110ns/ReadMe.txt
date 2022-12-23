ReadMe.txt - 7/9/22
The files in this mew110ns directory are the same as the other components, with the sole exception that
all of them have the namespace: mew110ns in their yaml so that they may only be accessed by kubernetes
services running in the mew110ns K8s namespace.  These are intended to be used for K8s and not self hosted.

All the components herein need to be "applied" to a K8s cluster via kubectl apply, like the below example:
kubectl apply -f svs-pubsub-redis-k8.yaml -n mew110ns

In the above example the full path name for the yaml file is missing for simplicity.  For the complete
component deployment .bat file see K8sDeployEssentials-Dapr ddirectory.
kubectl apply -f ..\..\iFX\Configuration\mew110ns svs-pubsub-redis-k8.yaml -n mew110ns

Note the k8s yaml files need to have a secret deployed in the namespace by running Deploy-mew110-secrets.bat in the
K8sDeployEssentials-Dapr directory.