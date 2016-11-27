using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace TheWorld.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
          
        [Required]
        public string Password { get; set; }
    }
}