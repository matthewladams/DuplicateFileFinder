using System;

namespace DuplicateFileFinder
{
	public class InputHandler
	{
		private string _pathToValidate;

		// Setup cleanly and prompt user
		public void Initialise()
		{
			_pathToValidate = Environment.CurrentDirectory;

			Console.Write($"Enter a directory to scan, or leave blank to scan {_pathToValidate}: ");

			GetUserPath();
		}

		// Get and validate the user provided path
		public void GetUserPath()
		{
			var userInput = Console.ReadLine();

			if(!string.IsNullOrWhiteSpace(userInput))
            {
                _pathToValidate = userInput;
            }

            // Validate the input
            var validateResponse = DirectoryValidator.ValidatePath(_pathToValidate);
            if(validateResponse.Error)
            {
                Console.WriteLine(Utils.MessagesToString(validateResponse.Messages));
				Initialise();
            }
			else
			{
				// Path has been validated, find all duplicates and return the resulting messages to the user
				var finder = new DuplicateFileFinder.DuplicateFinder();
				var duplicateFinderResponse = finder.FindDuplicates(validateResponse.Path);
				Console.WriteLine(Utils.MessagesToString(duplicateFinderResponse.Messages));
			}

			// Reset for another use
			Initialise();
		}
	}
}
