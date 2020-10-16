using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
  public class MoviesController : Controller
  {
    private readonly MvcMovieContext _context;

    public MoviesController(MvcMovieContext context)
    {
      _context = context;
    }

    // GET: Movies
    public async Task<IActionResult> Index(string movieGenre, string searchString, int? pageNumber, int? pageSize, string orderBy)
    {
      // Use LINQ to get list of genres
      IQueryable<string> genreQuery = from m in _context.Movie
                                      orderby m.Genre
                                      select m.Genre;

      var movies = from m in _context.Movie select m;

      // Get a list of the Movie model properties
      List<PropertyInfo> moviePropsInfo = typeof(Movie).GetProperties().ToList();
      List<string> movieProps = new List<string>();
      foreach(var prop in moviePropsInfo)
      {
        movieProps.Add(prop.Name);
      }

      if (!String.IsNullOrEmpty(searchString))
      {
        movies = movies.Where(s => s.Title.Contains(searchString));
        if(movies.Count() < pageSize * pageNumber)
        {
          pageNumber = 1;
        }
      }

      if (!String.IsNullOrEmpty(movieGenre))
      {
        movies = movies.Where(x => x.Genre == movieGenre);
        if (movies.Count() < pageSize * pageNumber)
        {
          pageNumber = 1;
        }
      }

      ViewData["MovieGenre"] = movieGenre;
      ViewData["SearchString"] = searchString;      

      var movieGenreVM = new MovieGenreViewModel
      {
        Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
        Movies = await PaginatedList<Movie>.CreateAsync(movies.OrderByDescending(orderBy ?? "ReleaseDate").AsNoTracking(), pageNumber ?? 1, pageSize ?? 10),
        MovieProperties = new SelectList(movieProps.Skip(1))
    };

      return View(movieGenreVM);
    }

    [HttpPost]
    public string Index(string searchString, bool notUsed)
    {
      return $"From [HttpPost]Index: filter on {searchString}";
    }

    // GET: Movies/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var movie = await _context.Movie
          .FirstOrDefaultAsync(m => m.Id == id);
      if (movie == null)
      {
        return NotFound();
      }

      return View(movie);
    }

    // GET: Movies/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: Movies/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating,BoxOffice")] Movie movie)
    {
      if (ModelState.IsValid)
      {
        _context.Add(movie);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(movie);
    }

    // GET: Movies/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var movie = await _context.Movie.FindAsync(id);
      if (movie == null)
      {
        return NotFound();
      }
      return View(movie);
    }

    // POST: Movies/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating,BoxOffice")] Movie movie)
    {
      if (id != movie.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(movie);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!MovieExists(movie.Id))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(Index));
      }
      return View(movie);
    }

    // GET: Movies/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var movie = await _context.Movie
          .FirstOrDefaultAsync(m => m.Id == id);
      if (movie == null)
      {
        return NotFound();
      }

      return View(movie);
    }

    // POST: Movies/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var movie = await _context.Movie.FindAsync(id);
      _context.Movie.Remove(movie);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(int id)
    {
      return _context.Movie.Any(e => e.Id == id);
    }
  }

  public static class IQueryableExtensions
  {
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
    {
      return source.OrderBy(ToLambda<T>(propertyName));
    }

    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
    {
      return source.OrderByDescending(ToLambda<T>(propertyName));
    }

    private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
    {
      var parameter = Expression.Parameter(typeof(T));
      var property = Expression.Property(parameter, propertyName);
      var propAsObject = Expression.Convert(property, typeof(object));

      return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    }
  }
}
