using Godot;
using YAT.Interfaces;

namespace YAT.Scenes;

public partial class OsInfoItem : PanelContainer, IDebugScreenItem
{
	public string Title { get; set; } = "OS";

	private Label _label;
	private string _osInfo;

	private readonly string _osName = OS.GetName();
	private readonly string _locale = OS.GetLocale();
	private readonly string _osVersion = OS.GetVersion();
	private readonly string _displayServerName = DisplayServer.GetName();

	public override void _Ready()
	{
		_osInfo = $"{_osName} {_osVersion} {_locale} {(
			_osName == _displayServerName
			? string.Empty
			: $"({_displayServerName})")}";

		_label = GetNode<Label>("Label");
		_label.Text = _osInfo;
	}

	public void Update() { }
}
