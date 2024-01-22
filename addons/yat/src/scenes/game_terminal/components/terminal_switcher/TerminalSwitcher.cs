using System.Collections.Generic;
using Godot;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

namespace YAT.Scenes.GameTerminal.Components
{
	public partial class TerminalSwitcher : PanelContainer
	{
		[Signal] public delegate void CurrentTerminalChangedEventHandler(BaseTerminal.BaseTerminal terminal);

		public const ushort MAX_TERMINAL_INSTANCES = 3;
		public List<BaseTerminal.BaseTerminal> TerminalInstances = new();
		public BaseTerminal.BaseTerminal CurrentTerminal;

		private YAT _yat;
		private Button _add;
		private TabBar _tabBar;
		private PanelContainer _instancesContainer;
		private BaseTerminal.BaseTerminal _initialTerminal;

		public override void _Ready()
		{
			_yat = GetNode<YAT>("/root/YAT");
			_yat.CurrentTerminal = _initialTerminal;

			_add = GetNode<Button>("%Add");
			_add.Pressed += AddTerminal;

			_tabBar = GetNode<TabBar>("%TabBar");
			_tabBar.TabChanged += SwitchToTerminal;

			_instancesContainer = GetNode<PanelContainer>("%InstancesContainer");
			_initialTerminal = GetNode<BaseTerminal.BaseTerminal>("%BaseTerminal");

			TerminalInstances.Add(_initialTerminal);
			CurrentTerminal = _initialTerminal;
		}

		private void AddTerminal()
		{
			if (TerminalInstances.Count >= MAX_TERMINAL_INSTANCES) return;

			var newTerminal = GD.Load<PackedScene>("uid://dfig0yknmx6b7").Instantiate<BaseTerminal.BaseTerminal>();

			TerminalInstances.Add(newTerminal);
			_instancesContainer.AddChild(newTerminal);
			_tabBar.AddTab(newTerminal.Name);

			newTerminal.Print("Please note that support for multiple terminals is still experimental and does not work perfectly with threaded commands.", PrintType.Warning);
		}

		private void SwitchToTerminal(long index)
		{
			CurrentTerminal.Hide();
			CurrentTerminal.Input.ReleaseFocus();
			CurrentTerminal.SetProcessInput(false);

			CurrentTerminal = TerminalInstances[(int)index];

			CurrentTerminal.Show();
			CurrentTerminal.Input.GrabFocus();
			CurrentTerminal.SetProcessInput(true);

			EmitSignal(SignalName.CurrentTerminalChanged, CurrentTerminal);
		}
	}
}
