using System;
using System.Collections.Generic;
using UnityEngine;

public enum Timing
{
    PERFECT = 42,
    GREAT = 80,
    GOOD = 100,
    BAD = 150,
    MISS = 200,
}

public delegate void TimingEvent();
public delegate void PlayerInput();
public delegate void TimingOutput(Double timing);
public class InputTimingCalculator
{
    public event TimingEvent PerfectTiming;
    public event TimingEvent GreatTiming;
    public event TimingEvent GoodTiming;
    public event TimingEvent BadTiming;
    public event TimingEvent MissTiming;
    public event PlayerInput PlayerInputFunc;
    public event TimingOutput PlayerTimingOutput;

    private BeatCalculator m_beatCal;
    private Queue<Double> pCurBeat;

    public void AutoUpdate()
    {
        if(pCurBeat.Count != 0 && (Math.Abs(AudioSettings.dspTime - pCurBeat.Peek()) * 1000) <= 30)
            InputBeat();
    }

    public InputTimingCalculator(BeatCalculator beatCal)
    {
        m_beatCal = beatCal;
        m_beatCal.MissTiming += TooLateMissTiming;
        pCurBeat = m_beatCal.curBeat;

    }

    public void InputBeat()
    {
        if (!m_beatCal.isPlaying) return;
        if (PlayerInputFunc != null) PlayerInputFunc();
        if (pCurBeat.Count == 0) return;
        if ((pCurBeat.Peek() - AudioSettings.dspTime) * 1000 > (Double)Timing.MISS) //너무빨리침(무시)
            return;

        Double playerTiming = (AudioSettings.dspTime - pCurBeat.Peek()) * 1000;
        if(PlayerTimingOutput != null) PlayerTimingOutput(playerTiming);
        Debug.Log(playerTiming);

        if (Math.Abs(playerTiming) <= (Double)Timing.PERFECT)
        { PerfectTiming(); }
        else if (Math.Abs(playerTiming) <= (Double)Timing.GREAT)
        { GreatTiming(); }
        else if (Math.Abs(playerTiming) <= (Double)Timing.GOOD)
        { GoodTiming(); }
        else if (Math.Abs(playerTiming) <= (Double)Timing.BAD)
        { BadTiming(); }
        else
        { MissTiming(); }

        pCurBeat.Dequeue();
    }

    public void TooLateMissTiming()
    {
        MissTiming();
    }
}
