namespace Trivia;

public interface IProvideQuestionBank
{
    void AskQuestion(Player player);
    string CurrentCategory(Player player);
}