using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private HallwaySpawner m_hallWay;
    [SerializeField] private StageViewerPresenter m_FloorUI;
    [SerializeField] private GameObject m_lazer;
    [SerializeField] private Animator m_character;
    [SerializeField] private Boolean m_isAutoMode = false;
    [SerializeField] private TimingPresenter m_timingUI;
    [SerializeField] private GameObject m_hitObject;
    private PatternFileParser m_patternFileParser;

    private InputTimingCalculator m_inputCal;
    private BeatCalculator m_beatCal;

    private Boolean isSongStart = false;

    private Queue<Transform> m_lazers = new Queue<Transform>();

    private void Awake()
    {
        m_hallWay.musicPlayHandler += GameStart;
        m_patternFileParser = new PatternFileParser();
        StageInfoGiver.Ins.StageEnter(0);
    }

    private void Start()
    {
        m_FloorUI.Bind(StageInfoGiver.Ins.CurStage);
    }

    private void Update()
    {
        if (!isSongStart) return;
        if (Input.anyKeyDown && !m_isAutoMode) m_inputCal.InputBeat();
        if (m_isAutoMode) m_inputCal.AutoUpdate();
        m_beatCal.Update();
    }

    private void GameStart()
    {
        StagePattern pattern = m_patternFileParser.FindPatternFile(StageInfoGiver.Ins.CurStage);
        SongInfo songInfo = m_patternFileParser.FindSongInfoFile(StageInfoGiver.Ins.CurStage);

        m_beatCal = new BeatCalculator(pattern, songInfo);
        m_inputCal = new InputTimingCalculator(m_beatCal);

        m_beatCal.PreBeat += PreBeat;
        m_beatCal.MainBeat += MainBeat;
        m_beatCal.BPMBeat += BPMBeat;
        m_beatCal.MissTiming += MissTiming;

        m_inputCal.PerfectTiming += PerfectTiming;
        m_inputCal.GreatTiming += GreatTiming;
        m_inputCal.GoodTiming += GoodTiming;
        m_inputCal.BadTiming += BadTiming;
        m_inputCal.MissTiming += MissTiming;
        m_inputCal.PlayerInputFunc += PlayerInputFunc;
        m_inputCal.PlayerTimingOutput += Timing;

        m_beatCal.StartCalculate();
        isSongStart = true;
    }

    /* Game Direct Function */
    public void PreBeat() 
    {
        SoundManager.Ins.PrintSFX("Toc");
        GameObject obj = Instantiate(m_lazer);
        obj.transform.position = new Vector3(
            UnityEngine.Random.Range(-0.4f, 0.4f),
            UnityEngine.Random.Range(-0.7f, -1.7f),
            0);
        obj.transform.rotation = Quaternion.Euler(
            new Vector3(0, 0, UnityEngine.Random.Range(-20f, 200f))
            );
        m_lazers.Enqueue(obj.transform);
    }
    public void MainBeat() 
    {
        SoundManager.Ins.PrintSFX("GunShot");
        Destroy(m_lazers.Dequeue().gameObject);
    }
    public void BPMBeat() { }
    public void MissTiming() { }
    public void PerfectTiming() 
    {
        
    }
    public void GreatTiming() { }
    public void GoodTiming() { }
    public void BadTiming() { }
    public void PlayerInputFunc() 
    {
        m_character.SetTrigger("isParrying");
        SoundManager.Ins.PrintSFX("Hit");
        GameObject obj = Instantiate(m_hitObject);
        obj.transform.position = new Vector3(
            UnityEngine.Random.Range(-2f, 2f),
            UnityEngine.Random.Range(-3f, 0f),
            0
            );
        obj.transform.DOScale(0, 0.2f).OnComplete(() => { Destroy(obj); });
    }

    public void Timing(Double timing)
    {
        m_timingUI.Bind(timing);
    }
}