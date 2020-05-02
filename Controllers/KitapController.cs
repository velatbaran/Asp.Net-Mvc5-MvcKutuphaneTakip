using MvcKutuphane.Models.DataContext;
using MvcKutuphane.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    public class KitapController : Controller
    {
        KutuphaneDBContext db = new KutuphaneDBContext();
        // GET: Kitap
        public ActionResult Index()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            return View(db.Kitaplar.Include("KitapTuru").Include("Kullanici").Include("Yazar").Where(b=>b.KitapDurum == true).ToList().OrderByDescending(a => a.KayitTarihi));
        }
        public ActionResult KitapEkle()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            ViewBag.KitapTurId = new SelectList(db.KitapTuru, "KitapTurId", "TurAd");
            ViewBag.YazarId = new SelectList(db.Yazar, "YazarId", "AdSoyad");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult KitapEkle(Kitaplar kitaplar)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                kitaplar.KayitTarihi = Convert.ToDateTime(DateTime.Now.ToLongTimeString());
                kitaplar.KitapDurum = true;
                db.Kitaplar.Add(kitaplar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kitaplar);
        }
        public ActionResult KitapGuncelle(int? id)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            var g = db.Kitaplar.Where(a => a.KitapId == id).SingleOrDefault();
            ViewBag.KitapTurId = new SelectList(db.KitapTuru, "KitapTurId", "TurAd", g.KitapTurId);
            ViewBag.YazarId = new SelectList(db.Yazar, "YazarId", "AdSoyad", g.YazarId);
            return View(g);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult KitapGuncelle(int? id,Kitaplar kitaplar)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                var g = db.Kitaplar.Where(a => a.KitapId == id).SingleOrDefault();
                if(g == null)
                {
                    return HttpNotFound();
                }
                g.Ad = kitaplar.Ad;
                g.KitapTurId = kitaplar.KitapTurId;
                g.YazarId = kitaplar.YazarId;
                g.Yayınevi = kitaplar.Yayınevi;
                g.KullaniciId = kitaplar.KullaniciId;
                g.BasimYili = kitaplar.BasimYili;
                g.Sayfa = kitaplar.Sayfa;
                g.KayitTarihi = Convert.ToDateTime(DateTime.Now.ToLongTimeString());
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kitaplar);
        }
        public ActionResult KitapSil(int? id)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            if (id == null)
            {
                return HttpNotFound();
            }
            var s = db.Kitaplar.Where(a => a.KitapId == id).SingleOrDefault();
            if(s == null)
            {
                return HttpNotFound();
            }
            db.Kitaplar.Remove(s);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}