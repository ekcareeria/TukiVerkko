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

        private string HaeRooliTietokannasta(string rooli)
        {
            TikettiDBEntities1 db = new TikettiDBEntities1();

            var käyttäjänr = db.Logins.SingleOrDefault(u => u.Rooli == rooli);

            if (käyttäjänr != null)
            {
                return käyttäjänr.Rooli;
            }

            return "Vieras";
        }

        public ActionResult Index2()
        {
            var rooli = HaeRooliTietokannasta(HttpContext.User.Identity.Name);

            switch (rooli)
            {
                case "Opiskelija":
                    return PartialView("_NavbarOpiskelija");
                case "Ylläpitäjä":
                    return PartialView("_NavbarYlläpitäjä");
                default:
                    return PartialView("_Navbar");
            }
        }
    }
}