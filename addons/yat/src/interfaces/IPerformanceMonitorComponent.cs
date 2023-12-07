namespace YAT.Interfaces
{
	public interface IPerformanceMonitorComponent
	{
		public bool UseColors { get; set; }

		public void Update();
	}
}
