using System.Linq;
using System.Web.Mvc;
using System.Data.Entity; // dùng EntityState
using GymManagement.Models;
using GymManagement.Filters;

namespace GymManagement.Controllers
{
    [AuthorizeRole("Admin")]
    public class MemberController : Controller
    {
        private GymDbContext db = new GymDbContext();

        // GET: Member
        public ActionResult Index()
        {
            // Load cả thông tin gói tập khi hiển thị danh sách
            var members = db.Members.Include(m => m.Package).ToList();
            return View(members);
        }

        // GET: Member/Details/5
        public ActionResult Details(int id)
        {
            var member = db.Members.Include(m => m.Package).FirstOrDefault(m => m.ID == id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Member/Create
        public ActionResult Create()
        {
            ViewBag.PackageId = new SelectList(db.Packages.ToList(), "Id", "Name");
            return View();
        }

        // POST: Member/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Member member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Nếu lỗi, hiển thị lại dropdown
            ViewBag.PackageId = new SelectList(db.Packages.ToList(), "Id", "Name", member.PackageId);
            return View(member);
        }

        // GET: Member/Edit/5
        public ActionResult Edit(int id)
        {
            var member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }

            ViewBag.PackageId = new SelectList(db.Packages.ToList(), "Id", "Name", member.PackageId);
            return View(member);
        }

        // POST: Member/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PackageId = new SelectList(db.Packages.ToList(), "Id", "Name", member.PackageId);
            return View(member);
        }

        // GET: Member/Delete/5
        public ActionResult Delete(int id)
        {
            var member = db.Members.Include(m => m.Package).FirstOrDefault(m => m.ID == id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var member = db.Members.Find(id);
            if (member != null)
            {
                db.Members.Remove(member);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
