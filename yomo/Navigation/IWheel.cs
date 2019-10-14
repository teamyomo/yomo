namespace yomo.Navigation
{
    public interface IWheel
    {
        uint MinDuty { get; }
        uint MaxDuty { get; }
        double Speed { get; }

        void SetSpeed(int speed);
    }
}