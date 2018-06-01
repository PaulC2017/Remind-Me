using System.ComponentModel.DataAnnotations;



namespace RemindMe.ViewModels
{
    public class RegisterUserViewModel
    {

        [Required(ErrorMessage = "A User Name is Required")]
        [Display(Name = "User Name")]
        public string Username { get; set; }

        [Required(ErrorMessage = "A Password of at least 6 and maximum of 15 characters  is Required")]
        [Display(Name = "Password")]
        [StringLength(15, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Verify Password Must Match Password")]
        [Display(Name = "Verify Password")]
        [Compare("Password")]
        public string VerifyPassword { get; set; }

        [Required(ErrorMessage = "You must enter a 10 digit cell phone number where reminder text messages will be sent")]
        [Display(Name = "Cell Phone Number")]
        [StringLength(10, MinimumLength = 10)]
        public string CellPhoneNumber { get; set; }

        [Required(ErrorMessage = "Your Google Calendar Email is Required")]
        [Display(Name = "Google Calendar Email")]
        [EmailAddress]
        public string GCalEmail { get; set; }

        [Required(ErrorMessage = "A Password of at least 6 and maximum of 15 characters is Required")]
        [Display(Name = "Google Calendar Email Password")]
        [StringLength(15, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string GCalEmailPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Verify Google Calendar Email Must Match Google Calendar Email Password")]
        [Display(Name = "Verify Google Calendar Email Password")]
        [Compare("GCalEmailPassword")]
        public string VerifyGCalEmailPassword { get; set; }


        public string UserCreateDate { get; set; }
        public int ID { get; set; }

    }
}
