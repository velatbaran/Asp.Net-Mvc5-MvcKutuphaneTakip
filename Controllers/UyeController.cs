using MvcKutuphane.Models.DataContext;
using MvcKutuphane.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    public class UyeController : Controller
    {
        KutuphaneDBContext db = new KutuphaneDBContext();
        //GET: Uye
        public ActionResult Index()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            return View(db.Uye.Include("Kullanici").ToList().OrderByDescending(a => a.UyelikTarihi));
        }
        public ActionResult UyeEkle()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult UyeEkle(Uye uye)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {            
                uye.UyelikTarihi = Convert.ToDateTime(DateTime.Now.ToLongTimeString());
                db.Uye.Add(uye);
                db.SaveChanges();               

                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "baranvelat021@gmail.com";
                WebMail.Password = "bilgisayar21";
                WebMail.Send(uye.Email, "Kütüphane Sistemimize hoş geldiniz. Üye Kayıt Bilgileriniz ", "Üye No : " + uye.UyeId);
                return RedirectToAction("Index");
            }
            return View(uye);
        }
        public ActionResult UyeGuncelle(int id)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            var g = db.Uye.Where(a => a.UyeId == id).SingleOrDefault();
            return View(g);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult UyeGuncelle(int? id, Uye uye)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                var g = db.Uye.Where(a => a.UyeId == id).SingleOrDefault();
                if (g == null)
                {
                    return HttpNotFound();
                }
                g.TC = uye.TC;
                g.Ad = uye.Ad;
                g.Soyad = uye.Soyad;
                g.Email = uye.Email;
                g.Telefon = uye.Telefon;
                g.Adres = uye.Adres;
                g.KullaniciId = uye.KullaniciId;
                g.UyelikTarihi = Convert.ToDateTime(DateTime.Now.ToLongTimeString());
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(uye);
        }
        public ActionResult UyeSil(int? id)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            if (id == null)
            {
                return HttpNotFound();
            }
            var s = db.Uye.Where(a => a.UyeId == id).SingleOrDefault();
            if (s == null)
            {
                return HttpNotFound();
            }
            db.Uye.Remove(s);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}