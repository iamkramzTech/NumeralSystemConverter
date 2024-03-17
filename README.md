# NumeralSystemConverter Class Library

## Description

NumeralSystemConverter Class Library is a versatile library designed to facilitate operations involving various numeral systems. It offers seamless conversion between different numeral systems and provides support for two's complement representation, particularly for handling negative values.

### Features

- **BaseConverter** This static class enables conversion between the following numeral systems:
  - Decimal to Binary
  - Binary to Decimal
  - Decimal to Octal
  - Octal to Decimal
  - Decimal to Hexadecimal
  - Hexadecimal to Decimal

### Limitation

- Currently, the library does not support 64-bit signed integers.

## Usage

To use NumeralSystemClassLibrary in your project, simply import the library and utilize its provided functions for numeral system operations.

```csharp
// Example usage in C#

// Import the NumeralSystemClassLibrary namespace
using NumeralSystemConverter;

// Convert 32 bits signed integer to binary
var decimalNumber = -255;

var binaryNumber = BaseConverter.DecimalToBinary(decimalNumber);
Console.WriteLine($"Binary representation of {decimalValue} is: {binaryNumber}")

// Convert 32 bits binary to decimal
var binaryNumber = "11111111111111111111111100000001";

var decimalNumber = BaseConverter.BinaryToDecimal(binaryNumber);
Console.WriteLine($"Decimal representation of {binaryNumber} is: {decimalNumber}");

// Expected output:
// Binary representation of -255 is: 11111111111111111111111100000001
// Decimal representation of 11111111111111111111111100000001 is: -255
