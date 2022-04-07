namespace TrainsForGreenFuture.Areas.Admin.Models.Renovations
{
    using System.ComponentModel.DataAnnotations;

    using static TrainsForGreenFuture.Infrastructure.Data.DataConstants.Renovation;
    public class RenovationAdminFromModel
    {
        [Required]
        [Range(minDeadline, maxDeadline)]
        public int? Deadline { get; set; }

        [Required]
        [Range(minPriceValue, maxPriceValue)]
        public decimal? Price { get; set; }

        [StringLength(commentMaxLength, MinimumLength = commentMinLength)]
        public string Comment { get; set; }
    }
}
