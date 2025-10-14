// Main entry point for the API. Uses .NET 8 Minimal APIs for simplicity.
var builder = WebApplication.CreateBuilder(args);

// --- Service Configuration ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddScoped<IJudgeService, JudgeService>();

var app = builder.Build();

// --- Middleware Pipeline ---
app.UseCors("AllowAll");

// --- API Endpoints ---
app.MapGet("/api/problem/{id}", (int id) =>
{
    var problem = ProblemRepository.GetProblemById(id);
    return problem != null ? Results.Ok(problem) : Results.NotFound();
});

app.MapPost("/api/submit", async (Submission submission, IJudgeService judgeService) =>
{
    if (string.IsNullOrWhiteSpace(submission.Code))
    {
        return Results.BadRequest("Code cannot be empty.");
    }
    
    var result = await judgeService.JudgeAsync(submission);
    return Results.Ok(result);
});

app.Run();