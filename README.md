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

Also, You can create predicates based on **conditions**.

Eg. In below query, all employees are returned if **restrict** is false.

Only 1 employee is returned if restrict is true.

```C#
[Theory(DisplayName = "Test_Fluent_PredicateBuilder_Conditional")]
[InlineData(true)]
[InlineData(false)]
public void Test_Fluent_PredicateBuilder_Conditional(bool restrict)
{
    var list = new List<Employee>
    {
        new Employee { Id = 1, Name = "Cliff", Department = "Human Resources"},
        new Employee { Id = 2, Name = "John", Department = "IT"}
    };

    var predicate = new PredicateBuilder<Employee>()
                            .Initial(e => true)
                            .And(restrict,  e => e.Department == "IT")
                    .ToPredicate();

    var result = list.Where(predicate)
                     .ToList();

    if (restrict)
    {
        Assert.Single(result);
        Assert.Equal("John", result[0].Name);
    }                
    else
    {
        Assert.Equal(2, result.Count());
        Assert.Equal("Cliff", result[0].Name);
        Assert.Equal("John", result[1].Name);
    }                
}
```

**Note:**

**ToPredicate** produces a predicate (Func<T, bool>).

This can be used with IEnumerable\<T\>.Where.


**ToExpressionPredicate** produces an expression predicate (Expression<Func<T, bool>>).

This can be used with IQueryable\<T\>.Where

## Linq Where extensions

There is also an easy to use Linq Where extensions.

### Based on operation

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

### With Predicate Builder

```C#
var list = Enumerable.Range(1, 10000000);

var result = list.Where(builder => builder.Initial(x => x >= 12 && x <= 13)
                                          .Or(x => x <= 2)
                                          .Or(x => x > 99999999)
                                    .ToPredicate())
                 .ToList();

Assert.Equal(4, result.Count());
```

**Note:**

There are 2 Where extensions each for:

* IEnumerable\<T\>.Where
* IQueryable\<T\>.Where
