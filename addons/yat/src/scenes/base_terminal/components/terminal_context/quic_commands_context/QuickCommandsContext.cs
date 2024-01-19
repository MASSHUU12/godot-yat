using Godot;
using YAT.Helpers;
using YAT.Scenes.ContextMenu;

namespace YAT.Scenes.BaseTerminal
{
	public partial class QuickCommandsContext : ContextSubmenu
	{
		[Export]
		public QuickCommands QuickCommands { get; set; } = new();

		private YAT _yat;
		private const ushort _maxQuickCommands = 10;
		private const string _optionsPath = "user://yat_qc.tres";

		public override void _Ready()
		{
			base._Ready();

			_yat = GetNode<YAT>("/root/YAT");

			IdPressed += OnQuickCommandsPressed;
			GetQuickCommands();
		}

		/// <summary>
		/// Adds a quick command to the terminal context.
		/// </summary>
		/// <param name="name">The name of the quick command.</param>
		/// <param name="command">The command associated with the quick command.</param>
		/// <returns>True if the quick command was successfully added, false otherwise.</returns>
		public bool AddQuickCommand(string name, string command)
		{
			if (QuickCommands.Commands.ContainsKey(name) ||
				QuickCommands.Commands.Count >= _maxQuickCommands
			) return false;

			QuickCommands.Commands.Add(name, command);

			var status = StorageHelper.SaveResource(QuickCommands, _optionsPath);

			GetQuickCommands();

			return status;
		}

		/// <summary>
		/// Removes a quick command from the terminal context.
		/// </summary>
		/// <param name="name">The name of the quick command to remove.</param>
		/// <returns>True if the quick command was successfully removed, false otherwise.</returns>
		public bool RemoveQuickCommand(string name)
		{
			if (!QuickCommands.Commands.ContainsKey(name)) return false;

			QuickCommands.Commands.Remove(name);

			var status = StorageHelper.SaveResource(QuickCommands, _optionsPath);

			GetQuickCommands();

			return status;
		}

		/// <summary>
		/// Retrieves the quick commands and adds them to the list of quick commands.
		/// </summary>
		private void GetQuickCommands()
		{
			var qc = StorageHelper.LoadResource<QuickCommands>(_optionsPath);

			if (qc != null) QuickCommands = qc;

			Clear();

			foreach (var command in QuickCommands.Commands)
			{
				AddItem(command.Key);
			}
		}

		/// <summary>
		/// Event handler for when a quick command is pressed.
		/// </summary>
		/// <param name="id">The ID of the pressed command.</param>
		private void OnQuickCommandsPressed(long id)
		{
			var key = GetItemText((int)id);

			if (!QuickCommands.Commands.TryGetValue(key, out var command))
			{
				return;
			}

			_yat.CommandManager.Run(Text.SanitizeText(command), _yat.Terminal);
		}
	}
}
