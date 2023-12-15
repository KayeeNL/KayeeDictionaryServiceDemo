# Rendering Host Project

This Visual Studio / MSBuild project is an independently running ASP.NET Core
application which uses the Sitecore ASP.NET Core Rendering SDK to create a
*rendering host*. An ASP.NET Core *rendering host* uses Web APIs to obtain
Sitecore content and page layout, and implements components to render
those pages. See the Sitecore ASP.NET Core Rendering SDK documentation for more information.

The Docker image for this rendering host uses `dotnet watch` during
development, so changes to this code should automatically be available for
test/preview at [www.kayeedictionaryservicedemo.localhost](https://www.kayeedictionaryservicedemo.localhost).
Be sure to watch the `rendering` container logs for compilation errors. To debug, you
can attach to the `KayeeDictionaryServiceDemo.exe` process inside the `rendering` container.

You can also run and debug the Rendering Host directly from Visual Studio (F5). However
keep in mind that the Sitecore Experience Editor is configured to utilize the
container-based Rendering Host.