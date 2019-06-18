# LinkedInApi
.NET wrapper of LinkedIn API v2

## Quick start
For fully using all profile and organization APIs you app should be have following permissions:
- w_member_social - Post, comment and like posts on behalf of an authenticated member.
- r_member_social - Retrieve posts, comments, and likes on behalf of an authenticated member. This permission is granted to select developers only.
- w_organization_social - Post, comment and like posts on behalf of an organization. Restricted to organizations in which the authenticated member has one of the following company page roles: ADMINISTRATOR, DIRECT_SPONSORED_CONTENT_POSTER, RECRUITING_POSTER
- r_organization_social - Retrieve organizations' posts, comments, and likes. Restricted to organizations in which the authenticated member has one of the following company page roles: ADMINISTRATOR, DIRECT_SPONSORED_CONTENT_POSTER


You can use api client by using class `LinkedIn.Api.Client`

### Example
````csharp
Client client = new Client("YOUR_APP_ID", "YOUR_APP_SECRET", new Uri("https://your-app-redirect-url.com"));
string[] permissions = new string[] { "r_liteprofile", "r_emailaddress", "w_member_social" };

// To get 3-legged authorization url use GetAuthorizationUrl method
string authUrl = client.GetAuthorizationUrl(permissions);

// To get access token use GetAccessTokenAsync method
AccessToken token = await client.GetAccessTokenAsync("AUTHORIZATION_CODE");

// Or you can use existing access token by setting Token property
client.AccessToken = "YOUR_ACCESS_TOKEN";

// Get data information about authorized user, if your access token invalid you will get ApiException error
Profile user = await client.GetOwnProfileAsync();

// Get list of companies which authorized user have approved ADMINISTRATOR role on these companies, required r_organization permission
Organization[] organizations = await client.GetCompaniesAsync();

// Get list of shares in authorized user, required r_member_social permission
EntityElements<Share> shares = await client.GetPostsOnOwnProfileAsync();

// Get list of posts in company page, required r_organization_social permission
EntityElements<Share> companyShares = await client.GetPostsOnCompanyProfileAsync("COMPANY_ID");
````

### Example how to post shares in own profile 
To post share on own profile you need w_member_social permission.
Also you can see full details on [Microsoft documentation](https://docs.microsoft.com/en-us/linkedin/marketing/integrations/community-management/shares/share-api?context=linkedin/compliance/context#post-shares)

Method 1: post share with existing json data.

For example we have followed json data for request:
````json
{
    "content": {
        "contentEntities": [
            {
                "entityLocation": "https://www.example.com/content.html",
                "thumbnails": [
                    {
                        "resolvedUrl": "https://www.example.com/image.jpg"
                    }
                ]
            }
        ],
        "title": "Test Share with Content"
    },
    "distribution": {
        "linkedInDistributionTarget": {}
    },
    "subject": "Test Share Subject",
    "text": {
        "text": "Test Share!"
    }
}
````

````csharp
// Post share in authorized user profile, required w_member_social permission
Share share = Share.FromJson(jsonData); 
Share postedShare = await client.PostOnOwnProfileAsync(share);
````

Method 2: post with `LinkedIn.Api.SocialActions.Share` class

We can programatically create share then will post on LinkedIn

For example:
````csharp
var contentEntity = new ContentEntity(); //share content
contentEntity.EntityLocation = new Uri("https://www.example.com/content.html");
contentEntity.Thumbnails.Add(new ShareThumbnails() { ResolvedUrl = new Uri("https://www.example.com/image.jpg") });

var share = new Share();
share.Content.ContentEntities.Add(contentEntity);
share.Content.Title = "Test Share with Content";
share.Subject = "Test Share Subject";
share.Text.Content = "Test Share!";

var postedShare = await client.PostOnOwnProfileAsync(share);
````

Posting on company it similar like posting on own profile. But required w_organization_social permission

For example:
````csharp
// Post share in organiztion page, required w_organization_social permission also user should be have one of the following company page roles: ADMINISTRATOR, DIRECT_SPONSORED_CONTENT_POSTER, RECRUITING_POSTER
Share newPost = Share.FromJson(jsonData);
Share postedShare = await client.PostOnCompanyProfileAsync(newPost, "COMPANY_ID");
````

### How to post rich media share?
The Rich Media Share API allows you to upload images to reference in a personal or organization share. With this API, you can create rich and engaging personal or organization shares that appear on various feeds within the LinkedIn ecosystem. You must be a company administrator to create an organization share.

To create a rich media share:
1. Upload the media to LinkedIn's media platform.
2. Make a personal or organization share referencing that media.

Example:
````csharp
var uploadedMedia = await client.UploadRichMediaAsync("path/to/media/file/example.jpeg");

var contentEntity = new ContentEntity(useThumbnails: false); //share content
contentEntity.EntityUrn = uploadedMedia.LocationUrn; //add rich media urn to content entity

var share = new Share();
share.Content.ContentEntities.Add(contentEntity);
share.Content.Description = "Content description";
share.Content.Title = "Test Share with Content";
share.Subject = "Test Share Subject";
share.Text.Content = "Test Share!";

var postedShare = await client.PostOnOwnProfileAsync(share);
````
**Note:** Rich media is deleted when a member deletes all shares referencing the media. Attempting to post a share referencing deleted media results in an error.

To post a multi-image share, upload multiple images to be specified in `ContentEntities` property