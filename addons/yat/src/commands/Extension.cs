using System.Linq;
using System.Text;
using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;

namespace YAT.Commands
{
	public partial interface IExtension
	{
		public CommandResult Execute(ICommand command, params string[] args);

		/// <summary>
		/// Generates the manual for the extension.
		/// </summary>
		/// <param name="args">The arguments to be passed to the method.</param>
		/// <returns>A string containing the manual for the extension.</returns>
		public virtual string GenerateExtensionManual(params string[] args)
		{
			StringBuilder sb = new();
			ExtensionAttribute attribute = AttributeHelper.GetAttribute<ExtensionAttribute>(this);

			if (string.IsNullOrEmpty(attribute.Manual))
			{
				return $"Extension {attribute.Name} does not have a manual.";
			}

			sb.AppendLine($"[font_size=18]{attribute.Name}[/font_size]");
			sb.AppendLine(attribute.Description);
			sb.AppendLine('\n' + attribute.Manual);
			sb.AppendLine("\n[b]Aliases[/b]:");
			sb.AppendLine(attribute.Aliases.Length > 0
					? string.Join("\n", attribute.Aliases.Select(alias => $"[ul]\t{alias}[/ul]"))
					: "[ul]\tNone[/ul]");

			return sb.ToString();
		}
	}
}
