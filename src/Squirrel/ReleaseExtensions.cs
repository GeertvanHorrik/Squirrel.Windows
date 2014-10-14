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
        public static string ToVersion(this IReleasePackage package)
        {
            return package.InputPackageFile.ToVersion();
        }

        public static string ToVersion(this string fileName)
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

            return strippedFileName;
        }

        public static string StripDashPartOfVersion(this string version)
        {
            var dashIndex = version.IndexOf('-');
            if (dashIndex != -1)
            {
                version = version.Substring(0, dashIndex);
            }

            return version;
        }

        public static string GetMaxVersion(this IEnumerable<ReleaseEntry> releaseEntries)
        {
            return GetMaxVersion(releaseEntries.Select(x => x.Version));
        }

        public static string GetMaxVersion(this IEnumerable<string> versions)
        {
            var versionArray = versions.ToArray();

            var maxVersion = versionArray.FirstOrDefault();

            for (int i = 1; i < versionArray.Length; i++)
            {
                var version = versionArray[i];
                if (version.IsLargerThan(maxVersion))
                {
                    maxVersion = version;
                }
            }

            return maxVersion;
        }

        public static string GetMinVersion(this IEnumerable<ReleaseEntry> releaseEntries)
        {
            return GetMinVersion(releaseEntries.Select(x => x.Version));
        }

        public static string GetMinVersion(this IEnumerable<string> versions)
        {
            var versionArray = versions.ToArray();

            var minVersion = versionArray.FirstOrDefault();

            for (int i = 1; i < versionArray.Length; i++)
            {
                var version = versionArray[i];
                if (version.IsSmallerThan(minVersion))
                {
                    minVersion = version;
                }
            }

            return minVersion;
        }

        public static bool IsLargerThan(this string version, string versionToCheck)
        {
            return CompareVersions(version, versionToCheck) > 0;
        }

        public static bool IsSmallerThan(this string version, string versionToCheck)
        {
            return CompareVersions(version, versionToCheck) < 0;
        }

        private static int CompareVersions(string versionA, string versionB)
        {
            var originalVersionA = versionA;
            var originalVersionB = versionB;

            var versionWithoutDashPart = originalVersionA.StripDashPartOfVersion();
            var versionToCheckWithoutDashPart = originalVersionB.StripDashPartOfVersion();

            versionA = versionWithoutDashPart;
            versionB = versionToCheckWithoutDashPart;

            if (string.Equals(versionWithoutDashPart, versionToCheckWithoutDashPart))
            {
                // Without dash part, versions are equal, special care

                // If 1 of the items does not contain a dash, treat that as larger (1.0.0 is larger than 1.0.0-beta)
                if (string.Equals(originalVersionA, versionA) && !string.Equals(originalVersionB, versionB))
                {
                    return 1;
                }

                // If 1 of the items does not contain a dash, treat that as larger (1.0.0-beta is smaller than 1.0.0)
                if (!string.Equals(originalVersionA, versionA) && string.Equals(originalVersionB, versionB))
                {
                    return -1;
                }

                // Full compare
                versionA = originalVersionA;
                versionB = originalVersionB;
            }

            return string.Compare(versionA, versionB);
        }
    }
}
