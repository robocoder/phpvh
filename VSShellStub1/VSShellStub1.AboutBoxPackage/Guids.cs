// Guids.cs
// MUST match guids.h
using System;

namespace VSShellStub1.AboutBoxPackage
{
    static class GuidList
    {
        public const string guidAboutBoxPackagePkgString = "fe318031-58eb-4378-aec7-1c292d592cdf";
        public const string guidAboutBoxPackageCmdSetString = "ab42fe68-a5b0-478d-8a65-56c060ddc80a";

        public static readonly Guid guidAboutBoxPackageCmdSet = new Guid(guidAboutBoxPackageCmdSetString);
    };
}