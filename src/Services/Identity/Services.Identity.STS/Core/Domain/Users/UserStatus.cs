namespace Services.Identity.STS.Core.Domain.Users
{
    public enum UserStatus
    {
        Active = 1,
        Inactive,
        LockedOut,
        RequiresVerification
    }
}
