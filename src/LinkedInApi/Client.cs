using LinkedIn.Api.People;
using LinkedIn.Api.SocialAction;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LinkedIn.Api
{
    public class Client
    {
        private string _clientId;
        private string _clientSecret;
        private Uri _authHost;
        private Uri _apiHost;        
        private Uri _redirectUrl;
        private HttpClient _client;

        public string Token { get; set; }

        public Client(string clientId, string clientSecret, Uri redirectUrl)
        {
            _authHost = new Uri("https://www.linkedin.com");
            _apiHost = new Uri("https://api.linkedin.com");
            _client = new HttpClient();
            _clientId = clientId;
            _clientSecret = clientSecret;
            _redirectUrl = redirectUrl;                       
        }

        public Uri GetAuthorizationUrl()
        {
            return new Uri($"{_authHost}oauth/v2/authorization?response_type=code&client_id={_clientId}&redirect_uri={_redirectUrl.OriginalString}&scope=r_liteprofile%20r_emailaddress%20w_member_social");
        }

        public async Task<string> GetAccessTokenAsync(string authCode)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", authCode),
                new KeyValuePair<string, string>("redirect_uri", _redirectUrl.OriginalString),
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("client_secret", _clientSecret)
            });            

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_authHost}oauth/v2/accessToken")
            {
                Version = HttpVersion.Version11,
                Content = content,
            };

            var response = await _client.SendAsync(requestMessage);
            var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());
            Token = responseJson["access_token"].ToString();
            CheckTokenThenAddToHeaders();

            return Token;
        }

        public async Task<Profile> GetOwnProfileAsync()
        {
            CheckTokenThenAddToHeaders();
            var response = await _client.GetAsync($"{_apiHost}v2/me");
            var jsonData = await response.Content.ReadAsStringAsync();
            var profile = JsonConvert.DeserializeObject<Profile>(jsonData);
            return profile;
        }

        //public async Task<JObject> GetCompaniesAsync()
        //{
        //    CheckTokenThenAddToHeaders();
        //    //var respone = await _client.GetAsync($"{_apiHost}v2/organizations");
        //    var response = await _client.GetAsync($"{_apiHost}v2/organizationalEntityAcls?q=roleAssignee");
        //    string jsonData = await response.Content.ReadAsStringAsync();
        //    return JObject.Parse(await response.Content.ReadAsStringAsync());
        //}

        public async Task PostOnOwnProfileAsync(ShareRequest share)
        {
            var selfProfile = await GetOwnProfileAsync();
            share.Owner = $"urn:li:person:{selfProfile.Id}";
            var content = new StringContent(share.ToJson(), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{_apiHost}v2/shares", content);
            var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());

            //if (!response.IsSuccessStatusCode)
            //    throw new Exception(responseJson[""].ToString());
        }

        //public string PostOnCompanyProfile()
        //{

        //}

        //public List<string> GetPostsOnOwnProfile()
        //{

        //}

        //public List<string> GetPostsOnCompanyProfile()
        //{

        //}

        private void CheckTokenThenAddToHeaders()
        {
            if (_client.DefaultRequestHeaders.Authorization == null && !string.IsNullOrEmpty(Token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }

            if (!_client.DefaultRequestHeaders.Connection.Contains("Keep-Alive"))
            {
                _client.DefaultRequestHeaders.Connection.Add("Keep-Alive");
            }
        }
    }
}
