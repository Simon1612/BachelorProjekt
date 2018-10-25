using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntegrationTests.Helpers
{
    public interface IHttpClientWrapper : IDisposable
    {
        Task<HttpResponseMessage> SendAsyncWithCookie(HttpRequestMessage request, string cookiePath);
        Task<HttpResponseMessage> PostAsync(string urlString, HttpContent content);
        Task<HttpResponseMessage> GetAsync(string urlString);
    }
}