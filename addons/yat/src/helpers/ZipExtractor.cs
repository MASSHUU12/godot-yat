using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;

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
            using (ZipArchive archive = ZipFile.OpenRead(filePath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (!Regex.IsMatch(entry.FullName, folderToExtract))
                    {
                        continue;
                    }

                    string relativePath = entry.FullName.Substring(folderToExtract.Length).TrimStart('/');

                    if (string.IsNullOrEmpty(relativePath))
                    {
                        // Skip the folder itself
                        continue;
                    }

                    string path = string.Join("/", relativePath.Split("/").Skip(1));

                    if (entry.FullName.EndsWith("/"))
                    {
                        Directory.CreateDirectory(path);
                    }
                    else
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                        using (FileStream fileStream = new(path, FileMode.Create))
                        {
                            using (Stream entryStream = entry.Open())
                            {
                                entryStream.CopyTo(fileStream);
                            }
                        }
                    }
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
