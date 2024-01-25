using Godot;
using System;
using System.Collections.Generic;
using YAT.Attributes;
using YAT.Commands;
using YAT.Helpers;
using YAT.Interfaces;

namespace YAT.Scenes
{
	public partial class RegisteredCommands : Node
	{
		public Dictionary<string, Type> Registered { get; private set; } = new();

		private YAT _yat;

		public override void _Ready()
		{
			_yat = GetNode<YAT>("..");

			RegisterBuiltinCOmmands();
		}

		public enum AddingResult
		{
			Success,
			UnknownCommand,
			MissingAttribute
		}

		/// <summary>
		/// Adds a command to the command manager.
		/// </summary>
		/// <param name="commandType">The type of the command to add.</param>
		public AddingResult AddCommand(Type commandType)
		{
			var commandInstance = Activator.CreateInstance(commandType);

			if (commandInstance is not ICommand command)
				return AddingResult.UnknownCommand;

			if (AttributeHelper.GetAttribute<CommandAttribute>(command)
				is not CommandAttribute attribute)
				return AddingResult.MissingAttribute;

			Registered[attribute.Name] = commandType;
			foreach (string alias in attribute.Aliases) Registered[alias] = commandType;

			return AddingResult.Success;
		}

		public AddingResult[] AddCommand(params Type[] commands)
		{
			AddingResult[] results = new AddingResult[commands.Length];

			for (int i = 0; i < commands.Length; i++)
				results[i] = AddCommand(commands[i]);

			return results;
		}

		private void RegisterBuiltinCOmmands()
		{
			AddingResult[] results = AddCommand(new[] {
				typeof(Ls),
				typeof(Ip),
				typeof(Cn),
				typeof(Cs),
				typeof(Cls),
				typeof(Man),
				typeof(Set),
				typeof(Cat),
				typeof(Sys),
				typeof(Quit),
				typeof(Echo),
				typeof(List),
				typeof(View),
				typeof(Ping),
				typeof(Wenv),
				typeof(Pause),
				typeof(Watch),
				typeof(Stats),
				typeof(Reset),
				typeof(Cowsay),
				// typeof(Options),
				typeof(Restart),
				typeof(History),
				typeof(Whereami),
				typeof(Timescale),
				typeof(ToggleAudio),
				typeof(QuickCommands)
			});

			for (int i = 0; i < results.Length; i++)
			{
				switch (results[i])
				{
					case AddingResult.UnknownCommand:
						_yat.CurrentTerminal.Output.Error(
							Messages.UnknownCommand(results[i].ToString())
						);
						break;
					case AddingResult.MissingAttribute:
						_yat.CurrentTerminal.Output.Error(
							Messages.MissingAttribute("CommandAttribute", results[i].ToString())
						);
						break;
					default:
						break;
				}
			}
		}
	}
}
