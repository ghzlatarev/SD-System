﻿using Microsoft.AspNetCore.Identity;
using SD.Data.Models.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace SD.Data.Models.Identity
{
    public class ApplicationUser : IdentityUser, IDeletable, IAuditable
    {
        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }

        public byte[] AvatarImage { get; set; }

        [StringLength(25, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string FirstName { get; set; }

        [StringLength(25, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string LastName { get; set; }

        public string Story { get; set; }

        public bool GDPR { get; set; }
    }
}
