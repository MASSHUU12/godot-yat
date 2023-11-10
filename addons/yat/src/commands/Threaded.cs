namespace YAT.Commands
{
	[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
	public partial class ThreadedAttribute : System.Attribute
	{
		const bool THREADED = true;
	}
}
