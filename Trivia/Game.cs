namespace Trivia;

public class Game
{
    private Board Board { get; } = new(new QuestionBank());

    public void Roll(int diceNumber)
    {
        IntegrityForRollDice();
        Board.PlayerRollDice(new Dice(diceNumber));
    }

    public bool WrongAnswer()
    {
        IntegrityForAnsweredQuestion();
        return Board.PlayerGiveWrongAnswer();
    }

    public bool WasCorrectlyAnswered()
    {
        IntegrityForAnsweredQuestion();
        return Board.WasCorrectlyAnswered();
    }

    public void Add(string playerName)
    {
        Board.AddPlayer(new Player(playerName));
    }

    public bool IsPlayable()
    {
        return Board.IsPlayable();
    }

#if TEST
    public string CurrentCategory(string playerName)
    {
        return Board.CurrentCategory(playerName);
    }

    public Player GetPlayerByName(string playerName)
    {
        return Board.GetPlayerByName(playerName);
    }
#endif

    private void IntegrityForRollDice()
    {
        CheckPlayableGame();

        if (_nextGammeIntegrityForIndex != GammeIntegrityFor.CallRoll)
            throw new InvalidOperationException("You must roll the dice once, but not two");

        _nextGammeIntegrityForIndex = GammeIntegrityFor.CallAnsweredQuestion;
    }

    private void IntegrityForAnsweredQuestion()
    {
        CheckPlayableGame();

        if (_nextGammeIntegrityForIndex != GammeIntegrityFor.CallAnsweredQuestion)
            throw new InvalidOperationException("You must roll the dice before answering a question");

        _nextGammeIntegrityForIndex = GammeIntegrityFor.CallRoll;
    }

    private void CheckPlayableGame()
    {
        if (!Board.IsPlayable())
            throw new InvalidOperationException("Game is not playable");
    }

    private enum GammeIntegrityFor
    {
        CallRoll,
        CallAnsweredQuestion
    }

    private GammeIntegrityFor _nextGammeIntegrityForIndex = GammeIntegrityFor.CallRoll;
}