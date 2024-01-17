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
        //#todo : ����� Json���ϰ� �����Ͽ� ���������� �����ִٸ� true�� ��ȯ�ϰ� �ƴϸ� false�� ��ȯ�Ѵ�
        CurStage = stage;
        return true;
    }
}
