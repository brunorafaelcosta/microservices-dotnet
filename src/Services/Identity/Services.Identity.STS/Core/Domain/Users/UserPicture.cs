using Transversal.Domain.Entities;

namespace Services.Identity.STS.Core.Domain.Users
{
    public class UserPicture : AuditedEntity<int>
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public string Name { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }

        public byte[] Data { get; set; }
    }
}
