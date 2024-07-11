using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;
using YAT.Attributes;
using YAT.Classes.Managers;
using YAT.Enums;
using YAT.Helpers;
using YAT.Resources;

namespace YAT.Scenes;

public partial class Preferences : YatWindow
{
#nullable disable
    private TabContainer _tabContainer;
    private Button _load, _save, _update, _restoreDefaults;
    private PackedScene _section, _inputContainer, _preferencesTab;
#nullable restore

    private readonly Dictionary<StringName, PreferencesTab> _groups = new();
    private readonly Dictionary<StringName, PreferencesSection> _sections = new();

    public override void _Ready()
    {
        base._Ready();

        _yat.PreferencesManager.PreferencesUpdated += UpdateDisplayedPreferences;
        _tabContainer = GetNode<TabContainer>("%TabContainer");

        _section = GD.Load<PackedScene>("uid://o78tqt867i13");
        _preferencesTab = GD.Load<PackedScene>("uid://bxdeasqh565nr");
        _inputContainer = GD.Load<PackedScene>("uid://dgq3jncmxdomf");

        _load = GetNode<Button>("%Load");
        _load.Pressed += LoadPreferences;

        _save = GetNode<Button>("%Save");
        _save.Pressed += SavePreferences;

        _update = GetNode<Button>("%Update");
        _update.Pressed += () => UpdatePreferences();

        _restoreDefaults = GetNode<Button>("%RestoreDefaults");
        _restoreDefaults.Pressed += () =>
        {
            _yat.PreferencesManager.RestoreDefaults();
            _yat.CurrentTerminal.Print(
                "Preferences restored to default values.",
                EPrintType.Success
            );
        };

        CloseRequested += QueueFree;

        CreatePreferences();
    }

    private void SavePreferences()
    {
        UpdatePreferences();
        bool status = _yat.PreferencesManager.Save();

        _yat.CurrentTerminal.Print(
            status
                ? "Preferences saved successfully."
                : "Failed to save preferences.",
            status
                ? EPrintType.Success
                : EPrintType.Error
        );
    }

    private void LoadPreferences()
    {
        bool status = _yat.PreferencesManager.Load();

        _yat.CurrentTerminal.Print(
            status
                ? "Preferences loaded successfully."
                : "Failed to load preferences.",
            status
                ? EPrintType.Success
                : EPrintType.Error
        );
    }

    private void UpdatePreferences()
    {
        CallOnEveryPreference((InputContainer container) =>
        {
            PreferencesManager manager = _yat.PreferencesManager;
            manager.Preferences.Set(container.Text, container.GetValue());

            return true;
        });

        _yat.CurrentTerminal.Print("Preferences updated successfully.", EPrintType.Success);
        _ = _yat.PreferencesManager.EmitSignal(
            nameof(_yat.PreferencesManager.PreferencesUpdated),
            _yat.PreferencesManager.Preferences
        );
    }

    private void UpdateDisplayedPreferences(YatPreferences preferences)
    {
        CallOnEveryPreference((InputContainer container) =>
        {
            container.SetValue(preferences.Get(container.Text));

            return true;
        });
    }

    private void CallOnEveryPreference(Func<InputContainer, bool> func)
    {
        foreach (StringName key in _groups.Keys)
        {
            foreach (Node? child in _groups[key].Container.GetChildren())
            {
                if (child is InputContainer container)
                {
                    _ = func(container);
                }
            }
        }
    }

    private void CreatePreferences()
    {
        var properties = _yat.PreferencesManager.Preferences.GetPropertyList();

        ExportGroupAttribute? currentGroup = null;
        ExportSubgroupAttribute? currentSubgroup = null;

        foreach (var propertyInfo in properties)
        {
            if (GetAttribute<IgnoreAttribute>(propertyInfo) is not null)
            {
                continue;
            }

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

                CreateSection(currentSubgroup.Name, currentGroup!.Name);
            }

            if (currentGroup is null && currentSubgroup is null)
            {
                continue;
            }

            CreateInputContainer(currentGroup!.Name, propertyInfo);
        }
    }

    private void CreateSection(StringName name, StringName groupName)
    {
        var section = _section.Instantiate<PreferencesSection>();

        section.Name = name;

        _sections[name] = section;
        _groups[groupName].Container.AddChild(section);

        section.Title.Text = name;
    }

    private void CreateInputContainer(StringName groupName, Godot.Collections.Dictionary info)
    {
        string propertyType = ((Variant.Type)(int)info["type"]).ToString();
        StringName name = info.TryGetValue("name", out var n)
            ? (StringName)n
            : string.Empty;
        EInputType inputType = (EInputType)(Enum.TryParse(typeof(EInputType), propertyType, out var parsedType)
            ? parsedType
            : EInputType.String
        );
        PropertyHint hint = info.TryGetValue("hint", out var h)
            ? (PropertyHint)(short)h
            : PropertyHint.None;
        var (min, max, _) = hint == PropertyHint.Range
            ? Scene.GetRangeFromHint(info["hint_string"].AsString())
            : (0, float.MaxValue, 0);

        if (string.IsNullOrEmpty(name) || _groups.ContainsKey(name) || propertyType == "Nil")
        {
            return;
        }

        InputContainer inputContainer = _inputContainer.Instantiate<InputContainer>();

        inputContainer.Name = name;
        inputContainer.Text = name;
        inputContainer.InputType = inputType;
        inputContainer.MinValue = min;
        inputContainer.MaxValue = max;

        _groups[groupName].Container.AddChild(inputContainer);

        inputContainer.SetValue(_yat.PreferencesManager.Preferences.Get(name));
    }

    private void CreateTab(StringName name)
    {
        PreferencesTab tab = _preferencesTab.Instantiate<PreferencesTab>();

        tab.Name = name;

        _groups[name] = tab;
        _tabContainer.AddChild(tab);
        _tabContainer.SetTabTitle(_tabContainer.GetTabCount() - 1, name);
    }

    private static T? GetAttribute<T>(Godot.Collections.Dictionary propertyInfo) where T : Attribute
    {
        if (!propertyInfo.TryGetValue("name", out var name))
        {
            return null;
        }

        PropertyInfo? property = typeof(YatPreferences).GetProperty((string)name);

        return property?.GetCustomAttribute<T>();
    }
}
