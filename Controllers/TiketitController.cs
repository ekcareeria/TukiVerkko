﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TukiVerkko1.Models;
using TukiVerkko1.ViewModels;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MailKit.Security;

namespace TukiVerkko1.Controllers
{
    public class TiketitController : Controller                                             //Luotu cotrolleri valmiilla pohjalla tikettien tuontia listausta varten tietokannasta.
    {                                                                                       //Tekee samalla myös Tiketit-View:n automaattisesti.
        private TikettiDBEntities1 db = new TikettiDBEntities1();

        // GET: Tiketit
        public ActionResult Index()
        {
            var tiketit = db.Tiketit.Include(t => t.Asiakkaat).Include(t => t.Kategoriat);
            return View(tiketit.ToList());
        }

        #region Automaattisesti tulleet koodinpätkät
        // GET: Tiketit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tiketit tiketit = db.Tiketit.Find(id);
            if (tiketit == null)
            {
                return HttpNotFound();
            }
            return View(tiketit);
        }

        // GET: Tiketit/Create
        public ActionResult Create()
        {
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Etunimi");
            ViewBag.KategoriaID = new SelectList(db.Kategoriat, "KategoriaID", "Nimi");
            return View();
        }

        // POST: Tiketit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TikettiID,AsiakasID,KategoriaID,Otsikko,Kuvaus,Aika,Valmistumisaika,Status")] Tiketit tiketit)
        {
            if (ModelState.IsValid)
            {
                db.Tiketit.Add(tiketit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Etunimi", tiketit.AsiakasID);
            ViewBag.KategoriaID = new SelectList(db.Kategoriat, "KategoriaID", "Nimi", tiketit.KategoriaID);
            return View(tiketit);
        }

        // GET: Tiketit/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tiketit tiketit = db.Tiketit.Find(id);
            if (tiketit == null)
            {
                return HttpNotFound();
            }
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Etunimi", tiketit.AsiakasID);
            ViewBag.KategoriaID = new SelectList(db.Kategoriat, "KategoriaID", "Nimi", tiketit.KategoriaID);
            return View(tiketit);
        }

        // POST: Tiketit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TikettiID,AsiakasID,KategoriaID,Otsikko,Kuvaus,Aika,Valmistumisaika,Status")] Tiketit tiketit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tiketit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Etunimi", tiketit.AsiakasID);
            ViewBag.KategoriaID = new SelectList(db.Kategoriat, "KategoriaID", "Nimi", tiketit.KategoriaID);
            return View(tiketit);
        }

