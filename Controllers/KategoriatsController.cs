using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TukiVerkko1.Models;

namespace TukiVerkko1.Controllers
{
    public class KategoriatsController : Controller
    {
        private TikettiDBEntities db = new TikettiDBEntities();

        // GET: Kategoriats
        public ActionResult Index()
        {
            return View(db.Kategoriat.ToList());
        }

        // GET: Kategoriats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategoriat kategoriat = db.Kategoriat.Find(id);
            if (kategoriat == null)
            {
                return HttpNotFound();
            }
            return View(kategoriat);
        }

        // GET: Kategoriats/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kategoriats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KategoriaID,Nimi")] Kategoriat kategoriat)
        {
            if (ModelState.IsValid)
            {
                db.Kategoriat.Add(kategoriat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kategoriat);
        }

        // GET: Kategoriats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategoriat kategoriat = db.Kategoriat.Find(id);
            if (kategoriat == null)
            {
                return HttpNotFound();
            }
            return View(kategoriat);
        }

        // POST: Kategoriats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KategoriaID,Nimi")] Kategoriat kategoriat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kategoriat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kategoriat);
        }

        // GET: Kategoriats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategoriat kategoriat = db.Kategoriat.Find(id);
            if (kategoriat == null)
            {
                return HttpNotFound();
            }
            return View(kategoriat);
        }

        // POST: Kategoriats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kategoriat kategoriat = db.Kategoriat.Find(id);
            db.Kategoriat.Remove(kategoriat);
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
    }
}
