using System.Collections.Generic;
using System.Linq;
using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Scenes.Monitor;
using YAT.Scenes.Terminal;

namespace YAT.Commands
{
	[Command("stats", "Manages the game monitor.", "[b]Usage:[/b] stats", "st")]
	[Option("-fps", null, "Shows the FPS in the game monitor.", false)]
	[Option("-os", null, "Shows the OS information in the game monitor.", false)]
	[Option("-cpu", null, "Shows the CPU information in the game monitor.", false)]
	[Option("-ram", null, "Shows the RAM information in the game monitor.", false)]
	public sealed class Stats : ICommand
	{
		public YAT Yat { get; set; }

		private static Monitor _monitorInstance;
		private static readonly PackedScene _monitor = GD.Load<PackedScene>(
			"res://addons/yat/src/scenes/monitor/Monitor.tscn"
		);

		private const string _componentsPath = "res://addons/yat/src/scenes/monitor/components/";

		public Stats(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(Dictionary<string, object> cArgs, params string[] args)
		{
			if (cArgs.Any((arg) => (bool)arg.Value)) return Open(cArgs);
			return Close();
		}

		private CommandResult Open(Dictionary<string, object> cArgs)
		{
			bool fps = (bool)cArgs["-fps"];
			bool os = (bool)cArgs["-os"];
			bool cpu = (bool)cArgs["-cpu"];
			bool ram = (bool)cArgs["-ram"];

			List<Node> components = new();
			bool instanceValid = GodotObject.IsInstanceValid(_monitorInstance);

			if (instanceValid)
			{
				_monitorInstance.QueueFree();
				_monitorInstance = null;
			}

			if (fps) components.Add(GD.Load<PackedScene>(_componentsPath + "fps/Fps.tscn").Instantiate<Fps>());
			if (ram) components.Add(GD.Load<PackedScene>(_componentsPath + "memory_info/MemoryInfo.tscn").Instantiate<MemoryInfo>());
			if (os) components.Add(GD.Load<PackedScene>(_componentsPath + "os/Os.tscn").Instantiate<Os>());
			if (cpu) components.Add(GD.Load<PackedScene>(_componentsPath + "cpu_info/CpuInfo.tscn").Instantiate<CpuInfo>());

			_monitorInstance = _monitor.Instantiate<Monitor>();

			Yat.Windows.AddChild(_monitorInstance);

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
				Yat.Terminal.Print("The game monitor is not open.", Terminal.PrintType.Error);
				return CommandResult.Failure;
			}

			_monitorInstance.QueueFree();
			_monitorInstance = null;

			return CommandResult.Success;
		}
	}
}
