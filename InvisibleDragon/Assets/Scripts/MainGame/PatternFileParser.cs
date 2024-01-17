using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PatternFileParser
{
    private static readonly String[] m_stageFileNameInfo = new String[4] {
        "FirstStage",
        "SecondStage",
        "ThirdStage",
        "ForthStage"
    };
    public StagePattern FindPatternFile(String fileName)
    {
        String filePath = Path.Combine(Application.streamingAssetsPath, $"Patterns/{fileName}.txt");

        String[] lines = File.ReadAllLines(filePath);

        Int32 stageNum = Int32.Parse(lines[0]);
        String stageName = lines[1];

        var perBeats = new List<Tuple<Int32, Int32>>();
        var touchBeats = new List<String>();

        for (Int32 i = 2; i < lines.Length; i += 2)
        {
            Tuple<Int32, Int32> perBeat = new Tuple<Int32, Int32>(
                Int32.Parse(lines[i].Split('/')[0]),
                Int32.Parse(lines[i].Split('/')[1])
                );
            String touchBeat = lines[i + 1];

            perBeats.Add(perBeat);
            touchBeats.Add(touchBeat);
        }

        StagePattern pattern = new StagePattern()
        {
            stageNum = stageNum,
            stageName = stageName,
            perBeat = perBeats,
            touchBeat = touchBeats
        };

        return pattern;
    }

    public StagePattern FindPatternFile(Int32 index)
    {
        return FindPatternFile(m_stageFileNameInfo[index]);
    }

    public SongInfo FindSongInfoFile(String fileName)
    {
        String filePath = Path.Combine(Application.streamingAssetsPath, $"Songs/{fileName}.txt");

        String[] lines = File.ReadAllLines(filePath);

        String SongName = lines[0];
        Single BPM = Single.Parse(lines[1]);
        Double offset = Double.Parse(lines[2]);

        var perBeats = new List<Tuple<Int32, Int32>>();
        var touchBeats = new List<String>();

        SongInfo songInfo = new SongInfo()
        {
            SongName = SongName,
            BPM = BPM,
            offset = offset
        };

        return songInfo;
    }

    public SongInfo FindSongInfoFile(Int32 index)
    {
        return FindSongInfoFile(m_stageFileNameInfo[index]);
    }
}
