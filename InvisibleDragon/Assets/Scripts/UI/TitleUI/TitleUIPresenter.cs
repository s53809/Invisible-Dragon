using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUIPresenter : PresenterBase<TitleUIView>
{
    public override void Release()
    {
        
    }

    public override void Bind()
    {
        base.Bind();
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().DOFade(1.0f, 2);
    }
}
