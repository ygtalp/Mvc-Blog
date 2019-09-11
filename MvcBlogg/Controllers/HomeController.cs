using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBlogg.Models;
using X.PagedList;
using X.PagedList.Mvc;
using X.PagedList.Mvc.Bootstrap4;



namespace MvcBlogg.Controllers
{
    public class HomeController : Controller
    {
        mvcblogDB db = new mvcblogDB();
        // GET: Home
        public ActionResult Index(int page = 1, int pageSize = 5)
        {

            var makale = db.Makales.OrderByDescending(m => m.MakaleId);
            var paged = makale.ToPagedList(page, pageSize);
            return View(paged);
        }

        public ActionResult BlogAra(string Ara = null)
        {
            var aranan=db.Makales.Where(m => m.Icerik.Contains(Ara) || m.Baslik.Contains(Ara)).ToList();
            return View(aranan.OrderByDescending(m => m.Tarih));
        }

        public ActionResult SonYorumlar()
        {
            return View(db.Yorums.OrderByDescending(y=>y.YorumId).Take(5));
        }


        public ActionResult KategoriMakale(int id, int pagee = 1, int pageSizee = 2)
        {
            var makalee = db.Makales.Where(m => m.Kategori.KategoriId == id);
            var makaleee = makalee.OrderByDescending(m => m.Kategori.KategoriId);
            var pagedd = makaleee.ToPagedList(pagee, pageSizee);
            //var makaleler = db.Makales.Where(m => m.Kategori.KategoriId == id).ToList();
            return View(pagedd);
        }
        public ActionResult MakaleDetay(int id)
        {
            var makale = db.Makales.Where(m => m.MakaleId == id).SingleOrDefault();
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);

        }
        public ActionResult Hakkimizda()
        {
            return View();
        }
        public ActionResult Iletisim()
        {
            return View();
        }
        public ActionResult KategoriPartial()
        {
            return View(db.Kategoris.ToList());
        }

        public JsonResult YorumYap(string yorum, int Makaleid)
        {
            var uyeid = Session["uyeid"];
            if (yorum != null)
            {
                db.Yorums.Add(new Yorum { UyeId = Convert.ToInt32(uyeid), MakaleId = Makaleid, Icerik = yorum, Tarih = DateTime.Now });
                db.SaveChanges();
            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }

        public ActionResult YorumSil(int id)
        {
            var uyeid = Session["uyeid"];
            var yorum = db.Yorums.Where(y => y.YorumId == id).SingleOrDefault();
            var makale = db.Makales.Where(m => m.MakaleId == yorum.MakaleId).SingleOrDefault();
            if (yorum.UyeId == Convert.ToInt32(uyeid))
            {
                db.Yorums.Remove(yorum);
                db.SaveChanges();
                return RedirectToAction("MakaleDetay", "Home", new { id = makale.MakaleId });
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult OkunmaArttir(int Makaleid)
        {
            var makale = db.Makales.Where(m => m.MakaleId == Makaleid).SingleOrDefault();
            makale.Okunma += 1;
            db.SaveChanges();
            return View();
        }
    }
}