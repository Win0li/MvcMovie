namespace MvcMovie.Models
{
    public class ReviewSummaryViewModel
    {
       
        public int Id { get; set; }
        public decimal RatingScore { get; set; }
        public string Comment { get; set; }
        // need name of the reviewer
        public string ReviewerName { get; set; }

        //  need date of the review
        public DateTime CreatedAt { get; set; }


    }
}
