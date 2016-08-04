using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCProject2.Controllers
{
    public class HomeController : Controller
    {
        private gameDatabaseEntities2 db = new gameDatabaseEntities2();
        
        // GET: Home
        public ActionResult Index(string heroClass, string searchString)
        {
            //LINQ query to display all entries in database.
            var heroes = from h in db.heroBoards
                         select h;

            // Class search
            var classList = new List<string>();
            //select the classes from the database
            var classQuery = from c in db.heroBoards
                             orderby c.Class
                             select c.Class;
            //have each available option to only appear once in the dropdown menu
            classList.AddRange(classQuery.Distinct());
            ViewBag.heroClass = new SelectList(classList);
            // filters the database based on the search parameter
            if(!String.IsNullOrEmpty(heroClass))
            {
                heroes = heroes.Where(x => x.Class == heroClass);
            }

            // Name Search
            if(!String.IsNullOrEmpty(searchString))
            {
                heroes = heroes.Where(y => y.Name.Contains(searchString));
            }
            return View(heroes);
        }

        public ActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            heroBoard hero = db.heroBoards.Find(Id);

            if(hero == null)
            {
                return HttpNotFound();
            }

            return View(hero);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include ="Name, Class, Description, Difficulty, Story, Picture")]heroBoard hero)
        {
            //if the additions are valid, save the changes and go back to index
            if(ModelState.IsValid)
            {
                
                db.Entry(hero).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hero);
        }


        [HttpGet]
        public ActionResult Delete(int? Id)
        {
            if(Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);   
            }
            heroBoard hero = db.heroBoards.Find(Id);

            if(hero == null)
            {
                return HttpNotFound();
            }
            return View(hero);
        }


        [HttpPost, ActionName("Delete")]
        public ActionResult Delete([Bind(Include = "Id")]heroBoard hero)
        {
            //delete the database record
            db.Entry(hero).State = EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("Index");
        }





        public ActionResult Edit(int? Id)
        {
            if(Id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            heroBoard hero = db.heroBoards.Find(Id);

            if (hero == null)
            {
                return HttpNotFound();
            }
            return View(hero);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id, Name, Class, Description, Difficulty, Story, Picture")]heroBoard hero)
        {
            //if the edited version is valid, save changes and go back to index
            if (ModelState.IsValid)
            {
                db.Entry(hero).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(hero);
        }
    }
}