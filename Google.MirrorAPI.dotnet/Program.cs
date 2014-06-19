using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Apis.Analytics.v3.Data;
using Google.Apis.Analytics.v3;
using System.Globalization;
using System.IO;





//Project must be .net 4
// Install-Package Google.Apis.Mirror.v1 
// Install-Package Google.Apis.Analytics.v3
// https://developers.google.com/glass/tools-downloads/playground
// https://developers.google.com/glass/design/style

namespace Google.MirrorAPI.dotnet
{

    public class AccountInfo
    {

        public string AccountName = string.Empty;
        public string WebPeropertyName = string.Empty;
        public string ProfileName = string.Empty;

        public AccountInfo(string AccountName, String WebPropertyName, String ProfileName)
        {

            this.AccountName = AccountName;
            this.WebPeropertyName = WebPropertyName;
            this.ProfileName = ProfileName;


        }

    }

    class Program
    {
       
        static void Main(string[] args)
        {

            if (args.Count() != 2)
            {
                log("Invalid Request:  GoogleAnaltyicsGlass <profileid> <min number of users>");
            }

            int ProfileId;
            bool IsInt = Int32.TryParse(args[0], out ProfileId);

            if (!IsInt)
            {
                log("Invalid Request: Profile Id Must be a number. \r\n GoogleAnaltyicsGlass <profileid> <min number of users>");
            }

            int MinUsers;
            IsInt = Int32.TryParse(args[1], out MinUsers);
            if (!IsInt)
            {
                log("Invalid Request: minimum number of users must be a number. \r\n GoogleAnaltyicsGlass <profileid> <min number of users>");

            }

            const string ClientId = "563519685488.apps.googleusercontent.com";
            const string ClientSecret = "r39kMJxsh8RC38WPPRfI4ZiU";

            Autentication GoogleAutentication = Autentication.Get(ClientId, ClientSecret);
            var RealTimeRequest = GoogleAutentication.AnalyticsService.Data.Realtime.Get("ga:" + ProfileId.ToString(), "rt:activeUsers");
            try
            {
                var RealTimeResults = RealTimeRequest.Execute();

                if (RealTimeResults.TotalResults > 0)
                {
                    int ActiveUsersNow = Int32.Parse(RealTimeResults.Rows[0][0]);
                    if (ActiveUsersNow >= MinUsers)
                    {
                        AccountInfo MyAccountInfo = FindAccountInfo(GoogleAutentication.AnalyticsService, ProfileId);
                        Apis.Mirror.v1.Data.TimelineItem TimeLine = new Apis.Mirror.v1.Data.TimelineItem();
                        TimeLine.Html = "<article class=\"auto-paginate\"> <center><p class=\"yellow text-small\">Google Analytics<p> <p class=\"green text-minor\">Real-Time<p> <p class=\"gray text-minor\">" + MyAccountInfo.AccountName + " -> " + MyAccountInfo.WebPeropertyName + " -> " + MyAccountInfo.ProfileName + "</p> <p class=\"blue text-large\">" + ActiveUsersNow.ToString("N0",CultureInfo.CurrentCulture) + "</p> <p class=\"green text-minor\"> users</p></center> </article>";
                        var o = GoogleAutentication.GlassService.Timeline.Insert(TimeLine).Execute();

                        log("Wrote to time-line");
                    }
                    else
                    {                     
                        log("User count to low not writing to glass");
                    }
                }
                else
                {
                    log("No users not writing to glass");                    
                }
            }
            catch (Exception ex)
            {
                log(ex.Message);
            }
        }


        /// <summary>
        /// Retrieves a full list of Accounts, Web Properties and Views for a Authenticated user.
        /// </summary>
        /// <param name="service">Analytics service</param>
        /// <returns></returns>
        private static AccountSummaries AccountSummaryList(AnalyticsService service)
        {
            ManagementResource.AccountSummariesResource.ListRequest request = service.Management.AccountSummaries.List();
            try
            {
                AccountSummaries result = request.Execute();
                return result;
            }
            catch (Exception)
            {
                return null;
            }

        }


        private static void log(String logtext)
        {

            using (StreamWriter w = File.AppendText("log.txt"))
            {
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), logtext);
            }
        }

        private static AccountInfo FindAccountInfo(AnalyticsService service, int ProfileId)
        {
            foreach (AccountSummary account in AccountSummaryList(service).Items)
            {
                foreach (var WP in account.WebProperties)
                {

                    if (WP.Profiles != null)
                    {
                        foreach (var profile in WP.Profiles)
                        {
                            if (profile.Id == ProfileId.ToString())
                            {                                
                                return new AccountInfo(account.Name, WP.Name, profile.Name);
                            }


                        }
                    }

                }

            }

            return null;

        }


    }
}
