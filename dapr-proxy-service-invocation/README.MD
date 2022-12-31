Hello!  Welcome to this C# ASP.NET Core code example of Dapr's new Service Invocation that uses Dapr as a proxy to do the gRPC based Service Invocation rather than using the Dapr .NET SDK to do it. In other words, there are 2 ways of doing Service Invocation with Dapr:
1. Use the Dapr language specific SDK that provides Service Invocation methods to produce the service invocation request for Dapr to process.
2. Use the Dapr Proxy feature, which is lower level and requires a little more code (but not much) to produce the service invocation request.



In this code example there are 3 software apps that call each other as follows, where --> means "calls":   

  QuickTestClient .NET Core Console App --> WebAPI1 ASP.NET Core --> ServiceA ASP.NET Core gRPC service.
  
Standard HTTP is used on the first "hop", while the second "hop" (between WebAPI1 and ServiceA) is done by Dapr Service Invocation using gRPC.  Note that both WebAPI1 and ServiceA have their own Dapr sidecars that do the work of Service Invocation between the 2 services.

Here is the link to the relevant Dapr doc detailing this form of Service Invocation: https://docs.dapr.io/developing-applications/building-blocks/service-invocation/howto-invoke-services-grpc/  

And here is a link to the higher level Dapr doc describing the Service Invocation Building Block: https://docs.dapr.io/developing-applications/building-blocks/service-invocation/service-invocation-overview/

The code, in a Visual Studio Solution in this repo folder, is done and works great!  It is really fast, so be sure to try out running 1000 or 10000 iterations.  It may just amaze you!  


Here is the supporting documentation:

INSTALLME.MD  -- Not yet written.  But if you already have Dapr V 1.9.2 or later, and Visual Studio, installed on your dev system you should have nothing to install.

RUNME.MD -- Not yet written, but most of that sort of info is in the file StartAndRunServices.txt in the RunSelfHosted folder.

Using the Dapr Proxy for Service Invocation.pptx -- This PowerPoint slide deck has several diagrams of how the services interact and the various ports they, and their Dapr Sidecars, use.  This really helps understanding the dapr run commands used to start the services and their side cars, plus understanding how Dapr produces its magic.  Viewing the slide deck first is STRONGLY SUGGESTED so as to get you oriented in a useful way.  This slide deck also contains additional links to useful info sources to aid you in understanding Dapr and its Service Invocation.

I hope this helps. 12/29/22.

Thanks, 
George Stevens, Software Architect at Solid Value Software
