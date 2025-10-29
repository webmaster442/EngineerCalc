# Abs

Returns the absolute value of a given number.

`Abs(value)`

## Parameters

* value

     A number, whose absolute value will be returned.

# Avg

Returns the average value from the given parameters. 

`avg(parameters)`

## Parameters

   This function accepts variable ammount of parameters, but at least one must be provided.

# Bin

Parses a binary (base-2) string and converts it into a long integer. The string must consist only of characters 0 and 1. Throws an exception if the string is not a valid binary representation.

`bin(string)`

## Parameters

* string

    The binary string to convert. Should only contain '0' and '1' characters and no prefix like 0b.

# Bits

Couns the number of bits needed to represent the given integer in binary form.

`bits(number)`

## Parameters

* number

    A number whose bit count is to be determined. If the number is zero, the function returns 1. For negative numbers, the function counts the bits in the absolute value and adds one additional bit for the sign.

# ArcCos

Returns the angle whose cosine is the specified number.

`ArcCos(value)`

## Parameters

* value

     A number representing a cosine, where value must be greater than or equal to -1, but less than or equal to 1.

# ArcCtg

Returns the angle whose cotangent is the specified number.

`ArcCtg(value)`

## Parameters

* value

     An angle, θ, measured in radians, such that -π/2 ≤ θ ≤ π/2.

# ArcSin

Returns the angle whose sine is the specified number.

`ArcSin(value)`

## Parameters

* value

     A number representing a sine, where value must be greater than or equal to -1, but less than or equal to 1.

# ArcTan

Returns the angle whose tangent is the specified number.

`ArcTan(value)`

## Parameters

* value

     An angle, θ, measured in radians, such that -π/2 ≤ θ ≤ π/2.

# Ceiling

Returns the smallest integral value that is greater than or equal to the specified double-precision floating-point number. Note that this method returns a double instead of an integral type.

`Ceiling(value)`

## Parameters

* value

     A number to be rounded.

# Cos

Computes the cosine of the given input. Supports both double and Complex types. If the input is neither, an Exception is thrown.

`cos(value)`

## Parameters

* value

    The input value for which to calculate the cosine. Can be a double, a Complex number, or any type that can be converted to double.

# Count

Returns the number of elements passed to the function. If it has been called with a single parameter that is a string, it will return the length of the string.

`Count(numbers)`
`Count(string)`

## Parameters

* numbers

    Numbers of any type. The function will return the count of the numbers provided.

* string

    A string. The function will return the length of the string.

# Cplx

Create a complex number from real and imaginary number part.

`Cplx(real, imaginary)`

## Parameters

* real

    The real part of the complex number

* imaginary

    The imaginary part of the complex number

# Ctg

Computes the cotangent (1 / tangent) of the given input. Supports both double and Complex types. If the input is neither, an Exception is thrown.

`ctg(value)`

## Parameters

* value

    The input value for which to calculate the tangent. Can be a double, a Complex number, or any type that can be converted to double.

# Deg

Converts an angle given in radians to degrees.

`deg(radians)`

## Parameters

* radians

    An angle expressed in radians

# DegToRad

Converts an angle given in degrees to radians.

`degtorad(degrees)`

## Parameters

* degrees

    An angle expressed in degrees


# Floor

Returns the largest integral value less than or equal to the specified double-precision floating-point number. Note that this method returns a double instead of an integral type.

`Floor(value)`

## Parameters

* value

     A number.

# Grad

Converts an angle given in radians to gradians.

`grad(radians)`

## Parameters

* radians

    An angle expressed in radians

# GradToRad

Converts an angle given in gradians to radians.

`gradtorad(gradians)`

## Parameters

* gradians

    An angle expressed in gradians

# Hex

Parses a hexadecimal string and converts it into a long integer. Supports both uppercase and lowercase hexadecimal digits. Throws an exception if the string is not a valid hexadecimal representation.

`Hex(string)`

## Parameters

* string

    The hexadecimal string to convert. Should not include prefixes like 0x.

