using DG.Tweening;
using System;
using UnityEngine;

public class TitleUIPresenter : PresenterBase<TitleUIView>
{
    [SerializeField] private RectTransform Screen1;
    [SerializeField] private RectTransform Screen2;
    [SerializeField] private ElevatorPresenter elevatorUI;

    private Boolean m_isTutoOpen = false;
    public override void Release()
    {
        
    }

    public override void Bind()
    {
        base.Bind();
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().DOFade(1.0f, 2);

        _view.UpButton.onClick.AddListener(PressUpButton);
        _view.TutoButton.onClick.AddListener(PressTutoButton);
        m_isTutoOpen = false;
    }

    public void PressUpButton()
    {
        Screen1.DOAnchorPosY(-1080, 1.0f);
        Screen2.DOAnchorPosY(0, 1.0f);
        elevatorUI.Bind();
    }

    public void PressTutoButton()
    {
        if (!m_isTutoOpen)
        { _view.TutoImage.gameObject.SetActive(true); m_isTutoOpen = true; }
        else
        { _view.TutoImage.gameObject.SetActive(false); m_isTutoOpen = false; }
    }
}
