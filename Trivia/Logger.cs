namespace Trivia;

public static class Logger
{
    public static void AddPlayer(Player player, int playersCount)
    {
        Console.WriteLine(player.Name + " was added");
        Console.WriteLine("They are player number " + playersCount);
    }

    public static void CurrentPlayerWasSentToPenaltyBox(string currentPlayerName)
    {
        Console.WriteLine($"{currentPlayerName} was sent to the penalty box");
    }

    public static void IncorrectAnswer()
    {
        Console.WriteLine("Question was incorrectly answered");
    }

    public static void PlayerLocationAndCategory(string currentPlayerName, int currentPlayerLocation, string currentCategory)
    {
        Console.WriteLine($"{currentPlayerName}'s new location is {currentPlayerLocation}");
        Console.WriteLine($"The category is {currentCategory}");
    }

    public static void CurrentPlayerStatus(Dice dice, string currentPlayerName)
    {
        Console.WriteLine($"{currentPlayerName} is the current player");
        Console.WriteLine($"They have rolled a {dice.Number}");
    }

    public static void CorrectAnswer()
    {
        Console.WriteLine("Answer was correct!!!!");
    }

    public static void PlayerGoldCoinCount(string name, int goldCoins)
    {
        Console.WriteLine($"{name} now has {goldCoins} Gold Coins.");
    }

    public static void PlayerGettingOutOfPenaltyBox(string name)
    {
        Console.WriteLine($"{name} is getting out of the penalty box");
    }

    public static void PlayerGettingOutPenaltyBox(string name)
    {
        Console.WriteLine($"{name} is not getting out of the penalty box");
    }
}