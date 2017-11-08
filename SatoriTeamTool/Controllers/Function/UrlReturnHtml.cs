using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SatoriTeamTool.Controllers.Function
{
    class UrlReturnHtml
    {
        public string getPageHtml(string url)
        {
            WebClient client = new WebClient();
            string UrlAddress = url;

            //1.Create the request object
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlAddress);
            //request.AllowAutoRedirect = true;
            //request.MaximumAutomaticRedirections = 200;
            request.Proxy = null;
            request.UseDefaultCredentials = true;

            //2.Add the container with the active 
            CookieContainer cc = new CookieContainer();

            //3.Must assing a cookie container for the request to pull the cookies
            request.CookieContainer = cc;

            return request.ToString();
        }
    }
}
