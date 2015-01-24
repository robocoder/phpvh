using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpVH
{
    public static class PhpName
    {
        public const string Get = "$_GET",
            Post = "$_POST",
            Request = "$_REQUEST",
            Files = "$_FILES",
            Cookie = "$_COOKIE";

        public static readonly string[] Superglobals = new[]
        {
            Get,
            Post,
            Request,
            Files,
            Cookie
        };

        public static string[] SuperGlobalNames = new[]
        {
	        "GET",
	        "POST",
	        "COOKIE",
	        "REQUEST",
        };

        public static readonly string[] IncludeFunctions = new[]
        {
            "require",
            "include",
            "require_once",
            "include_once",
        };        
    }
}
