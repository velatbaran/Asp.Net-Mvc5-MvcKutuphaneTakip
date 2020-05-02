using MvcKutuphane.Models.DataContext;
using MvcKutuphane.Models.Model;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    public class OduncController : Controller
    {
        KutuphaneDBContext db = new KutuphaneDBContext();
        // GET: Odunc
        public ActionResult Index()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            return View(db.Islem.Include("Kitaplar").Include("Uye").Include("Kullanici").Where(b => b.Kitaplar.KitapDurum == false).ToList().OrderByDescending(a => a.VerilisTarihi));
        }
        public ActionResult KitapOduncVer()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult KitapOduncVer(Islem islem)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            var u = db.Uye.Where(a => a.UyeId == islem.UyeId).SingleOrDefault();
            var k = db.Kitaplar.Where(a => a.KitapId == islem.KitapId).SingleOrDefault();

            if (u == null && k != null)
            {
                ViewBag.Uyari1 = "Girilen Üye No bilgisi kayıtlı değildir.";
            }
            else if (k == null && u != null)
            {
                ViewBag.Uyari2 = "Girilen Kitap No bilgisi kayıtlı değildir.";
            }
            else if (k == null && u == null)
            {
                ViewBag.Uyari1 = "Girilen Üye No bilgisi kayıtlı değildir.";
                ViewBag.Uyari2 = "Girilen Kitap No bilgisi kayıtlı değildir.";
            }
            else if (k.KitapDurum == false)
            {
                ViewBag.Uyari3 = "Bu kitap şuan da ödünç verilmiş.Lütfen başka kitap alınız.";
            }
            else
            {
                db.Islem.Add(islem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(islem);
        }
        public ActionResult KitapOduncGuncelle(int? id)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            var g = db.Islem.Where(a => a.IslemId == id).SingleOrDefault();
            return View(g);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult KitapOduncGuncelle(int? id, Islem islem)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                var g = db.Islem.Where(a => a.IslemId == id).SingleOrDefault();
                var u = db.Uye.Where(a => a.UyeId == islem.UyeId).SingleOrDefault();
                var k = db.Kitaplar.Where(a => a.KitapId == islem.KitapId).SingleOrDefault();
                if (g == null)
                {
                    return HttpNotFound();
                }
                else if (u == null && k != null)
                {
                    ViewBag.Uyari1 = "Girilen Üye No bilgisi kayıtlı değildir.";
                }
                else if (k == null && u != null)
                {
                    ViewBag.Uyari2 = "Girilen Kitap No bilgisi kayıtlı değildir.";
                }
                else if (k == null && u == null)
                {
                    ViewBag.Uyari1 = "Girilen Üye No bilgisi kayıtlı değildir.";
                    ViewBag.Uyari2 = "Girilen Kitap No bilgisi kayıtlı değildir.";
                }
                else if (k.KitapDurum == false)
                {
                    ViewBag.Uyari3 = "Bu kitap şuan da ödünç verilmiş.Lütfen başka kitap alınız.";
                }
                else
                {
                    g.KitapId = islem.KitapId;
                    g.UyeId = islem.UyeId;
                    g.Aciklama = islem.Aciklama;
                    g.KullaniciId = islem.KullaniciId;
                    g.VerilisTarihi = islem.VerilisTarihi;
                    g.IadeTarihi = islem.IadeTarihi;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(islem);
        }
        public ActionResult KitapOduncSil(int? id)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            if (id == null)
            {
                return HttpNotFound();
            }
            var s = db.Islem.Where(a => a.IslemId == id).SingleOrDefault();
            if (s == null)
            {
                return HttpNotFound();
            }
            db.Islem.Remove(s);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult KitapIadeEt(int? id)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            var iade = db.Islem.Where(a => a.IslemId == id).SingleOrDefault();
            if (iade == null)
            {
                return HttpNotFound();
            }
            else
            {
                DateTime d1 = DateTime.Parse(iade.IadeTarihi.ToString());
                DateTime d2 = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                TimeSpan d3 = d2 - d1;
                if (d3.TotalDays <= 0)
                {
                    ViewBag.deger = 0;
                }
                else
                {
                    ViewBag.deger = d3.TotalDays;
                }
            }
            return View("KitapIadeEt", iade);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult KitapIadeEt(int? id, Islem islem)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                var iade = db.Islem.Include("Kitaplar").Where(a => a.IslemId == id).SingleOrDefault();
                if (iade == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    iade.UyeGetirTarihi = Convert.ToDateTime(DateTime.Now.ToLongDateString());
                    iade.GecGelenGunSayisi = islem.GecGelenGunSayisi;
                    iade.Kitaplar.KitapDurum = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(islem);
        }
        public ActionResult IadesiGecikenKitaplar()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            return PartialView(db.Islem.Include("Kitaplar").Include("Uye").Include("Kullanici").Where(a=>a.GecGelenGunSayisi == null && a.IadeTarihi < DateTime.Now).ToList().OrderBy(a => a.VerilisTarihi));            
        }
        public ActionResult IadesiYaklasanKitaplar()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            return PartialView(db.Islem.Include("Kitaplar").Include("Uye").Include("Kullanici").Where(a=>a.GecGelenGunSayisi == null).ToList().OrderBy(a => a.VerilisTarihi));
        }
        public ActionResult IadesiGecikenTumKitaplar()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            return View(db.Islem.Include("Kitaplar").Include("Uye").Include("Kullanici").Where(a => a.GecGelenGunSayisi == null && a.IadeTarihi < DateTime.Now).ToList().OrderByDescending(a => a.VerilisTarihi));
        }
        public ActionResult IadesiYaklasanTumKitaplar()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            return PartialView(db.Islem.Include("Kitaplar").Include("Uye").Include("Kullanici").Where(a => a.GecGelenGunSayisi == null).ToList().OrderBy(a => a.VerilisTarihi));
        }
        public ActionResult EnIlgiGorenKitaplar()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            return PartialView(db.Islem.Include("Kitaplar").GroupBy(a => a.Kitaplar.KitapId).OrderByDescending(b=>b.Count()).OrderBy(b => b.Count()).ToList());
        }
    }
}