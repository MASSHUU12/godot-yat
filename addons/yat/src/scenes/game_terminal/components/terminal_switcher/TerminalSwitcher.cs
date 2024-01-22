using System.Collections.Generic;
using Godot;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

namespace YAT.Scenes.GameTerminal.Components
{
	public partial class TerminalSwitcher : PanelContainer
	{
		[Signal] public delegate void TerminalSwitcherInitializedEventHandler();
		[Signal] public delegate void TerminalAddedEventHandler(BaseTerminal.BaseTerminal terminal);
		[Signal] public delegate void TerminalRemovedEventHandler(BaseTerminal.BaseTerminal terminal);
		[Signal] public delegate void CurrentTerminalChangedEventHandler(BaseTerminal.BaseTerminal terminal);

		public const ushort MAX_TERMINAL_INSTANCES = 5;
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
			_tabBar.TabClosePressed += RemoveTerminal;

			_instancesContainer = GetNode<PanelContainer>("%InstancesContainer");
			_initialTerminal = GetNode<BaseTerminal.BaseTerminal>("%BaseTerminal");

			TerminalInstances.Add(_initialTerminal);
			CurrentTerminal = _initialTerminal;

			UpdateTabBarVisibility();

			EmitSignal(SignalName.CurrentTerminalChanged, CurrentTerminal);
			EmitSignal(SignalName.TerminalSwitcherInitialized);
		}

		private void AddTerminal()
		{
			if (TerminalInstances.Count >= MAX_TERMINAL_INSTANCES) return;

			var newTerminal = GD.Load<PackedScene>("uid://dfig0yknmx6b7").Instantiate<BaseTerminal.BaseTerminal>();
			newTerminal.Name = $"Terminal {TerminalInstances.Count + 1}";

			TerminalInstances.Add(newTerminal);
			_instancesContainer.AddChild(newTerminal);
			_tabBar.AddTab(newTerminal.Name);

			newTerminal.Print("Please note that support for multiple terminals is still experimental and does not work perfectly with threaded commands.", PrintType.Warning);

			SwitchToTerminal(TerminalInstances.Count - 1);

			UpdateTabBarVisibility();

			EmitSignal(SignalName.TerminalAdded, newTerminal);
		}

		private void RemoveTerminal(long index)
		{
			if (TerminalInstances.Count <= 1) return;

			var terminal = TerminalInstances[(int)index];

			if (terminal.Locked)
			{
				terminal.Print("This terminal is currently executing a command and cannot be closed.", PrintType.Error);
				return;
			}

			_tabBar.RemoveTab((int)index);
			TerminalInstances.Remove(terminal);
			terminal.QueueFree();

			if (CurrentTerminal == terminal) SwitchToTerminal(0);

			UpdateTabBarVisibility();

			EmitSignal(SignalName.TerminalRemoved, terminal);
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

			_tabBar.CurrentTab = (int)index;

			EmitSignal(SignalName.CurrentTerminalChanged, CurrentTerminal);
		}

		private void UpdateTabBarVisibility()
		{
			_tabBar.VisibilityLayer = TerminalInstances.Count > 1 ? 1u : 0u;
		}
	}
}
