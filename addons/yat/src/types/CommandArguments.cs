using System.Collections.Generic;
using System.Threading;
using YAT.Scenes.BaseTerminal;

namespace YAT.Interfaces
{
	public record CommandArguments(
		YAT Yat,
		BaseTerminal Terminal,
		string CommandName,
		string[] Arguments,
		Dictionary<string, object> ConvertedArgs,
		CancellationToken? CancellationToken
	);
}
