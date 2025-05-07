using System;

namespace Trivia;

public class Game
{
    private readonly IProvideQuestionBank _questionBank = new QuestionBank();
    private Steps _nextStepIndex = Steps.RollDice;

    public Game()
    {
        Board = new Board(_questionBank);
    }

    public Board Board { get; }

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

        if (_nextStepIndex != Steps.RollDice)
            throw new InvalidOperationException("You must roll the dice once, but not two");

        _nextStepIndex = Steps.AnsweredQuestion;
    }

    private void IntegrityForAnsweredQuestion()
    {
        CheckPlayableGame();

        if (_nextStepIndex != Steps.AnsweredQuestion)
            throw new InvalidOperationException("You must roll the dice before answering a question");

        _nextStepIndex = Steps.RollDice;
    }

    private void CheckPlayableGame()
    {
        if (!IsPlayable())
            throw new InvalidOperationException("Game is not playable");
    }

    private enum Steps
    {
        RollDice,
        AnsweredQuestion
    }
}