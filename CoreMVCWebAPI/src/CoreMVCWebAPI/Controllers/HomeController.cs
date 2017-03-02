using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreMVCWebAPI.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreMVCWebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITodoRepository _context;
        private readonly TodoContext _todoContext;


        public HomeController(ITodoRepository todoRepository, TodoContext todoContext)
        {
            _context = todoRepository;
            _todoContext = todoContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {            
            return View(_context.GetAll());
        }
        // GET: <controller>/Details/5
        public  IActionResult Details(long id)
        {
            var todoItem =  _context.Find(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return View(todoItem);
        }

        // GET:  /<controller>/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind("Name,IsComplete")] TodoItem todoItem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(todoItem);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(todoItem);
        }

        //Get: /<controller>/Edit
        public IActionResult Edit(int id)
        {
            var todoItem = _context.Find(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return View(todoItem);
        }
        //Post /<controller>/Edit
        [HttpPost]
        public IActionResult Edit(long id, [Bind("Key,Name,IsComplete")] TodoItem todoItem)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Update(todoItem);
            //    return RedirectToAction("Index");
            //}
            if (todoItem == null || todoItem.Key != id)
            {
                return BadRequest();
            }

            var todo = _context.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = todoItem.IsComplete;
            todo.Name = todoItem.Name;
            //_todoContext.Update(todoItem);
            //_todoContext.SaveChanges();
            _todoContext.TodoItems.Update(todoItem);
            _todoContext.SaveChanges();
            //_context.Update(todoItem);
            return RedirectToAction("Index");
        }
        //Get /<controller>/Delete
        public IActionResult Delete(long id)
        {
            var todoItem = _context.Find(id);
            return View(todoItem);
        }
        //POST /<controller>/Delete
        [HttpPost,ActionName("Delete")]
        public IActionResult DeleteConfirmed(long id)
        {
            var todoItem = _context.Find(id);
            //_context.Remove(id);
            _todoContext.TodoItems.Remove(todoItem);
            _todoContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
