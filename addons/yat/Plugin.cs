#if TOOLS
using Godot;

namespace YAT
{
    [Tool]
    public partial class Plugin : EditorPlugin
    {
        private string _version = string.Empty;
        private const string _name = "YAT";

        public override void _EnterTree()
        {
            _version = GetPluginVersion();

            AddAutoloadSingleton(_name, GetPluginPath() + "/src/YAT.tscn");

            GD.Print($"{_name} {_version} loaded!");
            GD.PrintRich("Up to date information about YAT can be found at [url=https://github.com/MASSHUU12/godot-yat/tree/main]https://github.com/MASSHUU12/godot-yat/tree/main[/url].");
        }

        public override void _ExitTree()
        {
            RemoveAutoloadSingleton(_name);

            GD.Print($"{_name} {_version} unloaded!");
        }

        private string GetPluginPath()
        {
            return GetScript().As<Script>().ResourcePath.GetBaseDir();
        }
    }
}
#endif
