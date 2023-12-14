using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Abstractions
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected readonly ISender Sender;

        protected ApiController(ISender sender)
        {
            Sender = sender;
        }
    }
}
