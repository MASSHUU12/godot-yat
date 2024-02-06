using Godot;
using YAT.Classes;
using YAT.Helpers;

namespace YAT.Scenes.BaseTerminal;

public partial class SelectedNode : Node
{
	[Signal]
	public delegate void CurrentNodeChangedEventHandler(Node node);
	[Signal]
	public delegate void CurrentNodeChangeFailedEventHandler(RejectionReason reason);
	[Signal]
	public delegate void MethodCalledEventHandler(string method, Variant returnValue, MethodStatus status);

	public Node Current { get; private set; }

	public enum RejectionReason
	{
		InvalidNode,
		InvalidNodePath
	}

	public enum MethodStatus
	{
		Success,
		Failed
	}

	private BaseTerminal _terminal;

	public override void _Ready()
	{
		_terminal = GetNode<BaseTerminal>("../");
		Current = GetTree().Root;
	}

	/// <summary>
	/// Changes the selected node to the specified node path.
	/// </summary>
	/// <param name="node">The node path of the new selected node.</param>
	/// <returns>True if the selected node was successfully changed, false otherwise.</returns>
	public bool ChangeSelectedNode(NodePath node)
	{
		if (node is null || node.IsEmpty)
		{
			EmitSignal(SignalName.CurrentNodeChangeFailed, (ushort)RejectionReason.InvalidNodePath);
			return false;
		}

		Node newSelectedNode;
		if (IsInstanceValid(Current))
		{
			newSelectedNode = Current.GetNodeOrNull(node);
			newSelectedNode ??= GetNodeOrNull(node);
		}
		else newSelectedNode = GetNodeOrNull(node);

		if (!IsInstanceValid(newSelectedNode))
		{
			EmitSignal(SignalName.CurrentNodeChangeFailed, (ushort)RejectionReason.InvalidNode);
			return false;
		}

		Current = newSelectedNode;
		EmitSignal(SignalName.CurrentNodeChanged, Current);

		return true;
	}

	public bool CallMethods(string input)
	{
		_terminal.Output.Warning(
			"Please keep in mind that this feature is still in development.\nMany things may not work as expected.\n"
		);

		string[] methods = Text.SplitClean(input, ".");
		Variant result = new();

		if (methods.Length == 0) return false;

		// TODO: Method chaining

		foreach (string method in methods)
		{
			var tokens = Parser.ParseMethod(method);

			if (tokens.Item2.Length == 0
				? !CallMethod(tokens.Item1, out result)
				: !CallMethod(tokens.Item1, out result, tokens.Item2)
			) return false;
		}

		return true;
	}

	public bool CallMethod(StringName method, out Variant result, params Variant[] args)
	{
		result = new();

		if (!ValidateMethod(method))
		{
			EmitSignal(SignalName.MethodCalled, method, result, (ushort)MethodStatus.Failed);
			return false;
		}

		result = args.Length == 0 ? Current.Call(method) : Current.Call(method, args);

		_terminal.Print(result.ToString());

		EmitSignal(SignalName.MethodCalled, method, result, (ushort)MethodStatus.Success);

		return true;
	}

	private bool ValidateMethod(StringName method)
	{
		if (!IsInstanceValid(Current))
		{
			_terminal.Output.Error(Messages.DisposedNode);
			return false;
		}

		if (Current.HasMethod(method)) return true;

		_terminal.Output.Error(Messages.UnknownMethod(Current.Name, method));
		return false;
	}
}
