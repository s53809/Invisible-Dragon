using DG.Tweening;
using UnityEngine;

public class SkipInfoMessagePresenter : PresenterBase<SkipInfoMessageView>
{
    public override void Release()
    {
        
    }

    public override void Bind()
    {
        base.Bind();
        _view.GetComponent<CanvasGroup>().alpha = 0.0f;
        _view.GetComponent<CanvasGroup>().DOFade(1.0f, 3.0f);
    }
}
