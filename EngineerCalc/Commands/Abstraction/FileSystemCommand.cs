using System;
using System.Buffers;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

using EngineerCalc.Api;

using Spectre.Console.Cli;

namespace EngineerCalc.Commands.Abstraction;

internal abstract class FileSystemCommand<TArguments> : AsyncCommand<TArguments>
    where TArguments : CommandSettings
{
    protected readonly IFileSystem _fileSystem;
    protected readonly State _state;

    public FileSystemCommand(IFileSystem fileSystem, State state)
    {
        _fileSystem = fileSystem;
        _state = state;
    }

    protected string GetFullPath(string file)
    {
        if (Path.IsPathFullyQualified(file))
            return file;

        return Path.GetFullPath(Path.Combine(_state.CurrentDirectory, file));
    }

    private const int BufferSize = 4096;

    protected byte[] AllocateBuffer()
        => ArrayPool<byte>.Shared.Rent(BufferSize);

    protected HashAlgorithm CreateAlgorithm(Models.HashAlgorithm algorithm)
        => algorithm switch
        {
            Models.HashAlgorithm.Md5 => MD5.Create(),
            Models.HashAlgorithm.Sha1 => SHA1.Create(),
            Models.HashAlgorithm.Sha256 => SHA256.Create(),
            Models.HashAlgorithm.Sha384 => SHA384.Create(),
            Models.HashAlgorithm.Sha512 => SHA512.Create(),
            Models.HashAlgorithm.Sha3_256 => SHA3_256.Create(),
            Models.HashAlgorithm.Sha3_384 => SHA3_384.Create(),
            Models.HashAlgorithm.Sha3_512 => SHA3_512.Create(),
            _ => throw new InvalidOperationException($"Unsupported hash algorithm: {algorithm}")
        };
}
