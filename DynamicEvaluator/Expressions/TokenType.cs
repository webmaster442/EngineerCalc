//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace DynamicEvaluator.Expressions;

[Flags]
internal enum TokenType : uint
{
    None = 0,
    Constant = 1,
    Variable = 2,
    Not = 4,
    Plus = 8,
    Minus = 16,
    Multiply = 32,
    Divide = 64,
    Modulo = 128,
    And = 256,
    Or = 512,
    LessThan = 1024,
    LessThanOrEqual = 2048,
    GreaterThan = 4096,
    GreaterThanOrEqual = 8192,
    Equal = 16384,
    NotEqual = 32768,
    Exponent = 65536,
    Function = 131072,
    ArgumentDivider = 262144,
    MemberAccess = 524288,
    TennaryIf = 1048576,
    TennaryElse = 2097152,

    Assignment = 0x10000000,
    OpenParen = 0x20000000,
    CloseParen = 0x40000000,
    Eof = 0x80000000
}
