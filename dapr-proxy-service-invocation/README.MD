Hello!  Welcome to C# ASP.NET Core code example of Dapr's new Service Invocation via using Dapr as a proxy to do the Service Invocation rather than using the .NET SDK to do it.

There are 3 software apps that call each other as follows, where --> means "calls":   
  QuickTestClient .NET Core Console App --> WebAPI1 ASP.NET Core --> ServiceA ASP.NET Core gRPC service.
  
Standard HTTP is used on the first "hop", while the second "hop" (between WebAPI1 and ServiceA) is done by Dapr Service Invocation using gRPC.  Note that both WebAPI1 and ServiceA have their own personal Dapr sidecars that do the work of Service Invocation between the 2 services.

Here is the link to the relevant Dapr docs https://docs.dapr.io/developing-applications/building-blocks/service-invocation/howto-invoke-services-grpc/ 

The code, in a Visual Studio Solution is done and works great.  But the documentation is incomplete, as follows:

INSTALLME.MD  -- Not yet written.  But if you already have Dapr V 1.9.2 or later, and Visual Studio, installed on your dev system you should have nothing to install.

RUNME.MD -- Not yet written, but most of that sort of info is in the file StartAndRunServices.txt in the RunSelfHosted folder.

README.MD -- Needs a little more work.

Using the Dapr Proxy for Service Invocation.pptx -- While this is currently not done, it will have 2 diagrams.  A system level diagram showing how the 3 apps interact.  and a second diagram showing the details of how the apps and their Dapr side cars interact, especially paying attention to the port names they use for various things.  This can be a source of bafflement for the uninitiated, and a diagram can act to make it very clear.

I will set about doing the above work items soon after Christmas.

I hope this helps. 12/23/22

Thanks, 
George Stevens
Software Architect at Solid Value Software
