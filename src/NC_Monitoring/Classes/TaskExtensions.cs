using Microsoft.AspNetCore.Mvc;
using NC_Monitoring.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Threading.Tasks
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Pokud nenastane v tasku vyjimka tak je vracen OkResult, jinak je vracen BadRequestResult
        /// s chybovou hlaskou.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static IActionResult WaitForActionResult(this Task task)
        {
            if (!task.TryCatchAsync(out string error))
            {                
                return new BadRequestObjectResult(error);
            }

            return new OkResult();
        }

        /// <summary>
        /// Pokud v tasku nastane vyjimka, tak je zachycena a do out parametru je vlozne UI zprava
        /// pro koncoveho uzivatele.
        /// Vraci TRUE pokud NENI zachycena vyjimka.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static bool TryCatchAsync(this Task task, out string errorMessage)
        {
            string tmp = null;
            task.TryCatchAsync(exception =>
            {
                tmp = exception.UIMessage();
            }).Wait();

            errorMessage = tmp;

            return errorMessage == null;
        }
    }
}
