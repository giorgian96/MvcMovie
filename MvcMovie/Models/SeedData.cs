using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Data;
using System;
using System.Linq;

namespace MvcMovie.Models
{
  public class SeedData
  {
    public static void Initialize(IServiceProvider serviceProvider)
    {
      using (var context = new MvcMovieContext(serviceProvider.GetRequiredService<DbContextOptions<MvcMovieContext>>()))
      {
        // Look for any movies
        if (context.Movie.Any())
        {
          return; // DB has been seeded
        }

        context.Movie.AddRange(
          new Movie
          {
            Title = "Tenet",
            ReleaseDate = DateTime.Parse("2020-9-3"),
            Genre = "Action",
            Price = 9.99M,
            Rating = "PG-13",
            BoxOffice = 323.5M
          },
          new Movie
          {
            Title = "Rogue One: A Star Wars Story",
            ReleaseDate = DateTime.Parse("2016-12-16"),
            Genre = "Sci-Fi",
            Price = 4.39M,
            Rating = "PG-13",
            BoxOffice = 1056M
          },
          new Movie
          {
            Title = "The Kingsman: The Secret Service",
            ReleaseDate = DateTime.Parse("2014-12-13"),
            Genre = "Action Comedy",
            Price = 5.19M,
            Rating = "R",
            BoxOffice = 414.4M
          },
          new Movie
          {
            Title = "The Dark Knight",
            ReleaseDate = DateTime.Parse("2008-7-18"),
            Genre = "Action",
            Price = 3.29M,
            Rating = "PG-13",
            BoxOffice = 1005M
          },
          new Movie
          {
            Title = "The Lord of the Rings: The Return of the King",
            ReleaseDate = DateTime.Parse("2003-12-17"),
            Genre = "Adventure",
            Price = 5.49M,
            Rating = "PG-13",
            BoxOffice = 1142M
          },
          new Movie
          {
            Title = "Titanic",
            ReleaseDate = DateTime.Parse("1997-12-19"),
            Genre = "Romance",
            Price = 6.29M,
            Rating = "PG-13",
            BoxOffice = 2195M
          },
          new Movie
          {
            Title = "Contact",
            ReleaseDate = DateTime.Parse("1997-7-11"),
            Genre = "Drama",
            Price = 3.49M,
            Rating = "PG",
            BoxOffice = 171.1M
          },
          new Movie
          {
            Title = "The Shawshank Redemption",
            ReleaseDate = DateTime.Parse("1994-10-14"),
            Genre = "Drama",
            Price = 3.69M,
            Rating = "R",
            BoxOffice = 58.3M
          },
          new Movie
          {
            Title = "When Harry Met Sally",
            ReleaseDate = DateTime.Parse("1989-7-21"),
            Genre = "Romantic Comedy",
            Price = 7.89M,
            Rating = "R",
            BoxOffice = 92.8M
          },
          new Movie
          {
            Title = "Ghostbusters 2",
            ReleaseDate = DateTime.Parse("1989-6-16"),
            Genre = "Comedy",
            Price = 6.79M,
            Rating = "PG",
            BoxOffice = 215.4M
          },
          new Movie
          {
            Title = "Ghostbusters ",
            ReleaseDate = DateTime.Parse("1984-6-8"),
            Genre = "Comedy",
            Price = 5.99M,
            Rating = "PG",
            BoxOffice = 296.4M
          },
          new Movie
          {
            Title = "Apocalypse Now	",
            ReleaseDate = DateTime.Parse("1979-8-15"),
            Genre = "Drama",
            Price = 3.49M,
            Rating = "R",
            BoxOffice = 150M
          },
          new Movie
          {
            Title = "Bullitt",
            ReleaseDate = DateTime.Parse("1968-10-17"),
            Genre = "Action",
            Price = 2.59M,
            Rating = "M",
            BoxOffice = 42.30M
          },
          new Movie
          {
            Title = "Rio Bravo",
            ReleaseDate = DateTime.Parse("1959-3-18"),
            Genre = "Western",
            Price = 2.99M,
            Rating = "R",
            BoxOffice = 5.75M
          }
        );
        context.SaveChanges();
      }
    }
  }
}
