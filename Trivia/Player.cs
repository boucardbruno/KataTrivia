using static System.Console;

namespace Trivia;

public class Player(string name)
{
    private int _coldCoins;
    public string Name { get; } = name;
    public int Location { get; set; }
    public bool IsInPenaltyBox { get; private set; }
    public bool IsGettingOutOfPenaltyBox { get; private set; }

    public void GainGoldCoin()
    {
        WriteLine("Answer was correct!!!!");
        _coldCoins++;
        WriteLine($"{Name} now has {_coldCoins} Gold Coins.");
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

    public bool DidNotWin()
    {
        return _coldCoins != 6;
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