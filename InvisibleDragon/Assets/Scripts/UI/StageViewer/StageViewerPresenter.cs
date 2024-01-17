using DG.Tweening;
using System;

public class StageViewerPresenter : PresenterBase<StageViewerView>
{
    public override void Release()
    {
        
    }

    public void Bind(Int32 stageIndex)
    {
        base.Bind();
        _view.FloorText.DOText($"{stageIndex + 1} Floor",1f);
    }
}
