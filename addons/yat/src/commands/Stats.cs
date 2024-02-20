using System.Collections.Generic;
using System.Linq;
using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Command("stats", "Manages the game monitor.", "[b]Usage:[/b] stats", "st")]
[Option("-all", "bool", "Shows all the information in the game monitor.", false)]
[Option("-fps", "bool", "Shows the FPS in the game monitor.", false)]
[Option("-os", "bool", "Shows the OS information in the game monitor.", false)]
[Option("-cpu", "bool", "Shows the CPU information in the game monitor.", false)]
[Option("-mem", "bool", "Shows memory information in the game monitor.", false)]
[Option("-engine", "bool", "Shows the engine information in the game monitor.", false)]
[Option("-objects", "bool", "Shows the objects information in the game monitor.", false)]
[Option("-lookingat", "bool", "Shows the info about node the player is looking at. Only in 3D.", false)]
public sealed class Stats : ICommand
{
	private YAT _yat;
	private BaseTerminal _terminal;
	private static Monitor _monitorInstance;
	private static readonly PackedScene _monitor = GD.Load<PackedScene>(
		"uid://dekp8nra5yo6u"
	);

	public CommandResult Execute(CommandData data)
	{
		_yat = data.Yat;
		_terminal = data.Terminal;

		if (data.Options.Any((arg) => (bool)arg.Value)) return Open(data.Options);
		return Close();
	}

	private CommandResult Open(Dictionary<StringName, object> opts)
	{
		bool all = (bool)opts["-all"];
		bool fps = (bool)opts["-fps"];
		bool os = (bool)opts["-os"];
		bool cpu = (bool)opts["-cpu"];
		bool mem = (bool)opts["-mem"];
		bool engine = (bool)opts["-engine"];
		bool objects = (bool)opts["-objects"];
		bool lookingAt = (bool)opts["-lookingat"];

		List<Node> components = new();
		bool instanceValid = GodotObject.IsInstanceValid(_monitorInstance);

		if (instanceValid)
		{
			_monitorInstance.QueueFree();
			_monitorInstance = null;
		}

		if (all || fps) components.Add(GD.Load<PackedScene>("uid://coabiqpqlqj55").Instantiate<Fps>());
		if (all || mem) components.Add(GD.Load<PackedScene>("uid://cy3kq15d8pc2k").Instantiate<MemoryInfo>());
		if (all || os) components.Add(GD.Load<PackedScene>("uid://dpg2hilncaxuv").Instantiate<Os>());
		if (all || cpu) components.Add(GD.Load<PackedScene>("uid://bvjj8sj77c6wi").Instantiate<CpuInfo>());
		if (all || engine) components.Add(GD.Load<PackedScene>("uid://cjd2m6hgh0hmq").Instantiate<EngineInfo>());
		if (all || objects) components.Add(GD.Load<PackedScene>("uid://co0tw21o78ucy").Instantiate<SceneObjects>());
		if (all || lookingAt) components.Add(GD.Load<PackedScene>("uid://b4na0nuvbhst6").Instantiate<LookingAt>());

		_monitorInstance = _monitor.Instantiate<Monitor>();
		_yat.Windows.AddChild(_monitorInstance);

		foreach (Node component in components) _monitorInstance.AddComponent(component);

		return ICommand.Success();
	}

	private CommandResult Close()
	{
		if (!GodotObject.IsInstanceValid(_monitorInstance))
			return ICommand.Failure("The game monitor is not open.");

		_monitorInstance.QueueFree();
		_monitorInstance = null;

		return ICommand.Success();
	}
}
