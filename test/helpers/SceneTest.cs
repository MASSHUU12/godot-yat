using Confirma.Attributes;
using Confirma.Classes;
using Confirma.Extensions;
using Godot;
using YAT.Helpers;
using YAT.Types;

namespace YAT.Test;

[SetUp]
[TearDown]
[TestClass]
// [Parallelizable] // Manipulating SceneTree is only allowed from the main thread.
public partial class SceneTest
{
    private TestNode? _parent;

    private partial class TestNode : Node
    {
        public TestNode(string name)
        {
            Name = name;
        }
    }

    public void SetUp()
    {
        _parent = new("Parent");

        Global.Root.GetTree().CurrentScene.AddChild(_parent);
    }

    public void TearDown()
    {
        _parent!.Free();
    }

    #region PrintChildren
    [TestCase]
    public void PrintChildren_NoChildren()
    {
        _ = Scene.PrintChildren(_parent!)
            .ConfirmEqual("Node '/root/TestRunnerGame/Parent' has no children.");
    }

    [TestCase]
    public void PrintChildren_HasChildren()
    {
        Node child = new TestNode("Child1");
        _parent!.AddChild(child);

        _ = Scene.PrintChildren(_parent!).ConfirmEqual(
            "Found 1 children of 'Parent' node:\n[Child1] (TestNode) - "
            + "/root/TestRunnerGame/Parent/Child1\n"
        );
    }
    #endregion PrintChildren

    #region GetFromPathOrDefault
    [TestCase]
    public void GetFromPathOrDefault_WithDefaultPath_ReturnsDefaultNode()
    {
        Node? result = Scene.GetFromPathOrDefault(".", _parent!, out string path);

        _ = result.ConfirmEqual(_parent);
        _ = path.ConfirmEqual("/root/TestRunnerGame/Parent");
    }

    [TestCase]
    public void GetFromPathOrDefault_WithValidPath_ReturnsNode()
    {
        Node child = new TestNode("Child");
        _parent!.AddChild(child);

        Node? result = Scene.GetFromPathOrDefault("/Child", _parent, out string newPath);

        _ = result.ConfirmEqual(child);
        _ = newPath.ConfirmEqual("/Child");
    }
    #endregion GetFromPathOrDefault

    #region GetNodeMethods
    [TestCase]
    public void GetNodeMethods_ValidNode_ReturnsMethodsEnumerator()
    {
        _ = Scene.GetNodeMethods(_parent!).ConfirmNotNull();
    }
    #endregion GetNodeMethods

    #region TryFindNodeMethodInfo
    [TestCase]
    public void TryFindNodeMethodInfo_ValidNode_ReturnsTrue()
    {
        bool result = Scene.TryFindNodeMethodInfo(
            _parent!,
            "methodName",
            out NodeMethodInfo info
        );

        _ = result.ConfirmTrue();
        _ = info.ConfirmNotNull();
    }
    #endregion TryFindNodeMethodInfo

    #region GetRangeFromHint
    [TestCase]
    public void GetRangeFromHint_EmptyHint_ReturnsZeroTuple()
    {
        const string hint = "";
        _ = Scene.GetRangeFromHint(hint).ConfirmEqual((0, 0, 0));
    }

    [TestCase("", 0, 0, 0)]
    [TestCase("0, 32", 0, 32, 0)]
    [TestCase("1,255,1", 1, 255, 1)]
    [TestCase("0,1,0.1", 0, 1, 0.1f)]
    [TestCase("0.3,0.5,0.1", 0.3f, 0.5f, 0.1f)]
    public void GetRangeFromHint(string hint, float min, float max, float step)
    {
        (float aMin, float aMax, float aStep) = Scene.GetRangeFromHint(hint);

        _ = min.ConfirmEqual(aMin);
        _ = max.ConfirmEqual(aMax);
        _ = step.ConfirmEqual(aStep);
    }
    #endregion GetRangeFromHint
}
