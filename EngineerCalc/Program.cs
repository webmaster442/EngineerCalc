using EngineerCalc;
using EngineerCalc.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

WebApplication app = builder.Build();
EmbeddedServer embeddedServer = new();
StateManager stateManager = new();
EndpointFunctions endpoints = new();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

embeddedServer.MapEmbeddedFilesAsRoutes(app);

app.MapGet("/evaluate", async (HttpRequest request) =>
{
    string expression = request.Query["e"].FirstOrDefault() ?? "";

    Result result = Result.Default;
    await stateManager.WithState(request, async (state) =>
    {
        result = await endpoints.Evaluate(state, expression);
    });
    return result.ToIResult();
});

app.MapGet("/cmd", async (HttpRequest request) =>
{
    string commandLine = request.Query["c"].FirstOrDefault() ?? "";

    Result result = Result.Default;
    await stateManager.WithState(request, async (state) =>
    {
        result = await endpoints.RunCommand(state, commandLine);
    });
    return result.ToIResult();
});

app.MapGet("/intro", async (HttpRequest request) =>
{
    var result = await endpoints.Intro();
    return result.ToIResult();
});

app.Run();
