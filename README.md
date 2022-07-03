# Linq.Fluent.PredicateBuilder

## Fluent predicate builder for Linq

Using the Linq extension is really simple.

Basically, provide a predicate to the Linq Where method.

Using the **fluent PredicateBuilder**, you can perform

* **And**
* **AndAlso**
* **Or**
* **OrElse**

operations, to build your predicate.

In below example, an item is matched if any of the 3 predicates match.

```C#
var list = Enumerable.Range(1, 10000000);

var predicate = new PredicateBuilder<int>()
                        .Initial(x => x >= 12 && x <= 13)
                        .Or(x => x <= 2)
                        .Or(x => x > 99999999)
                .ToPredicate();

var result = list.Where(predicate)
                 .ToList();

Assert.Equal(4, result.Count());
```
