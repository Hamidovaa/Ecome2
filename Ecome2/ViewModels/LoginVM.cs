﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Ecome2.ViewModels
{
    public class LoginVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [ValidateNever]
        public bool IsRemember {  get; set; }
        [ValidateNever]
        public string Name {  get; set; }
    }
}
