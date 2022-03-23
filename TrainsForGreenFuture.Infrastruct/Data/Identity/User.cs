namespace TrainsForGreenFuture.Infrastructure.Data.Identity
{

    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    using static DataConstants.User;
    public class User : IdentityUser
    {
        [Required]
        [StringLength(nameMaxLength, MinimumLength = nameMinLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(nameMaxLength, MinimumLength = nameMinLength)]
        public string LastName { get; set; }


        [StringLength(companyMaxLength, MinimumLength = companyMinLength)]
        public string Company { get; set; }
    }
}
