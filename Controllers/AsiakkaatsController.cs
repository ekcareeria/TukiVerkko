﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TukiVerkko1.Models;
using TukiVerkko1.ViewModels;
  

namespace TukiVerkko1.Controllers
{
    public class AsiakkaatsController : Controller
    {
        private TikettiDBEntities1 db = new TikettiDBEntities1();

        // GET: Asiakkaats
        public ActionResult Index()
        {
            //var asiakkaat = db.Asiakkaat;
            return RedirectToAction("Index", "HomeController", new {});

        }

        // GET: Asiakkaats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asiakkaat asiakkaat = db.Asiakkaat.Find(id);
            if (asiakkaat == null)
            {
                return HttpNotFound();
            }
            return View(asiakkaat);
        }

        // GET: Asiakkaats/Create
        public ActionResult Create()
        {
            // Dropdown
            ViewBag.TikettiID = new SelectList(db.Tiketit, "TikettiID", "Otsikko");
            return View();
        }

        // POST: Asiakkaats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AsiakasID,TikettiID,Etunimi,Sukunimi,Puhelinnumero,Sähköposti")] Asiakkaat asiakkaat)
        {
            if (ModelState.IsValid)
            {
                db.Asiakkaat.Add(asiakkaat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //// Tämä SelectList liittynee vain dropdowniin
            //ViewBag.TikettiID = new SelectList(db.Tiketit, "TikettiID", "Otsikko", asiakkaat.TikettiID);
            return View(asiakkaat);
        }

        // GET: Asiakkaats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asiakkaat asiakkaat = db.Asiakkaat.Find(id);
            if (asiakkaat == null)
            {
                return HttpNotFound();
            }
            //ViewBag.TikettiID = new SelectList(db.Tiketit, "TikettiID", "Otsikko", asiakkaat.TikettiID);
            return View(asiakkaat);
        }

        // POST: Asiakkaats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AsiakasID,TikettiID,Etunimi,Sukunimi,Puhelinnumero,Sähköposti")] Asiakkaat asiakkaat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asiakkaat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ////Tämä oletuksena luotu, meni rikki kun malli päivitettiin
            //ViewBag.TikettiID = new SelectList(db.Tiketit, "TikettiID", "Otsikko", asiakkaat.TikettiID);
            return View(asiakkaat);
        }

        // GET: Asiakkaats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asiakkaat asiakkaat = db.Asiakkaat.Find(id);
            if (asiakkaat == null)
            {
                return HttpNotFound();
            }
            return View(asiakkaat);
        }

        // POST: Asiakkaats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Asiakkaat asiakkaat = db.Asiakkaat.Find(id);
            db.Asiakkaat.Remove(asiakkaat);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Create-NÄKYMÄ tukipyyntölomakkeelle:
        public ActionResult Tukipyynto() 
        {
            ViewBag.KategoriaID = new SelectList(db.Kategoriat, "KategoriaID", "Nimi");
            return View();
        }
        // POST: Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tukipyynto([Bind(Include = "AsiakasID,Otsikko,KategoriaID,Etunimi,Sukunimi,Sähköposti,Puhelinnumero,Kuvaus")] Lomaketiedot tiedot)
        {
            if (ModelState.IsValid) //huom tämä try-catch on vain akuuttiratkaisu estämään ohjelman kaatuminen jos vaikka kentissä on liian pitkä
            {
                try { 
                var tiketti = new Tiketit()
                {
                    Otsikko = tiedot.Otsikko,
                    Kuvaus = tiedot.Kuvaus,
                    KategoriaID = tiedot.KategoriaID,
                    AsiakasID = tiedot.AsiakasID,
                    Status = "Avoin",  
                };

                DateTime aikanyt = DateTime.Now;
                tiketti.Aika = aikanyt;
                db.Tiketit.Add(tiketti);

                var asiakas = new Asiakkaat()
                {
                    Etunimi = tiedot.Etunimi,
                    Sukunimi = tiedot.Sukunimi,
                    Sähköposti = tiedot.Sähköposti,
                    Puhelinnumero = tiedot.Puhelinnumero
                };

                db.Asiakkaat.Add(asiakas);
                db.SaveChanges();
                
                return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Lomakkeen lähetys epäonnistui, ole hyvä ja tarkista kentät.";
                }
            }

            ViewBag.KategoriaID = new SelectList(db.Kategoriat, "KategoriaID", "Nimi", tiedot.KategoriaID);

            return View(tiedot);
        }
        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
    }
}
