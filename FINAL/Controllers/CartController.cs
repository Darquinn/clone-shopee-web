using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FINAL.Models;
using System.IO;
using System.Data;
using System.Data.Entity;

namespace FINAL.Controllers
{
    public class CartController : Controller
    {
      
        ShopeeEntities db = new ShopeeEntities();
        // GET: Cart

        public ActionResult HomeCart()
        {
            List<CartModel> gioHang = GetCart();

            if (QuantityCart() == 0)
            {
                ViewBag.Quantity = 0;
                ViewBag.Cost = 0;
            }

            ViewBag.Quantity = QuantityCart();
            ViewBag.Cost = AllCostCart();

            return View(gioHang);
        }

        public ActionResult PartialCart()
        {
            if (QuantityCart() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }

            ViewBag.TongSoLuong = QuantityCart();
            ViewBag.TongTien = AllCostCart();
            return PartialView();
        }

        public ActionResult PartialPay()
        {
            if (QuantityCart() == 0)
            {
                ViewBag.Quantity = 0;
                ViewBag.Cost = 0;
                return PartialView();
            }

            ViewBag.Quantity = QuantityCart();
            ViewBag.Cost = AllCostCart();
            return PartialView();
        }

        public List<CartModel> GetCart()
        {
            List<CartModel> listCart = Session["Cart"] as List<CartModel>;

            if (listCart == null)
            {
                listCart = new List<CartModel>();
                Session["Cart"] = listCart;
            }
            return listCart;
        }


        public ActionResult AddToCart(int idPro, string URL)
        {
            Products product = db.Products.SingleOrDefault(n => n.ProID == idPro);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<CartModel> listCart = GetCart();
            CartModel proCheck = listCart.SingleOrDefault(n => n.CartID == idPro);
            if (proCheck != null)
            {
                if (product.ProStock < proCheck.CartQuantity)
                {
                    ViewBag.error = "Sản phẩm đã hết hàng";
                    return View();
                }
                proCheck.CartQuantity++;
                proCheck.CartTotalCost = proCheck.CartQuantity * proCheck.CartPrice;
                return Redirect(URL);
            }

            CartModel item = new CartModel(idPro);
            if (product.ProStock < item.CartQuantity)
            {
                ViewBag.error = "Sản phẩm đã hết hàng";
                return View();
            }
            listCart.Add(item);
            return Redirect(URL);
        }


        public double QuantityCart()
        {
            List<CartModel> cart = Session["Cart"] as List<CartModel>;
            if (cart == null)
            {
                return 0;
            }
            return cart.Sum(n => n.CartQuantity);
        }

        public decimal? AllCostCart()
        {
            List<CartModel> cost = Session["Cart"] as List<CartModel>;
            if (cost == null)
            {
                return 0;
            }
            return cost.Sum(n => n.CartTotalCost);
        }

        public ActionResult RemoveCart(int id)
        {
            if (Session["Cart"] == null)
            {
                return RedirectToAction("StoreHome", "Store");
            }

            Products product = db.Products.SingleOrDefault(n => n.ProID == id);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<CartModel> listCart = GetCart();
            CartModel proCheck = listCart.SingleOrDefault(n => n.CartID == id);

            if (proCheck == null)
            {
                return RedirectToAction("StoreHome", "Store");
            }
            listCart.Remove(proCheck);

            return RedirectToAction("HomeCart", "Cart");
        }


        public ActionResult Pay()
        {
            ShopeeEntities db = new ShopeeEntities();

            List<CartModel> listCart = Session["Cart"] as List<CartModel>;

            if (listCart == null)
            {
                return RedirectToAction("HomeCart", "Cart");
            }

            List<CartModel> cart = Session["Cart"] as List<CartModel>;
            cart.Clear();
            db.SaveChanges();
            return RedirectToAction("PaySuccess", "Cart");
        }

        public ActionResult PaySuccess()
        {
            return View();
        }


    }
}