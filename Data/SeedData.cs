using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MvcMovie.Models;
using System;
using System.Linq;

namespace MvcMovie.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new MvcMovieContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<MvcMovieContext>>()))
        {
            // Look for any movies.
            if (context.Movie.IsNullOrEmpty())
            {
                context.Movie.AddRange(
                new Movie
                {
                    Title = "When Harry Met Sally",
                    ReleaseDate = DateTime.Parse("1989-2-12"),
                    Genre = "Romantic Comedy",
                    Price = 7.99M
                },
                new Movie
                {
                    Title = "Ghostbusters ",
                    ReleaseDate = DateTime.Parse("1984-3-13"),
                    Genre = "Comedy",
                    Price = 8.99M
                },
                new Movie
                {
                    Title = "Ghostbusters 2",
                    ReleaseDate = DateTime.Parse("1986-2-23"),
                    Genre = "Comedy",
                    Price = 9.99M
                },
                new Movie
                {
                    Title = "Rio Bravo",
                    ReleaseDate = DateTime.Parse("1959-4-15"),
                    Genre = "Western",
                    Price = 3.99M
                }
            );

            }

            if (context.Director.IsNullOrEmpty())
            {
                context.Director.AddRange(
                new Director
                {
                    Name = "Rob Reiner",
                    BirthDate = DateTime.Parse("1947-03-06")
                },
                new Director
                {
                    Name = "Ivan Reitman",
                    BirthDate = DateTime.Parse("1946-10-27")
                },
                new Director
                {
                    Name = "Howard Hawks",
                    BirthDate = DateTime.Parse("1896-05-30")
                },
                new Director
                {
                    Name = "Steven Spielberg",
                    BirthDate = DateTime.Parse("1946-12-18")
                },
                new Director
                {
                    Name = "Quentin Tarantino",
                    BirthDate = DateTime.Parse("1963-03-27")
                },
                new Director
                {
                    Name = "Christopher Nolan",
                    BirthDate = DateTime.Parse("1970-07-30")
                });
            }
            
            if (context.Review.IsNullOrEmpty())
            {
                context.Review.AddRange(
                new Review
                {
                    Movie = context.Movie.FirstOrDefault(m => m.Title == "When Harry Met Sally"),
                    ReviewerName = "John Doe",
                    RatingScore = 4.5M,
                    Comment = "Great movie!",
                    // give random datetime for the review, between 1 and 30 days ago
                    CreatedAt = DateTime.Now.AddDays(-new Random().Next(1, 30))

                },
                // populate with multiple reviews for 2 movies
                new Review
                {
                    Movie = context.Movie.FirstOrDefault(m => m.Title == "When Harry Met Sally"),
                    ReviewerName = "Jane Smith",
                    RatingScore = 4.0M,
                    Comment = "Really enjoyed it.",
                    CreatedAt = DateTime.Now
                },
                // write a longer review for the first movie
                new Review
                {
                    Movie = context.Movie.FirstOrDefault(m => m.Title == "When Harry Met Sally"),
                    ReviewerName = "Alice Johnson",
                    RatingScore = 5.0M,
                    Comment = "This movie is a classic! The chemistry between the lead actors is fantastic, " +
                                "and the storyline is both heartwarming and hilarious." +
                                " I highly recommend it to anyone looking for a feel-good romantic comedy.",
                    // random datetime for the review, between 1 and 30 days ago
                    CreatedAt = DateTime.Now.AddDays(-new Random().Next(1, 30))
                },
                // add reviews for the 5th movie
                new Review
                {
                    Movie = context.Movie.FirstOrDefault(m => m.Title == "Oppenheimer"),
                    ReviewerName = "Bob Brown",
                    RatingScore = 3.5M,
                    Comment = "It was okay, but not my favorite.",
                    CreatedAt = DateTime.Now
                },
                // add another review for the 5th movie, Oppenheimer
                new Review
                {
                    Movie = context.Movie.FirstOrDefault(m => m.Title == "Oppenheimer"),
                    ReviewerName = "Charlie Davis",
                    RatingScore = 4.0M,
                    Comment = "A gripping and intense film. Highly recommended!",
                    CreatedAt = DateTime.Now
                },
                // add a review for the 5th movie, but leave Name and comment null
                new Review
                {
                    Movie = context.Movie.FirstOrDefault(m => m.Title == "Oppenheimer"),
                    ReviewerName = null,
                    RatingScore = 4.5M,
                    Comment = null,
                    CreatedAt = DateTime.Now
                });



        }


            context.SaveChanges();
        }
    }
}