namespace SoVariableTool.Tick
{
    public interface ITickable
    {
        void Tick();
        
        void StartTick();
        void StopTick();
    }
}