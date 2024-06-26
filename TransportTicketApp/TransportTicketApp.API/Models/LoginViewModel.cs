﻿namespace TransportTicketApp.API.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
    public class CreateRoleModel
    {
        public string RoleName { get; set; } = string.Empty;
    }
}
