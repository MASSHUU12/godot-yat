using System.Collections.Generic;
using System.Threading;
using YAT.Interfaces;
using YAT.Scenes;

namespace YAT.Types
{
	public sealed record CommandData(
		YAT Yat,
		BaseTerminal Terminal,
		ICommand Command,
		string[] RawData,
		Dictionary<string, object> Arguments,
		Dictionary<string, object> Options,
		CancellationToken CancellationToken
	);
}
