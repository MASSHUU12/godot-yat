<div align="center">
  <h3>Custom windows</h1>
  <p>Here you can find information on creating custom windows for YAT commands.</p>
</div>

### Custom windows

YAT internally uses the `YatWindow` node to create windows.
If you want to create your own windows for your commands, it's recommended to create them by inheriting from **YatWindow**.

This way, the window will behave similarly to native windows and its appearance will be consistent, plus you will get access to many additional out-of-the-box features.

Any windows should be added to **Yat.Windows** for order and clarity.

You can add your window this way:

```cs
Yat.Windows.AddChild(_yourWindow);
```

### Scaling with the main viewport

The **YatWindow** is able to scale to always contain itself in the main viewport.
To use this functionality, all you need to do is to use **base._Ready()** in the **_Ready** method of your window.

Using the variable **ViewportEdgeOffset** you can adjust how much the window will be smaller than the main viewport.
