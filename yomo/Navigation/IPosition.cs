using System;

namespace yomo.Navigation
{
    public interface IPosition
    {
        void LoopReadPosition(Action<Position.PositionRecord> onPosition);
    }
}