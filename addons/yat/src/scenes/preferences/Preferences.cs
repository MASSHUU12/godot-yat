using System;
using System.Reflection;
using Godot;
using YAT.Resources;

namespace YAT.Scenes;

public partial class Preferences : YatWindow.YatWindow
{
	[Export] YatPreferences YatPreferences { get; set; } = new();

	private TabContainer _tabContainer;

	public override void _Ready()
	{
		base._Ready();

		_tabContainer = GetNode<TabContainer>("%TabContainer");

		CreatePreferences();
	}

	private void CreatePreferences()
	{
		var type = typeof(YatPreferences);
		var properties = YatPreferences.GetPropertyList();

		ExportGroupAttribute currentGroup = null;
		ExportSubgroupAttribute currentSubgroup = null;

		foreach (var propertyInfo in properties)
		{
			var exportGroup = GetAttribute<ExportGroupAttribute>(propertyInfo);
			var exportSubgroup = GetAttribute<ExportSubgroupAttribute>(propertyInfo);

			if (exportGroup is not null)
			{
				currentGroup = exportGroup;
				currentSubgroup = null;

				CreateTab(exportGroup.Name);
			}

			if (exportSubgroup is not null) currentSubgroup = exportSubgroup;

			if (currentGroup is null && currentSubgroup is null) continue;

			GD.Print(currentGroup?.Name, " | ", currentSubgroup?.Name);
		}
	}

	private void CreateTab(StringName name)
	{
		var tab = new PanelContainer();

		_tabContainer.AddChild(tab);
		_tabContainer.SetTabTitle(_tabContainer.GetTabCount() - 1, name);
	}

	private static T GetAttribute<T>(Godot.Collections.Dictionary propertyInfo) where T : Attribute
	{
		if (!propertyInfo.TryGetValue("name", out var name)) return default;

		var type = typeof(YatPreferences);
		var property = type.GetProperty((string)name);

		if (property is null) return default;

		return property.GetCustomAttribute<T>();
	}
}
