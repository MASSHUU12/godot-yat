using System.Collections.Generic;
using Godot;

namespace YAT.Scenes.GameTerminal.Components
{
	public partial class TerminalSwitcher : PanelContainer
	{
		public const ushort MAX_TERMINAL_INSTANCES = 3;
		public List<BaseTerminal.BaseTerminal> TerminalInstances = new();
		public BaseTerminal.BaseTerminal CurrentTerminal;

		private Button _add;
		private TabBar _tabBar;
		private PanelContainer _instancesContainer;
		private BaseTerminal.BaseTerminal _initialTerminal;

		public override void _Ready()
		{
			_add = GetNode<Button>("%Add");
			_add.Pressed += AddTerminal;

			_tabBar = GetNode<TabBar>("%TabBar");
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
		}
	}
}
