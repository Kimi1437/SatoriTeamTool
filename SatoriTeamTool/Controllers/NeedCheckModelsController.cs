using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SatoriTeamTool.Models;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;
using SatoriTeamTool.ViewModel;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using PagedList;
using System.Security.Principal;
using SatoriTeamTool.Controllers.Function.MyApplication;
//using Microsoft.TeamFoundation.WorkItemTracking.Controls;

namespace SatoriTeamTool.Controllers
{
    public class NeedCheckModelsController : Controller
    {
        private SatoriDBContext db = new SatoriDBContext();
        //string datetimegGlobak= "";
        // GET: NeedCheckModels
        public ActionResult Index(string action, string versionID, string date, string currentFilter, int? page, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.VersionSortParm = String.IsNullOrEmpty(sortOrder) ? "version_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            if (versionID != null)
            {
                page = 1;
            }
            else
            {
                versionID = currentFilter;
            }

            ViewBag.CurrentFilter = versionID;


            //datetimegGlobak = date;

            var actionList = new List<string>();
            //var modelDateList = new List<string>();

            var action1 = from ac in db.NeedCheckOrFixModels orderby ac.Action select ac.Action;
            //var modeldate = from md in db.NeedCheckOrFixModels orderby md.ModelDate select md.ModelDate;

            //var modeldate1 = modeldate.ToString("yyyy-MM-dd HH:mm:ss");

            actionList.AddRange(action1.Distinct());

            ViewBag.action = new SelectList(actionList);
            ViewBag.datemessage = date;
            var alertmodel = from m in db.NeedCheckOrFixModels select m;
            if (!String.IsNullOrEmpty(versionID))
            {
                alertmodel = alertmodel.Where(s => s.VersionID.ToString().Contains(versionID));
            }

            if (!String.IsNullOrEmpty(date))
            {
                var datetime = Convert.ToDateTime(date);
                alertmodel = alertmodel.Where(d => d.ModelDate.Year == datetime.Year && d.ModelDate.Month == datetime.Month && d.ModelDate.Day == datetime.Day);
            }
            ViewBag.count = alertmodel.Count();
            //alertmodel = alertmodel.OrderBy(v=>v.VersionID);
            switch (sortOrder)
            {
                case "version_desc":
                    alertmodel = alertmodel.OrderByDescending(s => s.VersionID);
                    break;
                case "Date":
                    alertmodel = alertmodel.OrderBy(s => s.ModelDate);
                    break;
                case "date_desc":
                    alertmodel = alertmodel.OrderByDescending(s => s.ModelDate);
                    break;
                default:  // Name ascending 
                    alertmodel = alertmodel.OrderBy(s => s.VersionID);
                    break;
            }
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            if (action == "Index")
            {
                return View(alertmodel.ToPagedList(pageNumber, pageSize));
            }
            if (String.IsNullOrEmpty(action))
            {
                return View(alertmodel.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View(alertmodel.Where(x => x.Action == action).ToPagedList(pageNumber, pageSize));
            }
            //return View(db.NeedCheckOrFixModels.ToList());
        }
        public ActionResult SearchIndex(string action, string versionID)
        {
            var actionList = new List<string>();
            //var modelDateList = new List<string>();

            var action1 = from ac in db.NeedCheckOrFixModels orderby ac.Action select ac.Action;
            //var modeldate = from md in db.NeedCheckOrFixModels orderby md.ModelDate select md.ModelDate;

            //var modeldate1 = modeldate.ToString("yyyy-MM-dd HH:mm:ss");

            actionList.AddRange(action1.Distinct());

            ViewBag.action = new SelectList(actionList);

            var alertmodel = from m in db.NeedCheckOrFixModels select m;

            if (!String.IsNullOrEmpty(versionID))
            {
                alertmodel = alertmodel.Where(s => s.VersionID.ToString().Contains(versionID));
            }
            if (action == "SearchIndex")
            {
                return View(alertmodel);
            }
            if (String.IsNullOrEmpty(action))
            {
                return View(alertmodel);
            }
            else
            {
                return View(alertmodel.Where(x => x.Action == action));
            }
            //return View(alertmodel);
        }

        // GET: NeedCheckModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NeedCheckOrFixModel needCheckOrFixModel = db.NeedCheckOrFixModels.Find(id);
            if (needCheckOrFixModel == null)
            {
                return HttpNotFound();
            }
            return View(needCheckOrFixModel);
        }

