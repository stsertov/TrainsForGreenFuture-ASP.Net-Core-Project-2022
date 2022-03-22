namespace TrainsForGreenFuture.Infrastructure.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Category;
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(nameMaxLength, MinimumLength = nameMinLength)]
        public string Name { get; set; }

    }
}
