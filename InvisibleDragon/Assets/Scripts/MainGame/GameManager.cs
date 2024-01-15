using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Tuple<Int32, Int32>> testPerBeat = new List<Tuple<Int32, Int32>>();

    private BeatCalculator test;
    private PatternFileParser parser = new PatternFileParser();

    public GameObject metro;

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

        test.PreBeat += PreTicToc;
        test.MainBeat += MainTicToc;
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
    }
}
