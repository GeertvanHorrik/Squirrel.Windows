using Xunit;

namespace Squirrel.Tests
{
    public class ReleaseExtensionsTests
    {
        public class TheToVersionMethod
        {
            [Theory]
            [InlineData("MyProduct-1.7.0-unstable0036-full.nupkg", "1.7.0-unstable0036")]
            [InlineData("MyProduct-1.7.0-full.nupkg", "1.7.0")]
            public void ReturnsValidVersionForSemVer(string input, string expectedOutput)
            {
                var output = input.ToVersion();

                Assert.StrictEqual(expectedOutput, output);
            }
        }

        public class TheIsLargerThanMethod
        {
            [Theory]
            [InlineData("1.1.0", "1.1.0", false)]
            [InlineData("1.2.0", "1.1.0", true)]
            [InlineData("1.1.0", "1.2.0", false)]
            [InlineData("1.2.0-beta", "1.1.0", true)]
            [InlineData("1.1.0", "1.2.0-beta", false)]
            [InlineData("1.1.0", "1.1.0-beta", true)]
            [InlineData("1.1.0-beta", "1.1.0", false)]
            public void ReturnsRightBooleanForVersionComparison(string input, string versionToCheck, bool expectedOutput)
            {
                var output = input.IsLargerThan(versionToCheck);

                Assert.Equal(expectedOutput, output);
            }
        }

        public class TheIsSmallerThanMethod
        {
            [Theory]
            [InlineData("1.1.0", "1.1.0", false)]
            [InlineData("1.2.0", "1.1.0", false)]
            [InlineData("1.1.0", "1.2.0", true)]
            [InlineData("1.2.0-beta", "1.1.0", false)]
            [InlineData("1.1.0", "1.2.0-beta", true)]
            [InlineData("1.1.0", "1.1.0-beta", false)]
            [InlineData("1.1.0-beta", "1.1.0", true)]
            public void ReturnsRightBooleanForVersionComparison(string input, string versionToCheck, bool expectedOutput)
            {
                var output = input.IsSmallerThan(versionToCheck);

                Assert.Equal(expectedOutput, output);
            }
        }

        public class TheGetMaxVersionMethod
        {
            [Fact]
            public void ReturnsMaximumVersion()
            {
                var input = new[]
                {
                    "1.0.0",
                    "1.0.0-beta",
                    "2.0.0",
                    "2.0.0-unstable0025"
                };

                var output = input.GetMaxVersion();

                Assert.Equal("2.0.0", output);
            }
        }

        public class TheGetMinVersionMethod
        {
            [Fact]
            public void ReturnsMinimumVersion()
            {
                var input = new[]
                {
                    "1.0.0",
                    "1.0.0-beta",
                    "2.0.0",
                    "2.0.0-unstable0025"
                };

                var output = input.GetMinVersion();

                Assert.Equal("1.0.0-beta", output);
            }
        }
    }
}
