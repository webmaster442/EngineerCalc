using System.Net.Mime;
using System.Web;

using EngineerCalc;
using EngineerCalc.Calculator;
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

    (string response, bool ok) result = ("", false);
    await stateManager.WithState(request, async (state) =>
    {
        result = await endpoints.Evaluate(state, expression);
    });
    return result.ok
        ? Results.Content(result.response, MediaTypeNames.Text.Html)
        : Results.InternalServerError(result.response);
});

app.Run();
