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
                .Replace(".nupkg", string.Empty).Replace("-delta", string.Empty).Replace("-full", string.Empty);

            var numberRegex = new Regex(@"^\d+$");

            // While we can find a dash (-), check if the first item is a number (required in versioning, the rest is free to use)
            var dashIndex = strippedFileName.IndexOf('-');
            while (dashIndex >= 0)
            {
                // A version can be splitted by . or -
                var prefix = strippedFileName.Substring(0, dashIndex).Split(new [] { ".", "-" }, StringSplitOptions.RemoveEmptyEntries)[0];
                if (numberRegex.IsMatch(prefix))
                {
                    // We have the version
                    break;
                }

                // Strip and continue
                strippedFileName = strippedFileName.Substring(dashIndex + 1);
                dashIndex = strippedFileName.IndexOf('-');
            }

            return new SemVersion(strippedFileName);
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
