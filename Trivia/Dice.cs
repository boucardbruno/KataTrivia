namespace Trivia;

public record Dice(int Number)
{
    public bool IsOdd => Number % 2 != 0;
}