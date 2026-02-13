using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace delivery_order_services.ServicesCollectionExtensions
{
    public static class BulderExtensions
    {
        public static  IApplicationBuilder UseDefaultLocalization(this IApplicationBuilder app)
        {
            var cultureInfo = new CultureInfo("pt-BR");
            var defaultRequestCulture = new RequestCulture(cultureInfo);
            var listCultureInfo = new List<CultureInfo> { cultureInfo };
            var localizationOptions = new RequestLocalizationOptions
            {
                SupportedCultures = listCultureInfo,
                SupportedUICultures = listCultureInfo,
                DefaultRequestCulture = defaultRequestCulture,
                FallBackToParentCultures = false,
                FallBackToParentUICultures = false,
                RequestCultureProviders = null
            };

            app.UseRequestLocalization(localizationOptions);
            return app;
        }
    }
}
