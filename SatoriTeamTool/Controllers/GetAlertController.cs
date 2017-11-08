using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SatoriTeamTool.Models;
using System.IO;
using System.Text.RegularExpressions;

namespace SatoriTeamTool.Controllers
{
    public class GetAlertController : Controller
    {
        private SatoriDBContext db = new SatoriDBContext();

        // GET: GetAlert
        public ActionResult Index(string date)
        {
            string urlAddress;
            //string result;
            string datetimemeeage;
            if (String.IsNullOrEmpty(date))
            {
                urlAddress = "https://wrapstar.bing.net/Account/LogOn?logon=true";
            }
            else {
                datetimemeeage = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
                urlAddress = "https://wrapstar.bing.net/Dashboard/Alert?top=10000&orderby=%5BDate%5D+DESC%2C+%5BVersion%5D+DESC&where=([Date]='" + datetimemeeage + "') AND (dbo.[ContainToken]([Notification],'wsdatap@microsoft.com',';')=1)";
                ViewBag.dateMessage = datetimemeeage;
                GetAlertDate(Convert.ToDateTime(date));
            }
            ViewBag.urlMessage = urlAddress;
            
            //WebClient client = new WebClient();

            ////1.Create the request object
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:15092/GetAlert");
            ////request.AllowAutoRedirect = true;
            ////request.MaximumAutomaticRedirections = 200;
            //request.Proxy = null;
            //request.UseDefaultCredentials = true;
            ////2.Add the 4 with the active 
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
            //            //test.TeamMembers = null;
            //            test.CauseAnalysis = "test";


            //            db.NeedCheckOrFixModels.Add(test);
            //            db.SaveChanges();


            //        }
            //    }
            //    sr.Close();
            //}

            return View(db.NeedCheckOrFixModels.ToList());
        }

        // GET: GetAlert/Details/5
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

        // GET: GetAlert/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GetAlert/Create
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

        // GET: GetAlert/Edit/5
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

        // POST: GetAlert/Edit/5
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

        // GET: GetAlert/Delete/5
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

        // POST: GetAlert/Delete/5
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
        //public ActionResult GetAlertDate(DateTime date)
        public void  GetAlertDate(DateTime date)
        {
            string datetimemeeage = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
            WebClient client = new WebClient();
            string UrlAddress = "https://wrapstar.bing.net/Dashboard/Alert?top=10000&orderby=%5BDate%5D+DESC%2C+%5BVersion%5D+DESC&where=([Date]='" + datetimemeeage + "') AND (dbo.[ContainToken]([Notification],'wsdatap@microsoft.com',';')=1)";
            string result;
            //ViewBag.urlMessage = UrlAddress;

            //1.Create the request object
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlAddress);
            //request.AllowAutoRedirect = true;
            //request.MaximumAutomaticRedirections = 200;
            request.Proxy = null;
            request.UseDefaultCredentials = true;
            //2.Add the 4 with the active 
            CookieContainer cc = new CookieContainer();

            //3.Must assing a cookie container for the request to pull the cookies
            request.CookieContainer = cc;

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
                        test.Action = "need fix";
                        //test.TeamMembers = null;
                        test.CauseAnalysis = "test";


                        db.NeedCheckOrFixModels.Add(test);
                        db.SaveChanges();


                    }
                }
                sr.Close();
            }

            //return RedirectToAction("Index");
        }
    }
}
