using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models
{
    public class MovieDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [Display(Name = "Director")]
        public string DirectorName { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Genre { get; set; }

        public decimal Price { get; set; }

        public string Rating { get; set; }

        // add reviews to the movie details view model
        public int DirectorId { get; set; }

        public ICollection<Review>? Reviews { get; set; }

    }
}
