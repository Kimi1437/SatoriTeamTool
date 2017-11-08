using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SatoriTeamTool.Models;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace SatoriTeamTool.Controllers
{
    public class NeedFixModelsController : Controller
    {
        private SatoriDBContext db = new SatoriDBContext();
        DataSet myDataSet = new DataSet();
        // GET: NeedFixModels
        public ActionResult Index()
        {
            var teamMemberList = new List<string>();
            var team = from tm in db.TeamMembers orderby tm.Name select tm.Name;
            teamMemberList.AddRange(team.Distinct());
            ViewBag.teamms = new SelectList(teamMemberList);

            return View(db.NeedCheckOrFixModels.Where(n=>n.Action.ToString().Contains("Normal")).ToList());
        }
        [HttpPost]
        public ActionResult Save(int? id,string teamms)
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
            var needfix = db.NeedCheckOrFixModels.FirstOrDefault(m => m.ID.ToString().Contains(id.ToString()));
            var teammember = db.TeamMembers.FirstOrDefault(t => t.Name.ToString().Contains(teamms));
            needfix.TeamMembers.Add(teammember);
            db.SaveChanges();

            string context = "hi "+teammember.Name+"Please fix the model v"+needfix.VersionID +"\r\nthanks\r\n"+ Environment.UserName;
            SendEmail(Environment.UserName + "@microsoft.com",teammember.Mail,"Fix Model Task", context);
            
            return RedirectToAction("Index");
           
        }


        // GET: NeedFixModels/Details/5
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

        // GET: NeedFixModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NeedFixModels/Create
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

        // GET: NeedFixModels/Edit/5
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

        // POST: NeedFixModels/Edit/5
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

        // GET: NeedFixModels/Delete/5
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

        // POST: NeedFixModels/Delete/5
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
        public static bool SendEmail(string from, string tos, string subject, string content)
        {
            SmtpClient client = new SmtpClient("smtphost.redmond.corp.microsoft.com")
            {
                Credentials = CredentialCache.DefaultNetworkCredentials
            };
            string[] arr = tos.Split(',');
            MailAddress From = new MailAddress(from);
            MailAddress to = new MailAddress(arr[0]);
            MailMessage message = new MailMessage(From, to);
            if (arr.Length > 1)
            {
                for (int i = 1; i < arr.Length; i++)
                {
                    message.To.Add(new MailAddress(arr[i]));
                }
            }
            message.Subject = subject;
            message.Body = content;
            message.IsBodyHtml = true;
            try
            {
                client.Send(message);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                message.Dispose();
                client.Dispose();
            }
        }

    }
}
