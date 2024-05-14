using Biofeedback;

namespace Common.BFB
{
    public sealed class GameTimer
    {
        public double GameTime { get; private set; } = 0.0d;
        public double UnusedTime => _unusedTime;

        private double _unusedTime;
        private double _lastTimeSeek = 0.0d;


        public GameTimer()
        {
            ResetTime();
        }

        public void ResetTime()
        {
            _lastTimeSeek = TimeUtils.TimeSek;
            _unusedTime = 0;
            GameTime = 0;
        }

        public void GameTimerStart()
        {
            _lastTimeSeek = TimeUtils.TimeSek;
            _unusedTime = _lastTimeSeek;
            GameTime = 0;
        }
        public void UpdateGameTimer(bool inGameTime)
        {
            double timeNow = TimeUtils.TimeSek;

            if (inGameTime)
            {
                GameTime =  timeNow - _unusedTime;
            }
            else
            {
                _unusedTime += timeNow - _lastTimeSeek;
            }
            _lastTimeSeek = timeNow;
        }
    }
}