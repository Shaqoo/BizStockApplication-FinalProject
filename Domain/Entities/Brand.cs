using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Brand
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string? Description { get; private set; }
        public string WebsiteUrl { get; private set; } = default!;
        public string LogoUrl { get; private set; } = default!;
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private readonly HashSet<Product> _products = new();
        public IReadOnlyCollection<Product> Products => _products;

        private Brand() { }  

        public Brand(string name, string websiteUrl, string logoUrl, string? description = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            WebsiteUrl = websiteUrl;
            LogoUrl = logoUrl;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        public void UpdateDetails(string name,string websiteUrl, string logoUrl, string? description)
        {
            Name = name;
            Description = description;
            WebsiteUrl = websiteUrl;
            LogoUrl = logoUrl;
            UpdatedAt = DateTime.UtcNow;
        }
             
        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Brand name cannot be empty.", nameof(name));
            Name = name;
            UpdatedAt = DateTime.UtcNow;
        }
        public void UpdateWebsiteUrl(string websiteUrl)
        {
            if (string.IsNullOrWhiteSpace(websiteUrl))
                throw new ArgumentException("Website URL cannot be empty.", nameof(websiteUrl));
            WebsiteUrl = websiteUrl;
            UpdatedAt = DateTime.UtcNow;
        }
        public void UpdateLogoUrl(string logoUrl)
        {
            if (string.IsNullOrWhiteSpace(logoUrl))
                throw new ArgumentException("Logo URL cannot be empty.", nameof(logoUrl));
            LogoUrl = logoUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDescription(string? description)
        {
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }


        public void Activate()
        {
            if (!IsActive)
                IsActive = true;
        }

        public void Deactivate()
        {
            if (IsActive)
                IsActive = false;
        }
     }
}
