using SM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace SM.Entity.Entity
{
    public partial class User
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public int UserId { get; set; }

        /// <summary>
        /// FirstName input feild.
        /// </summary>
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        [MaxLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// LastName input feild.
        /// </summary>
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        [MaxLength(50)]
        public string Lastname { get; set; }

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

        /// <summary>
        /// ISactive feild for users.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// IsDeleted feild when you have to delete user.
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// IsBlocked feild when you have to delete user.
        /// </summary>
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Created by field for first time of creation of user.
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Created date field for first time of user create account date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// ModifiedBy field for modiffy details of user.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// ModifiedDate field for user modify their details.
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// RetypePassword it is for confirm user's password.
        /// This value is not stored into the database.
        /// </summary>
        [NotMapped]
        [Required(ErrorMessage = CommonValidations.RetypePasswordMesg)]
        [Compare("Password", ErrorMessage = CommonValidations.ComparePasswordMsg)]
        [DataType(DataType.Password)]
        [MaxLength(10)]
        public string RetypePassword { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
