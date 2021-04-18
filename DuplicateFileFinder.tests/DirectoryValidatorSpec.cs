using System;
using Xunit;

namespace DuplicateFileFinder.tests
{
    public class DirectoryValidatorSpec
    {
        [Fact]
        public void ValidDirectoryShouldNotError()
        {
            var validatorResponse = DirectoryValidator.ValidatePath(AppDomain.CurrentDomain.BaseDirectory);
            Assert.False(validatorResponse.Error);
        }

        [Fact]
        public void InvalidDirectoryShouldError()
        {
            var validatorResponse = DirectoryValidator.ValidatePath("qwertyuiop");
            Assert.True(validatorResponse.Error);
        }
    }
}
