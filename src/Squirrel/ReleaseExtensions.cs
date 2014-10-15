using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Squirrel
{
    public static class VersionExtensions
    {
        public static SemVersion ToVersion(this IReleasePackage package)
        {
            return package.InputPackageFile.ToVersion();
        }

        public static SemVersion ToVersion(this string fileName)
        {
            var strippedFileName = (new FileInfo(fileName)).Name
                .Replace(".nupkg", string.Empty).RemoveSquirrelPostfixes();

            var numberRegex = new Regex(@"^\d+$");

            // While we can find a , check if the first item is a number (required in versioning, the rest is free to use)
            var separatorIndex = strippedFileName.FindVersionPackageNameSeparatorIndex();
            while (separatorIndex >= 0)
            {
                // A version can be splitted by . or -
                var prefix = strippedFileName.Substring(0, separatorIndex).Split(new [] { ".", "-" }, StringSplitOptions.RemoveEmptyEntries)[0];
                if (numberRegex.IsMatch(prefix))
                {
                    // We have the version
                    break;
                }

                // Strip and continue
                strippedFileName = strippedFileName.Substring(separatorIndex + 1);
                separatorIndex = strippedFileName.FindVersionPackageNameSeparatorIndex();
            }

            return new SemVersion(strippedFileName);
        }

        public static string RemoveSquirrelPostfixes(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return value.Replace("-delta", string.Empty).Replace("-full", string.Empty);
        }

        private static int FindVersionPackageNameSeparatorIndex(this string value)
        {
            var dotIndex = value.IndexOf('.');
            var dashIndex = value.IndexOf('-');

            if (dotIndex < 0 && dashIndex < 0)
            {
                return -1;
            }

            if (dotIndex == -1)
            {
                return dashIndex;
            }

            if (dashIndex == -1)
            {
                return dotIndex;
            }

            return Math.Min(dotIndex, dashIndex);
        }

        public static SemVersion GetMaxVersion(this IEnumerable<ReleaseEntry> releaseEntries)
        {
            return releaseEntries.Select(x => x.Version).Max();
        }

        public static SemVersion GetMaxVersion(this IEnumerable<string> versions)
        {
            return versions.Select(x => new SemVersion(x)).Max();
        }

        public static SemVersion GetMinVersion(this IEnumerable<ReleaseEntry> releaseEntries)
        {
            return releaseEntries.Select(x => x.Version).Min();
        }

        public static SemVersion GetMinVersion(this IEnumerable<string> versions)
        {
            return versions.Select(x => new SemVersion(x)).Min();
        }
    }
}
