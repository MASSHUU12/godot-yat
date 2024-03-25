using Godot;
using YAT.Attributes;
using YAT.Classes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Command("$", "Executes a method on the selected node.")]
[Argument("method", "string", "The method to execute.")]
public sealed class Dollar : ICommand
{
#nullable disable
	private BaseTerminal _terminal;
#nullable restore

	public CommandResult Execute(CommandData data)
	{
		_terminal = data.Terminal;

		if (!ValidateInputData(data.RawData[1], out var methods))
			return ICommand.Failure("Invalid method.");

		return MethodChaining(methods)
			? ICommand.Success()
			: ICommand.Failure();
	}

	private void EmitStatus(string methods, Variant result, EMethodStatus status)
	{
		_terminal.EmitSignal(nameof(_terminal.MethodCalled), methods, result, (ushort)status);
	}

	private bool CallMethod(Node node, string method, out Variant result, params Variant[] args)
	{
		result = new();
		var validationResult = node.ValidateMethod(method);

		switch (validationResult)
		{
			case MethodValidationResult.InvalidInstance:
				_terminal.Output.Error(Messages.DisposedNode);
				EmitStatus(method, result, EMethodStatus.Failed);
				return false;
			case MethodValidationResult.InvalidMethod:
				_terminal.Output.Error(Messages.InvalidMethod(method));
				EmitStatus(method, result, EMethodStatus.Failed);
				return false;
		}

		result = args.Length == 0
			? node.CallMethod(method)
			: node.CallMethod(method, args);

		EmitStatus(method, result, EMethodStatus.Success);

		return true;
	}

	private bool MethodChaining(string[] methods)
	{
		Variant result = new();

		foreach (var method in methods)
		{
			var (name, args) = Parser.ParseMethod(method);

			if (result.As<Node>() is { })
			{
				var status = args.Length == 0
					? CallMethod((Node)result, name, out result)
					: CallMethod((Node)result, name, out result, args);

				if (!status) return false;
			}
			else
			{
				var status = args.Length == 0
					? CallMethod(_terminal.SelectedNode.Current, name, out result)
					: CallMethod(_terminal.SelectedNode.Current, name, out result, args);

				if (!status) return false;
			}

			_terminal.Print(result.ToString());
		}

		return true;
	}

	private static bool ValidateInputData(string input, out string[] methods)
	{
		methods = Text.SplitClean(input, ".");

		return !(methods.Length == 0);
	}
}
