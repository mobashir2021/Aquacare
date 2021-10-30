using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OnDemandService.Models
{
    public class UserViewModel
    {
        [Key]
        public int UserViewModelId { get; set; }

        [Display(Name = "Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage ="Name required")]
        public string Name { get; set; }

        [Display(Name="Email")]
        [Required(AllowEmptyStrings =false, ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage ="Address required")]
        public string Address { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage = "Mobile No required")]
        [DataType(DataType.PhoneNumber)]
        public string MobileNo { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage ="User name required")]
        public string userName { get; set; }

        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage ="Minimum six character required")]
        [Required(AllowEmptyStrings =false, ErrorMessage ="Password required")]
        public string Password { get; set; }

        [Display(Name="Confirm Password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password doesnot match")]
        public string ConfirmPassword { get; set; }

        public string SubCategoryId { get; set; }

        public string PincodeId { get; set; }

        public List<SelectListItem> ServiceModel { get; set; }
        public int[] ServiceIds { get; set; }
    }
}