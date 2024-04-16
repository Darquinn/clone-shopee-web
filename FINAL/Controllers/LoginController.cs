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
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(Users user)
        {
            ShopeeEntities db = new ShopeeEntities();
            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("TrangChu","Home");
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) == true | string.IsNullOrEmpty(password) == true)
            {
                ViewBag.error = "Chưa nhập username hoặc password";
            }

            var db = new ShopeeEntities();
            var user = db.Users.SingleOrDefault(n => n.UserEmail.ToLower() == username.ToLower());
            
            if (user == null)
            {
                ViewBag.error = "Tài khoản không tồn tại";
                ViewBag.username = username;
                return View();
            }
            if (user.UserPass != password)
            {
                ViewBag.username = username;
                ViewBag.error = "Mật khẩu không đúng";
                return View();
            }

            Session["Users"] = user;
            return RedirectToAction("TrangChu", "Home");
        }

        public ActionResult Logout()
        {
            Session.Remove("Users");
            return RedirectToAction("TrangChu", "Home");
        }
    }

    
}