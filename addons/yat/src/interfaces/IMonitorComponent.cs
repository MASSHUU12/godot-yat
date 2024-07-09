namespace YAT.Interfaces;

public interface IMonitorComponent
{
    public bool UseColors { get; set; }

    public void Update();
}
