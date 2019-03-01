using Microsoft.AspNetCore.Mvc;
using NC_Monitoring.Business.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.Controllers
{
    public class UsersController : BaseController
    {
        private readonly ApplicationUserManager userManager;
        public UsersController(ApplicationUserManager userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
