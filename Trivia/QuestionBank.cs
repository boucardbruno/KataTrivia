namespace Trivia;

public class QuestionBank : IProvideQuestionBank
{
    private const int MaxQuestions = 50;

    private readonly Dictionary<string, Stack<string>> _questionsPerCategory = new()
    {
        ["Pop"] = new Stack<string>(),
        ["Science"] = new Stack<string>(),
        ["Sports"] = new Stack<string>(),
        ["Rock"] = new Stack<string>()
    };

    public QuestionBank()
    {
        InitializeQuestions();
    }

    public void AskQuestion(Player player)
    {
       CurrentCategory(player);
    }

    public string CurrentCategory(Player player)
    {
        return _questionsPerCategory.Keys.ToArray()[player.Location % 4];
    }

    private void InitializeQuestions()
    {
        for (var i = MaxQuestions - 1; i >= 0; i--)
            foreach (var (key, _) in _questionsPerCategory)
                _questionsPerCategory[key].Push($"{key} Question {i}");
    }
}