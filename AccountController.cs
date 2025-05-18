using System.Linq;
using System.Web.Mvc;
using GymManagement.Models;

namespace GymManagement.Controllers
{
    public class AccountController : Controller
    {
        private GymDbContext db = new GymDbContext();

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                Session["UserId"] = user.Id;
                Session["FullName"] = user.FullName;
                Session["Email"] = user.Email;
                Session["Role"] = user.Role;

                TempData["Success"] = "Đăng nhập thành công!";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Email hoặc mật khẩu không đúng!";
            return View();
        }

        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = db.Users.FirstOrDefault(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    ViewBag.Error = "Email đã tồn tại!";
                    return View(user);
                }

                // Gán quyền mặc định là User nếu chưa có
                if (string.IsNullOrEmpty(user.Role))
                {
                    user.Role = "User";
                }

                db.Users.Add(user);
                db.SaveChanges();
                TempData["Success"] = "Tạo tài khoản thành công! Mời bạn đăng nhập.";
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
