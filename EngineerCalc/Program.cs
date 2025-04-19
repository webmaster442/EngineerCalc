using System.Net.Mime;
using System.Web;

using EngineerCalc;
using EngineerCalc.Calculator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

WebApplication app = builder.Build();
EmbeddedServer embeddedServer = new();
Calculator calculator = new Calculator();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

embeddedServer.MapEmbeddedFilesAsRoutes(app);

app.MapGet("/evaluate", async (HttpRequest request) =>
{
    var expression = HttpUtility.UrlDecode(request.Query["e"]) ?? string.Empty;
    string stateId = StateIdFactroy.Create(request.HttpContext.Connection.RemoteIpAddress,
                                           request.Scheme,
                                           request.Headers.UserAgent);
    
    var result = await calculator.Process(expression, stateId);

    return result.ok 
        ? Results.Content(result.response, MediaTypeNames.Text.Html) 
        : Results.InternalServerError(result.response);
});

app.Run();
