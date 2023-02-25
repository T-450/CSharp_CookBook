### Enumeration class in C# using records
The record implementation is faster and allocates less than the other implementations. This is because we are using the lazy generic approach that allows us to cache all items so that we only pay the reflection cost once. It hasn't anything to do with using records, really. The only thing that the record helps us with is that we need to write less code (Equals etc). :)

So once again, the "generic approach" that we're using in the record implementation will work just as good in a "regular class implementation".

Another thing that greatly improves the performance is that we are using a Dictionary when doing the FromName and FromValue lookup.


#### Benchmarks

|              Method | Categories |      Mean |    Error |   StdDev |    Median | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|-------------------- |----------- |----------:|---------:|---------:|----------:|------:|--------:|-------:|----------:|------------:|
|  FromName_Microsoft |   FromName | 148.71 ns | 2.985 ns | 5.228 ns | 146.31 ns |  1.00 |    0.00 | 0.0381 |     240 B |        1.00 |
|     FromName_Record |   FromName |  17.84 ns | 0.162 ns | 0.143 ns |  17.82 ns |  0.12 |    0.00 |      - |         - |        0.00 |
|                     |            |           |          |          |           |       |         |        |           |             |
| FromValue_Microsoft |  FromValue | 192.86 ns | 3.731 ns | 5.585 ns | 192.47 ns |  1.00 |    0.00 | 0.0381 |     240 B |        1.00 |
|    FromValue_Record |  FromValue |  10.89 ns | 0.087 ns | 0.081 ns |  10.88 ns |  0.06 |    0.00 |      - |         - |        0.00 |
|                     |            |           |          |          |           |       |         |        |           |             |
|    GetAll_Microsoft |     GetAll | 240.63 ns | 4.043 ns | 3.782 ns | 239.61 ns |  1.00 |    0.00 | 0.0381 |     240 B |        1.00 |
|       GetAll_Record |     GetAll |  23.43 ns | 0.324 ns | 0.287 ns |  23.32 ns |  0.10 |    0.00 | 0.0127 |      80 B |        0.33 |

Full article: [Josef Ottosson](https://josef.codes/enumeration-class-in-c-sharp-using-records/)

Source: [joseftw](https://github.com/joseftw/jos.enumeration?WT.mc_id=DT-MVP-5004074)