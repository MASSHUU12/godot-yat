using Godot;
using YAT.Helpers;
using YAT.Interfaces;

namespace YAT.Scenes.PerformanceMonitor
{
	public partial class MemoryInfo : PanelContainer, IPerformanceMonitorComponent
	{
		public bool UseColors { get; set; }

		private YAT _yat;
		private RichTextLabel _label;

		public override void _Ready()
		{
			_yat = GetNode<YAT>("/root/YAT");
			_label = GetNode<RichTextLabel>("RichTextLabel");
		}

		public void Update()
		{
			var mem = OS.GetMemoryInfo();
			var physical = mem["physical"];
			var free = mem["free"];
			var stack = mem["stack"];

			var freePercent = (float)free / (float)physical * 100f;

			_label.Clear();
			_label.AppendText(
				$"Memory: {NumericHelper.SizeToString(physical.AsInt64(), 3)}\n" +
				$"Free: {(
					freePercent < 15
					? $"[color={_yat.Options.ErrorColor}]{NumericHelper.SizeToString(free.AsInt64(), 3)}[/color]"
					: NumericHelper.SizeToString(free.AsInt64(), 3))}\n" +
				$"Stack: {NumericHelper.SizeToString(stack.AsInt64(), 3)}"
			);
		}
	}
}
