using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace TokenLoginApp
{
    public class ServiceHandler
    {
        static HttpClient client = new HttpClient();

        public ServiceHandler()
        {

        }

        public static async Task<T> GetDataAsync<T>(string endPoint)
        {
            client.DefaultRequestHeaders.ExpectContinue = false;
            T returnResult = default(T);
            var uri = new Uri(string.Format("{0}{1}", App.RestUrl, endPoint));
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                returnResult = JsonConvert.DeserializeObject<T>(content);

            }
            return returnResult;
        }

        public static async Task<T> PostData<T, Tr>(string endPoint, HttpMethod method, Tr content)
        {
            client.DefaultRequestHeaders.ExpectContinue = false;
            T returnResult = default(T);

            var uri = new Uri(string.Format("{0}{1}", App.RestUrl, endPoint));
            try
            {
                string jsonString = string.Empty;
                if (content != null)
                {
                    jsonString = JsonConvert.SerializeObject(content);
                }
                var httpcontent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, httpcontent);


                if (response.IsSuccessStatusCode)
                {
                    var content1 = response.Content.ReadAsStringAsync().Result;
                    returnResult = JsonConvert.DeserializeObject<T>(content1);
                }
            }
            catch (Exception ex)
            {
                string e = ex.InnerException.ToString();
            }
            return returnResult;
        }

        public static async Task<TokenResponse> GetToken(string username, string password)
        {
            TokenResponse returnResult = default(TokenResponse);
            var uri = new Uri(string.Format("{0}{1}", App.RestUrl, "token"));

            try
            {
                //Define Headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, password))));

                //Prepare Request Body
                List<KeyValuePair<string, string>> requestData = new List<KeyValuePair<string, string>>();
                requestData.Add(new KeyValuePair<string, string>("username", username));
                requestData.Add(new KeyValuePair<string, string>("password", password));
                requestData.Add(new KeyValuePair<string, string>("grant_type", "password"));

                FormUrlEncodedContent requestBody = new FormUrlEncodedContent(requestData);

                //Request Token
                var request = await client.PostAsync(uri, requestBody);

                if (request.IsSuccessStatusCode)
                {
                    var response = await request.Content.ReadAsStringAsync();
                    returnResult = JsonConvert.DeserializeObject<TokenResponse>(response);
                }
            }
            catch (Exception ex)
            {
                string e = ex.InnerException.ToString();
            }

            return returnResult;
        }
    }
}
