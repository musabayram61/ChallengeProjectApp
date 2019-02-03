using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using ChallengesApp.Models;
using HtmlAgilityPack;
using System.Text;
using ChallengeApp.Controllers.Managers;

namespace ChallengesApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        // Default : GET
        public ActionResult Index()
        {
            DatabaseContext db = new DatabaseContext();
            List<Game> model = db.Game.ToList();
            return View(model);
        }
    }
}