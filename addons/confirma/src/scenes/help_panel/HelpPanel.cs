using Confirma.Classes;
using Confirma.Helpers;
using Godot;

namespace Confirma.Scenes;

public partial class HelpPanel : Control
{
    public override async void _Ready()
    {
        Log.RichOutput = GetNode<RichTextLabel>("%Output");
        Log.RichOutput.MetaClicked += OnMetaClicked;

        ConfirmaAutoload autoload = GetNodeOrNull<ConfirmaAutoload>("/root/Confirma");

        bool success = await Help.ShowHelpPage(autoload.Props.SelectedHelpPage);
        if (!success)
        {
            GetTree().Quit(1);
            return;
        }

        if (Log.IsHeadless)
        {
            GetTree().Quit();
        }
    }

    public void OnMetaClicked(Variant meta)
    {
        switch(meta.VariantType)
        {
            case Variant.Type.String:
                OS.ShellOpen((string)meta);
                break;
            //place for future expansion
        }
    }
}
