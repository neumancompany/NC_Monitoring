using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NC_Monitoring.Controllers.Base
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public abstract class BaseApiController : ControllerBase
    {
    }
}