using System;

namespace FlickrNet
{
    /// <summary>
    /// Summary description for SafeNativeMethods.
    /// </summary>
#if !WindowsCE && !SILVERLIGHT && !DOTNETSTANDARD
    [System.Security.SuppressUnmanagedCodeSecurity]
#endif
    internal class SafeNativeMethods 
    {
        private SafeNativeMethods()
        {
        }

        internal static int GetErrorCode(System.IO.IOException ioe)
        {
#if !WindowsCE && !SILVERLIGHT && !DOTNETSTANDARD
            var permission = new System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode);
            permission.Demand();

            return System.Runtime.InteropServices.Marshal.GetHRForException(ioe) & 0xFFFF;
#else
            return 0;
#endif
        }
    }
}
