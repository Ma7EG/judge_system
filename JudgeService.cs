using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

public interface IJudgeService
{
    Task<JudgeResult> JudgeAsync(Submission submission);
}

public class JudgeService : IJudgeService
{
    public async Task<JudgeResult> JudgeAsync(Submission submission)
    {
        var problem = ProblemRepository.GetProblemById(submission.ProblemId);
        if (problem == null)
        {
            return new JudgeResult { Verdict = "Error: Problem not found." };
        }

        if (submission.Code.Contains("// ERROR"))
        {
            return new JudgeResult { Verdict = "Compilation Error" };
        }

        var totalExecutionTime = new Stopwatch();
        totalExecutionTime.Start();

        foreach (var testCase in problem.TestCases)
        {
            try
            {
                var scriptOptions = ScriptOptions.Default
                    .AddReferences(typeof(Console).Assembly)
                    .WithImports("System");

                using var inputReader = new StringReader(testCase.Input.Replace(" ", Environment.NewLine));
                using var outputWriter = new StringWriter();
                Console.SetIn(inputReader);
                Console.SetOut(outputWriter);

                await CSharpScript.RunAsync(submission.Code, scriptOptions);
                
                string actualOutput = outputWriter.ToString().Trim();
                if (actualOutput != testCase.ExpectedOutput)
                {
                    return new JudgeResult { Verdict = "Wrong Answer" };
                }
            }
            catch (Exception ex)
            {
                return new JudgeResult { Verdict = $"Runtime Error: {ex.Message}" };
            }
        }

        totalExecutionTime.Stop();

        return new JudgeResult
        {
            Verdict = "Accepted",
            ExecutionTimeMs = totalExecutionTime.ElapsedMilliseconds
        };
    }
}

// DTOs for API communication
public class Submission
{
    public int ProblemId { get; set; }
    public string Code { get; set; } = string.Empty;
}

public class JudgeResult
{
    public string Verdict { get; set; } = string.Empty;
    public long ExecutionTimeMs { get; set; }
}