namespace Trivia;

public class Board(IProvideQuestionBank questionBank)
{
    private readonly List<Player> _players = [];
    private int _currentIndexPlayers;
    private Player CurrentPlayer => _players[_currentIndexPlayers];

    public void AddPlayer(Player player)
    {
        _players.Add(player);
        Logger.AddPlayer(player, _players.Count);
    }

#if TEST
    public string CurrentCategory(string playerName)
    {
        return questionBank.CurrentCategory(GetPlayerByName(playerName));
    }

    public Player GetPlayerByName(string playerName)
    {
        var indexOfPlayer = FindIndex(p => p.Name == playerName);
        if (indexOfPlayer != -1) return _players[indexOfPlayer];

        throw new ArgumentException($"Player {playerName} not found");
    }

#endif

    public bool IsPlayable()
    {
        return _players.Count >= 2;
    }

    public bool WasCorrectlyAnswered()
    {
        return CurrentPlayer.IsInPenaltyBox ? CurrentPlayIsLeavingOrNotPenaltyBox() : CurrentPlayerGainColdCoin();
    }

    public bool PlayerGiveWrongAnswer()
    {
        Logger.IncorrectAnswer();

        CurrentPlayerSendToPenaltyBox();

        Logger.CurrentPlayerWasSentToPenaltyBox(CurrentPlayer.Name);

        return TurnToTheNextCurrentPlayer(CurrentPlayer.DidNotWin());
    }

    public void PlayerRollDice(Dice dice)
    {
        Logger.CurrentPlayerStatus(dice, CurrentPlayer.Name);

        if (CurrentPlayer.IsInPenaltyBox)
            CurrentPlayerAttemptToExitFromPenaltyBox(dice);
        else
            CurrentPlayerMovesForwardAndIsAskingQuestion(dice);
    }

    private void CurrentPlayerMovesForward(Dice dice)
    {
        CurrentPlayer.Location += dice.Number;

        if (CurrentPlayer.Location > 11) CurrentPlayer.Location -= 12;
        
        Logger.PlayerLocationAndCategory(CurrentPlayer.Name, CurrentPlayer.Location, CurrentCategory(CurrentPlayer.Name));
    }

    private int FindIndex(Func<Player, bool> predicate)
    {
        for (var i = 0; i < _players.Count; i++)
            if (predicate(_players[i]))
                return i;
        return -1;
    }

    private bool TurnToTheNextCurrentPlayer(bool previousPlayerDidNotWin)
    {
        return previousPlayerDidNotWin && TurnToTheNextPlayer();
    }

    private bool TurnToTheNextPlayer()
    {
        _currentIndexPlayers++;
        if (_currentIndexPlayers == _players.Count) _currentIndexPlayers = 0;
        return true;
    }

    private void AsKQuestionToCurrentPlayer()
    {
        questionBank.AskQuestion(CurrentPlayer);
    }

    private void CurrentPlayerIsGettingOutPenaltyBox()
    {
        CurrentPlayer.GettingOutPenaltyBox();
    }

    private void CurrentPlayerIsNotGettingOutPenaltyBox()
    {
        CurrentPlayer.NotGettingOutPenaltyBox();
    }

    private void CurrentPlayerMovesForwardAndIsAskingQuestion(Dice dice)
    {
        CurrentPlayerMovesForward(dice);
        AsKQuestionToCurrentPlayer();
    }

    private void CurrentPlayerAttemptToExitFromPenaltyBox(Dice dice)
    {
        if (dice.IsOdd)
        {
            CurrentPlayerIsGettingOutPenaltyBox();
            CurrentPlayerMovesForwardAndIsAskingQuestion(dice);
        }
        else
        {
            CurrentPlayerIsNotGettingOutPenaltyBox();
        }
    }

    private bool CurrentPlayIsLeavingOrNotPenaltyBox()
    {
        return CurrentPlayer.IsGettingOutOfPenaltyBox
            ? CurrentPlayerGainColdCoin()
            : TurnToTheNextCurrentPlayer(CurrentPlayer.DidNotWin());
    }

    private bool CurrentPlayerGainColdCoin()
    {
        CurrentPlayer.GainGoldCoin();
        return TurnToTheNextCurrentPlayer(CurrentPlayer.DidNotWin());
    }

    private void CurrentPlayerSendToPenaltyBox()
    {
        CurrentPlayer.SendToPenaltyBox();
    }
}