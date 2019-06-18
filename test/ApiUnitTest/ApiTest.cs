using System;
using Xunit;
using LinkedIn.Api;
using LinkedIn.Api.People;

namespace ApiUnitTest
{
    public class ApiTest
    {
        private readonly string clientId = "77cme2vqhhzgyz";
        private readonly string clientSecret = "Nj4oDp7u8OC9FkJt";
        private readonly string token = "AQUrSPHI-a43ZER82wbasxt3N3PZSFvD_a-fgnNdfEDApxlUGtPMi5e_gEa3TghyjYWCCcmTo0OEt_SOmhIzbo-DPpLKFCox5uKKFvinR61gta7BemMOW5YV5F63RYZLshPXCSYhQcd1DWWWVqBuVCeyrvFDPj-WZVj3C-Nc7AEXyYfeAciR0H0-vgnqEjOyjdONRaaufM7hrg-XguzRpKyTeoTpQmdPE09O-bTC3kyKsDFA1H3zY2xklUzTTSjf2VdoboVPR4bt-507Jv-W0kuxRTQdRMQOQ7sqI5j0dIAuQiL6_S_Jr7M7s5Jw3CMiMIYPazRc_GAzsub0bt-rPVQUkBH1Hg";
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
            var profileJson = profile.ToJson();

            Assert.Contains("id", profileJson);
            Assert.Contains("firstName", profileJson);
            Assert.Contains("lastName", profileJson);
            Assert.Contains("profilePicture", profileJson);
        }
    }
}
