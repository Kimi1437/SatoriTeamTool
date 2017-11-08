using SatoriTeamTool.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SatoriTeamTool.Controllers
{
    public class testController : Controller
    {
        private SatoriDBContext db = new SatoriDBContext();
        // GET: test
        public ActionResult Index()
        {
           
            //WebClient client = new WebClient();
            //string UrlAddress = "https://wrapstar.bing.net/Dashboard/Alert?top=10000&orderby=%5BDate%5D+DESC%2C+%5BVersion%5D+DESC&where=([Date]='2016-03-20') AND (dbo.[ContainToken]([Notification],'wsdatap@microsoft.com',';')=1)";
            //string result;

            ////1.Create the request object
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlAddress);
            ////request.AllowAutoRedirect = true;
            ////request.MaximumAutomaticRedirections = 200;
            //request.Proxy = null;
            //request.UseDefaultCredentials = true;

            ////2.Add the container with the active 
            //CookieContainer cc = new CookieContainer();

            ////3.Must assing a cookie container for the request to pull the cookies
            //request.CookieContainer = cc;

            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            //{
            //    result = sr.ReadToEnd();
            //    Regex reg = new Regex(@"(?<DATE>\d{2}/\d{2}/\d{4})</div>\s+</td>\s+<td>\s+<div [^>]+>(?<Ontology>[a-zA-Z_]+)</div>\s+</td>\s+<td>\s+<div [^>]+>(?<ModelID>\d{1,5})</div>\s+</td>\s+<td>\s+<div [^>]+>(?<VersionID>\d{1,5})</div>\s+</td>\s+<td>\s+<div [^>]+>(?<ModelName>[^<>]+)</div>\s+</td>\s+<td>\s+<div [^>]+>(?<ModelGroups>[^<>]+)</div>\s+</td>\s+<td>\s+<div [^>]+>(?<AlertType>[^<>]+)</div>\s+</td>\s+<td>\s+<div [^>]+>(?<AlertMessage>[^<>]+)</div>\s+</td>\s+<td>\s+");
            //    MatchCollection mc = reg.Matches(result);
            //    foreach (Match m in mc)
            //    {
            //        if (m.Success)
            //        {
            //            NeedCheckOrFixModel test = new NeedCheckOrFixModel();
            //            switch (m.Groups["AlertType"].Value)
            //            {
            //                case "Extraction":
            //                    test.ModelAlertType = AlertType.Extraction;
            //                    break;
            //                case "Regression":
            //                    test.ModelAlertType = AlertType.Regression;
            //                    break;
            //                case "Warning":
            //                    test.ModelAlertType = AlertType.Warning;
            //                    break;
            //                case "verification":
            //                    test.ModelAlertType = AlertType.Verification;
            //                    break;
            //            }
            //            int id = 27;
            //            test.ID = id + 1;
            //            test.ModelDate = Convert.ToDateTime(m.Groups["DATE"].Value);
            //            test.Ontology = m.Groups["Ontology"].Value;
            //            test.ModelID = Convert.ToInt32(m.Groups["ModelID"].Value);
            //            test.VersionID = Convert.ToInt32(m.Groups["VersionID"].Value);
            //            test.ModelName = m.Groups["ModelName"].Value;
            //            test.ModelOwner = m.Groups["ModelGroups"].Value;
            //            //test.ModelAlertType = AlertType.Verification;
            //            test.IssueSymptom = m.Groups["AlertMessage"].Value;
            //            test.Action = "need fix";
            //            test.CauseAnalysis = "test";


            //            db.NeedCheckOrFixModels.Add(test);
            //            db.SaveChanges();


            //        }
            //    }

            //    if (result.Contains("head"))
            //    {
            //        result = "yes";
            //    }
            //    //Close and clean up the StreamReader
            //    //(?<DATE>\d{2}/\d{2}/\d{4})</div> </td> <td> <div [^>]+>(?<Ontology>[a-zA-Z_]+) [\p{L}-]+( \([^()]\))?(, [\p{L}-]+( \([^()]\))?)*
            //    sr.Close();
            //}

            ///*
            //WebClient MyWebClient = new WebClient();


            //MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据

            //Byte[] pageData = MyWebClient.DownloadData("https://wrapstar.bing.net/Dashboard/Alert?where=([Date]='2016-03-20') AND (dbo.[ContainToken]([Notification],'wsdatap@microsoft.com',';')=1)"); //从指定网站下载数据

            //string pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句            

            ////string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句
            //*/
            ViewBag.Message = "result";
            ViewBag.Date = DateTime.Now;

            return View();
        }
    }
}