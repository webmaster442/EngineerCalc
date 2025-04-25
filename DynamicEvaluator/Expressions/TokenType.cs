namespace DynamicEvaluator.Expressions;

[Flags]
internal enum TokenType : uint
{
    None                    = 0,
    Constant                = 1,
    Variable                = 2,
    Not                     = 4,
    Plus                    = 8,
    Minus                   = 16,
    Multiply                = 32,
    Divide                  = 64,
    And                     = 128,
    Or                      = 256,
    LessThan                = 512,
    LessThanOrEqual         = 1024,
    GreaterThan             = 2048,
    GreaterThanOrEqual      = 4096,
    Equal                   = 8192,
    NotEqual                = 16384,
    Exponent                = 32768,
    Function                = 65536,
    ArgumentDivider         = 131072,

    OpenParen               = 0x20000000,
    CloseParen              = 0x40000000,
    Eof                     = 0x80000000
}