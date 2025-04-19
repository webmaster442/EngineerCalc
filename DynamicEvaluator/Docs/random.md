# Random

Generates a non negative random number. If called without parameters, the value will be between 0 and (2^63) - 1. If called with parameters, then the value that is generated will be in the specified interval by the parameters. If called with parameters and the specified min value is greater than the specified max value an exception will be thrown.

`Random()`
`Random(min, max)`

## Parameters

* min

    The inclusive lower bound of the random number returned.

* max

    The exclusive upper bound of the random number returned. Must be greater than or equal to min. 