using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MotoyolBlog.Models;
using System.Web.Security;
using MotoyolBlog.Models.Entity;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;

namespace MotoyolBlog.Controllers
{
    public class SideMenuController : Controller
    {
        // GET: SideMenu
        MotoyolBlogEntities db = new MotoyolBlogEntities();
       
       
        [Authorize]
        [HttpPost]
        public ActionResult Favorilerim(Favoriler fav)
        {
            Favoriler favori = new Favoriler();
            favori.icerik_id = fav.icerik_id;
            favori.uyeno = Convert.ToInt32(Session["uyeno"]);
            db.Favoriler.Add(favori);
            db.SaveChanges();
           return  RedirectToAction("Favorilerim");
        }
        [HttpGet]
        public ActionResult Favorilerim()
        {
            ViewModel vm = new ViewModel();
            var uyeno = Convert.ToInt32(Session["uyeno"]);
            var fav_liste = db.Favoriler.Where(x => x.uyeno == uyeno).ToList();
            vm.favori_model = fav_liste;
           
            return View(vm);
        }


        [Authorize]
        [HttpGet]
        public ActionResult MakaleOlustur()
        {
            List<SelectListItem> kategoriler =
                (from i in db.Kategoriler.ToList()
                 select new SelectListItem
                 {
                     Text = i.Kategori_ad,
                     Value = i.id.ToString()
                 }).ToList();
            ViewBag.Kategoriler = kategoriler;
            return View();
        }
        [HttpPost]
        public ActionResult MakaleOlustur([Bind(Include = "icerik,kategori_id,baslik,img")] Makaleler p1)
        {
            if(Request.Files.Count > 0)
            {
                string dosyaadi = Path.GetFileName(Request.Files[0].FileName);
                string uzanti = Path.GetExtension(Request.Files[0].FileName);
                string yol = "~/img/" + dosyaadi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(yol));
                p1.img = "/img/" + dosyaadi + uzanti;

            }
            p1.yazarid =Convert.ToInt32(Session["uyeno"]);
            db.Makaleler.Add(p1);
            db.SaveChanges();
            return RedirectToAction("Makalelerim","Main");
        }
        [HttpGet]
        public ActionResult GirisYap()
        {
            Session.Remove("uyeno");
            return View();
        }
        [HttpPost]
        public ActionResult GirisYap(Kullanicilar kullanicilar)
        {
            var uyeler = db.Kullanicilar.FirstOrDefault(x => x.eposta == kullanicilar.eposta && x.sifre == kullanicilar.sifre);
            if (uyeler != null)
            {
                FormsAuthentication.SetAuthCookie(uyeler.ad, false);
                Session["uyeno"] = uyeler.uyeno.ToString();
                Session["ad"] = uyeler.ad.ToString();
                return RedirectToAction("Index", "Main");
            }
            else
            {
                Response.Write("<script lang=''<javascript>alert('Hatalı Kullanıcı Adı Veya Şifre !!');</script>");
                return View();
            }

        }   
            [HttpGet]
            public ActionResult Kayitol()
            {

                return View();
            }
            [HttpPost]
            public ActionResult Kayitol(Kullanicilar kayit)
            {

            Kullanicilar kullanicilar = new Kullanicilar();

                kullanicilar.ad = kayit.ad;
                kullanicilar.eposta = kayit.eposta;
                kullanicilar.sifre = kayit.sifre;
                db.Kullanicilar.Add(kullanicilar);
                db.SaveChanges();
             Response.Write("<script lang=''<javascript>alert('Kayıt Başarılı.');</script>");
            return RedirectToAction("Index","Main");
               
               
            }
            
             public ActionResult CikisYap()
           {
            FormsAuthentication.SignOut();
            return RedirectToAction("GirisYap");
           }

            public ActionResult Profilim()
        {
            return View();
        }

        public ActionResult FavoriSil(int? icerik_id)
        {
            var uyeno = Convert.ToInt32(Session["uyeno"]);
            var favori = db.Favoriler.Where(x => x.icerik_id == icerik_id && x.uyeno==uyeno).FirstOrDefault();
            db.Favoriler.Remove(favori);
            db.SaveChanges();
            return RedirectToAction("Favorilerim");
        }
    }
}