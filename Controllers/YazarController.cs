using MvcKutuphane.Models.DataContext;
using MvcKutuphane.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    public class YazarController : Controller
    {
        KutuphaneDBContext db = new KutuphaneDBContext();
        // GET: Yazar
        public ActionResult Index()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            return View(db.Yazar.Include("Kullanici").ToList().OrderByDescending(a => a.KayitTarihi));
        }
        public ActionResult YazarEkle()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult YazarEkle(Yazar yazar)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                yazar.KayitTarihi = Convert.ToDateTime(DateTime.Now.ToLongDateString());
                db.Yazar.Add(yazar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(yazar);
        }
        public ActionResult YazarGuncelle(int? id)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            var g = db.Yazar.Where(a => a.YazarId == id).SingleOrDefault();
            return View(g);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult YazarGuncelle(int? id, Yazar yazar)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                var g = db.Yazar.Where(a => a.YazarId == id).SingleOrDefault();
                if (g == null)
                {
                    return HttpNotFound();
                }
                g.AdSoyad = yazar.AdSoyad;
                g.KullaniciId = yazar.KullaniciId;
                g.KayitTarihi = Convert.ToDateTime(DateTime.Now.ToLongDateString());
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(yazar);
        }
        public ActionResult YazarSil(int? id)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            if (id == null)
            {
                return HttpNotFound();
            }
            var s = db.Yazar.Where(a => a.YazarId == id).SingleOrDefault();
            if(s == null)
            {
                return HttpNotFound();
            }
            db.Yazar.Remove(s);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}