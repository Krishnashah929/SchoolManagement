using System;
using SM.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SM.Models
{
    public class LoginModel
    {

        /// <summary>
        /// Email Address input feild.
        /// </summary>
        [Required(ErrorMessage = CommonValidations.PleaseEnterValidEmail)]
        [MaxLength(50)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Password input feild.
        /// </summary>
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        [DataType(DataType.Password)]
        [MaxLength(10)]
        public string Password { get; set; }
    }
}
