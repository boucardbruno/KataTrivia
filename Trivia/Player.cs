using static System.Console;

namespace Trivia;

public class Player(string name)
{
    public string Name { get; } = name;
    public int Location { get; set; }
    public bool IsInPenaltyBox { get; private set; }
    public bool IsGettingOutOfPenaltyBox { get; private set; }

    private int GoldCoins { get; set; }

    public void GainGoldCoin()
    {
        WriteLine("Answer was correct!!!!");
        GoldCoins++;
        WriteLine($"{Name} now has {GoldCoins} Gold Coins.");
    }

    public void GettingOutPenaltyBox()
    {
        IsGettingOutOfPenaltyBox = true;
        GetOutOfPenaltyBox();
        WriteLine($"{Name} is getting out of the penalty box");
    }

    public void NotGettingOutPenaltyBox()
    {
        WriteLine($"{Name} is not getting out of the penalty box");
        IsGettingOutOfPenaltyBox = false;
    }

    public bool DidWin()
    {
        return GoldCoins != 6;
    }

    public void SendToPenaltyBox()
    {
        IsInPenaltyBox = true;
    }

    private void GetOutOfPenaltyBox()
    {
        IsInPenaltyBox = false;
    }
}