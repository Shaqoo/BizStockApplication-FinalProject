using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Users.UpdateLostAccessRequest
{
    public class UpdateLostAccessRequestCommand : IRequest<Result<Guid>>
    {
        public Guid RequestId { get; set; }
        public UpdateLostAccessRequestDto Dto { get; set; } = null!;
    }
}
