using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MotoyolBlog.Models.Entity;
using MotoyolBlog.Models;

namespace MotoyolBlog.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        MotoyolBlogEntities db = new MotoyolBlogEntities();
        [Authorize]
        public ActionResult Index()
        {
            var listele = db.MakaleListesi();
            return View(listele);
        }
        [Authorize]
        public ActionResult Makalelerim()
        {
            int uyeno = Convert.ToInt32(Session["uyeno"]);
            var liste = db.MakaleListesi().Where(x => x.yazarid == uyeno).ToList();
            return View(liste);
        }
        public ActionResult GuncelGelisme(Makaleler makale)
        {
            var guncel = db.MakaleListesi().Where(m => m.Kategori_ad == "Güncel Gelişmeler");

            return View(guncel);
        }
        public ActionResult Anilar()
        {
            var anilar = db.MakaleListesi().Where(m => m.Kategori_ad == "Anılar");
            return View(anilar);
        }
        public ActionResult KullaniciDeneyimleri()
        {
            var deneyimler = db.MakaleListesi().Where(m => m.Kategori_ad == "Kullanıcı Deneyimleri");
            return View(deneyimler);
        }
        public ActionResult MotoGP()
        {
            var gp = db.MakaleListesi().Where(m => m.Kategori_ad == "MotoGP");
            return View(gp);
        }
        public ActionResult Diger()
        {
            var diger = db.MakaleListesi().Where(m => m.Kategori_ad == "Diğer Konular");
            return View(diger);
        }
        [Authorize]
        [HttpGet]
        public ActionResult MakaleOku(int? id)
        {
            ViewModel vm = new ViewModel();
            var makale = db.Makaleler.Where(x => x.id == id).ToList();
            vm.Makale_model = makale;
            var bilgi = (from i in db.Makaleler.Where(x => x.id == id)
                         select new
                         {
                             i.kategori_id,
                             i.yazarid
                         }).FirstOrDefault();


            if (bilgi != null)
            {
                var ktgr = (from i in db.Kategoriler.Where(x => x.id == bilgi.kategori_id)

                            select new
                            {
                                i.Kategori_ad
                            }).FirstOrDefault();
                ViewBag.k = ktgr.Kategori_ad;
            }
           
            if (bilgi !=null)
            {
                var uye = (from i in db.Kullanicilar.Where(x => x.uyeno == bilgi.yazarid)

                            select new
                            {
                                i.ad
                            }).FirstOrDefault();
                ViewBag.ad = uye.ad;
            }
            var yorum = (from i in db.Yorumlar.Where(x => x.icerik_id == id)
                         select new
                         {
                             i.id,
                             i.uyeno,
                             i.yorum,
                             i.yorum_tarih
                         }).FirstOrDefault();

            var yorumliste = db.Yorumlar.Where(x => x.icerik_id == id).ToList();
            vm.yorum_model = yorumliste;



            return View(vm);
        }

        [Authorize]
        public ActionResult MakaleOku([Bind(Include = "yorum,icerik_id,uyeno")]string yorum,int icerik_id )
        {
            Yorumlar yr = new Yorumlar();
            yr.uyeno = Convert.ToInt32(Session["uyeno"]);
            yr.yorum = yorum;
            yr.icerik_id = icerik_id;
            db.Yorumlar.Add(yr);
            db.SaveChanges();
            
            return RedirectToAction("MakaleOku");
        }
        public ActionResult MakaleSil(int? id)
        {
            var  makale = db.Makaleler.Where(x => x.id == id).FirstOrDefault();
            db.Makaleler.Remove(makale);
            db.SaveChanges();
            return RedirectToAction("Makalelerim");
        }
    }
}