using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("toggleaudio", "Toggles audio on or off.")]
[Option("-id", "int(0:32767)", "The ID of the audio bus to toggle.", -1)]
[Option("-name", "string", "The name of the audio bus to toggle.")]
public sealed class ToggleAudio : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        var id = (int)data.Options["-id"];
        var name = (string)data.Options["-name"];

        if (id == -1 && string.IsNullOrEmpty(name))
        {
            ToggleAll();
            return ICommand.Success();
        }

        if (id != -1 && !ToggleById(id))
        {
            return ICommand.InvalidArguments("Invalid audio bus ID.");
        }

        return !string.IsNullOrEmpty(name) && !ToggleByName(name)
            ? ICommand.InvalidArguments("Invalid audio bus name.")
            : ICommand.Success();
    }

    private static void ToggleAll()
    {
        for (int i = 0; i < AudioServer.BusCount; i++)
        {
            AudioServer.SetBusMute(i, !AudioServer.IsBusMute(i));
        }
    }

    private static bool ToggleById(int id)
    {
        if (id < 0 || id >= AudioServer.BusCount)
        {
            return false;
        }

        AudioServer.SetBusMute(id, !AudioServer.IsBusMute(id));

        return true;
    }

    private static bool ToggleByName(string name)
    {
        int id = AudioServer.GetBusIndex(name);

        if (id == -1)
        {
            return false;
        }

        AudioServer.SetBusMute(id, !AudioServer.IsBusMute(id));

        return true;
    }
}
