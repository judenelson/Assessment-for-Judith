using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SeedingAdminUsers.Models;

namespace SeedingAdminUsers.Controllers
{
    public class ArticlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Articles
        public ActionResult Index(String search)
        {

            var articles = new List<Article>();

            if (search != null)
            {
                articles = db.Articles.Where(
                f => f.ArticleDescription.Contains(search)
                ).Include(f => f.Comments).ToList();

            }
            else
            {
                articles = db.Articles.Include(f => f.Comments).ToList();
            }

            return View(articles);
        }


        

        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var article = db.Articles
                .Where(b => b.ArticleID == id)
                .Include("Comments")
                .FirstOrDefault();
            if (article == null)
                if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }
        [HttpPost, ActionName("details")]
        public ActionResult Details(SeedingAdminUsers.Models.Comment res)
        {
            res.UserName = User.Identity.Name;
            res.CommentDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Comments.Add(res);
                db.SaveChanges();
                return RedirectToAction("Details", res.ArticleID);
            }
            return View();
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArticleID,ArticleTitle,ArticleDescription,PublishDate,UserName")] Article article)
        {
            article.PublishDate = DateTime.Now;
            article.UserName = User.Identity.Name;
            db.Articles.Add(article);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var article = db.Articles
                 .Where(b => b.ArticleID == id)
                 .Include("Comments")
                 .FirstOrDefault();
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArticleID,ArticleTitle,ArticleDescription,PublishDate,UserName")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(article);
        }
        [HttpPost, ActionName("DeleteComment")]
        public ActionResult DeleteComment(Comment cmt)
        {
            var modeltodelete = db.Comments.FirstOrDefault(s => s.CommentID == cmt.CommentID);

            if (ModelState.IsValid)
            {
                db.Comments.Remove(modeltodelete);
                db.SaveChanges();
                return RedirectToAction("Edit", "Articles", new { @id = cmt.ArticleID });
            }
            return View();

        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var article = db.Articles
                 .Where(b => b.ArticleID == id)
                 .Include("Comments")
                 .FirstOrDefault();
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var article = db.Articles
                 .Where(b => b.ArticleID == id)
                 .Include("Comments")
                 .FirstOrDefault();
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
