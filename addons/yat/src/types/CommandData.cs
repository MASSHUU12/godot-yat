using System.Collections.Generic;
using System.Threading;
using Godot;
using YAT.Interfaces;
using YAT.Scenes;

namespace YAT.Types;

public sealed record CommandData(
    YAT Yat,
    BaseTerminal Terminal,
    ICommand Command,
    string[] RawData,
    Dictionary<StringName, object> Arguments,
    Dictionary<StringName, object> Options,
    CancellationToken CancellationToken
);
