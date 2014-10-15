using System;
using System.Diagnostics;

namespace Squirrel
{
    [DebuggerDisplay("{Version}")]
    public class SemVersion : IComparable
    {
        private readonly string _version;
        private readonly Version _classicVersion;

        public SemVersion(string version)
        {
            _version = version;
            var classicVersion = StripDashPartOfVersion(version);
            _classicVersion = new Version(classicVersion);
        }

        public string Version { get { return _version; } }

        public Version ClassicVersion
        {
            get { return _classicVersion; }
        }

        public static bool operator >(SemVersion x, SemVersion y)
        {
            return CompareVersions(x.Version, y.Version) > 0;
        }

        public static bool operator <(SemVersion x, SemVersion y)
        {
            return CompareVersions(x.Version, y.Version) < 0;
        }

        public static bool operator >=(SemVersion x, SemVersion y)
        {
            return CompareVersions(x.Version, y.Version) >= 0;
        }

        public static bool operator <=(SemVersion x, SemVersion y)
        {
            return CompareVersions(x.Version, y.Version) <= 0;
        }

        public static bool operator ==(SemVersion x, SemVersion y)
        {
            if (ReferenceEquals(x, null) && ReferenceEquals(y, null))
            {
                return true;
            }

            if (ReferenceEquals(x, null))
            {
                return false;
            }

            if (ReferenceEquals(y, null))
            {
                return false;
            }

            return CompareVersions(x.Version, y.Version) < 0;
        }

        public static bool operator !=(SemVersion x, SemVersion y)
        {
            return !(x == y);
        }

        public override bool Equals(object o)
        {
            try
            {
                return this == (SemVersion)o;
            }
            catch
            {
                return false;
            }
        }

        public int CompareTo(object obj)
        {
            var otherVersion = obj as SemVersion;
            if (otherVersion == null)
            {
                return 0;
            }

            return CompareVersions(Version, otherVersion.Version);
        }

        public override string ToString()
        {
            return _version;
        }

        private static string StripDashPartOfVersion(string version)
        {
            var dashIndex = version.IndexOf('-');
            if (dashIndex != -1)
            {
                version = version.Substring(0, dashIndex);
            }

            return version;
        }

        private static int CompareVersions(string versionA, string versionB)
        {
            var originalVersionA = versionA;
            var originalVersionB = versionB;

            var versionWithoutDashPart = StripDashPartOfVersion(originalVersionA);
            var versionToCheckWithoutDashPart = StripDashPartOfVersion(originalVersionB);

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
