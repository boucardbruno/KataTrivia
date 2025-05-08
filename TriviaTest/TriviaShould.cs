using System;
using NFluent;
using NUnit.Framework;
using Trivia;

namespace TriviaTest;

internal class TriviaShould
{
    private Game _game = null!;

    [SetUp]
    public void Setup()
    {
        _game = new Game();
    }

    [Test]
    public void Run_when_players_are_at_least_two_players()
    {
        _game.Add("Chet");
        _game.Add("Pat");

        Check.That(_game.IsPlayable()).IsTrue();
    }

    [Test]
    public void Not_Run_when_players_are_not_at_least_two_players()
    {
        _game.Add("Chet");
        Check.That(_game.IsPlayable()).IsFalse();
    }

    [Test]
    public void Move_players_at_new_location_when_they_roll_the_dice()
    {
        _game.Add("Chet");
        _game.Add("Pat");

        _game.Roll(6);

        _game.WasCorrectlyAnswered();

        _game.Roll(2);

        _game.WasCorrectlyAnswered();

        Check.That(LocationForPlayer("Chet")).IsEqualTo(6);
        Check.That(LocationForPlayer("Pat")).IsEqualTo(2);
    }

    [Test]
    public void Current_category_when_curent_user_roll()
    {
        _game.Add("Chet");
        _game.Add("Pat");
        _game.Roll(2);
        Check.That(_game.Board.CurrentCategory(new Player("Chet"))).IsEqualTo("Pop");
        _game.WrongAnswer();
        _game.Roll(3);
        Check.That(_game.Board.CurrentCategory(new Player("Pat"))).IsEqualTo("Pop");
    }

    [Test]
    public void Raise_exception_when_the_game_is_not_playable()
    {
        _game.Add("Chet");

        Check.ThatCode(() => _game.Roll(2))
            .Throws<InvalidOperationException>()
            .WithMessage("Game is not playable");
    }

    [Test]
    public void Raise_exception_when_the_answer_is_correct_before_rolling_a_dice()
    {
        _game.Add("Chet");
        _game.Add("Pat");

        Check.ThatCode(() => _game.WasCorrectlyAnswered())
            .Throws<InvalidOperationException>()
            .WithMessage("You must roll the dice before answering a question");
    }

    [Test]
    public void Raise_exception_when_we_are_not_answer_correctly_before_rolling_a_dice()
    {
        _game.Add("Chet");
        _game.Add("Pat");

        Check.ThatCode(() => _game.WrongAnswer())
            .Throws<InvalidOperationException>()
            .WithMessage("You must roll the dice before answering a question");
    }

    [Test]
    public void Raise_exception_when_we_are_not_answer_correctly_before_rolling()
    {
        _game.Add("Chet");
        _game.Add("Pat");
        _game.Roll(2);
        
        Check.ThatCode(() => _game.Roll(1))
            .Throws<InvalidOperationException>()
            .WithMessage("You must roll the dice once, but not two");
    }

    [Test]
    public void Location_for_player_raise_exception_when_the_player_is_not_added()
    {
        Check.ThatCode(() => LocationForPlayer("Paul")).Throws<ArgumentException>()
            .WithMessage("Player Paul not found");
    }

    [Test]
    public void Go_to_player_in_penalty_box_when_bad_answer()
    {
        _game.Add("Chet");
        _game.Add("Pat");

        _game.Roll(6);
        _game.WrongAnswer();

        Check.That(PlayerIsInPenaltyBox("Chet")).IsTrue();
    }

    [Test]
    public void Player_is_in_penalty_box_raise_exception_when_the_player_is_not_added()
    {
        Check.ThatCode(() => PlayerIsInPenaltyBox("Paul"))
            .Throws<ArgumentException>().WithMessage("Player Paul not found");
    }

    [Test]
    public void Go_to_out_penalty_box_when_player_correctly_answered_with_odd_number()
    {
        _game.Add("Chet");
        _game.Add("Pat");

        _game.Roll(6);
        _game.WrongAnswer();

        Check.That(PlayerIsInPenaltyBox("Chet")).IsTrue();

        _game.Roll(2);
        _game.WasCorrectlyAnswered();

        // Get an odd number to get out the penalty box
        _game.Roll(3); 
        _game.WasCorrectlyAnswered();

        Check.That(PlayerIsInPenaltyBox("Chet")).IsFalse();
    }

    [Test]
    public void Do_not_go_to_out_penalty_box_when_player_correctly_answered_with_even_number()
    {
        _game.Add("Chet");
        _game.Add("Pat");

        _game.Roll(6);
        _game.WrongAnswer();

        _game.Roll(2);
        _game.WasCorrectlyAnswered();
        
        // Too bad, we get an even number!!!
        _game.Roll(2);
        _game.WasCorrectlyAnswered();

        Check.That(PlayerIsInPenaltyBox("Chet")).IsTrue();
    }

