namespace TrainsForGreenFuture.Infrastructure.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Interrail;
    public class Interrail
    {
        [Key]
        public int Id { get; set; }

        [Range(minLength, maxLength)]
        public int Length { get; set; }
    }
}
