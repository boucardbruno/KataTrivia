namespace Trivia;

public class QuestionBank : IProvideQuestionBank
{
    private const int MaxQuestionsPerCategory = 50;

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
        foreach (var (key, _) in _questionsPerCategory)
            if (CurrentCategory(player) == key)
            {
                _questionsPerCategory[key].Pop();
                break;
            }
    }

    public string CurrentCategory(Player player)
    {
        var questionLabels = _questionsPerCategory.Keys.ToArray();

        return (player.Location % 4) switch
        {
            var i => questionLabels[i]
        };
    }

    private void InitializeQuestions()
    {
        for (var i = 0; i < MaxQuestionsPerCategory; i++)
            foreach (var (key, value) in _questionsPerCategory)
                _questionsPerCategory[key].Push($"{value} Question {i}");
    }
}