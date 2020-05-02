using MvcKutuphane.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    public class ArsivController : Controller
    {
        KutuphaneDBContext db = new KutuphaneDBContext();
        // GET: Arsiv
        public ActionResult Index()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            return View(db.Islem.Include("Kitaplar").Include("Uye").Include("Kullanici").Where(b=>b.GecGelenGunSayisi != null).ToList().OrderByDescending(a=>a.IslemId));
        }
    }
}