using System;
using UnityEngine;

public enum TicTac
{
    Tic = 0,
    Tac = 1,
}

public delegate void BeatEventHandler(TicTac tic);

public class BeatCalculator
{
    public event BeatEventHandler PreBeat;
    public event BeatEventHandler MainBeat;

    private StagePattern m_inputedPattern;
    private SongInfo m_inputedSong;

    private Double m_startTime;
    private Double m_lastTickTime;
    private Double m_delayTime;
    private Boolean m_isPlaying = false;
    private Int32 m_curIndex = 0;
    private Int32 m_beatCount = 0;
    private Boolean m_isMainBeat = false;

    private Int32 m_songBPMIndex = 0;
    private Boolean m_canTick = false;

    public BeatCalculator() => InitialSetting(new StagePattern(), new SongInfo());
    public BeatCalculator(StagePattern inputedPattern, SongInfo inputedSong) 
        => InitialSetting(inputedPattern, inputedSong);

    public void InitialSetting(StagePattern inputedPattern, SongInfo inputedSong)
    {
        m_inputedPattern = inputedPattern;
        m_inputedSong = inputedSong;
        m_isPlaying = false;
        m_curIndex = 0;
        m_isMainBeat = false;
        m_songBPMIndex = 0;
    }

    public void StartCalculate()
    {
        m_startTime = AudioSettings.dspTime + m_inputedSong.offset;
        CalculateDelayTime();
        m_lastTickTime = m_startTime - m_delayTime;
        m_isPlaying = true;
    }

    private void CalculateDelayTime()
    {
        m_delayTime = (60 / m_inputedSong.BPM) / (m_inputedPattern.perBeat[m_curIndex].Item2 / 4);
    }

    private void UnPlayingUpdate()
    {

    }

    private void GameEnd()
    {
        
    }

    private void DropBeat(TicTac i)
    {
        if (!m_canTick) return;
        if (m_isMainBeat) MainBeat(i);
        else PreBeat(i);
    }
    private void PlayingUpdate()
    {
        if (AudioSettings.dspTime >= m_lastTickTime + m_delayTime)
        {
            CalculateDelayTime();
            m_lastTickTime = m_lastTickTime + m_delayTime;
            Debug.Log(m_inputedPattern.touchBeat[m_curIndex]);
            if (m_inputedPattern.touchBeat[m_curIndex][m_beatCount] == '1') m_canTick = true;
            else m_canTick = false;
            m_beatCount++;

            if (m_beatCount >= m_inputedPattern.perBeat[m_curIndex].Item1)
            {
                DropBeat(TicTac.Tac);
                if (m_isMainBeat)
                {
                    if (m_curIndex + 1 < m_inputedPattern.perBeat.Count) m_curIndex++;
                    else { m_isPlaying = false; GameEnd(); return; }
                }
                m_isMainBeat = !m_isMainBeat;
                m_beatCount = 0;
            }
            else DropBeat(TicTac.Tic);
        }
    } //#todo : TouchBeat String 만들기

    public void Update()
    {
        UnPlayingUpdate();
        if (!m_isPlaying) return;
        PlayingUpdate();
    }
}