// © 2021 Sitecore Corporation A/S. All rights reserved. Sitecore® is a registered trademark of Sitecore Corporation A/S.

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sitecore.AspNet.RenderingEngine.Extensions;
using Sitecore.AspNet.RenderingEngine.Localization;
using KayeeDictionaryServiceDemo.Configuration;
using KayeeDictionaryServiceDemo.Models;
using Sitecore.AspNet.Tracking;
using Sitecore.LayoutService.Client.Extensions;
using System.Collections.Generic;
using System.Globalization;
using Sitecore.AspNet.ExperienceEditor;
using KayeeDictionaryServiceDemo.Services.Dictionary.Extensions;

namespace KayeeDictionaryServiceDemo
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var handler = _config.GetSection(DefaultHandlerOptions.Key).Get<DefaultHandlerOptions>();
            var experienceEditor = _config.GetSection(ExperienceEditorConfiguration.Key).Get<ExperienceEditorConfiguration>();
            var jssEditingSecret = _config.GetValue<string>(ExperienceEditorConfiguration.JssEditingSecretKey);

            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });

            services
                .AddDictionaryService()
                .AddSitecoreLayoutService()
                .AddHttpHandler(handler.Name, handler.Uri)
                .WithRequestOptions(request =>
                {
                    foreach (var entry in handler.RequestDefaults)
                        request.Add(entry.Key, entry.Value);
                })
                .AsDefaultHandler();

            services.AddSitecoreRenderingEngine(options =>
                {
                    options
                        .AddPartialView("Styleguide-Layout", "_StyleguideLayout")
                        .AddPartialView("GraphQL-Layout", "_GraphQLLayout")
                        .AddModelBoundView<ContentBlock>("ContentBlock")
                        .AddModelBoundView<HeadingAndDescription>("Styleguide-ComponentParams")
                        .AddModelBoundView<CustomRouteType>("Styleguide-CustomRouteType")
                        .AddModelBoundView<FieldUsageCheckbox>("Styleguide-FieldUsage-Checkbox")
                        .AddModelBoundView<FieldUsageCustom>("Styleguide-FieldUsage-Custom")
                        .AddModelBoundView<FieldUsageDate>("Styleguide-FieldUsage-Date")
                        .AddModelBoundView<FieldUsageFile>("Styleguide-FieldUsage-File")
                        .AddModelBoundView<FieldUsageImage>("Styleguide-FieldUsage-Image")
                        .AddModelBoundView<FieldUsageLink>("Styleguide-FieldUsage-Link")
                        .AddModelBoundView<FieldUsageItemLink>("Styleguide-FieldUsage-ItemLink")
                        .AddModelBoundView<FieldUsageContentList>("Styleguide-FieldUsage-ContentList")
                        .AddModelBoundView<FieldUsageNumber>("Styleguide-FieldUsage-Number")
                        .AddModelBoundView<FieldUsageText>("Styleguide-FieldUsage-Text")
                        .AddModelBoundView<FieldUsageRichText>("Styleguide-FieldUsage-RichText")
                        .AddModelBoundView<HeadingAndDescription>("Styleguide-Layout-Reuse")
                        .AddModelBoundView<HeadingAndDescription>("Styleguide-Layout-Tabs")
                        .AddModelBoundView<HeadingAndDescription>("Styleguide-RouteFields")
                        .AddModelBoundView<HeadingOnly>("Styleguide-Section")
                        .AddModelBoundView<HeadingAndDescription>("Styleguide-SitecoreContext")
                        .AddModelBoundView<HeadingAndDescription>("Styleguide-Multilingual")
                        .AddModelBoundView<HeadingAndDescription>("Styleguide-Tracking")
                        .AddDefaultPartialView("_ComponentNotFound");
                })
                // In Experience Editor, relative links to resources of Rendering Host may render incorrectly,
                // Rendering Host therefore replaces such links with absolute ones, when sending the rendered layout back to Experience Editor.
                // By default, when generating absolute links, the current request from Experience Editor is used to get the Rendering Host URL.
                // You can change this behavior by setting your custom URL in ExperienceEditorOptions.
                // .WithExperienceEditor(options =>
                // {
                //     options.ApplicationUrl = new Uri("https://[your custom URL]");
                // })
                // More details see in ExperienceEditorOptions documentation.
                .WithExperienceEditor(options =>
                    {
                        options.Endpoint = experienceEditor.Endpoint;
                        options.JssEditingSecret = jssEditingSecret;
                        //This is an example to show how we can target custom routes for the Experience Editor by adding custom mapping handlers.
                        options.MapToRequest((sitecoreResponse, scPath, httpRequest) =>
                            httpRequest.Path = scPath + sitecoreResponse?.Sitecore?.Route?.DatabaseName);
                    })
                .WithTracking();

            services.AddSitecoreVisitorIdentification(options =>
            {
                // Usually SitecoreInstanceHostName is same as Layout service but can be any Sitecore CD/CM instance which shares same AspNet session with Layout Service.
                // This Sitecore instance will be used for Visitor identification.
                var uriSetting = _config.GetSection("Analytics:SitecoreInstanceUri").Get<Uri>();
                options.SitecoreInstanceUri = uriSetting ?? new Uri("https://SitecoreInstanceHostName");
            });

            // This configuration necessary for proper resolving of IP address and Scheme of original request in case reverse proxies sends XForwarded sets of headers. See Tracking documentation for details.
            // Uncomment if you expect to resolve x-forwarded headers.
            //services.Configure<ForwardedHeadersOptions>(options =>
            //{
            //    options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSitecoreExperienceEditor();
            app.UseRouting();
            app.UseStaticFiles();

            // Make sure to resolve IP address before Rendering engine functionality. It will allow xDb to record real client IP address.
            // Uncomment if you expect to resolve x-forwarded headers.
            //app.UseForwardedHeaders();
            app.UseSitecoreVisitorIdentification();

            //Adds localization functionality
            //Calling UseSitecoreRequestLocalization() on the localization  allows culture to be resolved from both the sc_lang query string and the culture token from route data.
            app.UseRequestLocalization(options =>
            {
                var supportedCultures = new List<CultureInfo> { new CultureInfo("en"), new CultureInfo("da"), new CultureInfo("da-DK") };
                options.DefaultRequestCulture = new RequestCulture(culture: "da", uiCulture: "da");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.UseSitecoreRequestLocalization();
            });
            app.UseSitecoreRenderingEngine();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSitecoreLocalizedRoute("Localized", "Index", "Sitecore");
                endpoints.MapFallbackToController("Index", "Sitecore");
            });
        }
    }
}
