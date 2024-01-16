using DG.Tweening;
using UnityEngine;

public class TitleUIPresenter : PresenterBase<TitleUIView>
{
    [SerializeField] private RectTransform Screen1;
    [SerializeField] private RectTransform Screen2;
    public override void Release()
    {
        
    }

    public override void Bind()
    {
        base.Bind();
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().DOFade(1.0f, 2);

        _view.UpButton.onClick.AddListener(PressUpButton);
    }

    public void PressUpButton()
    {
        Screen1.DOAnchorPosY(-1080, 1.0f);
        Screen2.DOAnchorPosY(0, 1.0f);
    }
}
