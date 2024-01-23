using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Scenes.BaseTerminal;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

namespace YAT.Commands
{
	[Command("wenv", "Manages the world environment.", "[b]Usage[/b]: wenv [i]action[/i]")]
	[Argument("action", "[remove, restore]", "Removes or restores the world environment.")]
	public sealed class Wenv : ICommand
	{
		private YAT _yat;
		private BaseTerminal _terminal;
		private static Environment _world3DEnvironment;

		public CommandResult Execute(CommandData data)
		{
			var action = (string)data.Arguments["action"];

			_yat = data.Yat;
			_terminal = data.Terminal;

			if (action == "remove") RemoveEnvironment();
			else RestoreEnvironment();

			return CommandResult.Success;
		}

		private void RestoreEnvironment()
		{
			var world3D = _yat.GetTree().Root.World3D;

			if (world3D is null)
			{
				_terminal.Print("No world to restore environment to.", PrintType.Error);
				return;
			}

			if (_world3DEnvironment is null)
			{
				_terminal.Print("No environment to restore.", PrintType.Error);
				return;
			}

			world3D.Environment = _world3DEnvironment;
			_world3DEnvironment = null;
		}

		private void RemoveEnvironment()
		{
			var world3D = _yat.GetTree().Root.World3D;
			var env = world3D?.Environment;

			if (world3D is null)
			{
				_terminal.Print("No world to remove environment from.", PrintType.Error);
				return;
			}

			if (env is null)
			{
				_terminal.Print("No environment to remove.", PrintType.Error);
				return;
			}

			_world3DEnvironment = env;
			world3D.Environment = null;
		}
	}
}
