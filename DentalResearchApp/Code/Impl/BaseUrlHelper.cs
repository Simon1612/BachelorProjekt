using Microsoft.AspNetCore.Http;

namespace DentalResearchApp.Code.Impl
{
    public static class BaseUrlHelper
    {
        public static string GetBaseUrl(HttpRequest request)
        {
            var host = request.Host.Host;
            if (host == "localhost")
                host += ":" + request.Host.Port;

            return "https://" + host;
        }
    }
}