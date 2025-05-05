using static System.Console;

namespace Trivia;

public class Game
{
    public Board Board { get; } = new();

    private readonly IProvideQuestionBank _questionBank = new QuestionBank();
    private Player CurrentPlayer => Board.CurrentPlayer;

    public bool IsPlayable()
    {
        return Board.Players.Count >= 2;
    }

    public void Add(string playerName)
    {
        Board.Add(new Player(playerName, _questionBank));
        WriteLine(playerName + " was added");
        WriteLine("They are player number " + Board.Players.Count);
    }

    public void Roll(int diceNumber)
    {
        var dice = new Dice(diceNumber);

        WriteLine($"{CurrentPlayer.Name} is the current player");
        WriteLine($"They have rolled a {dice.Number}");

        if (CurrentPlayer.IsInPenaltyBox)
            AttemptToExitFromPenaltyBox(dice);
        else
            PlayerMovesForwardAndIsAskedQuestion(dice);
    }

    private void PlayerMovesForwardAndIsAskedQuestion(Dice dice)
    {
        CurrentPlayer.MovesForward(dice);
        CurrentPlayer.AsKQuestion();
    }

    private void AttemptToExitFromPenaltyBox(Dice dice)
    {
        if (dice.IsOdd)
        {
            CurrentPlayer.GettingOutPenaltyBox();
            PlayerMovesForwardAndIsAskedQuestion(dice);
        }
        else
        {
            CurrentPlayer.NotGettingOutPenaltyBox();
        }
    }

    public bool WasCorrectlyAnswered()
    {
        return CurrentPlayer.IsInPenaltyBox ? IsLeavingOrNotPenaltyBox() : PlayerGainColdCoin();
    }

    public bool WrongAnswer()
    {
        WriteLine("Question was incorrectly answered");
        CurrentPlayer.SendToPenaltyBox();
        WriteLine($"{CurrentPlayer.Name} was sent to the penalty box");
        return Board.TurnToTheNextPlayer(CurrentPlayer.DidWin());
    }

    private bool IsLeavingOrNotPenaltyBox()
    {
        return CurrentPlayer.IsGettingOutOfPenaltyBox ? 
            PlayerGainColdCoin() : 
            Board.TurnToTheNextPlayer(CurrentPlayer.DidWin());
    }

    private bool PlayerGainColdCoin()
    {
        CurrentPlayer.GainGoldCoin();
        return Board.TurnToTheNextPlayer(CurrentPlayer.DidWin());
    }
}