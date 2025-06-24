using System.ComponentModel.DataAnnotations;

namespace BlazorUI.Models
{
    public class ClientDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
