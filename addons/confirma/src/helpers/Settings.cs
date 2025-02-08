using Godot;

namespace Confirma.Helpers;

public static class Settings
{
    public static bool CreateSetting(string path, Variant value)
    {
        if (ProjectSettings.HasSetting(path))
        {
            return false;
        }

        ProjectSettings.SetSetting(path, value);

        return true;
    }

    public static bool CreateSetting(
        string path,
        Variant value,
        Variant initialValue
    )
    {
        if (!CreateSetting(path, value))
        {
            return false;
        }

        ProjectSettings.SetInitialValue(path, initialValue);

        return true;
    }

    public static bool CreateSetting(
        string path,
        Variant value,
        Variant initialValue,
        bool isBasic
    )
    {
        if (!CreateSetting(path, value, initialValue))
        {
            return false;
        }

        ProjectSettings.SetAsBasic(path, isBasic);

        return true;
    }

    public static bool CreateSetting(
        string path,
        Variant value,
        Variant initialValue,
        bool isBasic,
        bool requireRestart
    )
    {
        if (!CreateSetting(path, value, initialValue, isBasic))
        {
            return false;
        }

        ProjectSettings.SetRestartIfChanged(path, requireRestart);

        return true;
    }
}