# Lcm

Calculate the Least Common Multiple (LCM) of two integers. The LCM of two integers is the smallest positive integer that is divisible by both numbers.

`Lcm(a, b)`

## Parameters

* a

    The first integer.

* b

     The second integer.

# Ln

Computes the natural logarithm (base e) of a given value. Supports both real and complex numbers. If the input is neither, an Exception is thrown.

`Ln(value)`

## Parameters

* value

    The input value to compute the natural logarithm of. Can be a real number, complex number, or a value convertible to a double.

# Log

Computes the logarithm of a number (number) with a specified base (@base). Supports both double and Complex types. If the input is neither, an Exception is thrown.

`Log(number, base)`

## Parameters

* number

    The number for which the logarithm is calculated. Can be a double or a Complex number.

* base

    The base of the logarithm. Must be convertible to a double.

# Min

Returns the minimum value from the given parameters.

`min(parameters)`

## Parameters

   This function accepts variable ammount of parameters, but at least one must be provided.

# Max

Returns the maximum value from the given parameters. 

`max(parameters)`

## Parameters

   This function accepts variable ammount of parameters, but at least one must be provided.

# Primefactors

Returns the prime factors of a given integer as a string.

`Primefactors(number)`

## Parameters

* number
    An integer whose prime factors are to be calculated.

# Random

Generates a non negative random number. If called without parameters, the value will be between 0 and (2^63) - 1. If called with parameters, then the value that is generated will be in the specified interval by the parameters. If called with parameters and the specified min value is greater than the specified max value an exception will be thrown.

`Random()`
`Random(min, max)`

## Parameters

* min

    The inclusive lower bound of the random number returned.

* max

    The exclusive upper bound of the random number returned. Must be greater than or equal to min. 

# Root

Calculates the nth root of a given number. Supports both double and Complex types. If the input is neither, an Exception is thrown.

`Root(y, y)`

## Parameters

* x

    The number whose root is to be calculated. Can be a double or a Complex number.

* y

    The degree of the root. Must be convertible to a double.

# Round

Rounds a double-precision floating-point value to a specified number of fractional digits, and rounds midpoint values to the nearest even number.

`round(value, digits)`

## Parameters

* value

     A number to be rounded.

* digits

    The number of fractional digits in the return value.

# Sin

Computes the sine of the given input. Supports both double and Complex types. If the input is neither, an Exception is thrown.

`sin(value)`

## Parameters

* value

    The input value for which to calculate the sine. Can be a double, a Complex number, or any type that can be converted to double.

# Tan

Computes the tangent of the given input. Supports both double and Complex types. If the input is neither, an Exception is thrown.

`tan(value)`

## Parameters

* value

    The input value for which to calculate the tangent. Can be a double, a Complex number, or any type that can be converted to double.

# ToBin

Converts an integer to its binary representation string.

`ToBin(number)`

## Parameters

* number

    An input integer number that will be converted to its binary representation

# ToHex

Converts an integer to its hexadecimal representation string.

`ToHex(number)`

## Parameters

* number

    An input integer number that will be converted to its hexadecimal representation

# ValueUnit

Create a value with a unit. The unit is represented as a string. The value must be a double type. 
Basic operations between values with units are supported, but the units must be compatible.

`ValueUnit(value, unit)`

## Parameters

* value
    The numeric value. Must be a double type.

* unit

    The unit of the value. Must be a string.

# Vect

Create a vector from the given parameters. The parameters can be of any type, but they must be convertible to double.
Parameter number can be 2, 3, or 4. If more than 4 or less than 1 parameters are provided, an exception will be thrown.

`Vect(x, y)`
`Vect(x, y, z)`
`Vect(x, y, z, w)`

## Parameters

* x

    The first component of the vector.

* y
    
    The second component of the vector.

* z

    The third component of the vector. Optional, if not provided, the vector will be 2D.

* w

    The fourth component of the vector. Optional, if not provided, the vector will be 2D or 3D depending on the number of parameters provided.
    