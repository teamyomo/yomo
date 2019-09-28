namespace yomo.Navigation
{
    public interface IWheel
    {
        uint DutyRange { get; }
        uint MaxDuty { get; }
        double Speed { get; }

        void SetSpeed(int speed);
    }
}