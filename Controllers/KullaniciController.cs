using MvcKutuphane.Models.DataContext;
using MvcKutuphane.Models.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    public class KullaniciController : Controller
    {
        KutuphaneDBContext db = new KutuphaneDBContext();
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            return View(db.Kullanici.Include("Yetki").ToList().OrderByDescending(a => a.KullaniciId));
        }
        public ActionResult KullaniciEkle()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            ViewBag.YetkiId = new SelectList(db.Yetki, "YetkiId", "YetkiAd");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult KullaniciEkle(Kullanici kullanici, HttpPostedFileBase ResimURL)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                if (ResimURL != null)
                {
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo info = new FileInfo(ResimURL.FileName);
                    string name = Guid.NewGuid().ToString() + info.Extension;
                    img.Resize(120, 120);
                    img.Save("~/Uploads/Kullanici/" + name);
                    kullanici.ResimURL = "/Uploads/Kullanici/" + name;
                }
                kullanici.Sifre = Crypto.Hash(kullanici.Sifre, "MD5");
                kullanici.KayitTarihi = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                db.Kullanici.Add(kullanici);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kullanici);
        }
        public ActionResult KullaniciGuncelle(int? id)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            var g = db.Kullanici.Where(a => a.KullaniciId == id).SingleOrDefault();
            ViewBag.YetkiId = new SelectList(db.Yetki, "YetkiId", "YetkiAd", g.YetkiId);
            return View(g);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult KullaniciGuncelle(int? id, Kullanici kullanici, HttpPostedFileBase ResimURL)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                var g = db.Kullanici.Where(a => a.KullaniciId == id).SingleOrDefault();
                var resim = g.ResimURL;
                var sifre = g.Sifre;
                if (ResimURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(g.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(g.ResimURL));
                    }
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo info = new FileInfo(ResimURL.FileName);
                    string name = Guid.NewGuid().ToString() + info.Extension;
                    img.Resize(120, 120);
                    img.Save("~/Uploads/Kullanici/" + name);
                    g.ResimURL = "/Uploads/Kullanici/" + name;
                    g.Ad = kullanici.Ad;
                    g.Soyad = kullanici.Soyad;
                    g.Email = kullanici.Email;
                    g.YetkiId = kullanici.YetkiId;
                    if (kullanici.Sifre == sifre)
                    {
                        g.Sifre = kullanici.Sifre;
                    }
                    else
                    {
                        g.Sifre = Crypto.Hash(kullanici.Sifre, "MD5");
                    }
                    g.KayitTarihi = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    g.ResimURL = resim;
                    g.Ad = kullanici.Ad;
                    g.Soyad = kullanici.Soyad;
                    g.Email = kullanici.Email;
                    g.YetkiId = kullanici.YetkiId;
                    if (kullanici.Sifre == sifre)
                    {
                        g.Sifre = kullanici.Sifre;
                    }
                    else
                    {
                        g.Sifre = Crypto.Hash(kullanici.Sifre, "MD5");
                    }
                    g.KayitTarihi = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(kullanici);
        }
        public ActionResult KullaniciSil(int? id)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            if (id == null)
            {
                return HttpNotFound();
            }
            var s = db.Kullanici.Where(a => a.KullaniciId == id).SingleOrDefault();
            if (s == null)
            {
                return HttpNotFound();
            }
            db.Kullanici.Remove(s);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult KullaniciSifreDegistir(int? id)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            var s = db.Kullanici.Where(a => a.KullaniciId == id).SingleOrDefault();
            return View(s);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult KullaniciSifreDegistir(int? id, Kullanici kullanici)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            var s = db.Kullanici.Where(a => a.KullaniciId == id).SingleOrDefault();
            if (s == null)
            {
                ViewBag.Uyari = "Şifre değiştirilirken hata oluştu";
            }
            else
            {
                s.Sifre = Crypto.Hash(kullanici.Sifre, "MD5");
                db.SaveChanges();
                ViewBag.Uyari = "Şifreniz başarılı bir şekilde değiştirilmiştir.";
            }
            return View();
        }
            
    }
}