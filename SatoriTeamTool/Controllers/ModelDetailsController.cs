using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SatoriTeamTool.Controllers
{
    public class ModelDetailsController : Controller
    {
        // GET: ModelDetails
        public ActionResult Index(string versionID)
        {
            string urltext = "https://wrapstar.bing.net/Model/Detail/" + versionID + "?environment=WrapStar";
            ViewBag.modelurl = urltext;
            return View();
        }
    }
}