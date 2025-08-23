using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.RequestModels
{
    public record CreateBrandDto
    {
        public required string Name { get; init; }
        public required string WebsiteUrl { get; init; }
        public required string LogoUrl { get; init; }
        public string? Description { get; init; }
    }

    public record UpdateBrandDto
    {
        public required Guid Id { get; init; }
        public string? Name { get; init; }
        public string? WebsiteUrl { get; init; }
        public string? LogoUrl { get; init; }
        public string? Description { get; init; }
    }


}
