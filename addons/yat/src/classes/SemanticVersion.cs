using System;
using System.Text.RegularExpressions;

namespace YAT.Classes;

public partial class SemanticVersion
{
    public int Major { get; private set; }
    public int Minor { get; private set; }
    public int Patch { get; private set; }
    public string? Prerelease { get; private set; }
    public string? BuildMetadata { get; private set; }

    private static readonly Regex _semanticVersionRegex = SemanticVersionRegex();

    public SemanticVersion() : this(0, 0, 0) { }

    public SemanticVersion(
        int major,
        int minor,
        int patch,
        string? prerelease = null,
        string? buildMetadata = null
    )
    {
        string versionString = $"{major}.{minor}.{patch}"
            + (prerelease is null ? string.Empty : $"-{prerelease}")
            + (buildMetadata is null ? string.Empty : $"+{buildMetadata}");

        if (!IsValid(versionString, out Match match))
        {
            string errorMessage;
            if (!match.Groups[1].Success)
            {
                errorMessage = "Major version is not a valid integer.";
            }
            else if (!match.Groups[2].Success)
            {
                errorMessage = "Minor version is not a valid integer.";
            }
            else if (!match.Groups[3].Success)
            {
                errorMessage = "Patch version is not a valid integer.";
            }
            else if (match.Groups[4].Success)
            {
                errorMessage = "Prerelease version is not in the correct format. It should be a dot-separated list of identifiers.";
            }
            else if (match.Groups[5].Success)
            {
                errorMessage = "Build metadata is not in the correct format. It should be a dot-separated list of identifiers.";
            }
            else
            {
                errorMessage = "Invalid semantic version format. The format should be Major.Minor.Patch[-Prerelease][+BuildMetadata].";
            }

            throw new FormatException(errorMessage);
        }

        Major = major;
        Minor = minor;
        Patch = patch;
        Prerelease = prerelease;
        BuildMetadata = buildMetadata;
    }

    public static bool IsValid(string versionString, out Match match)
    {
        match = _semanticVersionRegex.Match(versionString);

        return match.Success;
    }

    public static bool IsValid(string versionString)
    {
        return IsValid(versionString, out _);
    }

    public static SemanticVersion Parse(string versionString)
    {
        if (!TryParse(versionString, out SemanticVersion? version))
        {
            throw new FormatException(
                $"Semantic version string '{versionString}' is not in the correct format"
            );
        }

        return version!;
    }

    public static bool TryParse(string versionString, out SemanticVersion? version)
    {
        version = null;

        Match match = _semanticVersionRegex.Match(versionString);
        if (!match.Success)
        {
            return false;
        }

        version = new(
            int.Parse(match.Groups[1].Value),
            int.Parse(match.Groups[2].Value),
            int.Parse(match.Groups[3].Value),
            match.Groups[4].Success ? match.Groups[4].Value : null,
            match.Groups[5].Success ? match.Groups[5].Value : null
        );

        return true;
    }

    public override string ToString()
    {
        return string.Format(
            "{0}.{1}.{2}-{3}+{4}",
            Major,
            Minor,
            Patch,
            Prerelease,
            BuildMetadata
        );
    }

    public static bool operator ==(SemanticVersion v1, SemanticVersion v2)
    {
        return v1.Major == v2.Major
            && v1.Minor == v2.Minor
            && v1.Patch == v2.Patch
            && v1.Prerelease == v2.Prerelease
            && v1.BuildMetadata == v2.BuildMetadata;
    }

    public static bool operator !=(SemanticVersion v1, SemanticVersion v2)
    {
        return !(v1 == v2);
    }

    public static bool operator >(SemanticVersion v1, SemanticVersion v2)
    {
        if (v1.Major > v2.Major) return true;
        if (v1.Major < v2.Major) return false;

        if (v1.Minor > v2.Minor) return true;
        if (v1.Minor < v2.Minor) return false;

        if (v1.Patch > v2.Patch) return true;
        if (v1.Patch < v2.Patch) return false;

        if (v1.Prerelease is not null && v2.Prerelease is null) return true;
        if (v1.Prerelease is null && v2.Prerelease is not null) return false;

        if (v1.Prerelease is not null && v2.Prerelease is not null)
        {
            var v1Parts = v1.Prerelease.Split('.');
            var v2Parts = v2.Prerelease.Split('.');

            for (int i = 0; i < Math.Max(v1Parts.Length, v2Parts.Length); i++)
            {
                string v1Part = i < v1Parts.Length ? v1Parts[i] : string.Empty;
                string v2Part = i < v2Parts.Length ? v2Parts[i] : string.Empty;

                if (int.TryParse(v1Part, out int v1NumericPart) && int.TryParse(v2Part, out int v2NumericPart))
                {
                    if (v1NumericPart > v2NumericPart) return true;
                    if (v1NumericPart < v2NumericPart) return false;
                }
                else
                {
                    int compareResult = string.Compare(v1Part, v2Part, StringComparison.Ordinal);

                    if (compareResult > 0) return true;
                    if (compareResult < 0) return false;
                }
            }
        }

        if (v1.BuildMetadata is not null && v2.BuildMetadata is null) return true;
        if (v1.BuildMetadata is null && v2.BuildMetadata is not null) return false;

        if (v1.BuildMetadata is not null && v2.BuildMetadata is not null)
        {
            return string.Compare(v1.BuildMetadata, v2.BuildMetadata, StringComparison.Ordinal) > 0;
        }

        return false;
    }

    public static bool operator <(SemanticVersion v1, SemanticVersion v2)
    {
        return v2 > v1;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is null or not SemanticVersion)
        {
            return false;
        }

        return this == (SemanticVersion)obj;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash *= 23 + Major.GetHashCode();
            hash *= 23 + Minor.GetHashCode();
            hash *= 23 + Patch.GetHashCode();
            hash *= 23 + (Prerelease?.GetHashCode() ?? 0);
            hash *= 23 + (BuildMetadata?.GetHashCode() ?? 0);
            return hash;
        }
    }

    [GeneratedRegex("^(0|[1-9]\\d*)\\.(0|[1-9]\\d*)\\.(0|[1-9]\\d*)(?:-((?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\\.(?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\\+([0-9a-zA-Z-]+(?:\\.[0-9a-zA-Z-]+)*))?$", RegexOptions.Compiled)]
    private static partial Regex SemanticVersionRegex();
}
