using System.ComponentModel.DataAnnotations;

namespace Ecom.Core.DTO
{
    public record class RegisterDTO
    {
        [Required]
        public string FullName { get; init; }

        [Required]
        public string DisplayName { get; init; }

        [Required, EmailAddress]
        public string Email { get; init; }

        [Required, MinLength(6)]
        public string Password { get; init; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; init; }
    }
}
