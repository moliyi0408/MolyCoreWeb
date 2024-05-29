﻿using System.ComponentModel.DataAnnotations;

namespace MolyCoreWeb.Models.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
