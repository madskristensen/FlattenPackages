// Guids.cs
// MUST match guids.h
using System;

namespace MadsKristensen.FlattenPackages
{
    static class GuidList
    {
        public const string guidFlattenPackagesPkgString = "96559c66-f326-40e2-95c1-449a80387524";
        public const string guidFlattenPackagesCmdSetString = "d09a3dfa-d8e5-473a-8093-1ff848784077";

        public static readonly Guid guidFlattenPackagesCmdSet = new Guid(guidFlattenPackagesCmdSetString);
    };

    static class PkgCmdIDList
    {
        public const uint cmdidFlattenPackages = 0x100;
    };
}