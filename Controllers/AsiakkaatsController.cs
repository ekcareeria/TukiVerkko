using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
        private TikettiDBEntities db = new TikettiDBEntities();

        // GET: Asiakkaats
        public ActionResult Index()
        {
            var asiakkaat = db.Asiakkaat.Include(a => a.Tiketit);
            return View(asiakkaat.ToList());
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

            ViewBag.TikettiID = new SelectList(db.Tiketit, "TikettiID", "Otsikko", asiakkaat.TikettiID);
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
            ViewBag.TikettiID = new SelectList(db.Tiketit, "TikettiID", "Otsikko", asiakkaat.TikettiID);
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
            ViewBag.TikettiID = new SelectList(db.Tiketit, "TikettiID", "Otsikko", asiakkaat.TikettiID);
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

        public ActionResult Lomake()
        {
            var lomake = from a in db.Asiakkaat
                         join t in db.Tiketit on a.TikettiID equals t.TikettiID
                         join k in db.Kategoriat on t.KategoriaID equals k.KategoriaID
                         select new Lomake
                         {
                             //Otsikko = t.Otsikko,
                             //Nimi = k.Nimi,
                             Etunimi = a.Etunimi,
                             //Sukunimi = a.Sukunimi,
                             //Sähköposti = a.Sähköposti,
                             //Puhelinnumero = a.Puhelinnumero,
                             //Kuvaus = t.Kuvaus,
                         };
            if (lomake != null)

            {
                int i = 0;
                i = lomake.ToList().Count;
            }
            //var lomake = new Lomake();
            return View(lomake);
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
