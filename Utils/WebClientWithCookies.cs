using System.Net;

namespace RussianRailwaysUtility.Utils {
    public class WebClientWithCookies : WebClient {
        public readonly CookieContainer cc = new CookieContainer();
        protected override WebRequest GetWebRequest(System.Uri address) {
            WebRequest request = base.GetWebRequest(address);
            HttpWebRequest WR = request as HttpWebRequest;
            if (WR != null) {
                WR.CookieContainer = cc;
            }
            return request;
        }
    }
}
