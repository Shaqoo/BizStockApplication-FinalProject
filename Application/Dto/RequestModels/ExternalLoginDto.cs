using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.RequestModels
{
    public record ExternalLoginDto(string AccessToken, string? Provider = null);
    public class FacebookUserDto
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
    }

    public class GitHubEmailDto
    {
        public string Email { get; set; } = default!;
        public bool Primary { get; set; }
        public bool Verified { get; set; }
        public string Visibility { get; set; } = string.Empty;
    }
    public class GitHubUserDto
    {
        public string? Login { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }  
    }
    public class MicrosoftUserDto
    {
        public string? DisplayName { get; set; }
        public string? Mail { get; set; }  
        public string? UserPrincipalName { get; set; }  
        public string? Id { get; set; }
    }


}
