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

namespace SatoriTeamTool.Controllers
{
    public class TeamMembersController : Controller
    {

        private SatoriDBContext db = new SatoriDBContext();

        // GET: TeamMembers
        public ActionResult Index()
        {
            //var alertmodel = from m in db.NeedCheckOrFixModels select m ;
            //int alertNum = alertmodel.Count();
            //foreach (TeamMember tm in db.TeamMembers)
            //{
            //    int num = 0;
            //    //for (int i = 0; i < alertNum; i++)
            //    //{
            //    foreach (NeedCheckOrFixModel nf in (from m in db.NeedCheckOrFixModels select m))
            //        {
            //            var text = nf.TeamMembers.Where(n => n.Name.Contains(tm.Name));
            //            if (!string.IsNullOrEmpty(text.ToString()))
            //            {
            //                num++;
            //            }
            //        }
            //   // }
            //    tm.ID = tm.ID;
            //    tm.Name = tm.Name;
            //    tm.HireDate = tm.HireDate;
            //    tm.PhoneNumber = tm.PhoneNumber;
            //    tm.Mail = tm.Mail;
            //    tm.Tasks = num;

            //    db.SaveChanges();
            //}
            return View(db.TeamMembers.ToList());
        }

        // GET: TeamMembers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamMember teamMember = db.TeamMembers.Find(id);
            if (teamMember == null)
            {
                return HttpNotFound();
            }
            return View(teamMember);
        }

        // GET: TeamMembers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TeamMembers/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,HireDate,PhoneNumber,Mail,Tasks")] TeamMember teamMember)
        {
            if (ModelState.IsValid)
            {
                db.TeamMembers.Add(teamMember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(teamMember);
        }

        // GET: TeamMembers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamMember teamMember = db.TeamMembers.Find(id);
            if (teamMember == null)
            {
                return HttpNotFound();
            }
            return View(teamMember);
        }

        // POST: TeamMembers/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,HireDate,PhoneNumber,Mail,Tasks")] TeamMember teamMember)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teamMember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teamMember);
        }

        // GET: TeamMembers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamMember teamMember = db.TeamMembers.Find(id);
            if (teamMember == null)
            {
                return HttpNotFound();
            }
            return View(teamMember);
        }

        // POST: TeamMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TeamMember teamMember = db.TeamMembers.Find(id);
            db.TeamMembers.Remove(teamMember);
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
                OleDbCommand cmd = new OleDbCommand("Select [ID],[Name],[HireDate],[PhoneNumber],[Mail],[Tasks] from [Sheet1$]", excelConnection);

                excelConnection.Open();
                OleDbDataReader dReader;
                dReader = cmd.ExecuteReader();

                SqlBulkCopy sqlBulk = new SqlBulkCopy(sqlConnectionString);
                //Give your Destination table name
                sqlBulk.DestinationTableName = "TeamMembers";
                sqlBulk.WriteToServer(dReader);
                    
                excelConnection.Close();

                // SQL Server Connection String


            }

            return RedirectToAction("Index");
        }
    }
}
