using UnityEngine;

namespace BloodLine.Core.Simulation
{
    /// <summary>
    /// A hidden MonoBehavior responsible for pumping Unity's Time.deltaTime into the native C# GameUpdateLoop.
    /// This bridges Unity's engine loop with our deterministic simulation.
    /// </summary>
    public class UpdateLoopRunner : MonoBehaviour
    {
        private IUpdateLoop _updateLoop;

        public void Inject(IUpdateLoop updateLoop)
        {
            _updateLoop = updateLoop;
        }

        private void Update()
        {
            _updateLoop?.Update(Time.deltaTime);
        }
    }
}
