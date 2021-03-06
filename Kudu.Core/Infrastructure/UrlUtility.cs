﻿namespace Kudu.Core.Infrastructure {
    internal static class UrlUtility {
        internal static string EnsureTrailingSlash(string url) {
            if (url.EndsWith("/")) {
                return url;
            }
            return url + "/";
        }
    }
}
