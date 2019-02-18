using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using AdventureWorks.Common;

namespace AdventureWorks.Web.Helpers
{
    public class WebApiClient<T>
        where T : new()
    {
        private static string Host => ConfigurationManager.AppSettings["ApiHostUrl"];

        private string Token
        {
            get
            {
                var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity?.FindFirst(ClaimTypes.Thumbprint);

                return claim != null ? claim.Value : string.Empty;
            }
        }

        public HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri(Host) };
            client.DefaultRequestHeaders.Accept.Clear();

            var token = Token;
            if (!string.IsNullOrWhiteSpace(token))
            {
                //Add the authorization header
                client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("Bearer " + token);
            }

            var transactionId = Guid.NewGuid().ToString();
            client.DefaultRequestHeaders.Add("transactionId", transactionId);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public async Task<Result<T>> GetAsync(string route, params string[] paramStrings)
        {
            using (var client = GetHttpClient())
            {
                string url = $"{route}/{string.Join("/", paramStrings)}";

                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<Result<T>>();
                }

                return new Result<T>
                {
                    Data = default(T),
                    Status = response.StatusCode
                };
            }
        }

        public async Task<Result<T>> Update(string route, T model)
        {
            using (var client = GetHttpClient())
            {
                var response = await client.PutAsJsonAsync(route, model);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<Result<T>>();
                }

                return new Result<T>
                {
                    Data = default(T),
                    Status = response.StatusCode
                };
            }
        }

        public async Task<Result<T>> Post(string route, T model)
        {
            using (var client = GetHttpClient())
            {
                var response = await client.PostAsJsonAsync(route, model);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<Result<T>>(json);
                }

                return new Result<T>
                {
                    Data = default(T),
                    Status = response.StatusCode
                };
            }
        }

        public async Task<Result<User>> AuthenticateAsync(string userName, string password)
        {
            using (var client = GetHttpClient())
            {
                var result = await client.PostAsync("/Token", new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("userName", userName),
                    new KeyValuePair<string, string>("password", password)
                }));

                var json = await result.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(json))
                {
                    return new Result<User>
                    {
                        Data = default(User),
                        Status = HttpStatusCode.Unauthorized
                    };
                }

                var userResult = GetHeaderValueByKey(result, "User");
                var signInResult = JsonConvert.DeserializeObject<SignInResult>(json);
                HttpStatusCode httpStatusCode;

                bool success = Enum.TryParse(GetHeaderValueByKey(result, "HttpStatusCode"), out httpStatusCode)
                                && httpStatusCode == HttpStatusCode.OK
                                && !string.IsNullOrEmpty(userResult)
                                && result.IsSuccessStatusCode;

                if (success)
                {
                    var user = JsonConvert.DeserializeObject<User>(userResult);
                    user.AccessToken = signInResult.AccessToken;

                    return new Result<User>
                    {
                        Data = user,
                        Status = HttpStatusCode.OK
                    };
                }

                return new Result<User>
                {
                    Data = default(User),
                    Status = HttpStatusCode.Unauthorized
                };
            }
        }

        private static string GetHeaderValueByKey(HttpResponseMessage response, string key)
        {
            IEnumerable<string> values;
            if (response.Headers == null)
            {
                return null;
            }

            return response.Headers.TryGetValues(key, out values) ? values.FirstOrDefault() : null;
        }
    }
}