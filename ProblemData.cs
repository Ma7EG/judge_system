// Defines the data structures for problems and test cases.

public class Problem
{
    public int ProblemId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Statement { get; set; } = string.Empty;
    public string Constraints { get; set; } = string.Empty;
    public List<TestCase> TestCases { get; set; } = new();
}

public class TestCase
{
    public string Input { get; set; } = string.Empty;
    public string ExpectedOutput { get; set; } = string.Empty;
}

// In-memory data store for the MVP.
public static class ProblemRepository
{
    private static readonly List<Problem> Problems = new()
    {
        new Problem
        {
            ProblemId = 1,
            Title = "Sum of Two Numbers",
            Statement = "Read two integers from standard input, calculate their sum, and print the result to standard output.",
            Constraints = "Both input integers will be between -1,000,000 and 1,000,000.",
            TestCases = new List<TestCase>
            {
                new() { Input = "5 3", ExpectedOutput = "8" },
                new() { Input = "100 200", ExpectedOutput = "300" },
                new() { Input = "-10 10", ExpectedOutput = "0" },
                new() { Input = "0 0", ExpectedOutput = "0" }
            }
        }
    };

    public static Problem? GetProblemById(int id)
    {
        return Problems.FirstOrDefault(p => p.ProblemId == id);
    }
}