        // GET: Tiketit/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tiketit tiketit = db.Tiketit.Find(id);
            if (tiketit == null)
            {
                return HttpNotFound();
            }
            return View(tiketit);
        }

        // POST: Tiketit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tiketit tiketit = db.Tiketit.Find(id);
            db.Tiketit.Remove(tiketit);
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
        #endregion

        #region TikettiSummaus
        //currentFilter ja searchString on HAKUTOIMINTOA varten, tikettisummaus-näkymässä lisää koodia. sortOrder nousevaan/laskevaan järjestykseen.
        public ActionResult Tikettisummaus(string currentFilter1, string searchString1, string sortOrder)
        {
            ViewBag.EtunimiSortparm = String.IsNullOrEmpty(sortOrder) ? "etunimi_desc" : "";  //Nouseva/laskeva, ?=if.

            //Haku jää muistiin lajittelun nousevaan/laskevaan klikkaamisen jälkeenkin.
            if (searchString1 != null)
            {

            }
            else
            {
                searchString1 = currentFilter1;
            }

            ViewBag.currentFilter1 = searchString1;

            var tikettisummaus = from t in db.Tiketit                                         //LinQ-kysely, minkä perusteella taulut yhdistetty.

                                 join a in db.Asiakkaat on t.AsiakasID equals a.AsiakasID     //Luotu ViewModel. Muista lisätä using-lause ViewModels sivun ylös.

                                 join k in db.Kategoriat on t.KategoriaID equals k.KategoriaID
                                 select new YhdistettyTikettiData

                                 {
                                     Otsikko = t.Otsikko,

                                     Nimi = k.Nimi,

                                     Aika = (DateTime)t.Aika,

                                     Kuvaus = t.Kuvaus,

                                     Etunimi = a.Etunimi,

                                     Sukunimi = a.Sukunimi,

                                     Puhelinnumero = a.Puhelinnumero,

                                     Sähköposti = a.Sähköposti,

                                     TikettiID = (int)t.TikettiID,

                                     AsiakasID = (int)a.AsiakasID,

                                     KategoriaID = (int)k.KategoriaID,

                                     Valmistumisaika = (DateTime)t.Valmistumisaika,

                                     Status = t.Status,
                                 };

            #region Varuiksi tallessa. Hakutoiminto
            //if (!String.IsNullOrEmpty(searchString1))   //HAKUTOIMINTOA varten, etsii etunimen perusteella
            //{
            //    tikettisummaus = tikettisummaus.Where(a => a.Etunimi.Contains(searchString1));
            //}

            //switch (sortOrder)      //Nouseva/laskeva.
            //{
            //    case "etunimi_desc":
            //        tikettisummaus = tikettisummaus.OrderByDescending(a => a.Etunimi);
            //        break;
            //    default:
            //        tikettisummaus = tikettisummaus.OrderBy(a => a.Etunimi);
            //        break;
            //}
            #endregion

            //Yhdistelmähaku: sekä suodatus että lajittelu. Ei hukkaa haun jälkeen, laskevaan/nousevaan järjestykseen. Tarvii vielä käyttöliittymän puolelle koodia.
            if (!String.IsNullOrEmpty(searchString1))   //HAKUTOIMINTOA varten, etsii etunimen perusteella
            {
                switch (sortOrder)    //Nouseva/laskeva.
                {
                    case "etunimi_desc":
                        tikettisummaus = tikettisummaus.Where(a => a.Etunimi.Contains(searchString1)).OrderByDescending(a => a.Etunimi);
                        break;
                    default:
                        tikettisummaus = tikettisummaus.Where(a => a.Etunimi.Contains(searchString1)).OrderBy(a => a.Etunimi);
                        break;
                }
            }

            //Hakufiltteri ei käytössä, lajitellaan ilman suodatuksia.
            else
            {
                switch (sortOrder)
                {
                    case "etunimi_desc":
                        tikettisummaus = tikettisummaus.OrderByDescending(a => a.Etunimi);
                        break;
                    default:
                        tikettisummaus = tikettisummaus.OrderBy(a => a.Etunimi);
                        break;
                }
            };

            return View(tikettisummaus);
        }
        #endregion

        #region TikettiSummaus2 Arkisto hakua varten
        //currentFilter ja searchString on HAKUTOIMINTOA varten, tikettisummaus-näkymässä lisää koodia. sortOrder nousevaan/laskevaan järjestykseen.
        public ActionResult Tikettisummaus2(string currentFilter1, string searchString1, string sortOrder)
        {
            ViewBag.EtunimiSortparm = String.IsNullOrEmpty(sortOrder) ? "etunimi_desc" : "";  //Nouseva/laskeva, ?=if.

            //Haku jää muistiin lajittelun nousevaan/laskevaan klikkaamisen jälkeenkin.
            if (searchString1 != null)
            {

            }
            else
            {
                searchString1 = currentFilter1;
            }

            ViewBag.currentFilter1 = searchString1;

            var tikettisummaus = from t in db.Tiketit                                         //LinQ-kysely, minkä perusteella taulut yhdistetty.

                                 join a in db.Asiakkaat on t.AsiakasID equals a.AsiakasID     //Luotu ViewModel. Muista lisätä using-lause ViewModels sivun ylös.

                                 join k in db.Kategoriat on t.KategoriaID equals k.KategoriaID
                                 select new YhdistettyTikettiData

                                 {
                                     Otsikko = t.Otsikko,

                                     Nimi = k.Nimi,

                                     Aika = (DateTime)t.Aika,

                                     Kuvaus = t.Kuvaus,

                                     Etunimi = a.Etunimi,

                                     Sukunimi = a.Sukunimi,

                                     Puhelinnumero = a.Puhelinnumero,

                                     Sähköposti = a.Sähköposti,

                                     TikettiID = (int)t.TikettiID,

                                     AsiakasID = (int)a.AsiakasID,

                                     KategoriaID = (int)k.KategoriaID,

                                     Valmistumisaika = (DateTime)t.Valmistumisaika,

                                     Status = t.Status,
                                 };

            //Yhdistelmähaku: sekä suodatus että lajittelu. Ei hukkaa haun jälkeen, laskevaan/nousevaan järjestykseen. Tarvii vielä käyttöliittymän puolelle koodia.
            if (!String.IsNullOrEmpty(searchString1))   //HAKUTOIMINTOA varten, etsii etunimen perusteella
            {
                switch (sortOrder)    //Nouseva/laskeva.
                {
                    case "etunimi_desc":
                        tikettisummaus = tikettisummaus.Where(a => a.Etunimi.Contains(searchString1)).OrderByDescending(a => a.Etunimi);
                        break;
                    default:
                        tikettisummaus = tikettisummaus.Where(a => a.Etunimi.Contains(searchString1)).OrderBy(a => a.Etunimi);
                        break;
                }
            }

            //Hakufiltteri ei käytössä, lajitellaan ilman suodatuksia.
            else
            {
                switch (sortOrder)
                {
                    case "etunimi_desc":
                        tikettisummaus = tikettisummaus.OrderByDescending(a => a.Etunimi);
                        break;
                    default:
                        tikettisummaus = tikettisummaus.OrderBy(a => a.Etunimi);
                        break;
                }
            };

            return View(tikettisummaus);
        }
        #endregion

        public ActionResult TikettiOtsikot()                                             //Tästä metodista luotu näkymä, jonne laitettu accordion-lista yms.
        {
            var tiketit = db.Tiketit.Include(t => t.Kategoriat).Include(t => t.Asiakkaat).Where(t => t.Status == "Avoin" || t.Status == "Työn alla");
            return View(tiketit.ToList());
        }

        public ActionResult Arkisto()                                                   //Tästä metodista luotu näkymä, jonne laitettu accordion-lista yms.
        {
            var tiketit = db.Tiketit.Include(t => t.Kategoriat).Include(t => t.Asiakkaat).Where(t => t.Status == "Valmis");
            return View(tiketit.ToList());
        }

        //Tässä tallessa vanha versio, jossa molemmissa näkymissä näkyy kaikki tiketit
        //public ActionResult TikettiOtsikot()                                             //Tästä metodista luotu näkymä, jonne laitettu accordion-lista yms.
        //{
        //    var tiketit = db.Tiketit.Include(t => t.Kategoriat).Include(t => t.Asiakkaat);
        //    return View(tiketit.ToList());
        //}

        //public ActionResult Arkisto()                                                   //Tästä metodista luotu näkymä, jonne laitettu accordion-lista yms.
        //{
        //    var tiketit = db.Tiketit.Include(t => t.Kategoriat).Include(t => t.Asiakkaat);
        //    return View(tiketit.ToList());
        //}

        #region _Tikettirivit2 ja _Tikettirivit3
        public ActionResult _TikettiRivit2(int? asiakasid)                                 //Tästä metodista luodusta näkymästä tulee tiedot, kun listan otsikkoa painaa (avautuu etunimi, sukunimi yms)
        {
            var TikettiRivitLista2 = from t in db.Tiketit

                                     join a in db.Asiakkaat on t.AsiakasID equals a.AsiakasID
                                     where a.AsiakasID == asiakasid

                                     select new TikettiRivit2
                                     {
                                         Kuvaus = t.Kuvaus,

                                         Etunimi = a.Etunimi,

                                         Sukunimi = a.Sukunimi,

                                         Puhelinnumero = a.Puhelinnumero,

                                         Sähköposti = a.Sähköposti,

                                         Status = t.Status,

                                         TikettiID = (int)t.TikettiID,

                                     };

            return PartialView(TikettiRivitLista2);
        }

        public ActionResult _TikettiRivit3(int? asiakasid)                                 //Tästä metodista luodusta näkymästä tulee tiedot, kun listan otsikkoa painaa (avautuu etunimi, sukunimi yms)
        {
            var TikettiRivitLista3 = from t in db.Tiketit

                                     join a in db.Asiakkaat on t.AsiakasID equals a.AsiakasID
                                     where a.AsiakasID == asiakasid

                                     select new TikettiRivit2
                                     {
                                         Kuvaus = t.Kuvaus,

                                         Etunimi = a.Etunimi,

                                         Sukunimi = a.Sukunimi,

                                         Puhelinnumero = a.Puhelinnumero,

                                         Sähköposti = a.Sähköposti,

                                         Status = t.Status,

                                         TikettiID = (int)t.TikettiID,

                                     };

            return PartialView(TikettiRivitLista3);
        }
        #endregion

        //----------------Tästä alkaa tiketin statuksen hallinta--------------------//
        private void PaivitaTila(int tikettiID, string uusiTila)
        {
            try
            {
                Tiketit tiketti = db.Tiketit.Find(tikettiID);

                if (tiketti != null)
                {
                    tiketti.Status = uusiTila;
                    db.SaveChanges();
                    //MailIN LÄHETYS TÄSTÄ.Asenna ensin MailKit kirjoittamalla package manager consoleen: Install - Package MailKit
                    //if (uusiTila == "Työn alla")
                    //{
                    //    LahetaMaili(tiketti.AsiakasID, "Tukipyyntösi on otettu työn alle.");
                    //}
                    //else if (uusiTila == "Valmis")
                    //{
                    //    LahetaMaili(tiketti.AsiakasID, "Tukipyyntösi on valmis.");
                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Virhe tietokantapäivityksessä: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult TyoNappi(int tikettiID, string uusiTila)
        {
            PaivitaTila(tikettiID, uusiTila);

            Tiketit tiketti = db.Tiketit.Find(tikettiID);

            if (tiketti != null)
            {

                if (uusiTila == "Valmis")
                {
                    tiketti.Valmistumisaika = DateTime.Now;

                }

                else if (uusiTila == "Avoin")
                {
                    tiketti.Valmistumisaika = null;
                }
            }
            db.SaveChanges();
            return new EmptyResult();  
        }

        private void LahetaMaili(int asiakasId, string viestinTeksti)
        {
            var asiakas = db.Asiakkaat.Find(asiakasId);

            if (asiakas != null)
            {
                var viesti = new MimeMessage();
                viesti.From.Add(new MailboxAddress("Tukiverkkoinfo", "Tukiverkko@outlook.com"));
                viesti.To.Add(new MailboxAddress("", asiakas.Sähköposti));
                viesti.Subject = "Tukipyynnön tila on muuttunut";
                viesti.Body = new TextPart("plain")
                {
                    Text = viestinTeksti
                };

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp-mail.outlook.com", 587, false);
                    smtp.Authenticate("tukiverkko@outlook.com", "tiketticareeria694");
                    smtp.Send(viesti);
                    smtp.Disconnect(true);
                }
            }
        }

        [HttpPost]
        public ActionResult PoistaTiketti(int id)
        {
            {
                var tiketti = db.Tiketit.Find(id);

                if (tiketti != null)
                {
                    db.Tiketit.Remove(tiketti);
                    db.SaveChanges();
                    return RedirectToAction("Arkisto");
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
    }
}