using System;

namespace Backend.Core.Enums
{
    [Flags]
    public enum UserRole
    {
        User = 0,
        Admin = 1,
        Paladin = 99
    }
}
