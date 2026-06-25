using MvcMovie.Models;

// helpers to map between Movie and MovieFormViewModel, as well as updating a Movie entity from a MovieFormViewModel
public static class MovieMappingExtensions
{
    public static MovieFormViewModel ToFormViewModel(this Movie movie)
    {
        return new MovieFormViewModel
        {
            Id = movie.Id,
            Title = movie.Title,
            ReleaseDate = movie.ReleaseDate,
            Genre = movie.Genre,
            Price = movie.Price,
            Rating = movie.Rating,
            DirectorId = movie.DirectorId
        };
    }

    public static Movie ToMovie(this MovieFormViewModel model)
    {
        return new Movie
        {
            Id = model.Id,
            Title = model.Title,
            ReleaseDate = model.ReleaseDate,
            Genre = model.Genre,
            Price = model.Price,
            Rating = model.Rating,
            DirectorId = model.DirectorId
        };
    }

    public static void UpdateFrom(this Movie movie, MovieFormViewModel model)
    {
        movie.Title = model.Title;
        movie.ReleaseDate = model.ReleaseDate;
        movie.Genre = model.Genre;
        movie.Price = model.Price;
        movie.Rating = model.Rating;
        movie.DirectorId = model.DirectorId;
    }
}