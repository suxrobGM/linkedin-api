using System;
using Xunit;
using LinkedIn.Api;

namespace ApiUnitTest
{
    public class ApiTest
    {
        private readonly string clientId = "YOUR_CLIENT_ID";
        private readonly string clientSecret = "YOUR_CLIENT_SECRET";
        private readonly string token = "YOUR_ACCESS_TOKEN";
        private readonly Uri redirectUrl = new Uri("https://your-app-redirect-url.com");
        private readonly Client client;

        public ApiTest()
        {
            client = new Client(clientId, clientSecret, redirectUrl);
            client.AccessToken = token;
        }

        [Fact]
        public void GetUrlMethod()
        {
            var permissions = new string[] { "r_liteprofile", "r_emailaddress", "w_member_social" };
            var authUrl = client.GetAuthorizationUrl(permissions);
            var authUrlQuery = authUrl.Query.Split('&');

            foreach (var query in authUrlQuery)
            {
                var pair = query.Split("=");

                if (pair[0] == "?response_type")
                {
                    Assert.Contains("code", pair[1]);
                }
                if (pair[0] == "client_id")
                {
                    Assert.Contains(clientId, pair[1]);
                }
                if (pair[0] == "redirect_uri")
                {
                    Assert.Contains(redirectUrl.OriginalString, pair[1]);
                }
                if (pair[0] == "scope")
                {
                    Assert.Contains("r_liteprofile%20r_emailaddress%20w_member_social", pair[1]);
                }
            }            
        }

        [Fact]
        public async void GetOwnProfileMethod()
        {
            var profile = await client.GetOwnProfileAsync();
        }
    }
}
