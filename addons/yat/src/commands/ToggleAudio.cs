using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command(
	"toggleaudio",
	"Toggles audio on or off.",
	"[b]Usage[/b]: toggleaudio"
)]
[Option("-id", "int(0:32767)", "The ID of the audio bus to toggle. If not provided, all buses will be toggled.", -1)]
[Option("-name", "string", "The name of the audio bus to toggle. If not provided, all buses will be toggled.", null)]
public sealed class ToggleAudio : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		var id = (int)data.Options["-id"];
		var name = (string)data.Options["-name"];

		if (id == -1 && string.IsNullOrEmpty(name))
		{
			ToggleAll();
			return CommandResult.Success;
		}

		if (id != -1)
		{
			if (!ToggleById(id))
			{
				data.Terminal.Print("Invalid audio bus ID.", EPrintType.Error);
				return CommandResult.InvalidArguments;
			}
		}

		if (!string.IsNullOrEmpty(name))
		{
			if (!ToggleByName(name))
			{
				data.Terminal.Print("Invalid audio bus name.", EPrintType.Error);
				return CommandResult.InvalidArguments;
			}
		}

		return CommandResult.Success;
	}

	private static void ToggleAll()
	{
		for (var i = 0; i < AudioServer.BusCount; i++)
			AudioServer.SetBusMute(i, !AudioServer.IsBusMute(i));
	}

	private static bool ToggleById(int id)
	{
		if (id < 0 || id >= AudioServer.BusCount) return false;

		AudioServer.SetBusMute(id, !AudioServer.IsBusMute(id));

		return true;
	}

	private static bool ToggleByName(string name)
	{
		var id = AudioServer.GetBusIndex(name);

		if (id == -1) return false;

		AudioServer.SetBusMute(id, !AudioServer.IsBusMute(id));

		return true;
	}
}
