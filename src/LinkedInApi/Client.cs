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

        /// <summary>
        /// Create new client instance of API v2 
        /// </summary>
        /// <param name="clientId">Your LinkedIn app Id</param>
        /// <param name="clientSecret">Your LinkedIn app secret</param>
        /// <param name="redirectUrl">Your LinkedIn app redirect url</param>
        public Client(string clientId, string clientSecret, Uri redirectUrl)
        {
            _authHost = new Uri("https://www.linkedin.com");
            _apiHost = new Uri("https://api.linkedin.com");
            _client = new HttpClient();
            _clientId = clientId;
            _clientSecret = clientSecret;
            _redirectUrl = redirectUrl;                       
        }

        /// <summary>
        /// Get authorization code flow (3-legged) url
        /// </summary>
        /// <returns></returns>
        public Uri GetAuthorizationUrl()
        {
            return new Uri($"{_authHost}oauth/v2/authorization?response_type=code&client_id={_clientId}&redirect_uri={_redirectUrl.OriginalString}&scope=r_liteprofile%20r_emailaddress%20w_member_social");
        }

        /// <summary>
        /// Get access token by auth code
        /// </summary>
        /// <param name="authCode">Authorization code after user approved</param>
        /// <exception cref="ApiException"></exception>
        /// <returns></returns>
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

        /// <summary>
        /// Get information about authorized user, if your access token invalid you will get ApiException error
        /// </summary>
        /// <exception cref="ApiException"></exception>
        /// <returns></returns>
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

        /// <summary>
        /// Get list of companies which authorized user have approved ADMINISTRATOR role on these companies, required r_organization permission
        /// </summary>
        /// <exception cref="ApiException"></exception>
        /// <returns></returns>
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

        /// <summary>
        /// Post share in authorized user profile, required w_member_social permission 
        /// Full details see on https://docs.microsoft.com/en-us/linkedin/marketing/integrations/community-management/shares/share-api?context=linkedin/compliance/context#post-shares
        /// </summary>
        /// <param name="share"></param>
        /// <exception cref="ApiException"></exception>
        /// <returns></returns>
        public async Task PostOnOwnProfileAsync(Share share)
        {
            var selfProfile = await GetOwnProfileAsync();
            share.Owner = $"urn:li:person:{selfProfile.Id}";
            await PostShareAsync(share);
        }

        /// <summary>
        /// Post share in organiztion page, required w_organization_social permission also user should be have one of the following company page roles: ADMINISTRATOR, DIRECT_SPONSORED_CONTENT_POSTER, RECRUITING_POSTER
        /// Full details see on https://docs.microsoft.com/en-us/linkedin/marketing/integrations/community-management/shares/share-api?context=linkedin/compliance/context#post-shares
        /// </summary>
        /// <param name="share"></param>
        /// <param name="ownCompanyId">Company id which user have ADMINISTRATOR or DIRECT_SPONSORED_CONTENT_POSTER or RECRUITING_POSTER role</param>
        /// <exception cref="ApiException"></exception>
        /// <returns></returns>
        public async Task PostOnCompanyProfileAsync(Share share, string ownCompanyId)
        {
            share.Owner = $"urn:li:organization:{ownCompanyId}";
            await PostShareAsync(share);
        }

        /// <summary>
        /// Get list of shares in authorized user, required r_member_social permission
        /// </summary>
        /// <param name="sharesPerOwner">If you don't specify sharesPerOwner, the default is 1. That means that you only get 1 element in your result set. We recommend setting the sharesPerOwner to 1,000 and count to 50, which means the endpoint returns up to 1,000 shares per owner, while the total elements returned per response is 50. To get the next 50 of 1,000, paginate with the start query parameter.</param>
        /// <exception cref="ApiException"></exception>
        /// <returns>Elements of Share class</returns>
        public async Task<EntityElements<Share>> GetPostsOnOwnProfileAsync(int sharesPerOwner = 100)
        {
            var selfProfile = await GetOwnProfileAsync();
            var owner = $"urn:li:person:{selfProfile.Id}";           
            return await GetPostsAsync(owner, sharesPerOwner);
        }

        /// <summary>
        /// Get list of posts in company page, required r_organization_social permission
        /// </summary>
        /// <param name="sharesPerOwner">If you don't specify sharesPerOwner, the default is 1. That means that you only get 1 element in your result set. We recommend setting the sharesPerOwner to 1,000 and count to 50, which means the endpoint returns up to 1,000 shares per owner, while the total elements returned per response is 50. To get the next 50 of 1,000, paginate with the start query parameter.</param>
        /// <param name="ownCompanyId">Company id which user have ADMINISTRATOR or DIRECT_SPONSORED_CONTENT_POSTER role</param>
        /// <exception cref="ApiException"></exception>
        /// <returns>Elements of Share class</returns>
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

        private async Task PostShareAsync(Share share)
        {
            CheckTokenThenAddToHeaders();
            var content = new StringContent(share.ToJson(), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{_apiHost}v2/shares", content);

            if (!response.IsSuccessStatusCode)
                throw new ApiException(ExceptionModel.FromJson(await response.Content.ReadAsStringAsync()));
        }
    }
}
