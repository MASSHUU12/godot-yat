using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;
using YAT.Enums;
using YAT.Helpers;
using YAT.Resources;

namespace YAT.Scenes;

public partial class Preferences : YatWindow.YatWindow
{
	[Export] YatPreferences YatPreferences { get; set; } = new();

	private TabContainer _tabContainer;

	private readonly Dictionary<StringName, PreferencesTab> _groups = new();
	private readonly Dictionary<StringName, PreferencesSection> _sections = new();

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

			if (exportSubgroup is not null)
			{
				currentSubgroup = exportSubgroup;

				CreateSection(currentSubgroup.Name, currentGroup.Name);
			}

			if (currentGroup is null && currentSubgroup is null) continue;

			CreateInputContainer(currentGroup.Name, propertyInfo);
		}
	}

	private void CreateSection(StringName name, StringName groupName)
	{
		var section = GD.Load<PackedScene>("uid://o78tqt867i13").Instantiate<PreferencesSection>();

		section.Name = name;

		_sections[name] = section;
		_groups[groupName].Container.AddChild(section);

		section.Title.Text = name;
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
		var hint = info.TryGetValue("hint", out var h)
			? (PropertyHint)(short)h
			: PropertyHint.None;
		var (min, max, _) = hint == PropertyHint.Range
			? Scene.GetRangeFromHint(info["hint_string"].AsString())
			: (0, float.MaxValue, 0);

		if (string.IsNullOrEmpty(name) || _groups.ContainsKey(name) || propertyType == "Nil")
			return;

		var inputContainer = GD.Load<PackedScene>("uid://dgq3jncmxdomf").Instantiate<InputContainer>();

		inputContainer.Name = name;
		inputContainer.Text = name;
		inputContainer.InputType = inputType;
		inputContainer.MinValue = min;
		inputContainer.MaxValue = max;

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
