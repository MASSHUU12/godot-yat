using YAT.Classes;

namespace YAT.Types;

public record ReleaseTagInfo(
    SemanticVersion Version,
    string ZipballUrl,
    string TarballUrl,
    string CommitSha,
    string CommitUrl,
    string NodeId
);
