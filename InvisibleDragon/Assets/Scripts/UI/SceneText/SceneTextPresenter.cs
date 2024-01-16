using System;
using UnityEngine.UI;
using DG.Tweening;

public class SceneTextPresenter : PresenterBase<SceneTextView>
{
    public override void Release()
    {
        
    }

    public void Bind(String pText, Single pDuration)
    {
        _view.sceneText.text = "";
        _view.sceneText.DOText(pText, pDuration);
    }
}
