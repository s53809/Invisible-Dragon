using UnityEngine.SceneManagement;

public class ElevatorPresenter : PresenterBase<ElevatorView>
{
    public override void Release() { }

    public override void Bind()
    {
        base.Bind();
        _view.FirstFloorButton.onClick.AddListener(PressFirstStageButton);
        _view.ForthFloorButton.onClick.AddListener(PressForthStageButton);
    }

    public void PressFirstStageButton()
    {
        if (StageInfoGiver.Ins.StageEnter(0))
        {
            SoundManager.Ins.StopCurBGM();
            SceneManager.LoadScene(1);
        }
    }
    public void PressSecondStageButton()
    {
        if (StageInfoGiver.Ins.StageEnter(1))
        {
            SoundManager.Ins.StopCurBGM();
            SceneManager.LoadScene(1);
        }
    }
    public void PressThirdStageButton()
    {
        if (StageInfoGiver.Ins.StageEnter(2))
        {
            SoundManager.Ins.StopCurBGM();
            SceneManager.LoadScene(1);
        }
    }
    public void PressForthStageButton()
    {
        if (StageInfoGiver.Ins.StageEnter(3))
        {
            SoundManager.Ins.StopCurBGM();
            SceneManager.LoadScene(1);
        }
    }
}
