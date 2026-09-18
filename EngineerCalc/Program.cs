//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator;

using EngineerCalc;
using EngineerCalc.Domain;
using EngineerCalc.DomainServices;
using EngineerCalc.Extensions;
using EngineerCalc.Infrastructure;
using EngineerCalc.Tui;
using EngineerCalc.Tui.Readline;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Spectre.Console;

using Webmaster442.WindowsTerminal;

var appState = new State();

using var loggerprovider = new LoggerProvider(100, TimeProvider.System);
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.ClearProviders();
    builder.AddProvider(loggerprovider);
    builder.SetMinimumLevel(LogLevel.Debug);
});

var timeProvider = new NtpBasedTimeProvider(TimeProvider.System, loggerFactory);

await timeProvider.Update();

var expressionFactory = new ExpressionFactory(timeProvider);
var evaluatorApi = new EvaluatorApi(new VariablesAndConstantsCollection(), expressionFactory, appState);
var commandRunnerApi = new CommandRunnerApi();
var fileSystem = new FileSystem();

IConfiguration config = 
    new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

var services = new ServiceCollection();
services.AddSingleton(appState);
services.AddSingleton(loggerprovider);
services.AddSingleton<TimeProvider>(TimeProvider.System);
services.AddSingleton<IConfiguration>(config);
services.AddSingleton<ILoggerFactory>(loggerFactory);
services.AddSingleton<IApplicationApi, ApplicationApi>();
services.AddSingleton<IEvaluatorApi>(evaluatorApi);
services.AddSingleton<ICommandRunnerApi>(commandRunnerApi);
services.AddSingleton<IFileSystem>(fileSystem);
services.AddSingleton<IRemoteApiCache, ZipCache>();
services.AddSingleton<IRemoteApiClient, RemoteApiClient>();
services.AddSingleton<ScriptFileRunner>();
services.AddSingleton(expressionFactory);

Console.OutputEncoding = System.Text.Encoding.UTF8;

var runner = new CommandRunner(services, loggerFactory.CreateLogger<CommandRunner>());

await commandRunnerApi.Init(runner);

var lineCompleter = new LineCompleter(expressionFactory.KnownFunctions,
                                      commandRunnerApi.KnownCommands,
                                      evaluatorApi,
                                      fileSystem,
                                      appState);

var readline = new LineReader(lineCompleter);

AnsiConsole.Clear();

await runner.RunAsync([".intro"]);

var logger = loggerFactory.CreateLogger("Program");

while (true)
{
    Terminal.ShellIntegration.StartOfPrompt();
    Prompt.DoPrompt(appState);
    Terminal.ShellIntegration.CommandStart();
    string line = readline.ReadLine(" ─> ");
    Terminal.ShellIntegration.CommandExecuted();
    if (string.IsNullOrWhiteSpace(line))
        continue;

    List<string> tokens = line.SplitBySpaceOrQuotes().ToList();

    try
    {
        switch (tokens.IdentifyState(commandRunnerApi.KnownCommands.Keys))
        {
            case CommandState.Empty:
                continue;
            case CommandState.NotACommand:
                ScriptFileRunner.EvaluateExpression(appState, expressionFactory, evaluatorApi, line);
                break;
            case CommandState.KnownCommand:
                await runner.RunAsync(tokens);
                break;
            case CommandState.UnknownCommand:
                AnsiConsole.WriteLine($"Unknown command: {tokens[0]}");
                logger.LogWarning("Unknown command: {command}", tokens[0]);
                break;
        }
        Terminal.ShellIntegration.CommandFinished(0);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while executing the command: {Command}", line);

        if (ex is InvalidOperationException or OverflowException)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]{ex.Message}[/]");
            Terminal.ShellIntegration.CommandFinished(-1);
            continue;
        }
       
#if DEBUG
        AnsiConsole.WriteException(ex, ExceptionFormats.Default);
#else
        AnsiConsole.WriteException(ex, ExceptionFormats.NoStackTrace);
#endif
    }
    finally
    {
        AnsiConsole.WriteLine();
    }
}
