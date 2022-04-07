namespace TrainsForGreenFuture.Areas.Admin.Models.Renovations
{
    using System.ComponentModel.DataAnnotations;

    using static TrainsForGreenFuture.Infrastructure.Data.DataConstants.Renovation;
    public class RenovationAdminPictureFormModel
    {
        public string Id { get; set; }

        [StringLength(pictureMaxLength)]
        public string RenovationPicture { get; set; }
    }
}
