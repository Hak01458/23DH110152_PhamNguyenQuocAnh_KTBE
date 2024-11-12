using _23DH110152_PhamNguyenQuocAnh_KTBE.Models.ViewModel;
using _23DH110152_PhamNguyenQuocAnh_KTBE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity.Validation;
using System.Security.Cryptography;
using System.Text;


namespace _23DH110152_PhamNguyenQuocAnh_KTBE.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Register
        private MyStoreEntities db = new MyStoreEntities();
        //private string HashPassword(string password)
        //{
        //    using (var sha256 = SHA256.Create())
        //    {
        //        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        //        return Convert.ToBase64String(bytes);
        //    }
        //}
        public ActionResult Register()
        {
            return View();
        }
        // POST: account/register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // kiểm tra xem tên đăng nhập đã tồn tại chưa
                var existingUser = db.Users.FirstOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã tồn tại!!");
                    return View(model);
                }
                // nếu chưa tồn tại thì tạo bàn ghi thông tin tài khoản trong bảng user
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password, //lưu ý nên mã hóa mật khẩu trước khi lưu
                    UserRole = "Customer"
                };
                db.Users.Add(user);
                //và tạo bảng ghi thông tin khách hàng trong bảng Customer
                var customer = new Customer
                {
                    CustomerID = model.CustomerID,
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    CustomerPhone = model.CustomerPhone,
                    CustomerAddress = model.CustomerAddress,
                    Username = model.Username,
                };
                db.Customers.Add(customer);
                //Lưu thông tin tài khoản và  thông tin khách hàng vào CSDL
                //    db.SaveChanges();
                //    return RedirectToAction("Index", "Home");
                //}
                //return View(model);
                try
                {
                    // Lưu thông tin tài khoản và thông tin khách hàng vào CSDL
                    db.SaveChanges();
                    return RedirectToAction("Login", "Account");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError("", validationError.ErrorMessage);
                        }
                    }
                }
            }
            return View(model);
        }
        public ActionResult Login()
        {
            return View();
        }
        //post: account/login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {

                var user = db.Users.FirstOrDefault(u => u.Username == model.Username
                    && u.Password == model.Password
                    && u.UserRole == "Customer");
                if (user != null)
                {
                    //lưu trạng thái đăng nhập vào session
                    Session["Username"] = user.Username;
                    Session["UserRole"] = user.UserRole;
                    //lưu thông tin xác thực người dùng vào cookie
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            return View(model);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(LoginVM model)
        //{
        //    if (ModelState.IsValid) // Kiểm tra tính hợp lệ của model
        //    {
        //        // Lấy người dùng mà không mã hóa mật khẩu trong truy vấn
        //        var user = db.Users.FirstOrDefault(u => u.Username == model.Username);

        //        // Kiểm tra xem người dùng có tồn tại và so sánh mật khẩu đã mã hóa
        //        if (user != null && user.Password == HashPassword(model.Password)) // Mã hóa mật khẩu nhập vào
        //        {
        //            Session["Username"] = user.Username;
        //            Session["User Role"] = user.UserRole;
        //            FormsAuthentication.SetAuthCookie(user.Username, false);
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
        //        }
        //    }
        //    return View(model);
        //}
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}