using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FINAL.Models;
using System.IO;
using System.Data;
using System.Data.Entity;
using PagedList;

namespace FINAL.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store

       

        [ChildActionOnly]
        public PartialViewResult StorePartial()
        {
            ShopeeEntities db = new ShopeeEntities();
            var cateList = db.Cagetories.ToList();
            return PartialView(cateList);
        }

        public ActionResult StoreHome(int? category, int? page, string SearchString, double min = double.MinValue, double max = double.MaxValue)
        {
            ShopeeEntities db = new ShopeeEntities();

            var products = db.Products.Include(p => p.Cagetories);

            if (category == null)
            {
                products = db.Products.OrderBy(x => x.ProID);
            }
            else
            {
                products = db.Products.OrderBy(x => x.ProName).Where(x => x.CagetoryID == category);
            }

            if (!String.IsNullOrEmpty(SearchString))
            {
                products = products.Where(s => s.ProName.Contains(SearchString));
            }
            if (min >= 0 && max > 0)
            {
                products = db.Products.OrderBy(x => x.Price).Where(p => (double)p.Price >= min && (double)p.Price <= max);
            }

            int pageSize = 5;

            int pageNumber = (page ?? 1);

            if (page == null) page = 1;

            return View(products.ToPagedList(pageNumber, pageSize));

        }
    }
}