using System.Collections.Generic;
using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Scenes.PerformanceMonitor;
using YAT.Scenes.Terminal;

namespace YAT.Commands
{
	[Command("monitor", "Manages the performance monitor.", "[b]Usage:[/b] monitor", "mn")]
	[Argument("action", "[open, close]", "Opens or closes the performance monitor.")]
	[Option("-fps", null, "Shows the FPS in the performance monitor.", false)]
	[Option("-os", null, "Shows the OS information in the performance monitor.", false)]
	public sealed class Monitor : ICommand
	{
		public YAT Yat { get; set; }

		private static PerformanceMonitor _monitorInstance;
		private static readonly PackedScene _monitor = GD.Load<PackedScene>(
			"res://addons/yat/src/scenes/performance_monitor/PerformanceMonitor.tscn"
		);

		public Monitor(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(Dictionary<string, object> cArgs, params string[] args)
		{
			string action = (string)cArgs["action"];

			switch (action)
			{
				case "open":
					return Open(cArgs);
				case "close":
					return Close();
				default:
					Yat.Terminal.Print("Invalid action.", Terminal.PrintType.Error);
					return CommandResult.Failure;
			}
		}

		private CommandResult Open(Dictionary<string, object> cArgs)
		{
			bool fps = (bool)cArgs["-fps"];
			bool os = (bool)cArgs["-os"];

			List<Node> components = new();
			bool instanceValid = GodotObject.IsInstanceValid(_monitorInstance);

			if (instanceValid)
			{
				_monitorInstance.QueueFree();
				_monitorInstance = null;
			}

			if (fps) components.Add(GD.Load<PackedScene>(
				"res://addons/yat/src/scenes/performance_monitor/components/fps/Fps.tscn"
			).Instantiate<Fps>());

			if (os) components.Add(GD.Load<PackedScene>(
				"res://addons/yat/src/scenes/performance_monitor/components/os/Os.tscn"
			).Instantiate<Os>());

			if (components.Count == 0)
			{
				Yat.Terminal.Print("No components were selected.", Terminal.PrintType.Error);
				return CommandResult.Failure;
			}

			_monitorInstance = _monitor.Instantiate<PerformanceMonitor>();

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
				Yat.Terminal.Print("The performance monitor is not open.", Terminal.PrintType.Error);
				return CommandResult.Failure;
			}

			_monitorInstance.QueueFree();
			_monitorInstance = null;

			return CommandResult.Success;
		}
	}
}
