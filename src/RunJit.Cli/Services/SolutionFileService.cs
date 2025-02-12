using System.Text.RegularExpressions;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.Services
{
    internal static class AddSolutionFileServiceExtension
    {
        internal static void AddSolutionFileService(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<SolutionFileService>();
        }
    }

    // Ugly workaround because .net cli does not support adding any file into solution
    // in combination with solution folders :/ 
    // Microsoft open issue:
    // https://github.com/dotnet/sdk/issues/9611
    // More infos
    // https://stackoverflow.com/questions/49734208/net-core-how-to-create-solution-folders-from-the-command-line
    // https://dev.to/ssukhpinder/managing-net-solution-files-with-dotnet-sln-3dc9
    internal class SolutionFileService
    {
        // GUID that represents a "Solution Folder" project type
        // Important use the correct GUID = 2150E333-8FDC-42A3-9474-1A3956D46DE8
        private const string SOLUTION_FOLDER_GUID = "{2150E333-8FDC-42A3-9474-1A3956D46DE8}";

        /// <summary>
        ///     Adds or updates a solution folder by name. If the folder exists, it adds the specified files.
        ///     If it does not exist, it creates the folder and inserts the files.
        /// </summary>
        /// <param name="solutionFileAsLines">Solution file read as lines to avoid multiple read and write operation to the disk.</param>
        /// <param name="solutionFile">The solution file info</param>
        /// <param name="folderName">Name of the folder to add or update (as displayed in Solution Explorer)</param>
        /// <param name="files">Files to include in the folder (relative to .sln)</param>
        internal List<string> AddOrUpdateSolutionFolder(List<string> solutionFileAsLines,
                                                        FileInfo solutionFile,
                                                        string folderName,
                                                        params FileInfo[] files)
        {
            // Convert files to a list (avoid multiple enumerations)
            var filesList = files?.ToList() ?? new List<FileInfo>();

            // 1) Try to find an existing solution folder block by folder name
            var folderStartIndex = FindSolutionFolderStartIndex(solutionFileAsLines, folderName);

            if (folderStartIndex == -1)
            {
                // Folder does not exist -> Insert a new block
                InsertNewSolutionFolderBlock(solutionFileAsLines, folderName, solutionFile, filesList);
            }
            else
            {
                // Folder exists -> Add files into the existing ProjectSection(SolutionItems)
                InsertFilesIntoExistingFolder(solutionFileAsLines, folderStartIndex, solutionFile, filesList);
            }

            // 2) Write updated solutionFileAsLies to the solution file
            return solutionFileAsLines;
        }

        /// <summary>
        ///     Searches for a solution folder project block by its folder name.
        ///     Returns the line index where the block starts (the 'Project("{66A26720...}")' line),
        ///     or -1 if not found.
        /// </summary>
        private static int FindSolutionFolderStartIndex(List<string> solutionFileAsLies,
                                                        string folderName)
        {
            // We'll look for a line of the form:
            // Project("{66A26720-8FB5-11D2-AA7E-00C04F688DDE}") = "FolderName", "FolderName", "{GUID}"
            // We do a simplistic string match, ignoring potential extra spaces.
            // A more robust approach could use a Regex.
            var pattern = $@"Project\(""{Regex.Escape(SOLUTION_FOLDER_GUID)}""\)\s*=\s*""{Regex.Escape(folderName)}"",\s*""{Regex.Escape(folderName)}"",\s*""{{[A-Z0-9-]+}}""";

            for (var i = 0; i < solutionFileAsLies.Count; i++)
            {
                if (Regex.IsMatch(solutionFileAsLies[i], pattern, RegexOptions.IgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        ///     Inserts a brand new solution folder block (Project(...) ) before the Global section.
        /// </summary>
        private static void InsertNewSolutionFolderBlock(List<string> solutionFileAsLies,
                                                         string folderName,
                                                         FileInfo solutionFileInfo,
                                                         List<FileInfo> files)
        {
            // Find where "Global" section starts, typically near the end
            var globalIndex = solutionFileAsLies.FindIndex(l => l.TrimStart().StartsWith("Global", StringComparison.OrdinalIgnoreCase));

            if (globalIndex < 0)
            {
                throw new InvalidOperationException("Could not find 'Global' section. The .sln may be malformed.");
            }

            // Generate a new unique GUID for the folder
            var folderGuid = Guid.NewGuid().ToString("B").ToUpperInvariant(); // e.g. "{E3F88BC9-5568-4E99-B980-1AF3FBE373C8}"

            // Build solutionFileAsLies for the folder block
            var block = new List<string>();
            block.Add($"Project(\"{SOLUTION_FOLDER_GUID}\") = \"{folderName}\", \"{folderName}\", \"{folderGuid}\"");
            block.Add("    ProjectSection(SolutionItems) = preProject");

            foreach (var file in files)
            {
                var relativePath = Path.GetRelativePath(solutionFileInfo.Directory!.FullName, file.FullName);
                block.Add($"        {relativePath} = {relativePath}");
            }

            block.Add("    EndProjectSection");
            block.Add("EndProject");

            // Insert before "Global"
            solutionFileAsLies.InsertRange(globalIndex, block);
        }

        /// <summary>
        ///     Finds the 'ProjectSection(SolutionItems)' in the existing folder block
        ///     and inserts file solutionFileAsLies. If the folder block has no 'ProjectSection', we create one.
        /// </summary>
        private static void InsertFilesIntoExistingFolder(List<string> solutionFileAsLies,
                                                          int folderStartIndex,
                                                          FileInfo solutionFileInfo,
                                                          List<FileInfo> files)
        {
            // We have something like:
            // Project("{66A26720-8FB5-11D2-AA7E-00C04F688DDE}") = "MyFolder", "MyFolder", "{FOLDER_GUID}"
            //     ProjectSection(SolutionItems) = preProject
            //         file1 = file1
            //     EndProjectSection
            // EndProject

            // We'll search from folderStartIndex down to "EndProject"
            var projectEndIndex = -1;

            for (var i = folderStartIndex + 1; i < solutionFileAsLies.Count; i++)
            {
                if (solutionFileAsLies[i].TrimStart().StartsWith("EndProject", StringComparison.OrdinalIgnoreCase))
                {
                    projectEndIndex = i;

                    break;
                }
            }

            if (projectEndIndex == -1)
            {
                throw new InvalidOperationException("Could not find 'EndProject' for existing solution folder block.");
            }

            // Find or create the "ProjectSection(SolutionItems) = preProject" block
            var sectionStartIndex = -1;
            var sectionEndIndex = -1;

            for (var i = folderStartIndex + 1; i < projectEndIndex; i++)
            {
                var trimmed = solutionFileAsLies[i].TrimStart();

                if (trimmed.StartsWith("ProjectSection(SolutionItems)", StringComparison.OrdinalIgnoreCase))
                {
                    sectionStartIndex = i;

                    // Now find the matching 'EndProjectSection'
                    for (var j = i + 1; j <= projectEndIndex; j++)
                    {
                        if (solutionFileAsLies[j].TrimStart().StartsWith("EndProjectSection", StringComparison.OrdinalIgnoreCase))
                        {
                            sectionEndIndex = j;

                            break;
                        }
                    }

                    break;
                }
            }

            if (sectionStartIndex == -1)
            {
                // The folder block has no ProjectSection. We'll insert one right before EndProject
                // i.e., between projectEndIndex-1 and projectEndIndex
                sectionStartIndex = projectEndIndex; // insertion point
                var newSection = new List<string>();
                newSection.Add("    ProjectSection(SolutionItems) = preProject");

                // Add each file
                foreach (var f in files)
                {
                    var relativePath = Path.GetRelativePath(solutionFileInfo.Directory!.FullName, f.FullName); ;
                    newSection.Add($"        {relativePath} = {relativePath}");
                }

                newSection.Add("    EndProjectSection");

                solutionFileAsLies.InsertRange(sectionStartIndex, newSection);
            }
            else
            {
                // We already have ProjectSection. Insert new files before the EndProjectSection
                // Make sure we don't duplicate solutionFileAsLies.
                var existingFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                // Collect existing items from that section
                for (var i = sectionStartIndex + 1; i < sectionEndIndex; i++)
                {
                    // Typically solutionFileAsLies are "   relative\path = relative\path"
                    var line = solutionFileAsLies[i].Trim();

                    if (!string.IsNullOrWhiteSpace(line) &&
                        !line.StartsWith("EndProjectSection", StringComparison.OrdinalIgnoreCase))
                    {
                        var parts = line.Split('=');

                        if (parts.Length == 2)
                        {
                            var left = parts[0].Trim();

                            // typically left == right, so we can store either
                            existingFiles.Add(left);
                        }
                    }
                }

                // Insert any new files that are not in existingFiles
                var newLines = new List<string>();

                foreach (var f in files)
                {
                    var relativePath = Path.GetRelativePath(solutionFileInfo.Directory!.FullName, f.FullName);
                    if (!existingFiles.Contains(relativePath))
                    {
                        newLines.Add($"        {relativePath} = {relativePath}");
                    }
                }

                // Insert right before sectionEndIndex
                if (newLines.Count > 0)
                {
                    solutionFileAsLies.InsertRange(sectionEndIndex, newLines);
                }
            }
        }
    }
}
