using System.Collections.Generic;
using System.Threading;
using YAT.Scenes.BaseTerminal;

namespace YAT.Interfaces
{
	public sealed record CommandData(
		YAT Yat,
		BaseTerminal Terminal,
		ICommand Command,
		string[] RawData,
		Dictionary<string, object> Arguments,
		Dictionary<string, object> Options,
		CancellationToken? CancellationToken
	);
}
