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
    public class HomeController : Controller
    {
        KutuphaneDBContext db = new KutuphaneDBContext();
        public ActionResult Index()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            ViewBag.ToplamKitapSayisi = db.Kitaplar.Count();
            ViewBag.ToplamUyeSayisi = db.Uye.Count();
            ViewBag.ToplamOduncKitapSayisi = db.Kitaplar.Where(a => a.KitapDurum == false).Count();
            ViewBag.ToplamKasaTutar = db.Kasa.Sum(a => a.Tutar);
            return View();
        }
        public ActionResult Login()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Login(Kullanici kullanici)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            var giris = db.Kullanici.Include("Yetki").Where(a => a.Email == kullanici.Email).SingleOrDefault();
            if (giris.Email == kullanici.Email && giris.Sifre == Crypto.Hash(kullanici.Sifre, "MD5"))
            {
                Session["kullaniciId"] = giris.KullaniciId;
                Session["yetki"] = giris.Yetki.YetkiAd;
                Session["ad"] = giris.Ad;
                Session["soyad"] = giris.Soyad;
                Session["resim"] = giris.ResimURL;
                Session["email"] = giris.Email;
                Session["sifre"] = giris.Sifre;
                ViewBag.Uyari = "Giriş işlemi  başarılı";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Uyari = "Kullanıcı giriş bilgileriniz hatalı";
            return View(kullanici);
        }
        public ActionResult LogOut()
        {
            Session["kullaniciId"] = null;
            Session["yetki"] = null;
            Session["ad"] = null;
            Session["soyad"] = null;
            Session["resim"] = null;
            Session.Abandon();
            return RedirectToAction("/Login");
        }
        public ActionResult ForgetPassword()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ForgetPassword(Kullanici kullanici)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            var fg = db.Kullanici.Where(a => a.Email == kullanici.Email).SingleOrDefault();
            if(fg == null)
            {
                ViewBag.Uyari = "Şifre gönderilirken hata oluştu. Girdiğiniz mail sistemde kayıtlı olmayabilir.";
            }
            else
            {
                Random rnd = new Random();
                int yenisayi = rnd.Next();
                fg.Sifre = Crypto.Hash(Convert.ToString(yenisayi), "MD5");
                db.SaveChanges();

                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "baranvelat021@gmail.com";
                WebMail.Password = "bilgisayar21";
                WebMail.SmtpPort = 587;
                WebMail.Send(kullanici.Email, "Kütüphane Sistemi Yeni ", "Şifreniz : " + yenisayi);
                ViewBag.Uyari = "Yeni şifreniz başarıyla gönderilmiştir.";
            }
            return View();
        }
    }
}