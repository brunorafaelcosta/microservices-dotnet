using System.ComponentModel.DataAnnotations;

namespace Services.Identity.STS.Models.Account
{
    public class LogoutViewModel
    {
        public string LogoutId { get; set; }

        public string Name { get; set; }
    }
}