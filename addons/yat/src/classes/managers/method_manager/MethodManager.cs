using Godot;
using YAT.Enums;
using YAT.Helpers;
using YAT.Scenes;

namespace YAT.Classes.Managers;

public partial class MethodManager : Node
{
	[Signal]
	public delegate void MethodCalledEventHandler(
		StringName method, Variant returnValue, EMethodStatus status
	);

	[Export] public BaseTerminal Terminal { get; set; }
	[Export] public SelectedNode SelectedNode { get; set; }

	public bool CallMethods(string input)
	{
		string[] methods = Text.SplitClean(input, ".");

		if (methods.Length == 0) return false;

		if (methods.Length == 1)
		{
			var (name, args) = Parser.ParseMethod(methods[0]);

			if (args.Length == 0
				? !CallMethod(SelectedNode.Current, name, out var result)
				: !CallMethod(SelectedNode.Current, name, out result, args)) return false;

			Terminal.Print(result.ToString());
		}
		else if (!MethodChaining(methods)) return false;

		return true;
	}

	private bool CallMethod(Node node, string method, out Variant result, params Variant[] args)
	{
		result = new();
		var validationResult = node.ValidateMethod(method);

		switch (validationResult)
		{
			case MethodValidationResult.InvalidInstance:
				Terminal.Output.Error(Messages.DisposedNode);
				EmitSignal(SignalName.MethodCalled, method, result, (ushort)EMethodStatus.Failed);
				return false;
			case MethodValidationResult.InvalidMethod:
				Terminal.Output.Error(Messages.InvalidMethod(method));
				EmitSignal(SignalName.MethodCalled, method, result, (ushort)EMethodStatus.Failed);
				return false;
		}

		result = args.Length == 0
			? node.CallMethod(method)
			: node.CallMethod(method, args);

		EmitSignal(SignalName.MethodCalled, method, result, (ushort)EMethodStatus.Success);

		return true;
	}

	private bool MethodChaining(string[] methods)
	{
		Variant result = new();

		foreach (string method in methods)
		{
			var (name, args) = Parser.ParseMethod(method);

			if (result.As<Node>() is { })
			{
				if (args.Length == 0
					? !CallMethod((Node)result, name, out result)
					: !CallMethod((Node)result, name, out result, args)) return false;
			}
			else
			{
				if (args.Length == 0
					? !CallMethod(SelectedNode.Current, name, out result)
					: !CallMethod(SelectedNode.Current, name, out result, args)) return false;
			}

			Terminal.Print(result.ToString());
		}

		return true;
	}
}
