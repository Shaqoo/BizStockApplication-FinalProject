using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Products.UpdatePicture
{
    public record UpdateProductPictureCommand(
        UpdateProductPictureDto PictureDto,
        RequestMetadata RequestMetadata) : IRequest<Result<string>>;
   

}
