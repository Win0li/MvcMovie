using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int MovieId { get; set; }

        public Movie? Movie { get; set; }
        public string? ReviewerName { get; set; }

        [Column(TypeName = "decimal(3, 1)")]
        public decimal RatingScore { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
