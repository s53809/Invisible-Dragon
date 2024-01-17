using DG.Tweening;

public class LastScorePresenter : PresenterBase<LastScoreView>
{
    public override void Release()
    {
        
    }

    public void Bind(TimingScoreCount finalScore)
    {
        base.Bind();
        _view.ScoreText.DOText($"Final Score : {finalScore.TotalScore}\n" +
            $"Perfect : {finalScore.Perfect}\n" +
            $"Great : {finalScore.Great}\n" +
            $"Good : {finalScore.Good}\n" +
            $"Bad : {finalScore.Bad}\n" +
            $"Miss : {finalScore.Miss}\n" +
            $"Press ESC to back to main menu", 5);
    }

}
