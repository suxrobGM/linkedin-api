# LinkedInApi
.NET wrapper of LinkedIn API v2

## Quick start
For fully using all profile and organization APIs you app should be have following permissions:
- r_liteprofile - Required to retrieve name and photo for the authenticated user.
- w_member_social - Post, comment and like posts on behalf of an authenticated member.
- w_organization_social - Post, comment and like posts on behalf of an organization. Restricted to organizations in which the authenticated member has one of the following company page roles: ADMINISTRATOR, DIRECT_SPONSORED_CONTENT_POSTER, RECRUITING_POSTER


You can use api client by using class `LinkedIn.Api.Client`

### Example
````csharp
Client client = new Client("YOUR_APP_ID", "YOUR_APP_SECRET", new Uri("https://your-app-redirect-url.com"));

// To get 3-legged authorization url use GetAuthorizationUrl method
string authUrl = client.GetAuthorizationUrl();

// To get access token use GetAccessTokenAsync method
string token = await client.GetAccessTokenAsync("AUTHORIZATION_CODE");

// Or you can use existing access token by setting Token property
client.Token = "YOUR_ACCESS_TOKEN";

// Get data information about authorized user, if your access token invalid you will get ApiException error
Profile user = await client.GetOwnProfileAsync();

// Get list of companies which authorized user have approved ADMINISTRATOR role on these companies, required r_organization permission
Organization[] organizations = await client.GetCompaniesAsync();

// Post share in authorized user, required w_member_social permission
Share newPost = Share.FromJson(jsonData); // or with new Share() { } with required properties, for full details see https://docs.microsoft.com/en-us/linkedin/marketing/integrations/community-management/shares/share-api?context=linkedin/compliance/context#post-shares
await client.PostOnOwnProfileAsync(newPost);

// Post share in organiztion page, required w_organization_social permission also user should be have one of the following company page roles: ADMINISTRATOR, DIRECT_SPONSORED_CONTENT_POSTER, RECRUITING_POSTER
Share newPost = Share.FromJson(jsonData);
await client.GetPostsOnCompanyProfileAsync(newPost, "COMPANY_ID");

// Get list of shares in authorized user
EntityElements<Share> shares = await client.GetPostsOnOwnProfileAsync();

// Get list of posts in company page
EntityElements<Share> companyShares = await client.GetPostsOnCompanyProfileAsync("COMPANY_ID");
````

