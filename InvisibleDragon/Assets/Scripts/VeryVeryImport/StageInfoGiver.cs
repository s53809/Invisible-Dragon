using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageInfoGiver : MonoSingleton<StageInfoGiver>
{
    public Int32 CurStage { get; private set; }

    public Boolean StageEnter(Int32 stage)
    {
        //#todo : 저장된 Json파일과 대조하여 스테이지가 열려있다면 true를 반환하고 아니면 false를 반환한다
        CurStage = stage;
        return true;
    }
}
