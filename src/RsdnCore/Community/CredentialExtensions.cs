namespace Rsdn.Community
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using static System.String;

    public static class CredentialExtensions
    {
        public static bool IsEmpty(this NetworkCredential credential)
        {
            return IsNullOrEmpty(credential.UserName) && IsNullOrEmpty(credential.Password);
        }

        public static bool IsInvalid(this NetworkCredential credential)
        {
            return IsNullOrWhiteSpace(credential.UserName) || IsNullOrWhiteSpace(credential.Password);
        }
    }
}