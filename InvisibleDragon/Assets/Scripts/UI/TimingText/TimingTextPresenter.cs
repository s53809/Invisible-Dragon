using System;
public class TimingTextPresenter : PresenterBase<TimingTextView>
{
    public override void Release()
    {
        throw new NotImplementedException();
    }

    public void Bind(String text)
    {
        _view.Timing.text = text;
    }
}