        // GET: NeedCheckModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NeedCheckModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ModelDate,Ontology,ModelID,VersionID,ModelName,ModelOwner,ModelAlertType,IssueSymptom,Action,CauseAnalysis")] NeedCheckOrFixModel needCheckOrFixModel)
        {
            if (ModelState.IsValid)
            {
                db.NeedCheckOrFixModels.Add(needCheckOrFixModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(needCheckOrFixModel);
        }

        // GET: NeedCheckModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NeedCheckOrFixModel needCheckOrFixModel = db.NeedCheckOrFixModels.Find(id);
            if (needCheckOrFixModel == null)
            {
                return HttpNotFound();
            }
            return View(needCheckOrFixModel);
        }

        // POST: NeedCheckModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ModelDate,Ontology,ModelID,VersionID,ModelName,ModelOwner,ModelAlertType,IssueSymptom,Action,CauseAnalysis")] NeedCheckOrFixModel needCheckOrFixModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(needCheckOrFixModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(needCheckOrFixModel);
        }

        // GET: NeedCheckModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NeedCheckOrFixModel needCheckOrFixModel = db.NeedCheckOrFixModels.Find(id);
            if (needCheckOrFixModel == null)
            {
                return HttpNotFound();
            }
            return View(needCheckOrFixModel);
        }

        // POST: NeedCheckModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NeedCheckOrFixModel needCheckOrFixModel = db.NeedCheckOrFixModels.Find(id);
            db.NeedCheckOrFixModels.Remove(needCheckOrFixModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 实现了excel表格导入到数据库，方便以后对数据进行操作
        /// </summary>
        /// <returns></returns>
        public ActionResult Importexcel()
        {


            if (Request.Files["fileupload1"].ContentLength > 0)
            {
                string extension = Path.GetExtension(Request.Files["FileUpload1"].FileName);
                string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/UploadedFolder"), Request.Files["FileUpload1"].FileName);
                if (System.IO.File.Exists(path1))
                    System.IO.File.Delete(path1);

                Request.Files["FileUpload1"].SaveAs(path1);
                //Create the connection string for the Excel file to read it's data. Here ,use an OleDbConnection and call ExecuteReader to read each row from the Excel file. 
                //这里的ConnectionString用web.config里面的链接串，用到的是vs自带的数据库。
                //string sqlConnectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;Database=SatoriDBContext;Trusted_Connection=true;Persist Security Info=True";
                string sqlConnectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Movies.mdf;Integrated Security=True";

                //Create connection string to Excel work book
                string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=Excel 12.0;Persist Security Info=False";
                //Create Connection to Excel work book
                OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                //Create OleDbCommand to fetch data from Excel
                OleDbCommand cmd = new OleDbCommand("Select [ID],[ModelDate],[Ontology],[ModelID],[VersionID],[ModelName],[ModelOwner],[ModelAlertType],[IssueSymptom],[Action],[CauseAnalysis] from [Sheet1$]", excelConnection);

                excelConnection.Open();
                OleDbDataReader dReader;
                dReader = cmd.ExecuteReader();

                SqlBulkCopy sqlBulk = new SqlBulkCopy(sqlConnectionString);
                //Give your Destination table name
                sqlBulk.DestinationTableName = "NeedCheckOrFixModels";
                sqlBulk.WriteToServer(dReader);


                excelConnection.Close();

                // SQL Server Connection String


            }

            return RedirectToAction("Index");
        }
        /// <summary>
        /// 根据输入的日期，得到该日期下面的alert
        /// 还没有做判空处理
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ActionResult GetAlertDate(DateTime date)
        {
            HttpWebRequest request0 = (HttpWebRequest)WebRequest.Create("https://wrapstar.bing.net/Account/LogOn?logon=true");

            request0.Method = "post";
            request0.ContentType = "application/x-www-form-urlencoded";
            request0.ContentLength = 83;
            //request0.Connection = "keep-alive";
            request0.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.112 Safari/537.36";
            request0.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request0.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
            request0.CookieContainer = new CookieContainer();
            request0.CookieContainer.Add(new Cookie("ASP.NET_SessionId", "qenaqndh4thcdp5k5tpfu3iz", "/", "wrapstar.bing.net"));
            request0.CookieContainer.Add(new Cookie("username", "v-junliu@microsoft.com", "/", "wrapstar.bing.net"));
            request0.CookieContainer.Add(new Cookie(".Report-AUTH", "80ADD429549B8A0546488F8E4C4777CC47788B42D3B6E95E84154690B069C4067BD93028A57EBA92A9A0DF555174907DEB75606B4233B97AAC2150D4C191C1B23292D7B38215B4EE937E979F968F533ACB271DBAA2134D86036C7305891EF49CAA61674540B4CE2DC306DC9C8C9E3E9794C35D1F", "/", "wrapstar.bing.net"));
            using (var stream = request0.GetRequestStream())
            {
                stream.Write(Encoding.ASCII.GetBytes("UserName=v-junliu%40microsoft.com&Password=LJJ2135330&RememberMe=false&LogOn=Log+on"), 0, 83);
            }
            HttpWebResponse response0 = (HttpWebResponse)request0.GetResponse();
            using (StreamReader sr0 = new StreamReader(response0.GetResponseStream(), Encoding.GetEncoding(response0.CharacterSet)))
            {
                string result0 = sr0.ReadToEnd();
                sr0.Close();
            }
            string key = request0.CookieContainer.GetCookies(new Uri("https://wrapstar.bing.net"))[".Report-AUTH"].Value;

            string datetimemeeage = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
            WebClient client = new WebClient();
            string UrlAddress = "https://wrapstar.bing.net/Dashboard/Alert?top=10000&orderby=%5BDate%5D+DESC%2C+%5BVersion%5D+DESC&where=([Date]='" + datetimemeeage + "') AND (dbo.[ContainToken]([Notification],'wsdatap@microsoft.com',';')=1)";
            string result;

            //1.Create the request object
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlAddress);
            //request.AllowAutoRedirect = true;
            //request.MaximumAutomaticRedirections = 200;
            request.Proxy = null;
            request.UseDefaultCredentials = true;
            //2.Add the 4 with the active 
            CookieContainer cc = new CookieContainer();

            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Cookie("ASP.NET_SessionId", "qenaqndh4thcdp5k5tpfu3iz", "/", "wrapstar.bing.net"));
            request.CookieContainer.Add(new Cookie("username", "v-junliu@microsoft.com", "/", "wrapstar.bing.net"));
            request.CookieContainer.Add(new Cookie(".Report-AUTH", key, "/", "wrapstar.bing.net"));


