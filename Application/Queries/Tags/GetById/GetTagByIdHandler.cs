using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Tags.GetById
{
    public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, Result<TagDto>>
    {
        private readonly ITagRepository _tagRepository;
        private readonly ILogger<GetTagByIdQueryHandler> _logger;
        private readonly IMemoryCacheService _cacheService;

        public GetTagByIdQueryHandler(
            ITagRepository tagRepository,
            ILogger<GetTagByIdQueryHandler> logger,
            IMemoryCacheService cacheService)
        {
            _tagRepository = tagRepository;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<Result<TagDto>> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching tag with Id: {TagId}", request.Id);
            string cacheKey = $"Tag_{request.Id}";

            var tag = await _cacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    return await _tagRepository.GetByIdAsync(request.Id);
                },TimeSpan.FromMinutes(10));


            if (tag == null)
            {
                _logger.LogWarning("Tag with Id: {TagId} not found", request.Id);
                return Result<TagDto>.Failure("Tag not found");
            }

            var dto = new TagDto
            {
                Id = tag.Id,
                Name = tag.Name
            };

            return Result<TagDto>.Success(dto);
        }
    }

}
