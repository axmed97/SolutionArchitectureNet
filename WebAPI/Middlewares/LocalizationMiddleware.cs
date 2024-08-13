using System.Globalization;

namespace WebAPI.Middlewares
{
    public class LocalizationMiddleware : IMiddleware
    {
        private readonly ILogger<LocalizationMiddleware> _logger;

        public LocalizationMiddleware(ILogger<LocalizationMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var lang = context.Request.Headers.AcceptLanguage.FirstOrDefault();
            if (lang == "az" || lang == "en-US" || lang == "ru-RU")
            {
                var culture = new CultureInfo(lang);
                _logger.LogInformation($"Setting culture to: {culture.Name}");
                Thread.CurrentThread.CurrentUICulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
            }
            else
            {
                var defaultCulture = new CultureInfo("az");
                _logger.LogInformation($"Setting culture to: {defaultCulture.Name}");
                Thread.CurrentThread.CurrentUICulture = defaultCulture;
                Thread.CurrentThread.CurrentCulture = defaultCulture;
            }

            await next(context);
        }
    }
}
