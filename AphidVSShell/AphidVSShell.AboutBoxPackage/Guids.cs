// Guids.cs
// MUST match guids.h
using System;

namespace AphidVSShell.AboutBoxPackage
{
    static class GuidList
    {
        public const string guidAboutBoxPackagePkgString = "8f0310b2-1205-40d2-b636-89f030e375b5";
        public const string guidAboutBoxPackageCmdSetString = "9583b111-0359-43db-bd1e-8f9c4b87c87e";

        public static readonly Guid guidAboutBoxPackageCmdSet = new Guid(guidAboutBoxPackageCmdSetString);
    };
}