using System.Collections.Generic;

namespace DuplicateFileFinder
{
	public static class Utils
	{
		public static string MessagesToString(List<string> messages)
		{
			var response = string.Empty;

			foreach(var message in messages)
			{
				response += message + "\n";
			}

			return response;
		}
	}
}
