Hello!  Welcome to this C# ASP.NET Core code example of Dapr's new Service Invocation that uses Dapr as a proxy.  Dapr Service Invocation provides the capability of having one service invoke methods on another service.  

This simple code example not only shows how to do Dapr Proxied Service Invocation, but can also serve as a starter to aid in exploring many other Dapr caapabilities.  Just extend it by adding other services and/or other Dapr Building blocks.  Also note that this code example uses Dapr's Self Hosted mode so that you can run it on your desktop dev machine and avoid the extra setup required when running it under Docker Compose or Kubernetes.

This code example shows how to use Dapr-as-a-proxy to do gRPC based Service Invocation rather than using the Dapr .NET SDK to do it. In other words, there are 2 ways of doing Service Invocation with Dapr:
1. Use the a language specific SDK that provides Dapr Service Invocation methods to produce the Service Invocation request and send it to a Dapr sidecar to process. A list of language specific Dapr SDKs is available at this link -- https://docs.dapr.io/developing-applications/sdks/
2. Use the Dapr Proxy feature, which is lower level and requires a little more code (but not much), to produce and send the same Service Invocation request.

A Service Invocation request example: `http://localhost:50001/v1.0/invoke/ServiceA/method/EchoMessage`  As you can see, the request syntax is straight forward.

The Dapr Proxy approach is mainly of use when using languages which have no Dapr SDK.  However, the Dapr Proxy approach is also an excellent way to learn about how Service Invocation works specifically, and more generally how the Dapr Proxy approach applies in the same general way to the other Dapr Building Blocks.  This is the main goal of this code sample, including explicitly showing how the port numbers used in the Dapr Run command (and Kubernetes Dapr annotations) work via diagrams in a PowerPoint slide deck in this folder.  I suggest you view the PowerPoint diagrams as soon as you finish reading this README.MD.

But first, a little more about Dapr Service Invocation to show the value it produces beyond just allowing one service to invoke methods on another service.  The use of Dapr Service Invocation also provides the following capabilities, built into Dapr and its implementation of Service Invocation:
1. Service Discovery, using Dapr's [mDNS](https://docs.dapr.io/reference/components-reference/supported-name-resolution/nr-mdns/).
2. "Mutual Authentication.
3. Tracing.
4. Metrics.
5. Access Control Lists for authorization.
6. Network Level Resiliency.
7. API Token Based Authentication."

The above items in quotes are from https://docs.dapr.io/developing-applications/building-blocks/service-invocation/howto-invoke-services-grpc/ .  Just think, with Dapr you don't have to write much, or maybe even any, code to use the above capabilities.  Just write some yaml to supply the settings required by the Dapr components that supply the capabilities you need.  Please check the Dapr Docs for more information on how this works at https://docs.dapr.io/concepts/components-concept/.

And here is a link to the higher level Dapr doc describing the Service Invocation Building Block: https://docs.dapr.io/developing-applications/building-blocks/service-invocation/service-invocation-overview/ . This docment has lots of key information AND diagrams that really help understand how Dapr implements Service Invocation.  **I suggest you look it over**.

In this code example there are 3 software apps that call each other as follows, where **-->** means "calls":   

  QuickTestClient .NET Core Console App **-->** WebAPI1 ASP.NET Core **-->** ServiceA ASP.NET Core gRPC service.
  
Standard non-Daprized HTTP is used on the first "hop", while the second "hop" (between WebAPI1 and ServiceA) is done by Dapr Service Invocation using gRPC.  Note that both WebAPI1 and ServiceA have their own Dapr sidecars that do the work of Service Invocation between the 2 services.

The code, in a Visual Studio Solution in this repo folder, is tested and works great!  It is really fast, so be sure to try out running 1000 or 5000 iterations.  It may just amaze you!  


Here is the supporting documentation:

INSTALLME.MD  -- This is almost ready to use.

RUNME.MD -- This is almost ready to use, with most of that sort of info being in the file StartAndRunServices.txt in the RunSelfHosted folder of the Visual Studio solution.

The PowerPoint slide deck "Using the Dapr Proxy for Service Invocation.pptx" -- This is located in the top level dapr-proxy-service-invocation directory.  **It has several diagrams of how the services interact and the various ports they, and their Dapr Sidecars, use**.  This really helps understanding the dapr run commands used to start the services and their side cars in Self Hosted mode, plus understanding how Dapr produces its magic.  **Viewing the slide deck first is STRONGLY SUGGESTED so as to get you oriented in a useful way**.  The slide deck also contains additional links to useful info sources to aid you in understanding Dapr and its Service Invocation.  
I hope this helps. 1/2/23.

Thanks, 
George Stevens, Software Architect at Solid Value Software
