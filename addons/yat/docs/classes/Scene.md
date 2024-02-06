<div align="center">
	<h3>Scene</h1>
	<p>A class containing scene-specific methods.</p>
</div>

### Description

**Inherits**: N/A

### Properties

| Type | Name | Default |
| ---- | ---- | ------- |
| -    | -    | -       |

### Methods

| Type                           | Definition                                                                    |
| ------------------------------ | ----------------------------------------------------------------------------- |
| static bool                    | PrintChildren (BaseTerminal terminal, string path)                            |
| static Node                    | GetFromPathOrDefault (string path, Node defaultNode, out string newPath)      |
| static IEnumerator<Dictionary> | GetNodeMethods (Node node)                                                    |
| static bool                    | TryFindNodeMethodInfo (Node node, string methodName, out NodeMethodInfo info) |
| static NodeMethodInfo          | GetNodeMethodInfo (Dictionary method)                                         |
| static MethodValidationResult  | ValidateMethod (this Node node, StringName method)                            |
| static Variant                 | CallMethod (this Node node, StringName method, params Variant[] args)         |

### Signals

**-**

### Enumerations

**-**