            //3.Must assing a cookie container for the request to pull the cookies
            //request.CookieContainer = cc;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                Regex reg = new Regex(@"(?<DATE>\d{2}/\d{2}/\d{4})</div>\s+</td>\s+<td>\s+<div [^>]+>(?<Ontology>[a-zA-Z_]+)</div>\s+</td>\s+<td>\s+<div [^>]+>(?<ModelID>\d{1,5})</div>\s+</td>\s+<td>\s+<div [^>]+>(?<VersionID>\d{1,5})</div>\s+</td>\s+<td>\s+<div [^>]+>(?<ModelName>[^<>]+)</div>\s+</td>\s+<td>\s+<div [^>]+>(?<ModelGroups>[^<>]+)</div>\s+</td>\s+<td>\s+<div [^>]+>(?<AlertType>[^<>]+)</div>\s+</td>\s+<td>\s+<div [^>]+>(?<AlertMessage>[^<>]+)</div>\s+</td>\s+<td>\s+");
                MatchCollection mc = reg.Matches(result);
                foreach (Match m in mc)
                {
                    if (m.Success)
                    {
                        NeedCheckOrFixModel test = new NeedCheckOrFixModel();
                        switch (m.Groups["AlertType"].Value)
                        {
                            case "Extraction":
                                test.ModelAlertType = AlertType.Extraction;
                                break;
                            case "Regression":
                                test.ModelAlertType = AlertType.Regression;
                                break;
                            case "Warning":
                                test.ModelAlertType = AlertType.Warning;
                                break;
                            case "verification":
                                test.ModelAlertType = AlertType.Verification;
                                break;
                        }
                        int id = 27;
                        test.ID = id + 1;
                        test.ModelDate = Convert.ToDateTime(m.Groups["DATE"].Value);
                        test.Ontology = m.Groups["Ontology"].Value;
                        test.ModelID = Convert.ToInt32(m.Groups["ModelID"].Value);
                        test.VersionID = Convert.ToInt32(m.Groups["VersionID"].Value);
                        test.ModelName = m.Groups["ModelName"].Value;
                        test.ModelOwner = m.Groups["ModelGroups"].Value;
                        //test.ModelAlertType = AlertType.Verification;
                        test.IssueSymptom = m.Groups["AlertMessage"].Value;
                        test.Action = "Normal";
                        //test.TeamMembers = null;
                        test.CauseAnalysis = "test";


                        db.NeedCheckOrFixModels.Add(test);
                        db.SaveChanges();


                    }
                }
                sr.Close();
            }


            /*
            WebClient MyWebClient = new WebClient();


            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据

            Byte[] pageData = MyWebClient.DownloadData("https://wrapstar.bing.net/Dashboard/Alert?where=([Date]='2016-03-20') AND (dbo.[ContainToken]([Notification],'wsdatap@microsoft.com',';')=1)"); //从指定网站下载数据

            string pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句            

            //string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句
            */
            //ViewBag.Message = result;
            //ViewBag.Date = DateTime.Now;
            return RedirectToAction("Index");
        }

        //public ActionResult SearchByModelVersionID(string versionID)
        //{
        //    var alertmodel = from m in db.NeedCheckOrFixModels select m;

        //    if (!String.IsNullOrEmpty(versionID))
        //    {
        //        alertmodel = alertmodel.Where(s => s.VersionID.Equals(versionID));
        //    }
        //    return View(alertmodel);
        //}

        public FileResult ExportToExcel(string dateExport)
        {

            var sbHtml = new StringBuilder();
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            sbHtml.Append("<tr>");

            var lstTitle = new List<string> { "ModelDate", "Ontology", "ModelID", "VersionID", "ModelName", "ModelOwner", "ModelAlertType", "IssueSymptom", "Action", "CauseAnalysis" };
            foreach (var item in lstTitle)
            {
                sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", item);
            }
            sbHtml.Append("</tr>");

            var datetimeglo = Convert.ToDateTime(dateExport);
            foreach (var m in db.NeedCheckOrFixModels.Where(g => g.ModelDate.Year == datetimeglo.Year && g.ModelDate.Month == datetimeglo.Month && g.ModelDate.Day == datetimeglo.Day))
            {
                sbHtml.Append("<tr>");
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", m.ModelDate);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", m.Ontology);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", m.ModelID);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", m.VersionID);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", m.ModelName);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", m.ModelOwner);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", m.ModelAlertType);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", m.IssueSymptom);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", m.Action);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", m.CauseAnalysis);
                sbHtml.Append("</tr>");
            }
            sbHtml.Append("</table>");

            //第一种:使用FileContentResult
            byte[] fileContents = Encoding.Default.GetBytes(sbHtml.ToString());
            return File(fileContents, "application/ms-excel", "fileContents.xls");

            //第二种:使用FileStreamResult
            //var fileStream = new MemoryStream(fileContents);
            //return File(fileStream, "application/ms-excel", "fileStream.xls");

            ////第三种:使用FilePathResult
            ////服务器上首先必须要有这个Excel文件,然会通过Server.MapPath获取路径返回.
            //var fileName = Server.MapPath("~/Files/fileName.xls");
            //return File(fileName, "application/ms-excel", "fileName.xls");
        }
        //private void queryBuilderControl1_Load(object sender, EventArgs e)
        //{
        //    Uri collectionUri = new Uri("http://vstfbing:8080/tfs/Bing");
        //    TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(collectionUri);
        //    WorkItemStore _store = tpc.GetService<WorkItemStore>();
        //    _teamProjectName = "bing";
        //    queryBuilderControl1.Initialize(_store, _teamProjectName);
        //    queryBuilderControl1.SetWiql("SELECT [System.Id],[System.Work Item Type],[System.Title],[System.Assigned To],[System.State].[Description] FROM WorkItems WHERE [System.TeamProject] = @project  AND  [System.WorkItemType] = 'Task'  AND  [System.Title] CONTAINS 'wrapstarmodel' AND  [System.Title] CONTAINS '28339'");
        //}
    }
}
