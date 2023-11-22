using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TukiVerkko1.Models;

namespace TukiVerkko1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Authorize(Logins LoginModel)
        {
            TikettiDBEntities db = new TikettiDBEntities();

            var LoggedUser = db.Logins.SingleOrDefault(x => x.Käyttäjätunnus == LoginModel.Käyttäjätunnus && x.Salasana == LoginModel.Salasana);
            if (LoggedUser != null)
            {
                //ViewBag.LoggedStatus = "Paikalla";
                ViewBag.LoginError = 0;
                Session["Käyttäjätunnus"] = LoggedUser.Käyttäjätunnus;
                return RedirectToAction("About", "Home");
            }
            else
            {
                //ViewBag.LoggedStatus = "Vieras";
                ViewBag.LoginError = 1;
                LoginModel.KirjautumisVirheIlmoitus = "Tuntematon käyttäjätunnus tai salasana";
                return View("Index", LoginModel);
            }
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            //ViewBag.LoggedStatus = "Vieras";
            return RedirectToAction("Index", "Home");
        }
    }
}