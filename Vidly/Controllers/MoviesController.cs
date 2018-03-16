using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        /// <summary>
        /// Method displays the list of all movies in DB
        /// </summary>
        /// <returns></returns>
        public ViewResult Index()
        {
            if (User.IsInRole(RoleName.CanManageMovies))
            {
                return View("List");
            }

            return View("ReadOnlyList");
        }

        /// <summary>
        /// Method redirects user to new movie form page
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult New()
        {
            var genres = _context.Genres.ToList();
            var movieViewModel = new MovieFormViewModel()
            {
                Genres = genres
            };

            return View("MoviesForm", movieViewModel);
        }

        /// <summary>
        /// Method saves a new movie to Database
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleName.CanManageMovies)]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new MovieFormViewModel(movie)
                {
                    Genres = _context.Genres.ToList()
                };

                return View("MoviesForm", viewModel);
            }

            if (movie.Id == 0)
            {
                movie.DateAdded = DateTime.Now;
                _context.Movies.Add(movie);
            }   
            else
            {
                var movieInDb = _context.Movies.Single(m => m.Id == movie.Id);
                movieInDb.Name = movie.Name;
                movieInDb.ReleaseDate = movie.ReleaseDate;
                movieInDb.GenreId = movie.GenreId;
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Movies");
        }

        /// <summary>
        /// Not in use now
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);
            var genres = _context.Genres.ToList();

            if (movie == null)
                return HttpNotFound();

            var moviesViewModel = new MovieFormViewModel(movie)
            {
               Genres = genres
            };

            return View("MoviesForm", moviesViewModel);
        }

        /// <summary>
        /// Method for testing. 
        /// </summary>
        /// <returns></returns>
        public ActionResult Random()
        {
            var movie = new Movie() { Name = "Shrek!" };
            var customers = new List<Customer>
            {
                new Customer { Name = "Customer 1" },
                new Customer { Name = "Customer 2" }
            };

            var viewModel = new RandomMovieViewModel
            {
                Movie = movie,
                Customers = customers
            };

            return View(viewModel);
        }
    }
}