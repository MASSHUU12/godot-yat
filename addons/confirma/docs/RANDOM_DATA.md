<div align="center">
 <h3>Generating random data</h1>
</div>

Confirma includes various simple extensions to the Random class, allowing you to generate random data for tests.

These extensions include:

- **RandomNumberExtensions**
  - NextInt32()
  - NextDigit()
  - NextNonZeroDigit()
  - NextDecimal()
  - NextDecimal(bool sign)
  - NextDecimal(decimal maxValue)
  - NextDecimal(decimal minValue, decimal maxValue)
  - NextNonNegativeDecimal()
  - NextLong(long maxValue)
  - NextLong(long minValue, long maxValue)
  - NextNonNegativeLong()
- **RandomUuidExtensions**
  - NextUuid4()
