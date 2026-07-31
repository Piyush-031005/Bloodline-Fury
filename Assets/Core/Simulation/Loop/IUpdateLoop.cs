namespace BloodLine.Core.Simulation
{
    /// <summary>
    /// Contract for the core simulation update loop.
    /// Responsible for fixed timestep deterministic ticks independent of Unity's frame rate.
    /// </summary>
    public interface IUpdateLoop
    {
        event System.Action OnTick;
        void Initialize(int targetTickRate);
        void Update(float deltaTime);
    }
}
