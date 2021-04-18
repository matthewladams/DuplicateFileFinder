using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DuplicateFileFinder
{
	public class DuplicateFinderMessage
	{
		public string Path;
		public List<string> Messages = new List<string>();
		public bool Error;

	}
	public class DuplicateFinder
	{
		private Dictionary<string, List<string>> _hashPaths = new Dictionary<string, List<string>>();

		public DuplicateFinderMessage FindDuplicates(string path)
		{
			var itemToProcess = new DuplicateFinderMessage { Path = path };

			// Find all files in the given path recursively, storing them in a list against the hash
			RecursivePathDuplicateFinder(itemToProcess.Path);

			// Add messages to the response for each file
			PrepareDuplicateMessages(itemToProcess);

			return itemToProcess;
		}

		private void RecursivePathDuplicateFinder(string path)
		{
			// Check our current directories files
			CheckAllFilesForPath(path);

			// Recurse into subdirectories of this directory.
			var subdirectoryEntries = Directory.GetDirectories(path);
			foreach(var subdirectory in subdirectoryEntries)
				RecursivePathDuplicateFinder(subdirectory);
		}

		private void CheckAllFilesForPath(string path)
		{
			var fileEntries = Directory.GetFiles(path);
			foreach (var fileName in fileEntries)
				ProcessFile(fileName);
		}

		private void ProcessFile(string filePath)
		{
			// MD5 is fine for our requirements of checking duplicates on disk
			using (var md5 = MD5.Create())
			{
				using (var stream = File.OpenRead(filePath))
				{
					// Convert the MD5 Hash byte array to a readable string
					var fileMd5 = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-","");
					
					// Keep track of each unique file we find and where we found it
					if(_hashPaths.ContainsKey(fileMd5))
					{
						_hashPaths[fileMd5].Add(filePath);
					}
					else
					{
						_hashPaths.Add(fileMd5, new List<string>{filePath});
					}
				}
			}
		}

		private void PrepareDuplicateMessages(DuplicateFinderMessage itemToProcess)
		{
			if(!_hashPaths.Any(hashPath => hashPath.Value.Count > 1))
			{
				itemToProcess.Messages.Add("No duplicates found.");
			}

			foreach(var hashPath in _hashPaths.Where(hashPath => hashPath.Value.Count > 1))
			{
				// Add a message for every hash that was found in more than 1 place
				var message = $"{hashPath.Value.Count} occurences of file with hash {hashPath.Key} found:\n";
				foreach(var filePath in hashPath.Value)
				{
					message += $"	{filePath}\n";
				}
				itemToProcess.Messages.Add(message);
			}
		}
	}
}
