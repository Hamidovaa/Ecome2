﻿using System.ComponentModel.DataAnnotations;

namespace Ecome2.Models
{
    public class ResetPassword
    {
        public string Token { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

    }
}
