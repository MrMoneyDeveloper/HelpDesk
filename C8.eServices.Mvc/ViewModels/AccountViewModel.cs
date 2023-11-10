using System;
using System.ComponentModel.DataAnnotations;

namespace C8.eServices.Mvc.ViewModels
{
    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,15}$", ErrorMessage = "The password must contain at least one uppercase character, one lowercase character, one numeric and one special character")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        public string MobileNumber { get; set; }

        // Add the new properties with appropriate data annotations
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [Display(Name = "System User Type")]
        public string SystemUserTypeId { get; set; }

        [Display(Name = "Status")]
        public string StatusId { get; set; }

        [Display(Name = "Password Reset")]
        public bool IsPasswordReset { get; set; }

        [Display(Name = "Temporary Password")]
        public bool IsTemporaryPassword { get; set; }

        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name = "Identification Number")]
        public string IdentificationNumber { get; set; }

        [Display(Name = "Notification Type")]
        public int? NotificationTypeId { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Locked")]
        public bool? IsLocked { get; set; }

        [Display(Name = "Created By System User")]
        public int? CreatedBySystemUserId { get; set; }

        [Display(Name = "Created Date and Time")]
        public DateTime? CreatedDateTime { get; set; }

        [Display(Name = "Modified By System User")]
        public int? ModifiedBySystemUserId { get; set; }

        [Display(Name = "Modified Date and Time")]
        public DateTime? ModifiedDateTime { get; set; }

        [Display(Name = "Client ID")]
        public int? ClientId { get; set; }

        [Display(Name = "Employee ID")]
        public int? EmployeeId { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Period To")]
        public DateTime? PeriodTo { get; set; }

        // Add the property for preferred method of communication
        [Display(Name = "Preferred Method Of Communication")]
        public int PrefMethodOfCommunication { get; set; }
    }


    public class LoginViewModel
    {
        [Required]
        [Display( Name = "Username" )]
        public string UserName { get; set; }

        [Required]
        [DataType( DataType.Password )]
        [Display( Name = "Password" )]
        public string Password { get; set; }

        [Display( Name = "Remember me?" )]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display( Name = "First Name" )]
        [RegularExpression( @"^[a-zA-Z ]+$", ErrorMessage = "The FirstName field should consist of characters only" )]
        public string FirstName { get; set; }

        [Required]
        [Display( Name = "Last Name" )]
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "The LastName field should consist of characters only")]
        public string LastName { get; set; }

        [Display( Name = "Email Address" )]
        [EmailAddress( ErrorMessage = "Invalid Email Address" )]
        [DataType( DataType.EmailAddress )]
        public string EmailAddress { get; set; }

        [Display( Name = "Confirm Email Address" )]
        [DataType( DataType.EmailAddress )]
        [Compare( "EmailAddress", ErrorMessage = "The email and confirmation email do not match." )]
        public string ConfirmEmailAddress { get; set; }

        [Phone(ErrorMessage = "Invalid Mobile Number")]
        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        public string MobileNumber { get; set; }

        [Display(Name = "Confirm Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Compare("MobileNumber", ErrorMessage = "The mobile number and confirmation mobile number do not match.")]
        public string ConfirmMobileNumber { get; set; }

        [Display( Name = "Designation" )]
        public string Designation { get; set; }

        [Display( Name = "Company Name" )]
        public string CompanyName { get; set; }
        [Required]
        [Display(Name = "RSA ID No")]
        [MaxLength(13)]
        [MinLength(13)]
        public string IdentificationNumber { get; set; }

        [Required]
        [Display( Name = "Username" )]
        [RegularExpression(@"^[a-zA-Z0-9]{6,20}$", ErrorMessage = "The Username field must be 6 to 20 characters long, without any special characters eg. !,@,#,$,%,&,*.")]
        public string UserName { get; set; }

        [Required]
        [StringLength( 100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6 )]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,15}$", ErrorMessage = "The password must contain at least one uppercase character, one lowercase character, one numeric and one special character")]
        [DataType( DataType.Password )]
        [Display( Name = "Password" )]
        public string Password { get; set; }

        [DataType( DataType.Password )]
        [Display( Name = "Confirm password" )]
        [Compare( "Password", ErrorMessage = "The password and confirmation password do not match." )]
        public string ConfirmPassword { get; set; }
        
        [Display( Name = "System User Type" )]
        public string SystemUserTypeId { get; set; }
        
        [Display( Name = "Status" )]
        public string StatusId { get; set; }
    }

    public class RegisterViewModelApi
    {
        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "The FirstName field should consist of characters only")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "The LastName field should consist of characters only")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Username")]
        [RegularExpression(@"^[a-zA-Z0-9]{6,20}$", ErrorMessage = "The Username field must be 6 to 20 characters long, without any special characters eg. !,@,#,$,%,&,*.")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,15}$", ErrorMessage = "The password must contain at least one uppercase character, one lowercase character, one numeric and one special character")]
        //[DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "System User Type")]
        public string SystemUserTypeId { get; set; }

        [Display(Name = "Status")]
        public string StatusId { get; set; }

        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [Display(Name = "ID/ Passport No")]
        [MaxLength(100)]
        public string IdentificationNumber { get; set; }

        public string CellPhoneNumber { get; set; }
        public string WorkPhoneNumber { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        
        [EmailAddress]
        [Display( Name = "Email" )]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }
    }
}