namespace YAT.Helpers
{
	public static class Messages
	{
		public static string UnknownCommand(string command) => $"{command} is not a valid command.";
		public static string UnknownMethod(string something, string method) => $"{something} does not have a method named {method}.";

		public static string MissingInterface(string something, string @interface) => $"{something} does not implement {@interface}.";
		public static string MissingArguments(string command, params string[] args) => $"{command} expected {string.Join(", ", args)} to be provided.";
		public static string MissingAttribute(string something, string attribute) => $"{something} is missing the {attribute} attribute.";
		public static string MissingValue(string command, string option) => $"{command} expected {option} to be provided.";

		public static string InvalidNodePath(string path) => $"{path} is not a valid node path.";
		public static string InvalidArgument(string command, string argument, string expected) => $"{command} expected {argument} to be an {expected}.";

		public static string NodeHasNoChildren(string path) => $"{path} has no children.";
	}
}
