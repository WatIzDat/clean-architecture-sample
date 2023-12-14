using Application.Followers.StartFollowing;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("[controller]")]
    public sealed class FollowersController : ApiController
    {
        public FollowersController(ISender sender)
            : base(sender)
        {
        }

        [HttpPost]
        public async Task<IActionResult> StartFollowing(
            [FromBody] StartFollowingCommand command,
            CancellationToken cancellationToken)
        {
            Result result = await Sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
