using Microsoft.AspNetCore.Mvc;

namespace NC_Monitoring.Controllers.Base
{
    [Route("api/[controller]/[action]")]
    public abstract class BaseApiController : ControllerBase
    {
    }
}