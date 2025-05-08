using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia;

using static Console;

public class Board(IProvideQuestionBank questionBank)
{
    private readonly List<Player> _players = [];
    private int _currentPlayer;
    private Player CurrentPlayer => _players[_currentPlayer];

    public Player this[int index] => _players[index];
    public IReadOnlyCollection<Player> Players => _players.ToList();

    public void AddPlayer(Player player)
    {
        _players.Add(player);
        WriteLine(player.Name + " was added");
        WriteLine("They are player number " + Players.Count);
    }

    public int FindIndex(Func<Player, bool> predicate)
    {
        for (var i = 0; i < _players.Count; i++)
            if (predicate(_players[i]))
                return i;
        return -1;
    }

    public bool WasCorrectlyAnswered()
    {
        return CurrentPlayer.IsInPenaltyBox ? CurrentPlayIsLeavingOrNotPenaltyBox() : CurrentPlayerGainColdCoin();
    }

    public bool PlayerGiveWrongAnswer()
    {
        WriteLine("Question was incorrectly answered");

        CurrentPlayerSendToPenaltyBox();

        WriteLine($"{CurrentPlayer.Name} was sent to the penalty box");

        return TurnToTheNextCurrentPlayer(CurrentPlayer.DidNotWin());
    }

    public void PlayerRollDice(Dice dice)
    {
        WriteLine($"{CurrentPlayer.Name} is the current player");
        WriteLine($"They have rolled a {dice.Number}");

        if (CurrentPlayer.IsInPenaltyBox)
            CurrentPlayerAttemptToExitFromPenaltyBox(dice);
        else
            CurrentPlayerMovesForwardAndIsAskingQuestion(dice);
    }

    public string CurrentCategory(Player player)
    {
        return questionBank.CurrentCategory(player);
    }

    private void CurrentPlayerMovesForward(Dice dice)
    {
        CurrentPlayer.Location += dice.Number;

        if (CurrentPlayer.Location > 11) CurrentPlayer.Location -= 12;

        WriteLine($"{CurrentPlayer.Name}'s new location is {CurrentPlayer.Location}");
        WriteLine($"The category is {questionBank.CurrentCategory(CurrentPlayer)}");
    }

    private bool TurnToTheNextCurrentPlayer(bool previousPlayerDidNotWin)
    {
        return previousPlayerDidNotWin && TurnToTheNextPlayer();
    }

    private bool TurnToTheNextPlayer()
    {
        _currentPlayer++;
        if (_currentPlayer == _players.Count) _currentPlayer = 0;
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