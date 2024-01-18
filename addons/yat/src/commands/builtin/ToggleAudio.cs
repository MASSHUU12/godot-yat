using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command(
		"toggleaudio",
		"Toggles audio on or off.",
		"[b]Usage[/b]: toggleaudio"
	)]
	public sealed class ToggleAudio : ICommand
	{
		public CommandResult Execute(CommandData data)
		{
			for (var i = 0; i < AudioServer.BusCount; i++)
				AudioServer.SetBusMute(i, !AudioServer.IsBusMute(i));

			return CommandResult.Success;
		}
	}
}
