using System;
using Confirma.Attributes;
using Confirma.Classes;
using Confirma.Extensions;
using YAT.Update;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public class SemanticVersionTest
{
    #region Constructor
    [TestCase]
    public void Constructor()
    {
        SemanticVersion version = new(1, 2, 3, "beta", "meta");

        _ = version.Major.ConfirmEqual(1);
        _ = version.Minor.ConfirmEqual(2);
        _ = version.Patch.ConfirmEqual(3);
        _ = version.Prerelease.ConfirmEqual("beta");
        _ = version.BuildMetadata.ConfirmEqual("meta");
    }

    [TestCase]
    public void Constructor_Empty()
    {
        SemanticVersion version = new();

        _ = version.Major.ConfirmEqual(0);
        _ = version.Minor.ConfirmEqual(0);
        _ = version.Patch.ConfirmEqual(0);
        _ = version.Prerelease.ConfirmNull();
        _ = version.BuildMetadata.ConfirmNull();
    }

    [TestCase]
    public void Constructor_Invalid_MajorVersion()
    {
        _ = Confirm.Throws<FormatException>(
            () => _ = new SemanticVersion(-5, 0, 0)
        );
    }

    [TestCase]
    public void Constructor_Invalid_MinorVersion()
    {
        _ = Confirm.Throws<FormatException>(
            () => _ = new SemanticVersion(5, -5, 0)
        );
    }

    [TestCase]
    public void Constructor_Invalid_PatchVersion()
    {
        _ = Confirm.Throws<FormatException>(
            () => _ = new SemanticVersion(5, 5, -5)
        );
    }
    #endregion Constructor

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
    public void IsValid_WhenVersionIsValid(string versionString)
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
    public void IsValid_WhenVersionIsInvalid(string versionString)
    {
        _ = SemanticVersion.IsValid(versionString).ConfirmFalse();
    }
    #endregion IsValid

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
    public void Parse_WhenVersionIsValid(
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
    public void Parse_WhenVersionIsInvalid(string versionString)
    {
        _ = Confirm.Throws<FormatException>(() => SemanticVersion.Parse(versionString));
    }
    #endregion Parse

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
    public void TryParse_WhenVersionIsValid(
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
    public void TryParse_WhenVersionIsInvalid(string versionString)
    {
        _ = SemanticVersion.TryParse(versionString, out SemanticVersion? version)
            .ConfirmFalse();

        _ = version.ConfirmNull();
    }
    #endregion TryParse

    #region ToString
    [TestCase]
    public void ToString_WO_PrereleaseAndBuildMetadata()
    {
        _ = new SemanticVersion(1, 1, 2)
            .ToString()
            .ConfirmEqual("1.1.2");
    }

    [TestCase]
    public void ToString_W_PrereleaseAndBuildMetadata()
    {
        _ = new SemanticVersion(1, 1, 2, "prerelease", "meta")
            .ToString()
            .ConfirmEqual("1.1.2-prerelease+meta");
    }
    #endregion ToString

    #region Operator_Equal
    [TestCase]
    public void Operator_Equal_EqualVersions()
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
    public void Operator_Equal_NotEqualVersions(string v1Str, string v2Str)
    {
        SemanticVersion v1 = SemanticVersion.Parse(v1Str);
        SemanticVersion v2 = SemanticVersion.Parse(v2Str);

        _ = Confirm.IsFalse(v1 == v2);
        _ = Confirm.IsFalse(v2 == v1);
    }
    #endregion Operator_Equal

    #region Operator_NotEqual
    [TestCase("1.1.1", "1.1.2")]
    [TestCase("1.1.1", "1.2.1")]
    [TestCase("1.1.1", "2.1.1")]
    [TestCase("1.1.1-beta1", "1.1.1-beta2")]
    public void Operator_NotEqual_NotEqualVersions(string v1Str, string v2Str)
    {
        SemanticVersion v1 = SemanticVersion.Parse(v1Str);
        SemanticVersion v2 = SemanticVersion.Parse(v2Str);

        _ = Confirm.IsTrue(v1 != v2);
        _ = Confirm.IsTrue(v2 != v1);
    }

    [TestCase]
    public void Operator_NotEqual_EqualVersions()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3");
        SemanticVersion v2 = SemanticVersion.Parse("1.2.3");

        _ = Confirm.IsFalse(v1 != v2);
        _ = Confirm.IsFalse(v2 != v1);
    }
    #endregion Operator_NotEqual

    #region Operator_GreaterThan
    [TestCase]
    public void Operator_GreaterThan_MajorVersion_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3");
        SemanticVersion v2 = SemanticVersion.Parse("2.2.3");

        _ = Confirm.IsTrue(v2 > v1);
        _ = Confirm.IsFalse(v1 > v2);
    }

    [TestCase]
    public void Operator_GreaterThan_MinorVersion_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3");
        SemanticVersion v2 = SemanticVersion.Parse("1.3.3");

        _ = Confirm.IsTrue(v2 > v1);
        _ = Confirm.IsFalse(v1 > v2);
    }

    [TestCase]
    public void Operator_GreaterThan_PatchVersion_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3");
        SemanticVersion v2 = SemanticVersion.Parse("1.2.4");

        _ = Confirm.IsTrue(v2 > v1);
        _ = Confirm.IsFalse(v1 > v2);
    }

    [TestCase]
    public void Operator_GreaterThan_PrereleaseVersion_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3-alpha");
        SemanticVersion v2 = SemanticVersion.Parse("1.2.3-beta"); // !

        _ = Confirm.IsTrue(v2 > v1);
        _ = Confirm.IsFalse(v1 > v2);
    }

    [TestCase]
    public void Operator_GreaterThan_BuildMetadata_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3+build.123");
        SemanticVersion v2 = SemanticVersion.Parse("1.2.3+build.456");

        _ = Confirm.IsTrue(v2 > v1);
        _ = Confirm.IsFalse(v1 > v2);
    }

    [TestCase]
    public void Operator_GreaterThan_EqualVersions_AreNotGreaterThan()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3");
        SemanticVersion v2 = SemanticVersion.Parse("1.2.3");

        _ = Confirm.IsFalse(v1 > v2);
        _ = Confirm.IsFalse(v2 > v1);
    }
    #endregion Operator_GreaterThan

    #region Operator_GreaterThanOrEqual
    [TestCase]
    public void Operator_GreaterThanOrEqual_ReturnsTrue_WhenLeftIsGreaterThanRight()
    {
        SemanticVersion version1 = new(1, 1, 1);
        SemanticVersion version2 = new(1, 1, 0);

        _ = Confirm.IsTrue(version1 >= version2);
    }

    [TestCase]
    public void Operator_GreaterThanOrEqual_ReturnsTrue_WhenLeftIsEqualToRight()
    {
        SemanticVersion version1 = new(1, 1, 0);
        SemanticVersion version2 = new(1, 1, 0);

        _ = Confirm.IsTrue(version1 >= version2);
    }

    [TestCase]
    public void Operator_GreaterThanOrEqual_ReturnsFalse_WhenLeftIsLessThanRight()
    {
        SemanticVersion version1 = new(1, 0, 0);
        SemanticVersion version2 = new(1, 1, 0);

        _ = Confirm.IsFalse(version1 >= version2);
    }
    #endregion Operator_GreaterThanOrEqual

    #region Operator_LessThan
    [TestCase]
    public void Operator_LessThan_MajorVersion_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3");
        SemanticVersion v2 = SemanticVersion.Parse("2.2.3");

        _ = Confirm.IsTrue(v1 < v2);
        _ = Confirm.IsFalse(v2 < v1);
    }

    [TestCase]
    public void Operator_LessThan_MinorVersion_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3");
        SemanticVersion v2 = SemanticVersion.Parse("1.3.3");

        _ = Confirm.IsTrue(v1 < v2);
        _ = Confirm.IsFalse(v2 < v1);
    }

    [TestCase]
    public void Operator_LessThan_PatchVersion_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3");
        SemanticVersion v2 = SemanticVersion.Parse("1.2.4");

        _ = Confirm.IsTrue(v1 < v2);
        _ = Confirm.IsFalse(v2 < v1);
    }

    [TestCase]
    public void Operator_LessThan_PrereleaseVersion_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3-alpha");
        SemanticVersion v2 = SemanticVersion.Parse("1.2.3-beta");

        _ = Confirm.IsTrue(v1 < v2);
        _ = Confirm.IsFalse(v2 < v1);
    }

    [TestCase]
    public void Operator_LessThan_BuildMetadata_Different()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3+build.123");
        SemanticVersion v2 = SemanticVersion.Parse("1.2.3+build.456");

        _ = Confirm.IsTrue(v1 < v2);
        _ = Confirm.IsFalse(v2 < v1);
    }

    [TestCase]
    public void Operator_LessThan_EqualVersions_AreNotLessThan()
    {
        SemanticVersion v1 = SemanticVersion.Parse("1.2.3");
        SemanticVersion v2 = SemanticVersion.Parse("1.2.3");

        _ = Confirm.IsFalse(v1 < v2);
        _ = Confirm.IsFalse(v2 < v1);
    }
    #endregion Operator_LessThan

    #region Operator_LessThanOrEqual
    [TestCase]
    public void Operator_LessThanOrEqual_ReturnsTrue_WhenLeftIsLessThanRight()
    {
        SemanticVersion version1 = new(1, 0, 0);
        SemanticVersion version2 = new(1, 1, 0);

        _ = Confirm.IsTrue(version1 <= version2);
    }

    [TestCase]
    public void Operator_LessThanOrEqual_ReturnsTrue_WhenLeftIsEqualToRight()
    {
        SemanticVersion version1 = new(1, 1, 0);
        SemanticVersion version2 = new(1, 1, 0);

        _ = Confirm.IsTrue(version1 <= version2);
    }

    [TestCase]
    public void Operator_LessThanOrEqual_ReturnsFalse_WhenLeftIsGreaterThanRight()
    {
        SemanticVersion version1 = new(1, 1, 1);
        SemanticVersion version2 = new(1, 1, 0);

        _ = Confirm.IsFalse(version1 <= version2);
    }
    #endregion Operator_LessThanOrEqual

    #region Equals
    [TestCase]
    public void Equals_SameObject_ReturnsTrue()
    {
        SemanticVersion version = new(1, 2, 3);

        _ = version.Equals(version).ConfirmTrue();
    }

    [TestCase]
    public void Equals_SameValues_ReturnsTrue()
    {
        SemanticVersion version1 = new(1, 2, 3);
        SemanticVersion version2 = new(1, 2, 3);

        _ = version1.Equals(version2).ConfirmTrue();
    }

    [TestCase]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        SemanticVersion version1 = new(1, 2, 3);
        SemanticVersion version2 = new(1, 2, 4);

        _ = version1.Equals(version2).ConfirmFalse();
    }

    [TestCase]
    public void Equals_Null_ReturnsFalse()
    {
        SemanticVersion version = new(1, 2, 3);

        _ = version.Equals(null).ConfirmFalse();
    }

    [TestCase]
    public void Equals_DifferentType_ReturnsFalse()
    {
        SemanticVersion version = new(1, 2, 3);

        _ = version.Equals(new object()).ConfirmFalse();
    }
    #endregion Equals

    #region GetHashCode
    [TestCase]
    public void GetHashCode_SameValues_ReturnsSameHashCode()
    {
        SemanticVersion version1 = new(1, 2, 3);
        SemanticVersion version2 = new(1, 2, 3);

        _ = version1.GetHashCode().ConfirmEqual(version2.GetHashCode());
    }

    [TestCase]
    public void GetHashCode_DifferentValues_ReturnsDifferentHashCode()
    {
        SemanticVersion version1 = new(1, 2, 3);
        SemanticVersion version2 = new(1, 2, 4);

        _ = version1.GetHashCode().ConfirmNotEqual(version2.GetHashCode());
    }
    #endregion GetHashCode

    #region IncrementMajor
    [TestCase]
    public void IncrementMajor_Defaults_PreserveValues()
    {
        SemanticVersion version = new(1, 2, 3, "alpha", "build001");
        SemanticVersion incremented = version.IncrementMajor();

        _ = incremented.Major.ConfirmEqual(2);
        _ = incremented.Minor.ConfirmEqual(0);
        _ = incremented.Patch.ConfirmEqual(0);
        _ = incremented.Prerelease.ConfirmNull();
        _ = incremented.BuildMetadata.ConfirmNull();
    }

    [TestCase]
    public void IncrementMajor_WithMetadata_PreserveValues()
    {
        SemanticVersion version = new(1, 2, 3, "alpha", "build001");
        SemanticVersion incremented = version.IncrementMajor("beta", "build002");

        _ = incremented.Major.ConfirmEqual(2);
        _ = incremented.Minor.ConfirmEqual(0);
        _ = incremented.Patch.ConfirmEqual(0);
        _ = incremented.Prerelease.ConfirmEqual("beta");
        _ = incremented.BuildMetadata.ConfirmEqual("build002");
    }
    #endregion IncrementMajor

    #region IncrementMinor
    [TestCase]
    public void IncrementMinor_Defaults_PreserveValues()
    {
        SemanticVersion version = new(1, 2, 3, "alpha", "build001");
        SemanticVersion incremented = version.IncrementMinor();

        _ = incremented.Major.ConfirmEqual(1);
        _ = incremented.Minor.ConfirmEqual(3);
        _ = incremented.Patch.ConfirmEqual(0);
        _ = incremented.Prerelease.ConfirmNull();
        _ = incremented.BuildMetadata.ConfirmNull();
    }

    [TestCase]
    public void IncrementMinor_WithMetadata_PreserveValues()
    {
        SemanticVersion version = new(1, 2, 3, "alpha", "build001");
        SemanticVersion incremented = version.IncrementMinor("beta", "build002");

        _ = incremented.Major.ConfirmEqual(1);
        _ = incremented.Minor.ConfirmEqual(3);
        _ = incremented.Patch.ConfirmEqual(0);
        _ = incremented.Prerelease.ConfirmEqual("beta");
        _ = incremented.BuildMetadata.ConfirmEqual("build002");
    }
    #endregion IncrementMinor

    #region IncrementPatch
    [TestCase]
    public void IncrementPatch_Defaults_PreserveValues()
    {
        SemanticVersion version = new(1, 2, 3, "alpha", "build001");
        SemanticVersion incremented = version.IncrementPatch();

        _ = incremented.Major.ConfirmEqual(1);
        _ = incremented.Minor.ConfirmEqual(2);
        _ = incremented.Patch.ConfirmEqual(4);
        _ = incremented.Prerelease.ConfirmNull();
        _ = incremented.BuildMetadata.ConfirmNull();
    }

    [TestCase]
    public void IncrementPatch_WithMetadata_PreserveValues()
    {
        SemanticVersion version = new(1, 2, 3, "alpha", "build001");
        SemanticVersion incremented = version.IncrementPatch("beta", "build002");

        _ = incremented.Major.ConfirmEqual(1);
        _ = incremented.Minor.ConfirmEqual(2);
        _ = incremented.Patch.ConfirmEqual(4);
        _ = incremented.Prerelease.ConfirmEqual("beta");
        _ = incremented.BuildMetadata.ConfirmEqual("build002");
    }
    #endregion IncrementPatch

    #region DecrementMajor
    [TestCase]
    public void DecrementMajor_Defaults_PreserveValues()
    {
        SemanticVersion version = new(1, 2, 3, "alpha", "build001");
        SemanticVersion decremented = version.DecrementMajor();

        _ = decremented.Major.ConfirmEqual(0);
        _ = decremented.Minor.ConfirmEqual(0);
        _ = decremented.Patch.ConfirmEqual(0);
        _ = decremented.Prerelease.ConfirmNull();
        _ = decremented.BuildMetadata.ConfirmNull();
    }

    [TestCase]
    public void DecrementMajor_WithMetadata_PreserveValues()
    {
        SemanticVersion version = new(1, 2, 3, "alpha", "build001");
        SemanticVersion decremented = version.DecrementMajor("beta", "build002");

        _ = decremented.Major.ConfirmEqual(0);
        _ = decremented.Minor.ConfirmEqual(0);
        _ = decremented.Patch.ConfirmEqual(0);
        _ = decremented.Prerelease.ConfirmEqual("beta");
        _ = decremented.BuildMetadata.ConfirmEqual("build002");
    }

    [TestCase]
    public void DecrementMajor_ThrowsException_IfZero()
    {
        SemanticVersion version = new(0, 0, 0);
        _ = Confirm.Throws<InvalidOperationException>(() => _ = version.DecrementMajor());
    }
    #endregion DecrementMajor

    #region DecrementMinor
    [TestCase]
    public void DecrementMinor_Defaults_PreserveValues()
    {
        SemanticVersion version = new(1, 2, 3, "alpha", "build001");
        SemanticVersion decremented = version.DecrementMinor();

        _ = decremented.Major.ConfirmEqual(1);
        _ = decremented.Minor.ConfirmEqual(1);
        _ = decremented.Patch.ConfirmEqual(0);
        _ = decremented.Prerelease.ConfirmNull();
        _ = decremented.BuildMetadata.ConfirmNull();
    }

    [TestCase]
    public void DecrementMinor_WithMetadata_PreserveValues()
    {
        SemanticVersion version = new(1, 2, 3, "alpha", "build001");
        SemanticVersion decremented = version.DecrementMinor("beta", "build002");

        _ = decremented.Major.ConfirmEqual(1);
        _ = decremented.Minor.ConfirmEqual(1);
        _ = decremented.Patch.ConfirmEqual(0);
        _ = decremented.Prerelease.ConfirmEqual("beta");
        _ = decremented.BuildMetadata.ConfirmEqual("build002");
    }

    [TestCase]
    public void DecrementMinor_ThrowsException_IfZero()
    {
        SemanticVersion version = new(1, 0, 0);
        _ = Confirm.Throws<InvalidOperationException>(() => _ = version.DecrementMinor());
    }
    #endregion DecrementMinor

    #region DecrementPatch
    [TestCase]
    public void DecrementPatch_Defaults_PreserveValues()
    {
        SemanticVersion version = new(1, 2, 3, "alpha", "build001");
        SemanticVersion decremented = version.DecrementPatch();

        _ = decremented.Major.ConfirmEqual(1);
        _ = decremented.Minor.ConfirmEqual(2);
        _ = decremented.Patch.ConfirmEqual(2);
        _ = decremented.Prerelease.ConfirmNull();
        _ = decremented.BuildMetadata.ConfirmNull();
    }

    [TestCase]
    public void DecrementPatch_WithMetadata_PreserveValues()
    {
        SemanticVersion version = new(1, 2, 3, "alpha", "build001");
        SemanticVersion decremented = version.DecrementPatch("beta", "build002");

        _ = decremented.Major.ConfirmEqual(1);
        _ = decremented.Minor.ConfirmEqual(2);
        _ = decremented.Patch.ConfirmEqual(2);
        _ = decremented.Prerelease.ConfirmEqual("beta");
        _ = decremented.BuildMetadata.ConfirmEqual("build002");
    }

    [TestCase]
    public void DecrementPatch_ThrowsException_IfZero()
    {
        SemanticVersion version = new(1, 1, 0);
        _ = Confirm.Throws<InvalidOperationException>(() => _ = version.DecrementPatch());
    }
    #endregion DecrementPatch

    #region CompareTo
    [TestCase]
    public void CompareTo_MajorVersionDifferent()
    {
        SemanticVersion version1 = new(1, 0, 0);
        SemanticVersion version2 = new(2, 0, 0);

        _ = Confirm.IsTrue(version1.CompareTo(version2) < 0);
        _ = Confirm.IsTrue(version2.CompareTo(version1) > 0);
    }

    [TestCase]
    public void CompareTo_MinorVersionDifferent()
    {
        SemanticVersion version1 = new(1, 1, 0);
        SemanticVersion version2 = new(1, 2, 0);

        _ = Confirm.IsTrue(version1.CompareTo(version2) < 0);
        _ = Confirm.IsTrue(version2.CompareTo(version1) > 0);
    }

    [TestCase]
    public void CompareTo_PatchVersionDifferent()
    {
        SemanticVersion version1 = new(1, 0, 1);
        SemanticVersion version2 = new(1, 0, 2);

        _ = Confirm.IsTrue(version1.CompareTo(version2) < 0);
        _ = Confirm.IsTrue(version2.CompareTo(version1) > 0);
    }

    [TestCase]
    public void CompareTo_PrereleaseVersionDifferent()
    {
        SemanticVersion version1 = new(1, 0, 0, "alpha");
        SemanticVersion version2 = new(1, 0, 0, "beta");

        _ = Confirm.IsTrue(version1.CompareTo(version2) < 0);
        _ = Confirm.IsTrue(version2.CompareTo(version1) > 0);
    }

    [TestCase]
    public void CompareTo_PrereleaseVsRelease()
    {
        SemanticVersion version1 = new(1, 0, 0, "alpha");
        SemanticVersion version2 = new(1, 0, 0);

        _ = Confirm.IsTrue(version1.CompareTo(version2) < 0);
        _ = Confirm.IsTrue(version2.CompareTo(version1) > 0);
    }

    [TestCase]
    public void CompareTo_BuildMetadataDifferent()
    {
        SemanticVersion version1 = new(1, 0, 0, null, "build1");
        SemanticVersion version2 = new(1, 0, 0, null, "build2");

        _ = Confirm.IsTrue(version1.CompareTo(version2) < 0);
        _ = Confirm.IsTrue(version2.CompareTo(version1) > 0);
    }

    [TestCase]
    public void CompareTo_EqualVersions()
    {
        SemanticVersion version1 = new(1, 0, 0, "alpha", "build1");
        SemanticVersion version2 = new(1, 0, 0, "alpha", "build1");

        _ = Confirm.IsTrue(version1.CompareTo(version2) == 0);
    }

    [TestCase]
    public void CompareTo_NullVersion()
    {
        SemanticVersion version1 = new(1, 0, 0);

        _ = Confirm.IsTrue(version1.CompareTo(null) > 0);
    }

    [TestCase]
    public void CompareTo_ReturnsPositive_WhenThisIsGreaterThanOther()
    {
        SemanticVersion version1 = new(1, 1, 1);
        SemanticVersion version2 = new(1, 1, 0);

        _ = version1.CompareTo((object)version2).ConfirmIsPositive();
    }

    [TestCase]
    public void CompareTo_ReturnsZero_WhenThisIsEqualToOther()
    {
        SemanticVersion version1 = new(1, 1, 0);
        SemanticVersion version2 = new(1, 1, 0);

        _ = version1.CompareTo((object)version2).ConfirmIsZero();
    }

    [TestCase]
    public void CompareTo_ReturnsNegative_WhenThisIsLessThanOther()
    {
        SemanticVersion version1 = new(1, 0, 0);
        SemanticVersion version2 = new(1, 1, 0);

        _ = version1.CompareTo((object)version2).ConfirmIsNegative();
    }

    [TestCase]
    public void CompareTo_ReturnsPositive_WhenOtherIsNull()
    {
        SemanticVersion version1 = new(1, 0, 0);

        _ = version1.CompareTo((object?)null).ConfirmIsPositive();
    }

    [TestCase]
    public void CompareTo_ThrowsArgumentException_WhenOtherIsNotSemanticVersion()
    {
        SemanticVersion version1 = new(1, 0, 0);
        object other = new();

        _ = Confirm.Throws<ArgumentException>(() => version1.CompareTo(other));
    }
    #endregion CompareTo
}
