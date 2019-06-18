using System;
using Xunit;
using LinkedIn.Api;
using LinkedIn.Api.People;
using LinkedIn.Api.Organizations;
using LinkedIn.Api.SocialActions;

namespace ApiUnitTest
{
    public class ApiTest
    {
        private readonly string clientId = "YOUR_APP_ID";
        private readonly string clientSecret = "YOUR_APP_SECRET";
        private readonly string token = "YOUR_ACCESS_TOKEN";
        private readonly Uri redirectUrl = new Uri("https://your-app-redirect-url.com");
        private readonly Client client;

        public ApiTest()
        {
            client = new Client(clientId, clientSecret, redirectUrl)
            {
                AccessToken = token
            };
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

        [Fact]
        public async void PostOnOwnProfileMethod()
        {
            var jsonData = @"{
                                'content': {
                                    'contentEntities': [
                                        {
                                            'entityLocation': 'https://www.example.com/content.html',
                                            'thumbnails': [
                                                {
                                                    'resolvedUrl': 'https://www.example.com/image.jpg'
                                                }
                                            ]
                                        }
                                    ],
                                    'title': 'Test Share with Content'
                                },
                                'distribution': {
                                    'linkedInDistributionTarget': {}
                                },
                                'subject': 'Test Share Subject from LinkedIn API v2',
                                'text': {
                                    'text': 'Test Share!'
                                }
                            }";

            var share = Share.FromJson(jsonData);

            //var contentEntity = new ContentEntity();
            //contentEntity.EntityLocation = new Uri("https://www.example.com/content.html");
            //contentEntity.Thumbnails.Add(new ShareThumbnails() { ResolvedUrl = new Uri("http://suxrobgm.net/Blogs/Introducing-NET-5") });

            //var share = new Share();
            //share.Content.ContentEntities.Add(contentEntity);
            //share.Content.Title = "Test Share with Content";
            //share.Subject = "Test Share Subject";
            //share.Text.Content = "Test Share!";

            var postedShare = await client.PostOnOwnProfileAsync(share);
            var postedShareJson = postedShare.ToJson();

            Assert.Contains("id", postedShareJson);
            Assert.Contains("created", postedShareJson);
            Assert.Contains("edited", postedShareJson);
            Assert.Contains("linkedInDistributionTarget", postedShareJson);
            Assert.Contains("lastModified", postedShareJson);
            Assert.Contains("owner", postedShareJson);
            Assert.Contains("subject", postedShareJson);
            Assert.Contains("text", postedShareJson);
        }

        //[Fact]
        //public async void GetCompaniesMethod()
        //{
        //    var companies = await client.GetCompaniesAsync();

        //    foreach (var company in companies)
        //    {
        //        var companyJson = company.ToJson();

        //        Assert.Contains("id", companyJson);
        //        Assert.Contains("localizedName", companyJson);
        //        Assert.Contains("name", companyJson);
        //        Assert.Contains("vanityName", companyJson);
        //    }
        //}
    }
}
