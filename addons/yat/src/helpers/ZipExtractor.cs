using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace YAT.Helpers;

public static class ZipExtractor
{
    public static bool ExtractFolderFromZipFile(
        string filePath,
        string extractPath,
        string folderToExtract
    )
    {
        try
        {
            using ZipArchive archive = ZipFile.OpenRead(filePath);

            IEnumerable<ZipArchiveEntry> result = from currEntry in archive.Entries
                                                  where currEntry.FullName.Contains(folderToExtract)
                                                  where !string.IsNullOrEmpty(currEntry.Name)
                                                  select currEntry;

            foreach (ZipArchiveEntry entry in result)
            {
                string[] pathElements = entry.FullName.Split(Path.DirectorySeparatorChar);
                int folderIndex = Array.IndexOf(pathElements, folderToExtract);

                // folderIndex + 1 to skip folder with the addon itself
                if (folderIndex == -1 || folderIndex + 1 > pathElements.Length)
                {
                    continue;
                }

                string relativePath = string.Join(
                    Path.DirectorySeparatorChar,
                    pathElements[(folderIndex + 1)..]
                );
                string path = Path.Combine(extractPath, relativePath);

                if (path.EndsWith($"{Path.DirectorySeparatorChar}", StringComparison.InvariantCulture))
                {
                    _ = Directory.CreateDirectory(path);
                }
                else
                {
                    _ = Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                    using FileStream fileStream = new(path, FileMode.Create);
                    using Stream entryStream = entry.Open();

                    entryStream.CopyTo(fileStream);
                }
            }
            return true;
        }
        catch (Exception e) when (
            e is ArgumentException
            or IOException
            or FileNotFoundException
            or InvalidDataException
            or UnauthorizedAccessException
            or DirectoryNotFoundException
        )
        {
            return false;
        }
    }
}
