using System.Text;
using Godot;
using YAT.Scenes.BaseTerminal;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

namespace YAT.Helpers
{
	public static class Scene
	{
		/// <summary>
		/// Prints the children of a given node to the specified terminal.
		/// </summary>
		/// <param name="terminal">The terminal to print the children to.</param>
		/// <param name="path">The path of the node whose children should be printed.</param>
		/// <returns><c>true</c> if the children were printed successfully; otherwise, <c>false</c>.</returns>
		public static bool PrintChildren(BaseTerminal terminal, string path)
		{
			Node node = GetFromPathOrDefault(path, terminal.SelectedNode.Current, out path);

			if (node is null)
			{
				terminal.Print($"Node '{path}' does not exist.", PrintType.Error);
				return false;
			}

			var children = node.GetChildren();

			if (children.Count == 0)
			{
				terminal.Print($"Node '{path}' has no children.", PrintType.Warning);
				return true;
			}

			StringBuilder sb = new();
			sb.AppendLine($"Found {children.Count} children of '{node.Name}' node:");

			foreach (Node child in children)
			{
				sb.Append($"[{child.Name}] ({child.GetType().Name}) - {child.GetPath()}");
				sb.AppendLine();
			}

			terminal.Print(sb);

			return true;
		}

		/// <summary>
		/// Retrieves a Node from a given path or returns a default Node if the path is "." or "./".
		/// </summary>
		/// <param name="path">The path to the Node.</param>
		/// <param name="defaultNode">The default Node to return if the path is "." or "./".</param>
		/// <param name="newPath">The updated path of the retrieved Node.</param>
		/// <returns>The retrieved Node if it is a valid instance; otherwise, null.</returns>
		public static Node GetFromPathOrDefault(string path, Node defaultNode, out string newPath)
		{
			Node node = (path == "." || path == "./")
				? defaultNode
				: defaultNode.GetNodeOrNull(path);

			newPath = node?.GetPath();
			newPath = newPath is null ? path : newPath;

			if (GodotObject.IsInstanceValid(node)) return node;

			return null;
		}
	}
}
