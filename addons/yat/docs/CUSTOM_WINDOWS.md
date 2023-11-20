<div align="center">
	<h3>Custom windows</h1>
	<p>Here you can find information on creating custom windows for YAT commands.</p>
</div>

## Custom windows

YAT internally uses the `YatWindow` node to create windows in the overlay.
If you want to create your own windows for your commands, I recommend creating them by inheriting from `YatWindow`.

This way your window will behave similarly to native windows as well as the appearance will be consistent.

Of course, nothing prevents you from creating your own window from scratch and using it when creating your commands.

You can add your window to the overlay this way:

```csharp
Yat.Overlay.AddChild(_yourWindow);
```
