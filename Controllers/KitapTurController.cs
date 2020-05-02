using MvcKutuphane.Models.DataContext;
using MvcKutuphane.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    public class KitapTurController : Controller
    {
        KutuphaneDBContext db = new KutuphaneDBContext();
        // GET: KitapTur
        public ActionResult Index()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            return View(db.KitapTuru.Include("Kullanici").ToList().OrderByDescending(a => a.KitapTurId));
        }
        public ActionResult KitapTurEkle()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult KitapTurEkle(KitapTuru kitaptur)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                kitaptur.KayitTarihi = Convert.ToDateTime(DateTime.Now.ToLongDateString());
                db.KitapTuru.Add(kitaptur);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kitaptur);
        }
        public ActionResult KitapTurGuncelle(int? id)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            var g = db.KitapTuru.Where(a => a.KitapTurId == id).SingleOrDefault();
            return View(g);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult KitapTurGuncelle(int? id, KitapTuru kitaptur)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                var g = db.KitapTuru.Where(a => a.KitapTurId == id).SingleOrDefault();
                if (g == null)
                {
                    return HttpNotFound();
                }
                g.TurAd = kitaptur.TurAd;
                g.KullaniciId = kitaptur.KullaniciId;
                kitaptur.KayitTarihi = Convert.ToDateTime(DateTime.Now.ToLongDateString());
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kitaptur);
        }
        public ActionResult KitapTurSil(int? id)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            if (id == null)
            {
                return HttpNotFound();
            }
            var s = db.KitapTuru.Where(a => a.KitapTurId == id).SingleOrDefault();
            if (s == null)
            {
                return HttpNotFound();
            }
            db.KitapTuru.Remove(s);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}