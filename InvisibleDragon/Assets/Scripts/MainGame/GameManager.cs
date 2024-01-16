using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public List<Tuple<Int32, Int32>> testPerBeat = new List<Tuple<Int32, Int32>>();

    private BeatCalculator test;
    private InputTimingCalculator timingTest;
    private PatternFileParser parser = new PatternFileParser();

    public GameObject metro;
    public TimingTextPresenter testTimingText;

    public GameObject InputEffector;
    public Boolean isAutoPlay = false;

    private void Start()
    {
        StagePattern TesterPattern;
        TesterPattern = parser.FindPatternFile("Test");
        Debug.Log($"Parsed File {TesterPattern.stageNum}, {TesterPattern.stageName}");

        SongInfo TesterSong = new SongInfo()
        {
            fileName = "",
            BPM = 90,
            offset = 0
        };

        test = new BeatCalculator(TesterPattern, TesterSong);
        timingTest = new InputTimingCalculator(test);

        test.PreBeat += PreTicToc;
        test.MainBeat += MainTicToc;

        timingTest.PerfectTiming += PerfectTiming;
        timingTest.GreatTiming += GreatTiming;
        timingTest.GoodTiming += GoodTiming;
        timingTest.BadTiming += BadTiming;
        timingTest.MissTiming += MissTiming;

        timingTest.PlayerInputFunc += InputEffect;
    }

    private void PreTicToc()
    {
        SoundManager.Ins.PrintSFX("Toc");
        metro.transform.Rotate(new Vector3(0, 0, 30));
    }

    private void MainTicToc()
    {
        SoundManager.Ins.PrintSFX("Toc");
        metro.transform.Rotate(new Vector3(0, 0, 30));
    }

    [InspectorButton("Run Game")]
    public void RunGame()
    {
        test.StartCalculate();
    }

    public void Update()
    {
        test.Update();
        if (Input.anyKeyDown && !isAutoPlay)
        {
            timingTest.InputBeat();
        }
        else if (isAutoPlay)
        {
            timingTest.AutoUpdate();
        }
    }

    public void PerfectTiming()
    {
        metro.transform.DOScale(new Vector3(3, 3, 3), 0.1f).OnComplete(() =>
        {
            metro.transform.DOScale(new Vector3(0.2f, 1, 1), 0.1f);
        });
        testTimingText.Bind("Perfect");
    }
    public void GreatTiming()
    {
        testTimingText.Bind("GREAT");
    }
    public void GoodTiming()
    {
        testTimingText.Bind("Good");
    }
    public void BadTiming()
    {
        testTimingText.Bind("Bad");
    }
    public void MissTiming()
    {
        testTimingText.Bind("Miss");
    }

    public void InputEffect()
    {
        InputEffector.transform.DOScale(new Vector3(3, 3, 3), 0.1f).OnComplete(() =>
        {
            InputEffector.transform.DOScale(new Vector3(0.2f, 1, 1), 0.1f);
        });

        SoundManager.Ins.PrintSFX("Tic");
    }
}
