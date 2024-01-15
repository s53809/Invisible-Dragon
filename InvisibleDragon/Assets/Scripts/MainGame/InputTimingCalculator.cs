using System;

public enum Timing
{
    PERFECT = 42,
    GREAT = 80,
    GOOD = 100,
    BAD = 150,
}
public class InputTimingCalculator
{
    private BeatCalculator m_beatCal;

    public InputTimingCalculator(BeatCalculator beatCal)
    {
        m_beatCal = beatCal;
        m_beatCal.MissTiming += TooLateMissTiming;
    }

    public void InputBeat()
    {
        if (!m_beatCal.isPlaying) return;
    }

    public void TooLateMissTiming()
    {
        throw new NotImplementedException("too late miss");
    }
}
