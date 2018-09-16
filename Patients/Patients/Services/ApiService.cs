﻿namespace Patients.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Patients.Helpers;
    using Patients.Models;
    using Plugin.Connectivity;
    using Xamarin.Forms;

    public class ApiService
    {
        public async Task<Response> CheckConnection()
        {
               if (!CrossConnectivity.Current.IsConnected)
               {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Languages.TurnOnInternet,
                    };
               }

            //var url = Application.Current.Resources["UrlGoogle"].ToString();
            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
                if (!isReachable)
                {
                       return new Response
                       {
                           IsSuccess = false,
                           Message = Languages.NoInternet,
                       };
                }
                return new Response
                {
                       IsSuccess = true,
                };
        }

        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}";
                var response = await client.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = list,
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
