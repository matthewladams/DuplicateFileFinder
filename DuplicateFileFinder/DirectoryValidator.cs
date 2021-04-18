using System.Collections.Generic;
using System.IO;

namespace DuplicateFileFinder
{
	public class DirectoryValidatorMessage
	{
		public string Path;
		public List<string> Messages = new List<string>();
		public bool Error;

	}
    public static class DirectoryValidator
    {
        public static DirectoryValidatorMessage ValidatePath(string pathToValidate)
        {
			var response = new DirectoryValidatorMessage{ Path = pathToValidate };

			// Check the supplied directory is valid
			if(Directory.Exists(pathToValidate))
			{
				// Directory is fine
				return response;
			}
			else if(File.Exists(pathToValidate))
			{
				response.Messages.Add($"Path {pathToValidate} is a file. Please specify a directory.");
				response.Error = true;
				return response;
			}
			else
			{
				response.Messages.Add($"Path {pathToValidate} is not valid, please specify a valid directory.");
				response.Error = true;
				return response;
			}

		}
	}
}
