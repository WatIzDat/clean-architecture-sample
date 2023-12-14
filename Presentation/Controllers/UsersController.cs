using Application.Abstractions.Data;
using Application.Users;
using Application.Users.Create;
using Application.Users.GetByEmail;
using Application.Users.GetById;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("[controller]")]
    public sealed class UsersController : ApiController
    {
        public UsersController(ISender sender)
            : base(sender)
        {
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
        {
            GetUserByIdQuery query = new(id);

            Result<UserResponse> result = await Sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByEmail(
            [FromQuery] string email,
            CancellationToken cancellationToken)
        {
            GetUserByEmailQuery query = new(email);

            Result<UserResponse> result = await Sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(
            [FromBody] CreateUserCommand command,
            CancellationToken cancellationToken)
        {
            Result<Guid> result = await Sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
