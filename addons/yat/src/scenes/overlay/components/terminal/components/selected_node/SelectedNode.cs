using Godot;

namespace YAT.Scenes.Overlay.Components.Terminal.Components.SelectedNode
{
	public partial class SelectedNode : Node
	{
		[Signal]
		public delegate void SelectedNodeChangedEventHandler(Node node);
		[Signal]
		public delegate void SelectedNodeChangeFailedEventHandler(RejectionReason reason);

		public Node CurrentNode { get; private set; }

		public enum RejectionReason
		{
			InvalidNode,
			InvalidNodePath
		}

		public override void _Ready()
		{
			CurrentNode = GetTree().Root;
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
				EmitSignal(SignalName.SelectedNodeChangeFailed, (ushort)RejectionReason.InvalidNodePath);
				return false;
			}

			var newSelectedNode = GetNodeOrNull(node);
			if (!IsInstanceValid(newSelectedNode))
			{
				EmitSignal(SignalName.SelectedNodeChangeFailed, (ushort)RejectionReason.InvalidNode);
				return false;
			}

			CurrentNode = newSelectedNode;
			EmitSignal(SignalName.SelectedNodeChanged, CurrentNode);

			return true;
		}
	}
}
