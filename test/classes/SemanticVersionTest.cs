using System;
using Confirma.Attributes;
using Confirma.Classes;
using Confirma.Extensions;
using YAT.Classes;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public static class SemanticVersionTest
{
    #region Constructor
    [TestCase]
    public static void Constructor()
    {
        SemanticVersion version = new(1, 2, 3, "beta", "meta");

        _ = version.Major.ConfirmEqual(1);
        _ = version.Minor.ConfirmEqual(2);
        _ = version.Patch.ConfirmEqual(3);
        _ = version.Prerelease.ConfirmEqual("beta");
        _ = version.BuildMetadata.ConfirmEqual("meta");
    }

    [TestCase]
    public static void Constructor_Empty()
    {
        SemanticVersion version = new();

        _ = version.Major.ConfirmEqual(0);
        _ = version.Minor.ConfirmEqual(0);
        _ = version.Patch.ConfirmEqual(0);
        _ = version.Prerelease.ConfirmNull();
        _ = version.BuildMetadata.ConfirmNull();
    }

    [TestCase]
    public static void Constructor_Invalid_MajorVersion()
    {
        Confirm.Throws<FormatException>(
            () => _ = new SemanticVersion(-5, 0, 0)
        );
    }

    [TestCase]
    public static void Constructor_Invalid_MinorVersion()
    {
        Confirm.Throws<FormatException>(
            () => _ = new SemanticVersion(5, -5, 0)
        );
    }

    [TestCase]
    public static void Constructor_Invalid_PatchVersion()
    {
        Confirm.Throws<FormatException>(
            () => _ = new SemanticVersion(5, 5, -5)
        );
    }
    #endregion

    #region IsValid
    [TestCase("0.0.4")]
    [TestCase("1.2.3")]
    [TestCase("10.20.30")]
    [TestCase("1.1.2-prerelease+meta")]
    [TestCase("1.1.2+meta")]
    [TestCase("1.1.2+meta-valid")]
    [TestCase("1.0.0-alpha")]
    [TestCase("1.0.0-beta")]
    [TestCase("1.0.0-alpha.beta")]
    [TestCase("1.0.0-alpha.beta.1")]
    [TestCase("1.0.0-alpha.1")]
    [TestCase("1.0.0-alpha0.valid")]
    [TestCase("1.0.0-alpha.0valid")]
    [TestCase("1.0.0-alpha-a.b-c-somethinglong+build.1-aef.1-its-okay")]
    [TestCase("1.0.0-rc.1+build.1")]
    [TestCase("2.0.0-rc.1+build.123")]
    [TestCase("1.2.3-beta")]
    [TestCase("10.2.3-DEV-SNAPSHOT")]
    [TestCase("1.2.3-SNAPSHOT-123")]
    [TestCase("1.0.0")]
    [TestCase("2.0.0")]
    [TestCase("1.1.7")]
    [TestCase("2.0.0+build.1848")]
    [TestCase("2.0.1-alpha.1227")]
    [TestCase("1.0.0-alpha+beta")]
    [TestCase("1.2.3----RC-SNAPSHOT.12.9.1--.12+788")]
    [TestCase("1.2.3----R-S.12.9.1--.12+meta")]
    [TestCase("1.2.3----RC-SNAPSHOT.12.9.1--.12")]
    [TestCase("1.0.0+0.build.1-rc.10000aaa-kk-0.1")]
    [TestCase("99999999999999999999999.999999999999999999.99999999999999999")]
    [TestCase("1.0.0-0A.is.legal")]
    public static void IsValid_WhenVersionIsValid(string versionString)
    {
        _ = SemanticVersion.IsValid(versionString).ConfirmTrue();
    }

    [TestCase("")]
    [TestCase("1")]
    [TestCase("1.2")]
    [TestCase("1.2.3-0123")]
    [TestCase("1.2.3-0123.0123")]
    [TestCase("1.1.2+.123")]
    [TestCase("+invalid")]
    [TestCase("-invalid")]
    [TestCase("-invalid+invalid")]
    [TestCase("-invalid.01")]
    [TestCase("alpha")]
    [TestCase("alpha.beta")]
    public static void IsValid_WhenVersionIsInvalid(string versionString)
    {
        _ = SemanticVersion.IsValid(versionString).ConfirmFalse();
    }
    #endregion

    #region Parse
    [TestCase("0.0.4", 0, 0, 4, null, null)]
    [TestCase("1.2.3", 1, 2, 3, null, null)]
    [TestCase("10.20.30", 10, 20, 30, null, null)]
    [TestCase("1.1.2-prerelease+meta", 1, 1, 2, "prerelease", "meta")]
    [TestCase("1.1.2+meta", 1, 1, 2, null, "meta")]
    [TestCase("1.1.2+meta-valid", 1, 1, 2, null, "meta-valid")]
    [TestCase("1.0.0-alpha", 1, 0, 0, "alpha", null)]
    [TestCase("1.0.0-beta", 1, 0, 0, "beta", null)]
    [TestCase("1.0.0-alpha.beta", 1, 0, 0, "alpha.beta", null)]
    [TestCase("1.0.0-alpha.beta.1", 1, 0, 0, "alpha.beta.1", null)]
    [TestCase("1.0.0-alpha.1", 1, 0, 0, "alpha.1", null)]
    [TestCase("1.0.0-alpha0.valid", 1, 0, 0, "alpha0.valid", null)]
    [TestCase("1.0.0-alpha.0valid", 1, 0, 0, "alpha.0valid", null)]
    [TestCase(
        "1.0.0-alpha-a.b-c-somethinglong+build.1-aef.1-its-okay",
        1, 0, 0, "alpha-a.b-c-somethinglong", "build.1-aef.1-its-okay"
    )]
    [TestCase("1.0.0-rc.1+build.1", 1, 0, 0, "rc.1", "build.1")]
    [TestCase("2.0.0-rc.1+build.123", 2, 0, 0, "rc.1", "build.123")]
    [TestCase("1.2.3-beta", 1, 2, 3, "beta", null)]
    [TestCase("10.2.3-DEV-SNAPSHOT", 10, 2, 3, "DEV-SNAPSHOT", null)]
    [TestCase("1.2.3-SNAPSHOT-123", 1, 2, 3, "SNAPSHOT-123", null)]
    [TestCase("1.0.0", 1, 0, 0, null, null)]
    [TestCase("2.0.0", 2, 0, 0, null, null)]
    [TestCase("1.1.7", 1, 1, 7, null, null)]
    [TestCase("2.0.0+build.1848", 2, 0, 0, null, "build.1848")]
    [TestCase("2.0.1-alpha.1227", 2, 0, 1, "alpha.1227", null)]
    [TestCase("1.0.0-alpha+beta", 1, 0, 0, "alpha", "beta")]
    [TestCase(
        "1.2.3----RC-SNAPSHOT.12.9.1--.12+788",
        1, 2, 3, "---RC-SNAPSHOT.12.9.1--.12", "788"
    )]
    [TestCase(
        "1.2.3----R-S.12.9.1--.12+meta",
        1, 2, 3, "---R-S.12.9.1--.12", "meta"
    )]
    [TestCase(
        "1.2.3----RC-SNAPSHOT.12.9.1--.12",
        1, 2, 3, "---RC-SNAPSHOT.12.9.1--.12", null
    )]
    [TestCase(
        "1.0.0+0.build.1-rc.10000aaa-kk-0.1",
        1, 0, 0, null, "0.build.1-rc.10000aaa-kk-0.1"
    )]
    [TestCase("1.0.0-0A.is.legal", 1, 0, 0, "0A.is.legal", null)]
    public static void Parse_WhenVersionIsValid(
        string v,
        int major,
        int minor,
        int patch,
        string? prerelease,
        string? buildMetadata
    )
    {
        SemanticVersion ver = SemanticVersion.Parse(v);

        _ = ver.Major.ConfirmEqual(major);
        _ = ver.Minor.ConfirmEqual(minor);
        _ = ver.Patch.ConfirmEqual(patch);
        _ = ver.Prerelease.ConfirmEqual(prerelease);
        _ = ver.BuildMetadata.ConfirmEqual(buildMetadata);
    }

    [TestCase("")]
    [TestCase("1")]
    [TestCase("1.2")]
    [TestCase("1.2.3-0123")]
    [TestCase("1.2.3-0123.0123")]
    [TestCase("1.1.2+.123")]
    [TestCase("+invalid")]
    [TestCase("-invalid")]
    [TestCase("-invalid+invalid")]
    [TestCase("-invalid.01")]
    [TestCase("alpha")]
    [TestCase("alpha.beta")]
    public static void Parse_WhenVersionIsInvalid(string versionString)
    {
        _ = Confirm.Throws<FormatException>(() => SemanticVersion.Parse(versionString));
    }
    #endregion

    #region TryParse
    [TestCase("0.0.4", 0, 0, 4, null, null)]
    [TestCase("1.2.3", 1, 2, 3, null, null)]
    [TestCase("10.20.30", 10, 20, 30, null, null)]
    [TestCase("1.1.2-prerelease+meta", 1, 1, 2, "prerelease", "meta")]
    [TestCase("1.1.2+meta", 1, 1, 2, null, "meta")]
    [TestCase("1.1.2+meta-valid", 1, 1, 2, null, "meta-valid")]
    [TestCase("1.0.0-alpha", 1, 0, 0, "alpha", null)]
    [TestCase("1.0.0-beta", 1, 0, 0, "beta", null)]
    [TestCase("1.0.0-alpha.beta", 1, 0, 0, "alpha.beta", null)]
    [TestCase("1.0.0-alpha.beta.1", 1, 0, 0, "alpha.beta.1", null)]
    [TestCase("1.0.0-alpha.1", 1, 0, 0, "alpha.1", null)]
    [TestCase("1.0.0-alpha0.valid", 1, 0, 0, "alpha0.valid", null)]
    [TestCase("1.0.0-alpha.0valid", 1, 0, 0, "alpha.0valid", null)]
    [TestCase(
        "1.0.0-alpha-a.b-c-somethinglong+build.1-aef.1-its-okay",
        1, 0, 0, "alpha-a.b-c-somethinglong", "build.1-aef.1-its-okay"
    )]
    [TestCase("1.0.0-rc.1+build.1", 1, 0, 0, "rc.1", "build.1")]
    [TestCase("2.0.0-rc.1+build.123", 2, 0, 0, "rc.1", "build.123")]
    [TestCase("1.2.3-beta", 1, 2, 3, "beta", null)]
    [TestCase("10.2.3-DEV-SNAPSHOT", 10, 2, 3, "DEV-SNAPSHOT", null)]
    [TestCase("1.2.3-SNAPSHOT-123", 1, 2, 3, "SNAPSHOT-123", null)]
    [TestCase("1.0.0", 1, 0, 0, null, null)]
    [TestCase("2.0.0", 2, 0, 0, null, null)]
    [TestCase("1.1.7", 1, 1, 7, null, null)]
    [TestCase("2.0.0+build.1848", 2, 0, 0, null, "build.1848")]
    [TestCase("2.0.1-alpha.1227", 2, 0, 1, "alpha.1227", null)]
    [TestCase("1.0.0-alpha+beta", 1, 0, 0, "alpha", "beta")]
    [TestCase(
        "1.2.3----RC-SNAPSHOT.12.9.1--.12+788",
        1, 2, 3, "---RC-SNAPSHOT.12.9.1--.12", "788"
    )]
    [TestCase(
        "1.2.3----R-S.12.9.1--.12+meta",
        1, 2, 3, "---R-S.12.9.1--.12", "meta"
    )]
    [TestCase(
        "1.2.3----RC-SNAPSHOT.12.9.1--.12",
        1, 2, 3, "---RC-SNAPSHOT.12.9.1--.12", null
    )]
    [TestCase(
        "1.0.0+0.build.1-rc.10000aaa-kk-0.1",
        1, 0, 0, null, "0.build.1-rc.10000aaa-kk-0.1"
    )]
    [TestCase("1.0.0-0A.is.legal", 1, 0, 0, "0A.is.legal", null)]
    public static void TryParse_WhenVersionIsValid(
        string v,
        int major,
        int minor,
        int patch,
        string? prerelease,
        string? buildMetadata
    )
    {
        _ = SemanticVersion.TryParse(v, out SemanticVersion? version)
            .ConfirmTrue();

        SemanticVersion ver = version.ConfirmNotNull().ConfirmType<SemanticVersion>();
        _ = ver.Major.ConfirmEqual(major);
        _ = ver.Minor.ConfirmEqual(minor);
        _ = ver.Patch.ConfirmEqual(patch);
        _ = ver.Prerelease.ConfirmEqual(prerelease);
        _ = ver.BuildMetadata.ConfirmEqual(buildMetadata);
    }

    [TestCase("")]
    [TestCase("1")]
    [TestCase("1.2")]
    [TestCase("1.2.3-0123")]
    [TestCase("1.2.3-0123.0123")]
    [TestCase("1.1.2+.123")]
    [TestCase("+invalid")]
    [TestCase("-invalid")]
    [TestCase("-invalid+invalid")]
    [TestCase("-invalid.01")]
    [TestCase("alpha")]
    [TestCase("alpha.beta")]
    public static void TryParse_WhenVersionIsInvalid(string versionString)
    {
        _ = SemanticVersion.TryParse(versionString, out SemanticVersion? version)
            .ConfirmFalse();

        _ = version.ConfirmNull();
    }
    #endregion

    [TestCase]
    public static void ToString_()
    {
        _ = new SemanticVersion(1, 1, 2, "prerelease", "meta")
            .ToString()
            .ConfirmEqual("1.1.2-prerelease+meta");
    }

    #region EqualOperator
    [TestCase]
    public static void EqualOperator_EqualVersions()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3");
        SemanticVersion v2 = SemanticVersion.Parse("1.2.3");

        _ = Confirm.IsTrue(v1 == v2);
        _ = Confirm.IsTrue(v2 == v1);
    }

    [TestCase("1.1.1", "1.1.2")]
    [TestCase("1.1.1", "1.2.1")]
    [TestCase("1.1.1", "2.1.1")]
    [TestCase("1.1.1-beta1", "1.1.1-beta2")]
    public static void EqualOperator_NotEqualVersions(string v1Str, string v2Str)
    {
        SemanticVersion v1 = SemanticVersion.Parse(v1Str);
        SemanticVersion v2 = SemanticVersion.Parse(v2Str);

        _ = Confirm.IsFalse(v1 == v2);
        _ = Confirm.IsFalse(v2 == v1);
    }
    #endregion

    #region NotEqualOperator
    [TestCase("1.1.1", "1.1.2")]
    [TestCase("1.1.1", "1.2.1")]
    [TestCase("1.1.1", "2.1.1")]
    [TestCase("1.1.1-beta1", "1.1.1-beta2")]
    public static void NotEqualOperator_NotEqualVersions(string v1Str, string v2Str)
    {
        SemanticVersion v1 = SemanticVersion.Parse(v1Str);
        SemanticVersion v2 = SemanticVersion.Parse(v2Str);

        _ = Confirm.IsTrue(v1 != v2);
        _ = Confirm.IsTrue(v2 != v1);
    }

    [TestCase]
    public static void NotEqualOperator_EqualVersions()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3");
        SemanticVersion v2 = SemanticVersion.Parse("1.2.3");

        _ = Confirm.IsFalse(v1 != v2);
        _ = Confirm.IsFalse(v2 != v1);
    }
    #endregion

    #region GreaterThan
    [TestCase]
    public static void GreaterThan_MajorVersion_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3");
        SemanticVersion v2 = SemanticVersion.Parse("2.2.3");

        _ = Confirm.IsTrue(v2 > v1);
        _ = Confirm.IsFalse(v1 > v2);
    }

    [TestCase]
    public static void GreaterThan_MinorVersion_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3");
        SemanticVersion v2 = SemanticVersion.Parse("1.3.3");

        _ = Confirm.IsTrue(v2 > v1);
        _ = Confirm.IsFalse(v1 > v2);
    }

    [TestCase]
    public static void GreaterThan_PatchVersion_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3");
        SemanticVersion v2 = SemanticVersion.Parse("1.2.4");

        _ = Confirm.IsTrue(v2 > v1);
        _ = Confirm.IsFalse(v1 > v2);
    }

    [TestCase]
    public static void GreaterThan_PrereleaseVersion_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3-alpha");
        SemanticVersion v2 = SemanticVersion.Parse("1.2.3-beta"); // !

        _ = Confirm.IsTrue(v2 > v1);
        _ = Confirm.IsFalse(v1 > v2);
    }

    [TestCase]
    public static void GreaterThan_BuildMetadata_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3+build.123");
        SemanticVersion v2 = SemanticVersion.Parse("1.2.3+build.456");

        _ = Confirm.IsTrue(v2 > v1);
        _ = Confirm.IsFalse(v1 > v2);
    }

    [TestCase]
    public static void EqualVersions_AreNotGreaterThan()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3");
        SemanticVersion v2 = SemanticVersion.Parse("1.2.3");

        _ = Confirm.IsFalse(v1 > v2);
        _ = Confirm.IsFalse(v2 > v1);
    }
    #endregion

    #region LessThan
    [TestCase]
    public static void LessThan_MajorVersion_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3");
        SemanticVersion v2 = SemanticVersion.Parse("2.2.3");

        _ = Confirm.IsTrue(v1 < v2);
        _ = Confirm.IsFalse(v2 < v1);
    }

    [TestCase]
    public static void LessThan_MinorVersion_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3");
        SemanticVersion v2 = SemanticVersion.Parse("1.3.3");

        _ = Confirm.IsTrue(v1 < v2);
        _ = Confirm.IsFalse(v2 < v1);
    }

    [TestCase]
    public static void LessThan_PatchVersion_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3");
        SemanticVersion v2 = SemanticVersion.Parse("1.2.4");

        _ = Confirm.IsTrue(v1 < v2);
        _ = Confirm.IsFalse(v2 < v1);
    }

    [TestCase]
    public static void LessThan_PrereleaseVersion_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3-alpha");
        SemanticVersion v2 = SemanticVersion.Parse("1.2.3-beta");

        _ = Confirm.IsTrue(v1 < v2);
        _ = Confirm.IsFalse(v2 < v1);
    }

    [TestCase]
    public static void LessThan_BuildMetadata_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3+build.123");
        SemanticVersion v2 = SemanticVersion.Parse("1.2.3+build.456");

        _ = Confirm.IsTrue(v1 < v2);
        _ = Confirm.IsFalse(v2 < v1);
    }

    [TestCase]
    public static void EqualVersions_AreNotLessThan()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3");
        SemanticVersion v2 = SemanticVersion.Parse("1.2.3");

        _ = Confirm.IsFalse(v1 < v2);
        _ = Confirm.IsFalse(v2 < v1);
    }
    #endregion

    #region Equals
    [TestCase]
    public static void Equals_SameObject_ReturnsTrue()
    {
        SemanticVersion version = new(1, 2, 3);

        _ = version.Equals(version).ConfirmTrue();
    }

    [TestCase]
    public static void Equals_SameValues_ReturnsTrue()
    {
        SemanticVersion version1 = new(1, 2, 3);
        SemanticVersion version2 = new(1, 2, 3);

        _ = version1.Equals(version2).ConfirmTrue();
    }

    [TestCase]
    public static void Equals_DifferentValues_ReturnsFalse()
    {
        SemanticVersion version1 = new(1, 2, 3);
        SemanticVersion version2 = new(1, 2, 4);

        _ = version1.Equals(version2).ConfirmFalse();
    }

    [TestCase]
    public static void Equals_Null_ReturnsFalse()
    {
        SemanticVersion version = new(1, 2, 3);

        _ = version.Equals(null).ConfirmFalse();
    }

    [TestCase]
    public static void Equals_DifferentType_ReturnsFalse()
    {
        SemanticVersion version = new(1, 2, 3);

        _ = version.Equals(new object()).ConfirmFalse();
    }
    #endregion

    #region GetHashCode
    [TestCase]
    public static void GetHashCode_SameValues_ReturnsSameHashCode()
    {
        SemanticVersion version1 = new(1, 2, 3);
        SemanticVersion version2 = new(1, 2, 3);

        _ = version1.GetHashCode().ConfirmEqual(version2.GetHashCode());
    }

    [TestCase]
    public static void GetHashCode_DifferentValues_ReturnsDifferentHashCode()
    {
        SemanticVersion version1 = new(1, 2, 3);
        SemanticVersion version2 = new(1, 2, 4);

        _ = version1.GetHashCode().ConfirmNotEqual(version2.GetHashCode());
    }
    #endregion
}
