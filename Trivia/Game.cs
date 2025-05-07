namespace Trivia;

public class Game
{
    public Board Board { get; } = new(new QuestionBank());

    public bool IsPlayable()
    {
        return Board.Players.Count >= 2;
    }

    public void Add(string playerName)
    {
        Board.AddPlayer(new Player(playerName));
    }

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

    private void IntegrityForRollDice()
    {
        CheckPlayableGame();

        if (_nextIntegrityStepIndex != IntegritySteps.RollDice)
            throw new InvalidOperationException("You must roll the dice once, but not two");

        _nextIntegrityStepIndex = IntegritySteps.AnsweredQuestion;
    }

    private void IntegrityForAnsweredQuestion()
    {
        CheckPlayableGame();

        if (_nextIntegrityStepIndex != IntegritySteps.AnsweredQuestion)
            throw new InvalidOperationException("You must roll the dice before answering a question");

        _nextIntegrityStepIndex = IntegritySteps.RollDice;
    }

    private void CheckPlayableGame()
    {
        if (!IsPlayable())
            throw new InvalidOperationException("Game is not playable");
    }

    private enum IntegritySteps
    {
        RollDice,
        AnsweredQuestion
    }

    private IntegritySteps _nextIntegrityStepIndex = IntegritySteps.RollDice;
}