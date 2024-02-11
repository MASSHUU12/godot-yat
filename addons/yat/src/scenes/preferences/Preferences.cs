using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;
using YAT.Enums;
using YAT.Resources;

namespace YAT.Scenes;

public partial class Preferences : YatWindow.YatWindow
{
	[Export] YatPreferences YatPreferences { get; set; } = new();

	private TabContainer _tabContainer;

	private readonly Dictionary<StringName, PreferencesTab> _groups = new();

	public override void _Ready()
	{
		base._Ready();

		_tabContainer = GetNode<TabContainer>("%TabContainer");

		CreatePreferences();
	}

	private void CreatePreferences()
	{
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

				CreateTab(currentGroup.Name);
			}

			if (exportSubgroup is not null) currentSubgroup = exportSubgroup;

			if (currentGroup is null && currentSubgroup is null) continue;

			// TODO: Ranges
			// TODO: Subgroups
			// TODO: Prevent getting next category as a preference

			if (!_groups.ContainsKey(currentGroup.Name)) continue;

			CreateInputContainer(currentGroup.Name, propertyInfo);
		}
	}

	private void CreateInputContainer(StringName groupName, Godot.Collections.Dictionary info)
	{
		var propertyType = ((Variant.Type)(int)info["type"]).ToString();
		StringName name = info.TryGetValue("name", out var n)
			? (StringName)n
			: string.Empty;
		var inputType = (EInputType)(Enum.TryParse(typeof(EInputType), propertyType, out var parsedType)
			? parsedType
			: EInputType.String
		);

		if (string.IsNullOrEmpty(name) || _groups.ContainsKey(name)) return;

		var inputContainer = GD.Load<PackedScene>("uid://dgq3jncmxdomf").Instantiate<InputContainer>();

		inputContainer.Name = name;
		inputContainer.Text = name;
		inputContainer.InputType = inputType;
		inputContainer.MinValue = 0;
		inputContainer.MaxValue = uint.MaxValue;

		_groups[groupName].Container.AddChild(inputContainer);

		inputContainer.SetValue(YatPreferences.Get(name));
	}

	private void CreateTab(StringName name)
	{
		var tab = GD.Load<PackedScene>("uid://bxdeasqh565nr").Instantiate<PreferencesTab>();

		tab.Name = name;

		_groups[name] = tab;
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
