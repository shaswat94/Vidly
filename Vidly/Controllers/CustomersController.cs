using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext(); //disposable Db object that needs to be disposed properly
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ViewResult Index()
        {
            //Queries to the database to fetch the list of customers because of the presence of .ToList() function
            //deferred execution
            var customers = _context.Customers.Include(c => c.MembershipType).ToList();
            //Eager loading - Loads the Customer and its related properties here membership type.

            return View(customers);
        }

        public ActionResult Details(int id)
        {
            var customer = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }

        public ActionResult New()
        {
            var membershipTypes = _context.MembershipTypes.ToList();
            var viewModel = new CustomerFormViewModel()
            {
                MembershipTypes = membershipTypes
            };
            
            return View("CustomerForm", viewModel);
        }

        [HttpPost]
        public ActionResult Save(Customer customer) //name must be same as that of the model for model binding to work.
        {
            if (customer.Id == 0)
            {
                _context.Customers.Add(customer);
            }
            else
            {
                var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);
                customerInDb.Name = customer.Name;
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
                customerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
            }

            _context.SaveChanges(); //To persist the changes in database
            return RedirectToAction("Index","Customers");
        }

        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customer == null)
                return HttpNotFound();

            var viewModel = new CustomerFormViewModel()
            {
                MembershipTypes = _context.MembershipTypes.ToList(),
                Customer = customer
            };

            return View("CustomerForm", viewModel);
        }

    }
}