namespace YAT.Update;

public record ReleaseTagInfo(
    SemanticVersion Version,
    string ZipballUrl,
    string TarballUrl,
    string CommitSha,
    string CommitUrl,
    string NodeId
);
