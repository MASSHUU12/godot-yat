using Godot;

namespace YAT
{
	public partial class YatOptions : Resource
	{
		[ExportGroup("Terminal")]
		[Export] public string Prompt { get; set; } = ">";
		[Export] public bool ShowPrompt { get; set; } = true;
		[Export] public uint HistoryLimit { get; set; } = 25;
		[Export] public bool AutoFocus { get; set; } = true;
		[Export] public bool AutoScroll { get; set; } = true;
		[Export] public bool AutoComplete { get; set; } = true;
		[Export] public bool AutoCompleteOnTab { get; set; } = true;
		[Export] public bool AutoCompleteOnEnter { get; set; } = true;
		[Export] public uint DefaultWidth { get; set; } = 720;
		[Export] public uint DefaultHeight { get; set; } = 320;

		[ExportGroup("Colors")]
		[Export] public Color BackgroundColor { get; set; } = new("1d2229");
		[Export] public Color InputColor { get; set; } = new("15191fc0");
		[Export] public Color OutputColor { get; set; } = new("dfdfdf80");
		[Export] public Color ErrorColor { get; set; } = new("ff786b");
		[Export] public Color WarningColor { get; set; } = new("ffdd65");
		[Export] public Color SuccessColor { get; set; } = new("a5ff8a");

		[ExportGroup("Window")]
		[Export] public bool WindowResizable { get; set; } = true;
		[Export] public bool WindowMovable { get; set; } = true;
		[Export] public uint MinWidth { get; set; } = 256;
		[Export] public uint MinHeight { get; set; } = 256;

		[ExportGroup("Other")]
		[ExportSubgroup("YatEnableFile")]
		[Export] public bool UseYatEnableFile { get; set; } = false;
		[Export] public string YatEnableFile { get; set; } = ".yatenable";
		[Export] public YatEnableFileLocation YatEnableLocation { get; set; } = YatEnableFileLocation.UserData;

		public enum YatEnableFileLocation
		{
			UserData,
			CurrentDirectory
		}
	}
}
