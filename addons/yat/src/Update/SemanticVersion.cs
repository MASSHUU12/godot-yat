using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace YAT.Update;

public partial class SemanticVersion :
    IComparable<SemanticVersion>,
    IEquatable<SemanticVersion>,
    IComparable
{
    public int Major { get; init; }
    public int Minor { get; init; }
    public int Patch { get; init; }
    public string? Prerelease { get; init; }
    public string? BuildMetadata { get; init; }

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
        ValidateVersion(major, minor, patch, prerelease, buildMetadata);

        Major = major;
        Minor = minor;
        Patch = patch;
        Prerelease = prerelease;
        BuildMetadata = buildMetadata;
    }

    public SemanticVersion IncrementMajor(
        string? prerelease = null,
        string? buildMetadata = null
    )
    {
        return new(Major + 1, 0, 0, prerelease, buildMetadata);
    }

    public SemanticVersion IncrementMinor(
        string? prerelease = null,
        string? buildMetadata = null
    )
    {
        return new(Major, Minor + 1, 0, prerelease, buildMetadata);
    }

    public SemanticVersion IncrementPatch(
        string? prerelease = null,
        string? buildMetadata = null
    )
    {
        return new(Major, Minor, Patch + 1, prerelease, buildMetadata);
    }

    public SemanticVersion DecrementMajor(
        string? prerelease = null,
        string? buildMetadata = null
    )
    {
        return Major == 0
            ? throw new InvalidOperationException("Major version cannot be less than 0.")
            : new(Major - 1, 0, 0, prerelease, buildMetadata);
    }

    public SemanticVersion DecrementMinor(
        string? prerelease = null,
        string? buildMetadata = null
    )
    {
        return Minor == 0
            ? throw new InvalidOperationException("Minor version cannot be less than 0.")
            : new(Major, Minor - 1, 0, prerelease, buildMetadata);
    }

    public SemanticVersion DecrementPatch(
        string? prerelease = null,
        string? buildMetadata = null
    )
    {
        return Patch == 0
            ? throw new InvalidOperationException("Patch version cannot be less than 0.")
            : new(Major, Minor, Patch - 1, prerelease, buildMetadata);
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
        return !TryParse(versionString, out SemanticVersion? version)
            ? throw new FormatException(
                $"Semantic version string '{versionString}' is not in the correct format"
            )
            : version!;
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
            int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture),
            int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture),
            int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture),
            match.Groups[4].Success ? match.Groups[4].Value : null,
            match.Groups[5].Success ? match.Groups[5].Value : null
        );

        return true;
    }

    public override string ToString()
    {
        return ToString(Major, Minor, Patch, Prerelease, BuildMetadata);
    }

    private static string ToString(
        int major,
        int minor,
        int patch,
        string? prerelease,
        string? buildMetadata
    )
    {
        StringBuilder sb = new();
        _ = sb.Append(CultureInfo.InvariantCulture, $"{major}.{minor}.{patch}");
        if (prerelease is not null)
        {
            _ = sb.Append(CultureInfo.InvariantCulture, $"-{prerelease}");
        }
        if (buildMetadata is not null)
        {
            _ = sb.Append(CultureInfo.InvariantCulture, $"+{buildMetadata}");
        }
        return sb.ToString();
    }

    public static bool operator ==(SemanticVersion v1, SemanticVersion v2)
    {
        return ReferenceEquals(v1, v2)
            || (v1 is not null && v2 is not null && v1.Equals(v2));
    }

    public static bool operator !=(SemanticVersion v1, SemanticVersion v2)
    {
        return !(v1 == v2);
    }

    public static bool operator >(SemanticVersion v1, SemanticVersion v2)
    {
        return v1.CompareTo(v2) > 0;
    }

    public static bool operator <(SemanticVersion v1, SemanticVersion v2)
    {
        return v1.CompareTo(v2) < 0;
    }

    public static bool operator <=(SemanticVersion left, SemanticVersion right)
    {
        return left is null || left.CompareTo(right) <= 0;
    }

    public static bool operator >=(SemanticVersion left, SemanticVersion right)
    {
        return left is null ? right is null : left.CompareTo(right) >= 0;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj)
            || (obj is not null and SemanticVersion && Equals((SemanticVersion)obj));
    }

    public bool Equals(SemanticVersion? other)
    {
        return ReferenceEquals(this, other)
            || (
                other is not null
                && Major == other.Major
                && Minor == other.Minor
                && Patch == other.Patch
                && Prerelease == other.Prerelease
                && BuildMetadata == other.BuildMetadata
            );
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Major, Minor, Patch, Prerelease, BuildMetadata);
    }

    public int CompareTo(SemanticVersion? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (other is null)
        {
            return 1;
        }

        int result = Major.CompareTo(other.Major);
        if (result != 0)
        {
            return result;
        }

        result = Minor.CompareTo(other.Minor);
        if (result != 0)
        {
            return result;
        }

        result = Patch.CompareTo(other.Patch);
        if (result != 0)
        {
            return result;
        }

        result = ComparePrerelease(Prerelease, other.Prerelease);
        return result != 0
            ? result
            : CompareBuildMetadata(BuildMetadata, other.BuildMetadata);
    }

    public int CompareTo(object? obj)
    {
        if (obj is null)
        {
            return 1;
        }

        return obj is SemanticVersion x
            ? CompareTo(x)
            : throw new ArgumentException("", nameof(obj));
    }

    private static int ComparePrerelease(string? prerelease1, string? prerelease2)
    {
        if (prerelease1 == prerelease2)
        {
            return 0;
        }

        if (prerelease1 is null)
        {
            return 1;
        }

        if (prerelease2 is null)
        {
            return -1;
        }

        string[] v1Parts = prerelease1.Split('.');
        string[] v2Parts = prerelease2.Split('.');

        for (int i = 0; i < Math.Max(v1Parts.Length, v2Parts.Length); i++)
        {
            string v1Part = i < v1Parts.Length ? v1Parts[i] : string.Empty;
            string v2Part = i < v2Parts.Length ? v2Parts[i] : string.Empty;

            if (
                int.TryParse(v1Part, out int v1NumericPart)
                && int.TryParse(v2Part, out int v2NumericPart)
            )
            {
                int comparison = v1NumericPart.CompareTo(v2NumericPart);
                if (comparison != 0)
                {
                    return comparison;
                }
            }
            else
            {
                int comparison = string.CompareOrdinal(v1Part, v2Part);
                if (comparison != 0)
                {
                    return comparison;
                }
            }
        }

        return 0;
    }

    private static int CompareBuildMetadata(
        string? buildMetadata1,
        string? buildMetadata2
    )
    {
        if (buildMetadata1 == buildMetadata2)
        {
            return 0;
        }

        if (buildMetadata1 is null)
        {
            return -1;
        }

        return buildMetadata2 is null
            ? 1
            : string.CompareOrdinal(buildMetadata1, buildMetadata2);
    }

    private static void ValidateVersion(
        int major,
        int minor,
        int patch,
        string? prerelease,
        string? buildMetadata
    )
    {
        if (IsValid(ToString(major, minor, patch, prerelease, buildMetadata)))
        {
            return;
        }

        throw new FormatException(
            "Invalid semantic version format. " +
            "The format should be Major.Minor.Patch[-Prerelease][+BuildMetadata]."
        );
    }

    [GeneratedRegex(
        "^(0|[1-9]\\d*)\\.(0|[1-9]\\d*)\\.(0|[1-9]\\d*)(?:-((?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\\.(?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\\+([0-9a-zA-Z-]+(?:\\.[0-9a-zA-Z-]+)*))?$",
        RegexOptions.Compiled
    )]
    private static partial Regex SemanticVersionRegex();
}
