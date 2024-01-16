using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void BeatEventHandler();
public delegate void MissTimingHandler();

public class BeatCalculator
{
    public event BeatEventHandler PreBeat;
    public event BeatEventHandler MainBeat;

    public event BeatEventHandler BPMBeat;

    public event MissTimingHandler MissTiming;

    private StagePattern m_inputedPattern;
    private SongInfo m_inputedSong;

    private Double m_startTime;
    private Double m_lastTickTime;
    private Double m_delayTime;
    private Int32 m_curIndex = 0;
    private Int32 m_beatCount = 0;
    private Boolean m_isMainBeat = false;
    
    public Boolean isPlaying = false;
    public Queue<Double> curBeat = new Queue<Double>();

    private Double m_lastBPMTickTime;

    public BeatCalculator() => InitialSetting(new StagePattern(), new SongInfo());
    public BeatCalculator(StagePattern inputedPattern, SongInfo inputedSong) 
        => InitialSetting(inputedPattern, inputedSong);

    public void InitialSetting(StagePattern inputedPattern, SongInfo inputedSong)
    {
        m_inputedPattern = inputedPattern;
        m_inputedSong = inputedSong;
        isPlaying = false;
        m_curIndex = 0;
        m_isMainBeat = false;
    }

    public void StartCalculate()
    {
        m_startTime = AudioSettings.dspTime + m_inputedSong.offset;
        CalculateDelayTime();
        m_lastTickTime = m_startTime - m_delayTime;
        m_lastBPMTickTime = m_startTime - m_delayTime;
        isPlaying = true;
    }

    private void CalculateDelayTime()
    {
        m_delayTime = (60 / m_inputedSong.BPM) 
            / (m_inputedPattern.perBeat[m_curIndex].Item2 / 4);
    }

    private void DropBeat()
    {
        if (m_isMainBeat) MainBeat();
        else PreBeat();
    }
    private void PlayingUpdate()
    {
        ManagePlayerInputNote();
        if (AudioSettings.dspTime >= m_lastTickTime + m_delayTime)
        {
            m_lastTickTime = m_lastTickTime + m_delayTime;
            if (m_inputedPattern.touchBeat[m_curIndex][m_beatCount] == '1')
            {
                DropBeat();

                //mainBeat Timing calculate
                if (!m_isMainBeat) curBeat.Enqueue(m_lastTickTime +
                    ((60 / m_inputedSong.BPM) * (m_inputedPattern.perBeat[m_curIndex].Item1
                    / (m_inputedPattern.perBeat[m_curIndex].Item2 / 4))));
            }
            CalculateDelayTime();
            m_beatCount++;

            if (m_beatCount >= m_inputedPattern.perBeat[m_curIndex].Item1)
            {
                if (m_isMainBeat)
                {
                    if (m_curIndex + 1 < m_inputedPattern.perBeat.Count) m_curIndex++;
                    else { isPlaying = false; GameEnd(); return; }
                }
                m_isMainBeat = !m_isMainBeat;
                m_beatCount = 0;
            }
        }
    }

    private void BPMUpdate()
    {
        if(AudioSettings.dspTime >= m_lastBPMTickTime + (60 / m_inputedSong.BPM))
        {
            if(BPMBeat != null) BPMBeat();
            m_lastBPMTickTime = m_lastBPMTickTime + (60 / m_inputedSong.BPM);
        }
    }

    private void ManagePlayerInputNote()
    {
        if (curBeat.Count == 0) return;
        while((AudioSettings.dspTime - curBeat.Peek()) * 1000 > (Double)Timing.MISS)
        {
            curBeat.Dequeue();
            MissTiming();
        }
    }

    public void Update()
    {
        UnPlayingUpdate();
        if (!isPlaying) return;
        PlayingUpdate();
        BPMUpdate();
    }
    private void UnPlayingUpdate()
    {

    }

    private void GameEnd()
    {

    }
}