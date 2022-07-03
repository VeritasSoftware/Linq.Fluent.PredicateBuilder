# Linq.Fluent.PredicateBuilder

## Fluent Predicate Builder

Basically, you have to provide a predicate to the Linq Where method.

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

**Note:**

**ToPredicate** produces a predicate (Func<T, bool>).

This can be used with IEnumerable\<T\>.Where.


**ToExpressionPredicate** produces an expression predicate (Expression<Func<T, bool>>).

This can be used with IQueryable\<T\>.Where

## Linq Where extension

There is also an easy to use Linq Where extension.

Just select the operation and specify the predicates.

All predicates are combined as per the specified operation by the extension.

```C#
var list = Enumerable.Range(1, 10000000);

var result = list.Where(Operation.Or,
                            x => x >= 12 && x <= 13,
                            x => x <= 2,
                            x => x > 99999999)
                 .ToList();

Assert.Equal(4, result.Count());
```

**Note:**

There are 2 Where extensions.

* IEnumerable\<T\>.Where
* IQueryable\<T\>.Where
