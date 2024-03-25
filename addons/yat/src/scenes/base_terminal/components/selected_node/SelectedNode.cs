using Godot;
using YAT.Enums;

namespace YAT.Scenes;

public partial class SelectedNode : Node
{
	[Signal]
	public delegate void CurrentNodeChangedEventHandler(Node node);
	[Signal]
	public delegate void CurrentNodeChangeFailedEventHandler(ERejectionReason reason);

#nullable disable
	public Node Current { get; private set; }

	private BaseTerminal _terminal;
#nullable restore

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
			EmitSignal(SignalName.CurrentNodeChangeFailed,
				(ushort)ERejectionReason.InvalidNodePath
			);
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
			EmitSignal(SignalName.CurrentNodeChangeFailed,
				(ushort)ERejectionReason.InvalidNode
			);
			return false;
		}

		Current = newSelectedNode;
		EmitSignal(SignalName.CurrentNodeChanged, Current);

		return true;
	}
}
