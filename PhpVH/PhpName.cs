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

        public static string[] ArrayFunctions = new string[]
	    {
            "is_array",
		    "array_change_key_case",
            "array_chunk",
            "array_combine",
            "array_count_values",
            "array_diff_assoc",
            "array_diff_key",
            "array_diff_uassoc",
            "array_diff_ukey",
            "array_diff",
            "array_fill_keys",
            "array_fill",
            "array_filter",
            "array_flip",
            "array_intersect_assoc",
            "array_intersect_key",
            "array_intersect_uassoc",
            "array_intersect_ukey",
            "array_intersect",
            "array_key_exists",
            "array_keys",
            "array_map",
            "array_merge_recursive",
            "array_merge",
            "array_multisort",
            "array_pad",
            "array_pop",
            "array_product",
            "array_push",
            "array_rand",
            "array_reduce",
            "array_replace_recursive",
            "array_replace",
            "array_reverse",
            "array_search",
            "array_shift",
            "array_slice",
            "array_splice",
            "array_sum",
            "array_udiff_assoc",
            "array_udiff_uassoc",
            "array_udiff",
            "array_uintersect_assoc",
            "array_uintersect_uassoc",
            "array_uintersect",
            "array_unique",
            "array_unshift",
            "array_values",
            "array_walk_recursive",
            "array_walk",
            //"array",
            "arsort",
            "asort",
            "compact",
            "count",
            "current",
            "each",
            "end",
            "extract",
            "in_array",
            "key",
            "krsort",
            "ksort",
            //"list",
            "natcasesort",
            "natsort",
            "next",
            "pos",
            "prev",
            "range",
            "reset",
            "rsort",
            "shuffle",
            "sizeof",
            "sort",
            "uasort",
            "uksort",
            "usort"
	    };

        public const string Eval = "eval",
            System = "system",
            Exec = "exec",
            ShellExec = "shell_exec",
            PassThru = "passthru",
            MoveUploadedFile = "move_uploaded_file";
    }
}
