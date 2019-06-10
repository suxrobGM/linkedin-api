using System;
using System.IO;
using LinkedIn.Api;
using LinkedIn.Api.SocialAction;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string clientId = "77cme2vqhhzgyz";
            string clientSecret = "Nj4oDp7u8OC9FkJt";
            string authCode = "AQS0JNkWL0Nv9HLGSCwGkgehOAQF8NpfzVDigyTXDRZdm2E2AtrKHRBH2zWbHxSrsGp4fDOZN74-poWD0gC26zr7B-luYylIHU3Ql9sBpoWv1ByUn0Gy1zvhicFxVd13bBzjYLimIzqOr9TDO0hn9qXF2ioocpv-c2LpeX2SPfbBcS7tdyHk8JWpSfBtOQ";
            var redirectUrl = new Uri("http://suxrobgm.net");
            var client = new Client(clientId, clientSecret, redirectUrl);
            client.Token = "AQUrSPHI-a43ZER82wbasxt3N3PZSFvD_a-fgnNdfEDApxlUGtPMi5e_gEa3TghyjYWCCcmTo0OEt_SOmhIzbo-DPpLKFCox5uKKFvinR61gta7BemMOW5YV5F63RYZLshPXCSYhQcd1DWWWVqBuVCeyrvFDPj-WZVj3C-Nc7AEXyYfeAciR0H0-vgnqEjOyjdONRaaufM7hrg-XguzRpKyTeoTpQmdPE09O-bTC3kyKsDFA1H3zY2xklUzTTSjf2VdoboVPR4bt-507Jv-W0kuxRTQdRMQOQ7sqI5j0dIAuQiL6_S_Jr7M7s5Jw3CMiMIYPazRc_GAzsub0bt-rPVQUkBH1Hg";
            //string authUrl = client.GetAuthorizationUrl().AbsoluteUri;
            //client.GetAccessTokenAsync(authCode).Wait();
            //client.GetOwnProfileAsync().Wait();
            //client.GetCompaniesAsync().Wait();
            //string shareJson = File.ReadAllText("ShareJson.txt");
            //var share = Share.FromJson(shareJson);
            //client.PostOnOwnProfileAsync(share).Wait();
            //client.GetPostsOnOwnProfileAsync().Wait();

            Console.WriteLine("\nFinished!");
            Console.ReadKey();
        }
    }
}
