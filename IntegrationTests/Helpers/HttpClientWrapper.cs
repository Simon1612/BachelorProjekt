using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Pages.Internal.Account;
using Microsoft.Net.Http.Headers;

namespace IntegrationTests.Helpers
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly CookieContainer _cookies;
        private readonly HttpClient _client;

        public HttpClientWrapper(HttpClient client)
        {
            _client = client;
            _cookies = new CookieContainer();
        }

        public async Task<HttpResponseMessage> SendAsyncWithCookie(HttpRequestMessage request, string cookiePath)
        {
            var cookies = _cookies.GetCookies(new Uri($"http://localhost/cookie/{cookiePath}"));
            var authCookie = cookies["auth_cookie"];
            request.Headers.Add("Cookie", new CookieHeaderValue(authCookie.Name, authCookie.Value).ToString());

            var response = await _client.SendAsync(request);

            GetCookiesFromHeader(response);

            return response;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var response = await _client.SendAsync(request);

            GetCookiesFromHeader(response);

            return response;
        }



        public async Task<HttpResponseMessage> PostAsync(string urlString, HttpContent content)
        {
            var response = await _client.PostAsync(urlString, content);

            GetCookiesFromHeader(response);

            return response;
        }

        public async Task<HttpResponseMessage> GetAsync(string urlString)
        {
            var response = await _client.GetAsync(urlString);

            GetCookiesFromHeader(response);

            return response;
        }

        private void GetCookiesFromHeader(HttpResponseMessage response)
        {   
            if (response.Headers.TryGetValues("Set-Cookie", out var newCookies))
            {
                foreach (var item in SetCookieHeaderValue.ParseList(newCookies.ToList()))
                {
                    _cookies.Add(response.RequestMessage.RequestUri, new Cookie(item.Name.Value, item.Value.Value, item.Path.Value));
                }
            }
        }
        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
