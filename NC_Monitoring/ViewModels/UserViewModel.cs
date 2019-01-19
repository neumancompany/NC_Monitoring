using System;

namespace NC_Monitoring.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }

        public string RoleName { get; set; }

        public bool AllowDeleting { get; set; }
        public bool AllowEditing { get; set; }
        public bool GlobalAdmin { get; set; }

        public bool AllowResetPassword { get; set; }
    }
}
