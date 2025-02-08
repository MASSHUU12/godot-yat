using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Confirma.Enums;
using Confirma.Types;
using Godot;
using static Confirma.Enums.ELifecycleMethodName;

namespace Confirma.Classes;

public class GdScriptInfo : ScriptInfo
{
    public ImmutableDictionary<ELifecycleMethodName, ScriptMethodInfo>
        LifecycleMethods
    { get; init; }

    private static readonly Dictionary<string, ELifecycleMethodName>
        _lifecycleMethodNameMap = new()
    {
        { "after_all", AfterAll },
        { "before_all", BeforeAll },
        { "category", Category },
        { "ignore", Ignore },
        { "set_up", SetUp },
        { "tear_down", TearDown }
    };

    public GdScriptInfo(Script script, LinkedList<ScriptMethodInfo> methods)
    : base(script, methods)
    {
        IEnumerable<ScriptMethodInfo> lifecycleMethods = Methods.Where(
            static m => _lifecycleMethodNameMap.ContainsKey(m.Name)
        );

        LifecycleMethods = lifecycleMethods.ToImmutableDictionary(
            static m => _lifecycleMethodNameMap[m.Name]
        );

        Methods = new(Methods.Except(lifecycleMethods));
    }

    public static new GdScriptInfo Parse(in Script script)
    {
        return new(script, ScriptInfo.Parse(script).Methods);
    }
}
