using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Apis.Mirror.v1;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using Google.Apis.Analytics.v3;


namespace Google.MirrorAPI.dotnet
{
   public class Autentication
    {

        public MirrorService GlassService;
        public AnalyticsService AnalyticsService;
        public UserCredential credential;

        public static Autentication Get(string _client_id, string _client_secret)
        {
            Autentication Autenticated = new Autentication();


            try
            {
                Autenticated.credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = _client_id, ClientSecret = _client_secret },
                                                                         new[] { MirrorService.Scope.GlassTimeline , AnalyticsService.Scope.AnalyticsReadonly},
                                                                         "LindaGlass",
                                                                         CancellationToken.None,
                                                                         new FileDataStore("Real-timeMirror.Auth.Store")).Result;
            }
            catch (Exception)
            {
                return null;
            }

            if (Autenticated.credential != null)
            {
                // This is how we connect to Google Analytics. Everything you will be requesting will now go though the Service var.
                Autenticated.GlassService = new MirrorService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = Autenticated.credential,
                    ApplicationName = "Mirror API Sample",
                });

                Autenticated.AnalyticsService = new AnalyticsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = Autenticated.credential,
                    ApplicationName = "Mirror API Sample",
                });

                return Autenticated;
            }
            else {

                return null;
            }

            
        }

    }
}
