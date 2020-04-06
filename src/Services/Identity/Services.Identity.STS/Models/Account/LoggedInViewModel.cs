using System.Collections.Generic;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace Services.Identity.STS.Models.Account
{
    public class LoggedInViewModel
    {
        public string Name { get; set; }
        
        public Dictionary<string, string> Claims { get; set; }
    }
}