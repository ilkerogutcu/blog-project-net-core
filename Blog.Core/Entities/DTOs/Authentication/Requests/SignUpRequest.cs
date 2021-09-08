using Microsoft.AspNetCore.Http;

namespace Blog.Core.Entities.DTOs.Authentication.Requests
{
    public class SignUpRequest : IDto
    {
        /// <summary>
        ///     First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     User image url
        /// </summary>
        public IFormFile Image { get; set; }

        /// <summary>
        ///     Biography
        /// </summary>
        public string Bio { get; set; }

        /// <summary>
        ///     Gender ex: 'F'=female 'M'=male
        /// </summary>
        public char Gender { get; set; }

        /// <summary>
        /// Role
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        ///     Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     This variable validates for first password entered
        /// </summary>
        public string ConfirmPassword { get; set; }
    }
}