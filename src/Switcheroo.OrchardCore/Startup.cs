using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.DisplayManagement.Theming;
using OrchardCore.Modules;

namespace Switcheroo.OrchardCore {
    public class Startup : StartupBase {
        public override void ConfigureServices(IServiceCollection services) {
            services.AddScoped<IThemeSelector, SwitcherooThemeSelector>();
        }

        public override void Configure(IApplicationBuilder builder, IRouteBuilder routes,
            IServiceProvider serviceProvider) { }
    }
}