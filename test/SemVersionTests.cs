using System;
using Xunit;

namespace Squirrel.Tests
{
    public class SemVersionTests
    {
        public class TheToVersionMethod
        {
            [Theory]
            [InlineData("MyProduct-1.7.0-unstable0036-full.nupkg", "1.7.0-unstable0036")]
            [InlineData("MyProduct-1.7.0-full.nupkg", "1.7.0")]
            [InlineData("MyProduct.1.7.0-unstable0036-full.nupkg", "1.7.0-unstable0036")]
            [InlineData("MyProduct.1.7.0-full.nupkg", "1.7.0")]
            public void ReturnsValidVersionForSemVer(string input, string expectedOutput)
            {
                var output = input.ToVersion();

                Assert.StrictEqual(expectedOutput, output.Version);
            }
        }

        public class TheClassicVersionProperty
        {
            [Theory]
            [InlineData("1.1.0-unstable0036", "1.1.0")]
            [InlineData("1.1.0-beta", "1.1.0")]
            [InlineData("1.1.0", "1.1.0")]
            public void ReturnsClassicVersionForAllVersions(string input, string expectedOutput)
            {
                var version = new SemVersion(input);
                var expectedVersion = new Version(expectedOutput);

                Assert.Equal(expectedVersion, version.ClassicVersion);
            }
        }

        public class TheIsLargerOperator
        {
            [Theory]
            [InlineData("1.1.0", "1.1.0", false)]
            [InlineData("1.2.0", "1.1.0", true)]
            [InlineData("1.1.0", "1.2.0", false)]
            [InlineData("1.2.0-beta", "1.1.0", true)]
            [InlineData("1.1.0", "1.2.0-beta", false)]
            [InlineData("1.1.0", "1.1.0-beta", true)]
            [InlineData("1.1.0-beta", "1.1.0", false)]
            [InlineData("1.0.0-unstable0016", "1.0.0", false)]
            [InlineData("1.0.0-unstable0016", "1.0.0-beta0001", false)]
            [InlineData("1.0.0-beta0016", "1.0.0-unstable0001", true)]
            public void ReturnsRightBooleanForVersionComparison(string input, string versionToCheck, bool expectedOutput)
            {
                var output = new SemVersion(input) > new SemVersion(versionToCheck);

                Assert.Equal(expectedOutput, output);
            }
        }

        public class TheIsSmallerOperator
        {
            [Theory]
            [InlineData("1.1.0", "1.1.0", false)]
            [InlineData("1.2.0", "1.1.0", false)]
            [InlineData("1.1.0", "1.2.0", true)]
            [InlineData("1.2.0-beta", "1.1.0", false)]
            [InlineData("1.1.0", "1.2.0-beta", true)]
            [InlineData("1.1.0", "1.1.0-beta", false)]
            [InlineData("1.1.0-beta", "1.1.0", true)]
            [InlineData("1.0.0-unstable0016", "1.0.0", true)]
            [InlineData("1.0.0-unstable0016", "1.0.0-beta0001", true)]
            [InlineData("1.0.0-beta0016", "1.0.0-unstable0001", false)]
            public void ReturnsRightBooleanForVersionComparison(string input, string versionToCheck, bool expectedOutput)
            {
                var output = new SemVersion(input) < new SemVersion(versionToCheck);

                Assert.Equal(expectedOutput, output);
            }
        }

        public class TheGetMaxVersionMethod
        {
            [Fact]
            public void ReturnsMaximumVersionWithStableVersion()
            {
                var input = new[]
                {
                    "1.0.0",
                    "1.0.0-beta002",
                    "1.0.0-unstable002",
                    "2.0.0",
                    "2.0.0-beta005",
                    "2.0.0-unstable0025"
                };

                var output = input.GetMaxVersion();

                Assert.Equal("2.0.0", output.Version);
            }

            [Fact]
            public void ReturnsMaximumVersionWithBetaVersion()
            {
                var input = new[]
                {
                    "1.0.0-beta002",
                    "1.0.0-unstable002",
                    "2.0.0",
                    "2.0.0-beta005",
                    "2.0.0-unstable0025"
                };

                var output = input.GetMaxVersion();

                Assert.Equal("2.0.0-beta005", output.Version);
            }
        }

        public class TheGetMinVersionMethod
        {
            [Fact]
            public void ReturnsMinimumVersionWithStableVersion()
            {
                var input = new[]
                {
                    "1.0.0",
                    "1.0.0-beta002",
                    "1.0.0-unstable002",
                    "2.0.0",
                    "2.0.0-beta005",
                    "2.0.0-unstable0025"
                };

                var output = input.GetMinVersion();

                Assert.Equal("1.0.0-unstable002", output.Version);
            }

            [Fact]
            public void ReturnsMinimumVersionWithUnstableVersion()
            {
                var input = new[]
                {
                    "1.0.0-beta002",
                    "1.0.0-unstable002",
                    "2.0.0",
                    "2.0.0-beta005",
                    "2.0.0-unstable0025"
                };

                var output = input.GetMinVersion();

                Assert.Equal("1.0.0-unstable002", output.Version);
            }
        }
    }
}
