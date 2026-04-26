using System.ComponentModel;
using System.IO.Hashing;

using EngineerCalc.Api;
using EngineerCalc.Commands.Abstraction;
using EngineerCalc.Models;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class HashCommand : FileSystemCommand<HashCommand.Arguments>
{
    internal sealed class Arguments : CommandSettings
    {
        [CommandArgument(0, "<file>")]
        [Description("The file to calculate the hash for.")]
        public string File { get; set; } = string.Empty;

        [CommandOption("-a|--algorithm")]
        [Description("The hash algorithm to use.")]
        public string HashAlgorithm { get; set; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (string.IsNullOrEmpty(HashAlgorithm))
                return ValidationResult.Error("Hash algorithm cannot be empty.");

            if (!Enum.TryParse<HashAlgorithm>(HashAlgorithm, true, out _))
                return ValidationResult.Error($"Invalid hash algorithm: {HashAlgorithm}. Supported algorithms are: {string.Join(", ", Enum.GetNames<HashAlgorithm>())}");

            return string.IsNullOrWhiteSpace(File)
                ? ValidationResult.Error("File cannot be empty.")
                : base.Validate();
        }
    }

    public HashCommand(IFileSystem fileSystem, State state) 
        : base(fileSystem, state)
    {
    }

    private async Task<byte[]> CryptoHash(System.Security.Cryptography.HashAlgorithm algo,
                                          Stream stream,
                                          ProgressTask task,
                                          CancellationToken cancellationToken)
    {
        byte[] buffer = AllocateBuffer();
        int bytesRead = 0;
        long size = stream.Length;
        byte[] readAheadBuffer = AllocateBuffer();

        int readAheadBytesRead = await stream.ReadAsync(readAheadBuffer, cancellationToken);
        task.Increment(readAheadBytesRead);

        do
        {
            cancellationToken.ThrowIfCancellationRequested();
            bytesRead = readAheadBytesRead;
            Array.Copy(readAheadBuffer, buffer, buffer.Length); // buffer = readAheadBuffer;
            Array.Clear(readAheadBuffer); //readAheadBuffer = new byte[BufferSize];
            readAheadBytesRead = await stream.ReadAsync(readAheadBuffer, cancellationToken);

            task.Increment(readAheadBytesRead);

            if (readAheadBytesRead == 0)
                algo.TransformFinalBlock(buffer, 0, bytesRead);
            else
                algo.TransformBlock(buffer, 0, bytesRead, buffer, 0);

        }
        while (readAheadBytesRead != 0);

        return algo.Hash ?? throw new InvalidOperationException("Hash calculation failed.");
    }

    private async Task<byte[]> NonCryptoHash(NonCryptographicHashAlgorithm algo,
                                     Stream stream,
                                     ProgressTask task,
                                     CancellationToken cancellationToken)
    {
        byte[] buffer = AllocateBuffer();
        int bytesRead = 0;
        do
        {
            bytesRead = await stream.ReadAsync(buffer, cancellationToken);
            algo.Append(buffer.AsSpan(0, bytesRead));
            task.Increment(bytesRead);
        }
        while (bytesRead > 0);

        return algo.GetCurrentHash();

    }

    protected override async Task<int> ExecuteAsync(CommandContext context, Arguments settings, CancellationToken cancellationToken)
    {
        var filePath = GetFullPath(settings.File);
        if (!_fileSystem.FileExists(filePath))
        {
            throw new InvalidOperationException($"File '{filePath}' does not exist.");
        }

        using var stream = _fileSystem.OpenRead(filePath);
        var hashAlgorithm = Enum.Parse<HashAlgorithm>(settings.HashAlgorithm, true);

        HashResult result;
        if (IsCryptoHashAlgorithm(hashAlgorithm))
        {
            using var algo = CreateCryptoHashAlgorithm(hashAlgorithm);
            await AnsiConsole.Progress().StartAsync(async ctx =>
            {
                ProgressTask task = ctx.AddTask("Calculating hash...", maxValue: stream.Length);
                await CryptoHash(algo, stream, task, cancellationToken);
            });
            result = new(algo.Hash);
        }
        else
        {
            var algo = CreateNonCryptoHashAlgorithm(hashAlgorithm);
            await AnsiConsole.Progress().StartAsync(async ctx =>
            {
                ProgressTask task = ctx.AddTask("Calculating hash...", maxValue: stream.Length);
                await NonCryptoHash(algo, stream, task, cancellationToken);
            });
            result = new(algo.GetCurrentHash());
        }

        AnsiConsole.MarkupLine($"{result.ToString()}");

        return ExitCodes.Success;
    }
}
