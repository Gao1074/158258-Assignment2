using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using FunApplication.Data;
using FunApplication.Models;
using FunApplication.ViewModels;
using PagedList;

namespace FunApplication.Controllers
{
    public class GamesController : Controller
    {
        private FunApplicationContext db = new FunApplicationContext();

        // GET: Games
        public ActionResult Index(string category, string search, string sortBy, int? page)
        {
            //instantiate a new view model
            GameIndexViewModel viewModel = new GameIndexViewModel();
            var games = db.Games.Include(p => p.Category);
            if (!String.IsNullOrEmpty(search))
            {
                games = games.Where(p => p.Name.Contains(search) ||
                p.Description.Contains(search) ||
                p.Category.Name.Contains(search) ||
                p.Publisher.Name.Contains(search));
                // ViewBag.Search = search;
                viewModel.Search = search;
            }
            //group search results into categories and count how many items in each category
            viewModel.CatsWithCount = from matchingGames in games
                                      where matchingGames.CategoryID != null
                                      group matchingGames by matchingGames.Category.Name into catGroup
                                      select new CategoryWithCount() { CategoryName = catGroup.Key, GameCount = catGroup.Count() };

            //var categories = games.OrderBy(p => p.Category.Name).Select(p => p.Category.Name).Distinct();
            if (!String.IsNullOrEmpty(category))
            {
                games = games.Where(p => p.Category.Name == category);
                viewModel.Category = category;
            }
            viewModel.Sorts = new Dictionary<string, string> { { "Price low to high", "Price_lowest" }, { "Price high to low", "Price_highest" } };
            viewModel.SortBy = sortBy;
            switch (sortBy)
            {
                case "Price_lowest":
                    games = games.OrderBy(g => g.Price);
                    break;
                case "Price_highest":
                    games = games.OrderByDescending(g => g.Price);
                    break;
                default:
                    games = games.OrderBy(g => g.Name);
                    break;
            }

            //ViewBag.Category = new SelectList(categories); 

            const int PageItems = 10;
            int currentPage = (page ?? 1);
            viewModel.Games = games.ToPagedList(currentPage, PageItems);
            return View(viewModel);
        }

        // GET: Games/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // GET: Games/Create
        public ActionResult Create()
        {
            
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.PublisherID = new SelectList(db.Publishers, "ID", "Name");
            return View();
        }

        // POST: Games/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Game game)
        {
            if (ModelState.IsValid)
            { 
                //ViewBag.ImagePath = SaveThumbnail(ViewBag.ImagePath);
                db.Games.Add(game);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", game.CategoryID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "ID", "Name", game.PublisherID);
            return View(game);
        }

        // GET: Games/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", game.CategoryID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "ID", "Name", game.PublisherID);
            return View(game);
        }

        // POST: Games/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,Price,PublisherID,CategoryID,ImagePath")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Entry(game).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", game.CategoryID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "ID", "Name", game.PublisherID);
            return View(game);
        }

        // GET: Games/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Game game = db.Games.Find(id);
            db.Games.Remove(game);
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
        private string SaveThumbnail(HttpPostedFileBase file)
        {
            try
            {
                // 生成一个唯一的文件名
                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                // 获取服务器上保存文件的路径
                string filePath = Server.MapPath("~/Images/Thumbnails/" + fileName);

                // 保存上传的文件到指定路径
                file.SaveAs(filePath);

                // 返回文件在服务器上的相对路径
                return "/Images/Thumbnails/" + fileName;
            }
            catch (Exception ex)
            {
                // 记录异常信息
                Console.WriteLine("Error saving thumbnail: " + ex.Message);
                return null; // 或者返回一个默认的错误图片路径
            }
        }
        


    }

}
