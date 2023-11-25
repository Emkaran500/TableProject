using System;
using System.Net.Http;

namespace ClientApp.Models
{
    public class HttpClientInstance
    {
        private static readonly HttpClient httpClient = new HttpClient();
        public static HttpClient Instance = httpClient;
    }
}