    [Test]
    public void Make_a_winner_when_a_player_correctly_answer_six_time()
    {
        _game.Add("Chet");
        _game.Add("Pat");

        // Chet don't get a gold coin
        _game.Roll(6);
        _game.WrongAnswer();
        Check.That(PlayerIsInPenaltyBox("Chet")).IsTrue();
        Check.That(PlayerGoldCoins("Chet")).IsEqualTo(0);

        // Pat should get 1 gold coin
        _game.Roll(2);
        _game.WasCorrectlyAnswered();
        Check.That(PlayerGoldCoins("Pat")).IsEqualTo(1);

        // Chet is in penalty
        _game.Roll(2);
        _game.WasCorrectlyAnswered();
        Check.That(PlayerIsInPenaltyBox("Chet")).IsTrue();
        Check.That(PlayerGoldCoins("Chet")).IsEqualTo(0);

        // Pat should have 2 gold coins
        _game.Roll(2);
        _game.WasCorrectlyAnswered();
        Check.That(PlayerGoldCoins("Pat")).IsEqualTo(2);

        // Chet still in penalty box
        _game.Roll(2);
        _game.WasCorrectlyAnswered();
        Check.That(PlayerIsInPenaltyBox("Chet")).IsTrue();
        Check.That(PlayerGoldCoins("Chet")).IsEqualTo(0);

        // Pat should have 3 gold coins
        _game.Roll(2);
        _game.WasCorrectlyAnswered();
        Check.That(PlayerGoldCoins("Pat")).IsEqualTo(3);

        // Chet should get out the penalty box with an odd number and get 1 gold coins
        _game.Roll(3);
        _game.WasCorrectlyAnswered();
        Check.That(PlayerIsInPenaltyBox("Chet")).IsFalse();
        Check.That(PlayerGoldCoins("Chet")).IsEqualTo(1);

        // Pat should have 4 gold coins
        _game.Roll(2);
        _game.WasCorrectlyAnswered();
        Check.That(PlayerGoldCoins("Pat")).IsEqualTo(4);

        // Chet should have 2 gold coins
        _game.Roll(2);
        _game.WasCorrectlyAnswered();
        Check.That(PlayerGoldCoins("Chet")).IsEqualTo(2);

        // Pat should have 5 gold coins
        _game.Roll(2);
        _game.WasCorrectlyAnswered();
        Check.That(PlayerGoldCoins("Pat")).IsEqualTo(5);

       // Chet should have 3 gold coins
        _game.Roll(2);
        _game.WasCorrectlyAnswered();
        Check.That(PlayerGoldCoins("Chet")).IsEqualTo(3);

        // Pat should have 6 gold coins!!!
        _game.Roll(2);
        _game.WasCorrectlyAnswered();
        Check.That(PlayerGoldCoins("Pat")).IsEqualTo(6);

        Check.That(PlayerWin("Pat")).IsTrue();
        Check.That(PlayerWin("Chet")).IsFalse();
    }

    [Test]
    public void PlayerWin_raise_exception_when_the_player_is_not_added()
    {
        Check.ThatCode(() => PlayerWin("Paul")).Throws<ArgumentException>()
            .WithMessage("Player Paul not found");
    }

   [ Test]
    public void PlayerGoldCoins_raise_exception_when_the_player_is_not_added()
    {
        Check.ThatCode(() => PlayerGoldCoins("Paul")).Throws<ArgumentException>()
            .WithMessage("Player Paul not found");
    }

    private int LocationForPlayer(string playerName)
    {
        var indexOfPlayer = _game.Board.FindIndex(p => p.Name == playerName);

        if (indexOfPlayer != -1) return _game.Board[indexOfPlayer].Location;

        throw new ArgumentException($"Player {playerName} not found");
    }

    private bool PlayerIsInPenaltyBox(string playerName)
    {
        var indexOfPlayer = _game.Board.FindIndex(p => p.Name == playerName);

        if (indexOfPlayer != -1) return _game.Board[indexOfPlayer].IsInPenaltyBox;

        throw new ArgumentException($"Player {playerName} not found");
    }

    private bool PlayerWin(string playerName)
    {
        var indexOfPlayer = _game.Board.FindIndex(p => p.Name == playerName);

        if (indexOfPlayer != -1) return !_game.Board[indexOfPlayer].DidNotWin();

        throw new ArgumentException($"Player {playerName} not found");
    }

    private int PlayerGoldCoins(string playerName)
    {
        var indexOfPlayer = _game.Board.FindIndex(p => p.Name == playerName);
        if (indexOfPlayer != -1) return _game.Board[indexOfPlayer].GoldCoins;

        throw new ArgumentException($"Player {playerName} not found");
    }
}