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
    Modulo                  = 128,
    And                     = 256,
    Or                      = 512,
    LessThan                = 1024,
    LessThanOrEqual         = 2048,
    GreaterThan             = 4096,
    GreaterThanOrEqual      = 8192,
    Equal                   = 16384,
    NotEqual                = 32768,
    Exponent                = 65536,
    Function                = 131072,
    ArgumentDivider         = 262144,
    MemberAccess            = 524288,
    Assignment              = 1048576,

    OpenParen               = 0x20000000,
    CloseParen              = 0x40000000,
    Eof                     = 0x80000000
}