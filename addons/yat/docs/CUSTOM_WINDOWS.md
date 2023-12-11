<div align="center">
	<h3>Custom windows</h1>
	<p>Here you can find information on creating custom windows for YAT commands.</p>
</div>

### Custom windows

YAT internally uses the `YatWindow` node to create windows.
If you want to create your own windows for your commands, I recommend creating them by inheriting from `YatWindow`.

This way, the window will behave similarly to native windows and its appearance will be consistent, plus you will get access to many additional out-of-the-box features.

Any windows should be added to `Yat.Windows` for order and clarity.

You can add your window this way:

```csharp
Yat.Windows.AddChild(_yourWindow);
```
