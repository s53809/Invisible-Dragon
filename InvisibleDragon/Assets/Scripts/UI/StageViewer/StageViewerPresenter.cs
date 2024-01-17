using DG.Tweening;
using System;

public class StageViewerPresenter : PresenterBase<StageViewerView>
{
    public override void Release()
    {
        
    }

    public void Bind(String text)
    {
        base.Bind();
        _view.FloorText.DOText(text ,0.1f);
    }
}
