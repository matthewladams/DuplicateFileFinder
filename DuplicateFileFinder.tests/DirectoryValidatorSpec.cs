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
    }
}
