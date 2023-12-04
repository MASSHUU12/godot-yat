using Godot;
using YAT.Helpers;

namespace YAT.Scenes.Overlay.Components.Terminal.Components.SelectedNode
{
	public partial class SelectedNode : Node
	{
		[Signal]
		public delegate void CurrentNodeChangedEventHandler(Node node);
		[Signal]
		public delegate void CurrentNodeChangeFailedEventHandler(RejectionReason reason);
		[Signal]
		public delegate void MethodCalledEventHandler(string method, Variant returnValue, MethodStatus status);

		public Node CurrentNode { get; private set; }

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

		private Terminal _terminal;

		public override void _Ready()
		{
			_terminal = GetNode<Terminal>("../");
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
				EmitSignal(SignalName.CurrentNodeChangeFailed, (ushort)RejectionReason.InvalidNodePath);
				return false;
			}

			var newSelectedNode = GetNodeOrNull(node);
			if (!IsInstanceValid(newSelectedNode))
			{
				EmitSignal(SignalName.CurrentNodeChangeFailed, (ushort)RejectionReason.InvalidNode);
				return false;
			}

			CurrentNode = newSelectedNode;
			EmitSignal(SignalName.CurrentNodeChanged, CurrentNode);

			return true;
		}

		/// <summary>
		/// Calls a method on the current node and prints the result to the terminal.
		/// </summary>
		/// <param name="method">The name of the method to call.</param>
		/// <param name="args">The arguments to pass to the method.</param>
		/// <returns>True if the method was called successfully, false otherwise.</returns>
		public bool CallMethod(string method, params Variant[] args)
		{
			if (!CurrentNode.HasMethod(method))
			{
				LogHelper.UnknownMethod(CurrentNode.Name, method);
				EmitSignal(SignalName.MethodCalled, method, new(), (ushort)MethodStatus.Failed);
				return false;
			}

			var result = CurrentNode.Call(method, args);
			_terminal.Print(result.ToString());

			EmitSignal(SignalName.MethodCalled, method, result, (ushort)MethodStatus.Success);

			return true;
		}
	}
}
