<div align="center">
	<h3>Reflection</h1>
	<p>A static class supporting the use of reflection.</p>
</div>

### Description

**Inherits**: N/A

This class is used to perform reflection actions on objects, e.g., checking whether an object has a given attribute.

### Properties

**-**

### Methods

| Type               | Definition                                                                    |
| ------------------ | ----------------------------------------------------------------------------- |
| static EventInfo[] | GetEvents (this object obj, BindingFlags bindingFlags = BindingFlags.Default) |
| static T           | GetAttribute<T> (this object obj)                                             |
| static T[]         | GetAttributes<T> (this object obj)                                            |
| static bool        | HasAttribute<T> (this object obj)                                             |
| static bool        | HasInterface<T> (this object obj) where T : notnull                           |
| static bool        | HasInterface (Type type, StringName interfaceName)                            |

### Signals

**-**

### Enumerations

**-**
