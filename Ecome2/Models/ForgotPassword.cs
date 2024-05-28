﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ecome2.Models
{
    public class ForgotPassword
    {
        [Required, EmailAddress, Display(Name = "Registered email address")]
        public string Email { get; set; }
        public bool EmailSent { get; set; }
    }
}
