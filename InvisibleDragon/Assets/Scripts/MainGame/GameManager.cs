using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct TimingScoreCount
{
    public Int32 Perfect;
    public Int32 Great;
    public Int32 Good;
    public Int32 Bad;
    public Int32 Miss;

    public Int32 TotalScore;

    public TimingScoreCount(Int32 Perfect, Int32 Great,  Int32 Good, Int32 Bad, Int32 Miss, Int32 TotalScore)
    {
        this.Perfect = Perfect;
        this.Great = Great;
        this.Good = Good;
        this.Bad = Bad;
        this.Miss = Miss;
        this.TotalScore = TotalScore;
    }
}

public enum TimingScore
{
    PERFECT = 1000,
    GREAT = 500,
    GOOD = 300,
    BAD = 100,
    MISS = 0,
}
public class GameManager : MonoBehaviour
{
    [SerializeField] private HallwaySpawner m_hallWay;
    [SerializeField] private StageViewerPresenter m_FloorUI;
    [SerializeField] private GameObject m_lazer;
    [SerializeField] private Animator m_character;
    [SerializeField] private Boolean m_isAutoMode = false;
    [SerializeField] private TimingPresenter m_timingUI;
    [SerializeField] private GameObject m_hitObject;
    [SerializeField] private StageViewerPresenter m_ScoreUI;
    [SerializeField] private LastScorePresenter m_lastScoreUI;
    private PatternFileParser m_patternFileParser;

    private InputTimingCalculator m_inputCal;
    private BeatCalculator m_beatCal;

    private Boolean isSongStart = false;
    private Boolean isGameOver = false;

    private Queue<Transform> m_lazers = new Queue<Transform>();

    private TimingScoreCount m_playScore;

    private void Awake()
    {
        m_hallWay.musicPlayHandler += GameStart;
        m_patternFileParser = new PatternFileParser();
    }

    private void Start()
    {
        m_FloorUI.Bind($"{StageInfoGiver.Ins.CurStage + 1} Floor");
        m_hallWay.EventStart();
    }

    private void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene(0);
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
        m_beatCal.GameEndEvent += GameEnd;

        m_inputCal.PerfectTiming += PerfectTiming;
        m_inputCal.GreatTiming += GreatTiming;
        m_inputCal.GoodTiming += GoodTiming;
        m_inputCal.BadTiming += BadTiming;
        m_inputCal.MissTiming += MissTiming;
        m_inputCal.PlayerInputFunc += PlayerInputFunc;
        m_inputCal.PlayerTimingOutput += Timing;

        m_beatCal.StartCalculate();
        m_playScore = new TimingScoreCount(0, 0, 0, 0, 0, 0);
        isSongStart = true;
    }

    private void PlusScore(Int32 score)
    {
        m_playScore.TotalScore += score;
        m_ScoreUI.Bind(m_playScore.TotalScore.ToString());
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
    public void MissTiming() 
    {
        PlusScore((Int32)TimingScore.MISS);
        m_playScore.Miss++;
    }
    public void PerfectTiming() 
    {
        PlusScore((Int32)TimingScore.PERFECT);
        m_playScore.Perfect++;
    }
    public void GreatTiming() 
    {
        PlusScore((Int32)TimingScore.GREAT);
        m_playScore.Great++;
    }
    public void GoodTiming() 
    {
        PlusScore((Int32)TimingScore.GOOD);
        m_playScore.Good++;
    }
    public void BadTiming()
    {
        PlusScore((Int32)TimingScore.BAD);
        m_playScore.Bad++;
    }
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

    public void GameEnd()
    {
        m_timingUI.Hide();
        m_hallWay.EventStop();
        m_lastScoreUI.Bind(m_playScore);
        isGameOver = true;
    }
}