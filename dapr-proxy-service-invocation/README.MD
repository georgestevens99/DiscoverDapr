Hello!  Welcome to this C# ASP.NET Core code example of Dapr's new Service Invocation that uses Dapr as a proxy.  Dapr Service Invocation provides the capability of having one service to invoke methods on another service.  This code example show how to use Dapr as a proxy to do gRPC based Service Invocation, rather than using the Dapr .NET SDK to do it. In other words, there are 2 ways of doing Service Invocation with Dapr:
1. Use the Dapr language specific SDK that provides Service Invocation methods to produce the Service Invocation request and send it to Dapr to process.
2. Use the Dapr Proxy feature which is lower level and requires a little more code (but not much) to produce and send the same Service Invocation request.

A Service Invocation request example: `http://localhost:50001/v1.0/invoke/ServiceA/method/EchoMessage`

The Dapr Proxy approach is mainly of use when using languages which have no Dapr SDK.  However, the Dapr Proxy approach is an excellent way to learn about how Service Invocation works specifically, and more generally how the Dapr Proxy approach applies in the same general way to the other Dapr Building Blocks.  This is the main goal of this code sample.

But first, a little more about Dapr Service Invocation to show the value it produces beyond just allowing one service to invoke methods on another service.  The use of Dapr Service Invocation also provides the following capabilities, built into Dapr and its implementation of Service Invocation:
1. Service Discovery.
2. "Mutual Authentication.
3. Tracing.
4. Metrics.
5. Access Control Lists for authorization.
6. Network Level Resiliency.
7. API Token Based Authentication."

The above items in quotes are from https://docs.dapr.io/developing-applications/building-blocks/service-invocation/howto-invoke-services-grpc/ .

And here is a link to the higher level Dapr doc describing the Service Invocation Building Block: https://docs.dapr.io/developing-applications/building-blocks/service-invocation/service-invocation-overview/ . This docment has lots of key information AND diagrams that really help understand how Dapr implements Service Invocation.  **I suggest you look it over**.

In this code example there are 3 software apps that call each other as follows, where **-->** means "calls":   

  QuickTestClient .NET Core Console App **-->** WebAPI1 ASP.NET Core **-->** ServiceA ASP.NET Core gRPC service.
  
Standard HTTP is used on the first "hop", while the second "hop" (between WebAPI1 and ServiceA) is done by Dapr Service Invocation using gRPC.  Note that both WebAPI1 and ServiceA have their own Dapr sidecars that do the work of Service Invocation between the 2 services.

The code, in a Visual Studio Solution in this repo folder, is done and works great!  It is really fast, so be sure to try out running 1000 or 10000 iterations.  It may just amaze you!  


Here is the supporting documentation:

INSTALLME.MD  -- Not yet written.  But if you already have Dapr V 1.9.2 or later, and Visual Studio, installed on your dev system you should have nothing to install.

RUNME.MD -- Not yet written, but most of that sort of info is in the file StartAndRunServices.txt in the RunSelfHosted folder.

The PowerPoint slide deck Using the Dapr Proxy for Service Invocation.pptx -- This has several diagrams of how the services interact and the various ports they, and their Dapr Sidecars, use.  This really helps understanding the dapr run commands used to start the services and their side cars, plus understanding how Dapr produces its magic.  **Viewing the slide deck first is STRONGLY SUGGESTED so as to get you oriented in a useful way**.  The slide deck also contains additional links to useful info sources to aid you in understanding Dapr and its Service Invocation.

I hope this helps. 12/31/22.

Thanks, 
George Stevens, Software Architect at Solid Value Software
