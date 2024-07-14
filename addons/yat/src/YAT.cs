using Godot;
using YAT.Helpers;
using YAT.Scenes;
using YAT.Classes.Managers;

namespace YAT;

public partial class YAT : Node
{
#nullable disable
    public Node Windows { get; private set; }
    public YatEnable YatEnable { get; private set; }
    public RegisteredCommands Commands { get; private set; }
    public TerminalManager TerminalManager { get; private set; }
    public PreferencesManager PreferencesManager { get; private set; }
#nullable restore

    public override void _Ready()
    {
        Windows = GetNode<Node>("./Windows");
        YatEnable = GetNode<YatEnable>("./YatEnable");
        Commands = GetNode<RegisteredCommands>("./RegisteredCommands");
        TerminalManager = GetNode<TerminalManager>("./TerminalManager");
        PreferencesManager = GetNode<PreferencesManager>("%PreferencesManager");

        Keybindings.LoadDefaultActions();
    }
}
