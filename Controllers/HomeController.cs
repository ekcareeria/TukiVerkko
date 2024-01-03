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

        public ActionResult Info()
        {
            ViewBag.Message = "Infoa";

            return View();
        }

        public ActionResult Authorize(Logins LoginModel)
        {
            TikettiDBEntities1 db = new TikettiDBEntities1();

            var Kirjautunut = db.Logins.SingleOrDefault(x => x.Käyttäjätunnus == LoginModel.Käyttäjätunnus && x.Salasana == LoginModel.Salasana);
            if (Kirjautunut != null)
            {
                ViewBag.LoginVirhe = 0;
                Session["Käyttäjätunnus"] = Kirjautunut.Käyttäjätunnus;
                Session["Rooli"] = HaeRooliTietokannasta(Kirjautunut.Käyttäjätunnus);
                return RedirectToAction("TikettiOtsikot", "Tiketit");
            }
            else
            {
                ViewBag.LoginVirhe = 1;
                LoginModel.LoginVirheilmo = "Tuntematon käyttäjätunnus tai salasana";
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
            return RedirectToAction("Index", "Home");
        }

        [Authorize] // Varmista, että käyttäjä on kirjautunut sisään
        [HttpGet]
        public ActionResult HaeRooli()
        {
            // Tarkistetaan käyttäjän rooli Sessionista
            string käyttäjänRooli = Session["Rooli"] as string;

            if (käyttäjänRooli != null)
            {
                return Content(käyttäjänRooli);
            }
            else
            {
                return Content("Vieras");
            }
        }

        private string HaeRooliTietokannasta(string käyttäjätunnus)
        {
            TikettiDBEntities1 db = new TikettiDBEntities1();

            var käyttäjä = db.Logins.SingleOrDefault(u => u.Käyttäjätunnus == käyttäjätunnus);

            if (käyttäjä != null)
            {
                return käyttäjä.Rooli;
            }

            return "Vieras";
        }
    }
}