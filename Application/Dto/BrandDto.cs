using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public record BrandDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public string WebsiteUrl { get; init; } = default!;
        public string LogoUrl { get; init; } = default!;
        public string? Description { get; init; }
    }

}
