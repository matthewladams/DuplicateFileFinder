using System;
using System.IO;
using Xunit;

namespace DuplicateFileFinder.tests
{
    public class DuplicateFinderSpec
    {
		private void CreateTextFile(string path, string text)
		{
			using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(text);
				sw.Close();
            }	
		}

		// There should be a proper way to implement this in Xunit, but this will do for now
		private void RunBeforeAfterTest()
		{
			File.Delete(AppDomain.CurrentDomain.BaseDirectory + "Duplicate1.txt");
			File.Delete(AppDomain.CurrentDomain.BaseDirectory + "Duplicate2.txt");
			File.Delete(AppDomain.CurrentDomain.BaseDirectory + "Unique.txt");
			File.Delete(AppDomain.CurrentDomain.BaseDirectory + "/SF1/SF2/Duplicate1.txt");
			File.Delete(AppDomain.CurrentDomain.BaseDirectory + "Unique1.txt");
			File.Delete(AppDomain.CurrentDomain.BaseDirectory + "/SF1/SF2/Unique1.txt");
			File.Delete(AppDomain.CurrentDomain.BaseDirectory + "Unique2.txt");
			File.Delete(AppDomain.CurrentDomain.BaseDirectory + "Unique3.txt");
		}

        [Fact]
        public void ShouldFindDuplicatesTopLevelDirectory()
        {
			RunBeforeAfterTest();

			// Create 3 files, two of which are duplicates
			CreateTextFile(AppDomain.CurrentDomain.BaseDirectory + "Duplicate1.txt", "Duplicate");
			CreateTextFile(AppDomain.CurrentDomain.BaseDirectory + "Duplicate2.txt", "Duplicate");
			CreateTextFile(AppDomain.CurrentDomain.BaseDirectory + "Unique1.txt", "Unique");

			var finder = new DuplicateFileFinder.DuplicateFinder();
			var duplicateFinderResponse = finder.FindDuplicates(AppDomain.CurrentDomain.BaseDirectory);
            Assert.True(duplicateFinderResponse.Messages.Count == 1, "Should have a single message for the 2 duplicate files");

			RunBeforeAfterTest();
        }

        [Fact]
        public void ShouldFindDuplicatesIn2LevelSubFolder()
        {
			RunBeforeAfterTest();

			// Create 3 files, two of which are duplicates, but in differing parts of the tree
			Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/SF1/SF2/");
			CreateTextFile(AppDomain.CurrentDomain.BaseDirectory + "/SF1/SF2/Duplicate1.txt", "Duplicate");
			CreateTextFile(AppDomain.CurrentDomain.BaseDirectory + "Duplicate2.txt", "Duplicate");
			CreateTextFile(AppDomain.CurrentDomain.BaseDirectory + "Unique1.txt", "Unique");

			var finder = new DuplicateFileFinder.DuplicateFinder();
			var duplicateFinderResponse = finder.FindDuplicates(AppDomain.CurrentDomain.BaseDirectory);
            Assert.True(duplicateFinderResponse.Messages.Count == 1, "Should have a single message for the 2 duplicate files");
            Assert.True(duplicateFinderResponse.Messages[0].Contains("/SF1/SF2/Duplicate1.txt"), "Should have a single message containing the path of the duplicate");

			// Cleanup
			File.Delete(AppDomain.CurrentDomain.BaseDirectory + "/SF1/SF2/Duplicate1.txt");
			File.Delete(AppDomain.CurrentDomain.BaseDirectory + "Duplicate2.txt");
			File.Delete(AppDomain.CurrentDomain.BaseDirectory + "Unique1.txt");

			RunBeforeAfterTest();
        }

        [Fact]
        public void ShouldFindNoDuplicatesIn2LevelSubFolder()
        {
			RunBeforeAfterTest();

			// Create 3 files, two of which are duplicates, but in differing parts of the tree
			Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/SF1/SF2/");
			CreateTextFile(AppDomain.CurrentDomain.BaseDirectory + "/SF1/SF2/Unique1.txt", "Unique1");
			CreateTextFile(AppDomain.CurrentDomain.BaseDirectory + "Unique2.txt", "Unique2");
			CreateTextFile(AppDomain.CurrentDomain.BaseDirectory + "Unique3.txt", "Unique3");

			var finder = new DuplicateFileFinder.DuplicateFinder();
			var duplicateFinderResponse = finder.FindDuplicates(AppDomain.CurrentDomain.BaseDirectory);
            Assert.True(duplicateFinderResponse.Messages.Count == 1, $"Should have single message for no duplicates found.");
            Assert.True(duplicateFinderResponse.Messages[0].Contains("No duplicates found"), "Should have single message for no duplicates found");

			RunBeforeAfterTest();
        }
    }
}
