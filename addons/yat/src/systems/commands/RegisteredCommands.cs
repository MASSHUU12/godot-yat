using Godot;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using YAT.Attributes;
using YAT.Helpers;

namespace YAT.Systems
{
	public partial class RegisteredCommands : Node
	{
		public Dictionary<string, Type> Registered { get; private set; } = new();

		private YAT _yat;

		public override void _Ready()
		{
			_yat = GetNode<YAT>("..");
		}

		/// <summary>
		/// Adds a command to the command manager.
		/// </summary>
		/// <param name="commandType">The type of the command to add.</param>
		public void AddCommand(Type commandType)
		{
			var commandInstance = Activator.CreateInstance(commandType);

			if (commandInstance is not ICommand command)
			{
				_yat.CurrentTerminal.Output.Error(
					Messages.UnknownCommand(commandInstance.ToString())
				);
				return;
			}

			if (AttributeHelper.GetAttribute<CommandAttribute>(command)
				is not CommandAttribute attribute)
			{
				_yat.CurrentTerminal.Output.Error(
					Messages.MissingAttribute("CommandAttribute", commandType.Name)
				);
				return;
			}

			Registered[attribute.Name] = commandType;
			foreach (string alias in attribute.Aliases) Registered[alias] = commandType;
		}

		public void AddCommand(params Type[] commands)
		{
			foreach (var command in commands) AddCommand(command);
		}
	}
}
