using System;
using System.Collections.Generic;

Func<int, int, int> sum = (x, y) => x + y;
Console.WriteLine(sum(5, 4));