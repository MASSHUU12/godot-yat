using System.Collections.Generic;
using System.Linq;
using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Scenes.Monitor;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

namespace YAT.Commands
{
	[Command("stats", "Manages the game monitor.", "[b]Usage:[/b] stats", "st")]
	[Option("-fps", null, "Shows the FPS in the game monitor.", false)]
	[Option("-os", null, "Shows the OS information in the game monitor.", false)]
	[Option("-cpu", null, "Shows the CPU information in the game monitor.", false)]
	[Option("-mem", null, "Shows memory information in the game monitor.", false)]
	[Option("-engine", null, "Shows the engine information in the game monitor.", false)]
	[Option("-objects", null, "Shows the objects information in the game monitor.", false)]
	[Option("-lookingat", null, "Shows the info about node the player is looking at. Only in 3D.", false)]
	public sealed class Stats : ICommand
	{
		private YAT _yat;
		private static Monitor _monitorInstance;
		private static readonly PackedScene _monitor = GD.Load<PackedScene>(
			"uid://dekp8nra5yo6u"
		);

		public CommandResult Execute(CommandData data)
		{
			_yat = data.Yat;

			if (data.Arguments.Any((arg) => (bool)arg.Value)) return Open(data.Arguments);
			return Close();
		}

		private CommandResult Open(Dictionary<string, object> args)
		{
			bool fps = (bool)args["-fps"];
			bool os = (bool)args["-os"];
			bool cpu = (bool)args["-cpu"];
			bool mem = (bool)args["-mem"];
			bool engine = (bool)args["-engine"];
			bool objects = (bool)args["-objects"];
			bool lookingAt = (bool)args["-lookingat"];

			List<Node> components = new();
			bool instanceValid = GodotObject.IsInstanceValid(_monitorInstance);

			if (instanceValid)
			{
				_monitorInstance.QueueFree();
				_monitorInstance = null;
			}

			if (fps) components.Add(GD.Load<PackedScene>("uid://coabiqpqlqj55").Instantiate<Fps>());
			if (mem) components.Add(GD.Load<PackedScene>("uid://cy3kq15d8pc2k").Instantiate<MemoryInfo>());
			if (os) components.Add(GD.Load<PackedScene>("uid://dpg2hilncaxuv").Instantiate<Os>());
			if (cpu) components.Add(GD.Load<PackedScene>("uid://bvjj8sj77c6wi").Instantiate<CpuInfo>());
			if (engine) components.Add(GD.Load<PackedScene>("uid://cjd2m6hgh0hmq").Instantiate<EngineInfo>());
			if (objects) components.Add(GD.Load<PackedScene>("uid://co0tw21o78ucy").Instantiate<SceneObjects>());
			if (lookingAt) components.Add(GD.Load<PackedScene>("uid://b4na0nuvbhst6").Instantiate<LookingAt>());

			_monitorInstance = _monitor.Instantiate<Monitor>();

			_yat.Windows.AddChild(_monitorInstance);

			foreach (Node component in components)
			{
				_monitorInstance.AddComponent(component);
			}

			return CommandResult.Success;
		}

		private CommandResult Close()
		{
			if (!GodotObject.IsInstanceValid(_monitorInstance))
			{
				_yat.Terminal.Print("The game monitor is not open.", PrintType.Error);
				return CommandResult.Failure;
			}

			_monitorInstance.QueueFree();
			_monitorInstance = null;

			return CommandResult.Success;
		}
	}
}
