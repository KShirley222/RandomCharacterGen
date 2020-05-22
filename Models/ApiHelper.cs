using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CharacterGenerator.Models
{
    public class ApiHelper
    {
        public static HttpClient ApiClient {get; set;}

        public static void IntializeClient()
        {
            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri("http://dnd5eapi.co");
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}