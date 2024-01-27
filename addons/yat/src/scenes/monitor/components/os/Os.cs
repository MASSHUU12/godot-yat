using Godot;
using YAT.Interfaces;

namespace YAT.Scenes.Monitor;

public partial class Os : PanelContainer, IMonitorComponent
{
	public bool UseColors { get; set; }

	private readonly string _osName = OS.GetName();
	private readonly string _osVersion = OS.GetVersion();
	private readonly string _displayServerName = DisplayServer.GetName();
	private readonly string _locale = OS.GetLocale();
	private string _osInfo;

	private Label _label;

	public override void _Ready()
	{
		_label = GetNode<Label>("Label");

		_osInfo = $"{_osName} {_osVersion} {_locale} {(_osName == _displayServerName
			? string.Empty
			: $"({_displayServerName})")}";

		_label.Text = _osInfo;
	}

	public void Update() { }
}
