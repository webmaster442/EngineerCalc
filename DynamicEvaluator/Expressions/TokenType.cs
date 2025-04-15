namespace DynamicEvaluator.Expressions;

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
    And = 128,
    Or = 256,
    Exponent = 512,
    Function = 1024,
    ArgumentDivider = 2048,

    OpenParen = 0x20000000,
    CloseParen = 0x40000000,
    Eof = 0x80000000
}