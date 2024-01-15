using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Tuple<Int32, Int32>> testPerBeat = new List<Tuple<Int32, Int32>>();
    public List<String> testTouchBeat = new List<String>();
    public AudioSource audioSource;
    public AudioClip Tic, Tac;

    private BeatCalculator test;

    private void Start()
    {
        for(Int32 i = 0; i < 10; i++)
        {
            testPerBeat.Add(new Tuple<Int32, Int32>(7, 8));
        }

        audioSource = GetComponent<AudioSource>();

        SongInfo TesterSong = new SongInfo()
        {
            fileName = "",
            BPM = 180,
            offset = 0
        };
        
        StagePattern TesterPattern = new StagePattern()
        {
            stageNum = 0,
            stageName = "TestStage",
            perBeat = testPerBeat,
            touchBeat = testTouchBeat
        };

        test = new BeatCalculator(TesterPattern, TesterSong);

        test.PreBeat += OutputTicToc;
        test.MainBeat += OutputTicToc;
    }

    private void OutputTicToc(TicTac i)
    {
        if (i == TicTac.Tic) audioSource.clip = Tic;
        else audioSource.clip = Tac;
        audioSource.Play();
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
