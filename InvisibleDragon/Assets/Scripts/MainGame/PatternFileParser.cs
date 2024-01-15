using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PatternFileParser
{
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
}
