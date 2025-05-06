namespace Trivia;
using static Console;

public class Board(IProvideQuestionBank questionBank)
{
    private readonly List<Player> _players = [];
    private int _currentPlayer;

    public Player CurrentPlayer => _players[_currentPlayer];
    public Player this[int index] => _players[index];

    public IReadOnlyCollection<Player> Players => _players.ToList();

    public void Add(Player player)
    {
        _players.Add(player);
    }

    public int FindIndex(Func<Player, bool> predicate)
    {
        for (var i = 0; i < _players.Count; i++)
            if (predicate(_players[i]))
                return i;
        return -1;
    }

    public void MovesForward(Dice diceNumber)
    {
        CurrentPlayer.Location += diceNumber.Number;
        if (CurrentPlayer.Location > 11) CurrentPlayer.Location -= 12;

        WriteLine($"{CurrentPlayer.Name}'s new location is {CurrentPlayer.Location}");
        WriteLine($"The category is {questionBank.CurrentCategory(CurrentPlayer)}");
    }

    public bool TurnToTheNextPlayer(bool previousPlayerDidWin)
    {
        _currentPlayer++;
        if (_currentPlayer == _players.Count) _currentPlayer = 0;
        return previousPlayerDidWin;
    }

    public void AsKQuestion()
    {
        questionBank.AskQuestion(CurrentPlayer);
    }
}