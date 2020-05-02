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
    public class KimlikController : Controller
    {
        KutuphaneDBContext db = new KutuphaneDBContext();
        // GET: Kimlik
        public ActionResult Index()
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            return View(db.Kimlik.Include("Kullanici").SingleOrDefault());
        }
        public ActionResult KimlikGuncelle(int? id)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            var g = db.Kimlik.Where(a => a.KimlikId == id).SingleOrDefault();
            return View(g);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult KimlikGuncelle(int? id, Kimlik kimlik, HttpPostedFileBase ResimURL)
        {
            ViewBag.KimlikInfo = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                var g = db.Kimlik.Where(a => a.KimlikId == id).SingleOrDefault();
                var resim = g.ResimURL;
                if (ResimURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(g.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(g.ResimURL));
                    }
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo info = new FileInfo(ResimURL.FileName);
                    string name = Guid.NewGuid().ToString() + info.Extension;
                    img.Resize(250, 250);
                    img.Save("~/Uploads/Kimlik/" + name);
                    g.ResimURL = "/Uploads/Kimlik/" + name;
                    g.Title = kimlik.Title;
                    g.Keywords = kimlik.Keywords;
                    g.Description = kimlik.Description;
                    g.Unvan = kimlik.Unvan;
                    g.KullaniciId = kimlik.KullaniciId;
                    g.KayitTarihi = Convert.ToDateTime(DateTime.Now.ToLongTimeString());
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    g.Title = kimlik.Title;
                    g.Keywords = kimlik.Keywords;
                    g.Description = kimlik.Description;
                    g.Unvan = kimlik.Unvan;
                    g.KullaniciId = kimlik.KullaniciId;
                    g.ResimURL = resim;
                    g.KayitTarihi = Convert.ToDateTime(DateTime.Now.ToLongTimeString());
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            return View(kimlik);
        }
    }
}