using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SatoriTeamTool.Controllers
{
    public class VerificationAlertController : Controller
    {
        // GET: VerificationAlert
        public ActionResult Index(string  date, string versionID, string select)
        {
            string[] texts = new string[] { "200", "300" };
            versionID = string.IsNullOrEmpty(versionID) ? "1" : versionID;
            List<SelectListItem> select1 = new List<SelectListItem>();
            for (int i = 0; i < texts.Length; i++)
            {
                select1.Add(new SelectListItem
                {
                    Text = texts[i],
                    Value = texts[i],
                });
            };
            ViewData["select"] = new SelectList(select1, "Value", "Text", "200");
            string datetimemeeage = Convert.ToDateTime(date).ToString("MM/dd/yyyy");
            string urltext = "https://wrapstar.bing.net/Report/Verification?date=" + datetimemeeage + "%2012:00:00%20AM&vid=" + versionID + "&http=200&islatest=TRUE&envid=1";

            string result;
            //1.Create the request object
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urltext);
            //request.AllowAutoRedirect = true;
             request.Proxy = null;
           //request.MaximumAutomaticRedirections = 200;
            request.UseDefaultCredentials = true;

            //2.Add the container with the active 
            CookieContainer cc = new CookieContainer();

            //3.Must assing a cookie container for the request to pull the cookies
            request.CookieContainer = cc;

            HttpWebResponse response1 = (HttpWebResponse)request.GetResponse();
            List<string> urlvalues = new List<string>();
            using (StreamReader sr = new StreamReader(response1.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                Regex reg = new Regex(@">\s*(?<URL>http(s)?://.+)</a>(\s+)?</div>");
                MatchCollection mc = reg.Matches(result);
                foreach (Match m in mc)
                {
                    if (m.Success)
                    {
                        urlvalues.Add(m.Groups["URL"].Value);
                    }
                }
                ViewBag.UrlMessage = urlvalues;
                sr.Close();

                var path = "C:/Users/"+ Environment.UserName+ "/Desktop/url.txt";

                FileStream fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入
                //sw.BaseStream.Seek(0, SeekOrigin.Begin);
                for (int i = 0; i < urlvalues.Count(); i++) sw.WriteLine(urlvalues[i]);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }

            ViewBag.Message = urltext;
            return View();
        }

        //public ViewResult getVerificationAlert(DateTime date, string versionID)
        //{
        //    string datetimemeeage = Convert.ToDateTime(date).ToString("MM/dd/yyyy");
        //    string urltext = "https://wrapstar.bing.net/Report/Verification?date=" + datetimemeeage + "%2012:00:00%20AM&vid=" + versionID + "&http=200&islatest=TRUE&envid=1";
        //    ViewBag.Message = "urltext";
        //    return View();
        //}
    }
}

//https://wrapstar.bing.net/Report/Verification?date=2016-3-17%2012:00:00%20AM&vid=52158&http=200&islatest=TRUE&envid=1