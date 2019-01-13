using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Exceptions;
using System;
using System.Data.SqlClient;

namespace NC_Monitoring.Data.Extensions
{
    public static class ExceptionExtensions
    {
        public static string UIMessage(this Exception ex)
        {
            Exception tmp;

            if (TryGetClosestExceptionOfType<UIException>(ex, out tmp))
            {
                return ex.Message;
            }
            else if (TryGetClosestExceptionOfType<SqlException>(ex, out tmp))
            {
                switch ((SqlExceptionCodeEnum)(tmp as SqlException).Number)
                {
                    case SqlExceptionCodeEnum.DeleteConstraint:
                        return "You have to delete all constraints before delete that item";
                    case SqlExceptionCodeEnum.UniqueConstraint:
                        return "Duplicate record.";
                }
            }

            return "Unknown error";
        }


        private static bool TryGetClosestExceptionOfType<T>(Exception ex, out Exception foundException) where T : Exception
        {
            foundException = GetClosestExceptionOfType<T>(ex);

            return foundException != null;
        }

        private static bool TryGetClosestExceptionOfType<T>(Exception ex, out T foundException) where T : Exception
        {
            foundException = GetClosestExceptionOfType<T>(ex);

            return foundException != null;
        }

        private static T GetClosestExceptionOfType<T>(Exception ex) where T : Exception
        {
            Exception tmp = ex;

            while (tmp != null && !(tmp is T))
            {
                tmp = tmp.InnerException;
            }

            return tmp as T;
        }        
    }
}
