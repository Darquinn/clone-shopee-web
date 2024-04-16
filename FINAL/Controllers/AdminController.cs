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
    public class AdminController : Controller
    {
        // GET: Admin
        ShopeeEntities db = new ShopeeEntities();
        public PartialViewResult AdminPartial()
        {
            return PartialView();
        }

        public ActionResult ProductsList(int? category, int? page, string SearchString, double min = double.MinValue, double max = double.MaxValue)
        {	
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

        public ActionResult ProductDetail(int id)
        {
            var order = db.Products.SingleOrDefault(x => x.ProID == id);
            return View(order);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products product, HttpPostedFileBase fileAnh)
        {
            if (fileAnh.ContentLength > 0)
            {
                string root = Server.MapPath("/Content/image/");
                string pathFile = root + fileAnh.FileName;
                fileAnh.SaveAs(pathFile);

                product.ProImage = "/Content/image/" + fileAnh.FileName;
            }

            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("ProductsList");
        }

        public ActionResult Update(int id)
        {
            ShopeeEntities db = new ShopeeEntities();
            //Products model = db.Products.SingleOrDefault(m => m.ProID == id);
            Products model = db.Products.Find(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Products model, HttpPostedFileBase fileAnhMoi)
        {
            ShopeeEntities db = new ShopeeEntities();
            var updateModel = db.Products.Find(model.ProID);

            //updateModel.ProImage = model.ProImage;
            updateModel.ProName = model.ProName;
            updateModel.Price = model.Price;
            updateModel.CagetoryID = model.CagetoryID;
            updateModel.ProSupplier = model.ProSupplier;
            updateModel.ProColor = model.ProColor;

            db.SaveChanges();
            return RedirectToAction("ProductsList");
        }

        public ActionResult UpdateImage(Products model, HttpPostedFileBase fileAnhMoi)
        {
            ShopeeEntities db = new ShopeeEntities();
            var updateModel = db.Products.Find(model.ProID);

            if (fileAnhMoi == null)
            {
                ViewBag.error = "Chưa chọn file";
                ViewBag.ProID = model.ProID;
                return View();
            }
            if (fileAnhMoi.ContentLength == 0)
            {
                ViewBag.error = "File không có nội dung";
                ViewBag.ProID = model.ProID;
                return View();
            }
            string root = Server.MapPath("/Content/image/");
            string pathFile = root + fileAnhMoi.FileName;
            fileAnhMoi.SaveAs(pathFile);
            model.ProImage = "/Content/image/" + fileAnhMoi.FileName;

            updateModel.ProImage = model.ProImage;

            db.SaveChanges();
            return RedirectToAction("ProductsList");
        }



        public ActionResult Remove(int id)
        {
            ShopeeEntities db = new ShopeeEntities();
            var model = db.Products.Find(id);
            db.Products.Remove(model);
            db.SaveChanges();
            return RedirectToAction("ProductsList");
        }

        public ActionResult OrderList()
        {
            ShopeeEntities db = new ShopeeEntities();
            return View(db.Orders.ToList());
        }

        public ActionResult OrderCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderCreate(Orders order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return RedirectToAction("OrderList");
        }

        public ActionResult OrderUpdate(int id)
        {
            ShopeeEntities db = new ShopeeEntities();
            //Products model = db.Products.SingleOrDefault(m => m.ProID == id);
            Orders model = db.Orders.Find(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderUpdate(Orders model)
        {
            ShopeeEntities db = new ShopeeEntities();
            var updateModel = db.Orders.Find(model.OrderID);

            updateModel.OrderID = model.OrderID;
            updateModel.CusName = model.CusName;
            updateModel.Date = model.Date;
            updateModel.TotalCost = model.TotalCost;

            db.SaveChanges();
            return RedirectToAction("OrderList");
        }

        public ActionResult OrderRemove(int id)
        {
            ShopeeEntities db = new ShopeeEntities();
            var model = db.Orders.Find(id);
            db.Orders.Remove(model);
            db.SaveChanges();
            return RedirectToAction("OrderList");
        }

        public ActionResult OrderDetail(int id)
        {
            var order = db.Orders.SingleOrDefault(x => x.OrderID == id);
            return View(order);
        }
    }
}