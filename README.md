# KayeeDictionaryServiceDemo

## About this Solution
This solution is designed to help developers learn and get started quickly
with Sitecore Containers, the ASP.NET Core Rendering SDK, and Sitecore
Content Serialization.

For simplicity, this solution does not implement Sitecore Helix conventions for
solution architecture. As you begin building your Sitecore solution,
you should review [Sitecore Helix](https://helix.sitecore.net/) and the
[Sitecore Helix Examples](https://sitecore.github.io/Helix.Examples/) for guidance
on implementing a modular solution architecture.

## Support
The template output as provided is supported by Sitecore. Once changed or amended,
the solution becomes a custom implementation and is subject to limitations as
defined in Sitecore's [scope of support](https://kb.sitecore.net/articles/463549#ScopeOfSupport).

## Prerequisites
* .NET 6.0 SDK
* .NET Framework 4.8 SDK
* Visual Studio 2019
* Docker for Windows, with Windows Containers enabled

See Sitecore Containers documentation for more information on system requirements.

## What's Included
* A `docker-compose` environment for each Sitecore topology (XPO, XP1, XM1)
  with an ASP.NET Core rendering host.
  > The containers structure is organized by specific topology environment (see `run\sitecore-xp0`, `run\sitecore-xp1`, `run\sitecore-xm1`).
  > The included `docker-compose.yml` is a stock environment from the Sitecore
  > Container Support Package. All changes/additions for this solution are included
  > in the `docker-compose.override.yml`.

* Serialized items for the Styleguide sample site (see `src\Items.module.config`).
* An MSBuild project for an ASP.NET Core application which renders
  the site (see `src\rendering`).
* An MSBuild project for deploying configuration and code into
  the Sitecore Content Management role. (see `src\platform`).

## Running this Solution
1. If your local IIS is listening on port 443, you'll need to stop it.
   > This requires an elevated PowerShell or command prompt.
   ```
   iisreset /stop
   ```

1. Before you can run the solution, you will need to prepare the following
   for the Sitecore container environment:
   * A valid/trusted wildcard certificate for `*.kayeedictionaryservicedemo.localhost`
   * Hosts file entries for `kayeedictionaryservicedemo.localhost`
   * Required environment variable values in `.env` for the Sitecore instance
     * (Can be done once, then checked into source control.)

   See Sitecore Containers documentation for more information on these
   preparation steps. The provided `init.ps1` will take care of them,
   but **you should review its contents before running.**

   > You must use an elevated/Administrator Windows PowerShell 5.1 prompt for
   > this command, PowerShell 7 is not supported at this time.

    ```ps1
    .\init.ps1 -InitEnv -LicenseXmlPath "C:\path\to\license.xml" -AdminPassword "DesiredAdminPassword" -Topology xp0
    ```
    The ```-Topology ``` parameter specify topology you need. This parameter is optional. The default value ```xp0```

    If you check your `.env` into source control, other developers
    can prepare a certificate and hosts file entries by simply running:

    ```ps1
    .\init.ps1
    ```

    > Out of the box, this example does not include `.env` in the `.gitignore`.
    > Individual users may override values using process or system environment variables.
    > This file does contain passwords that would provide access to the running containers
    > in the developer's environment. If your Sitecore solution and/or its data are sensitive,
    > you may want to exclude these from source control and provide another
    > means of centrally configuring the information within.

1. After completing this environment preparation, run the startup script
   from the solution root:
    ```ps1
    .\up.ps1
    ```

1. When prompted, log into Sitecore via your browser, and
   accept the device authorization.

1. Wait for the startup script to open browser tabs for the rendered site
   and Sitecore Launchpad.

## Using the Solution
* A publish of the `Platform` project will update the running `cm` service.
* The running `rendering` service uses `dotnet watch` and will recompile
  automatically for any changes you make. You can also run the `Rendering`
  project directly from Visual Studio.
* Review README's found in the projects and throughout the solution
  for additional information.
