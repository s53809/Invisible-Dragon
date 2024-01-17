using System;
using UnityEngine;
using DG.Tweening;

public class TimingPresenter : PresenterBase<TimingView>
{
    [SerializeField] GameObject m_yourTimingBar;
    [SerializeField] Sprite[] m_timingUI;
    public override void Release()
    {
        
    }

    public void Bind(Double timing)
    {
        base.Bind();
        
        GameObject obj = Instantiate(m_yourTimingBar, _view.TimingBar.transform);
        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3((Single)timing, 0, 0);
        obj.GetComponent<RectTransform>().DOScale(0, 2f).OnComplete(() =>
        { Destroy(obj); });
        
        if (Math.Abs(timing) <= (Double)Timing.PERFECT)
            _view.TimingImage.sprite = m_timingUI[0];
        else if (Math.Abs(timing) <= (Double)Timing.GREAT)
            _view.TimingImage.sprite = m_timingUI[1];
        else if (Math.Abs(timing) <= (Double)Timing.GOOD)
            _view.TimingImage.sprite = m_timingUI[2];
        else if (Math.Abs(timing) <= (Double)Timing.BAD)
            _view.TimingImage.sprite = m_timingUI[3];
        else
            _view.TimingImage.sprite = m_timingUI[4];

        _view.TimingImage.rectTransform.localScale = new Vector3(0, 0, 0);
        _view.TimingImage.rectTransform.DOScale(new Vector3(1, 1, 1), 0.05f);
    }
}
