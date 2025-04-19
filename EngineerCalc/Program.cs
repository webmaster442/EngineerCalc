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

app.MapGet("/evaluate", (HttpRequest request) =>
{
    var expression = HttpUtility.HtmlEncode(request.Query["e"]) ?? string.Empty;
    string id = StateIdFactroy.Create(request.HttpContext.Connection.RemoteIpAddress,
                                    request.Scheme,
                                    request.Headers.UserAgent);
    
    return calculator.Process(expression, id);
});

app.Run();
