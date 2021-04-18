# Duplicate File Finder

A simple .NET Console application to find duplicate files in a given directory

## Build/Run/Test Instructions
Requires .NET 5.0 or greater.

From the solution directory: 

Build: ```dotnet build```

Run: ```dotnet run --project DuplicateFileFinder/DuplicateFileFinder.csproj```

Tests: ```dotnet test```

## What's missing/TODOs/Future Features
* Handling of directory permissions - app assumes it has access
* Exception handling in validation - app currently assumes structure is valid as it exists on disk
* Thorough unit testing
* More user friendly presentation of results
* Option to match on just file names, not hashes