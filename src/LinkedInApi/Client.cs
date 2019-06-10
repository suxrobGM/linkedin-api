using LinkedIn.Api.Exceptions;
using LinkedIn.Api.Organizations;
using LinkedIn.Api.People;
using LinkedIn.Api.SocialActions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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

            if (!response.IsSuccessStatusCode)
                throw new ApiException(ExceptionModel.FromJson(await response.Content.ReadAsStringAsync()));

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
            var profile = Profile.FromJson(jsonData);

            if (!response.IsSuccessStatusCode)
                throw new ApiException(ExceptionModel.FromJson(await response.Content.ReadAsStringAsync()));

            return profile;
        }

        public async Task<Organization[]> GetCompaniesAsync()
        {
            CheckTokenThenAddToHeaders();
            var response = await _client.GetAsync($"{_apiHost}v2/organizationalEntityAcls?q=roleAssignee&role=ADMINISTRATOR");
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new ApiException(ExceptionModel.FromJson(await response.Content.ReadAsStringAsync()));

            var organizationalEntityAcls = EntityElements<OrganizationalAccess>.FromJson(responseJson);
            var ownCompaniesId = new List<long>();

            foreach (var element in organizationalEntityAcls.Elements)
            {
                if (element.Role == "ADMINISTRATOR" && element.State == "APPROVED")
                {
                    long id = long.Parse(element.OrganizationalTarget.Split(':').Last());
                    ownCompaniesId.Add(id);
                }              
            }

            var companiesList = new List<Organization>();

            foreach (var companyId in ownCompaniesId)
            {
                response = await _client.GetAsync($"{_apiHost}v2/organizations/{companyId}");

                if (!response.IsSuccessStatusCode)
                    throw new ApiException(ExceptionModel.FromJson(await response.Content.ReadAsStringAsync()));

                responseJson = await response.Content.ReadAsStringAsync();
                companiesList.Add(Organization.FromJson(responseJson));
            }

            return companiesList.ToArray();
        }

        public async Task PostOnOwnProfileAsync(Share share)
        {
            var selfProfile = await GetOwnProfileAsync();
            share.Owner = $"urn:li:person:{selfProfile.Id}";
            await PostShareAsync(share);
        }

        public async Task PostOnCompanyProfileAsync(Share share, string ownCompanyId)
        {
            share.Owner = $"urn:li:organization:{ownCompanyId}";
            await PostShareAsync(share);
        }

        /// <summary>
        /// If you don't specify sharesPerOwner, the default is 1. That means that you only get 1 element in your result set. We recommend setting the sharesPerOwner to 1,000 and count to 50, which means the endpoint returns up to 1,000 shares per owner, while the total elements returned per response is 50. To get the next 50 of 1,000, paginate with the start query parameter.
        /// </summary>
        /// <param name="sharesPerOwner"></param>
        /// <returns></returns>
        public async Task<EntityElements<Share>> GetPostsOnOwnProfileAsync(int sharesPerOwner = 100)
        {
            var selfProfile = await GetOwnProfileAsync();
            var owner = $"urn:li:person:{selfProfile.Id}";           
            return await GetPostsAsync(owner, sharesPerOwner);
        }

        /// <summary>
        /// If you don't specify sharesPerOwner, the default is 1. That means that you only get 1 element in your result set. We recommend setting the sharesPerOwner to 1,000 and count to 50, which means the endpoint returns up to 1,000 shares per owner, while the total elements returned per response is 50. To get the next 50 of 1,000, paginate with the start query parameter.
        /// </summary>
        /// <param name="sharesPerOwner"></param>
        /// <param name="ownCompanyId">Company id which have ADMINISTRATOR role</param>
        /// <returns></returns>
        public async Task<EntityElements<Share>> GetPostsOnCompanyProfileAsync(string ownCompanyId, int sharesPerOwner = 100)
        {
            var owner = $"urn:li:organization:{ownCompanyId}";         
            return await GetPostsAsync(owner, sharesPerOwner);
        }

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

        private async Task<EntityElements<Share>> GetPostsAsync(string ownerURN, int sharesPerOwner = 100)
        {
            CheckTokenThenAddToHeaders();
            var response = await _client.GetAsync($"{_apiHost}v2/shares?q=owners&owners={ownerURN}&sharesPerOwner={sharesPerOwner}");
            var responseJson = await response.Content.ReadAsStringAsync();
            var shares = EntityElements<Share>.FromJson(responseJson);

            if (!response.IsSuccessStatusCode)
                throw new ApiException(ExceptionModel.FromJson(responseJson));

            return shares;
        }

        public async Task PostShareAsync(Share share)
        {
            CheckTokenThenAddToHeaders();
            var content = new StringContent(share.ToJson(), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{_apiHost}v2/shares", content);

            if (!response.IsSuccessStatusCode)
                throw new ApiException(ExceptionModel.FromJson(await response.Content.ReadAsStringAsync()));
        }
    }
}
