using Godot;
using YAT.Helpers;

namespace YAT.Scenes.BaseTerminal
{
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

			var newSelectedNode = Current.GetNodeOrNull(node);
			newSelectedNode ??= GetNodeOrNull(node);

			if (!IsInstanceValid(newSelectedNode))
			{
				EmitSignal(SignalName.CurrentNodeChangeFailed, (ushort)RejectionReason.InvalidNode);
				return false;
			}

			Current = newSelectedNode;
			EmitSignal(SignalName.CurrentNodeChanged, Current);

			return true;
		}

		public bool ParseAndCallMethods(string input)
		{
			_terminal.Print("Please keep in mind that this feature is still in development.\nMany things may not work as expected.\n", Terminal.PrintType.Warning);

			string[] tokens = Text.SplitClean(input, ".");
			Variant result = new();

			if (tokens.Length == 0) return false;

			// TODO: Method chaining

			foreach (string token in tokens)
			{
				var parts = token.Split("(", 2,
					System.StringSplitOptions.RemoveEmptyEntries |
					System.StringSplitOptions.TrimEntries
				);
				string name = parts[0];
				string args = parts.Length > 1 ? parts[1][..^1] : null;
				args = args == string.Empty ? null : args;

				if (args is null
					? !CallMethod(name, out result)
					: !CallMethod(name, out result, args)
				) return false;
			}

			return true;
		}

		/// <summary>
		/// Calls a method on the current node and prints the result to the terminal.
		/// </summary>
		/// <param name="method">The name of the method to call.</param>
		/// <param name="result">The result of the method call.</param>
		/// <param name="args">The arguments to pass to the method.</param>
		/// <returns>True if the method was called successfully, false otherwise.</returns>
		public bool CallMethod(string method, out Variant result, params Variant[] args)
		{
			result = new();

			if (!ValidateMethod(method)) return false;

			result = args.Length == 0
				? Current.Call(method)
				: Current.Call(method, args);

			_terminal.Print(result.ToString());

			EmitSignal(SignalName.MethodCalled, method, result, (ushort)MethodStatus.Success);

			return true;
		}

		private bool ValidateMethod(string method)
		{
			if (!Current.HasMethod(method))
			{
				LogHelper.UnknownMethod(Current.Name, method);
				EmitSignal(SignalName.MethodCalled, method, new(), (ushort)MethodStatus.Failed);
				return false;
			}

			return true;
		}
	}
}
