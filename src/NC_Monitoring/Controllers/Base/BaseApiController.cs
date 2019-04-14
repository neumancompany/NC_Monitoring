using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NC_Monitoring.Controllers.Base
{
    [Authorize]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
    }
}