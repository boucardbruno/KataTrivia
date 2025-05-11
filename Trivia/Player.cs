using static System.Console;

namespace Trivia;

public class Player(string name)
{
    public string Name { get; } = name;
    public int Location { get; set; }
    public bool IsInPenaltyBox { get; private set; }
    public bool IsGettingOutOfPenaltyBox { get; private set; }
    public int GoldCoins { get; private set;}

    public void GainGoldCoin()
    {
        Logger.CorrectAnswer();
        GoldCoins++;
        Logger.PlayerGoldCoinCount(Name, GoldCoins);
    }

    public void GettingOutPenaltyBox()
    {
        IsGettingOutOfPenaltyBox = true;
        GetOutOfPenaltyBox();
        Logger.PlayerGettingOutOfPenaltyBox(Name);
    }

    public void NotGettingOutPenaltyBox()
    {
        Logger.PlayerGettingOutPenaltyBox(Name);
        IsGettingOutOfPenaltyBox = false;
    }

    public bool DidNotWin()
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