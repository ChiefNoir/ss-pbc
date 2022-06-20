﻿namespace Abstractions.Security
{
    public static class Restrictions
    {
        public const string AuthorizedRoles = RoleNames.Admin + " , " + RoleNames.Demo;

        public const string EditorRoles = RoleNames.Admin;
    }
}
