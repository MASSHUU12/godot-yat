using System.Collections.Generic;
using System.Threading;
using YAT.Scenes.BaseTerminal;

namespace YAT.Interfaces
{
	public sealed record CommandData(
		YAT Yat,
		BaseTerminal Terminal,
		ICommand Command,
		string CommandName,
		string[] Arguments,
		Dictionary<string, object> ConvertedArgs,
		CancellationToken? CancellationToken
	);
}
