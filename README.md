# Linq.Extra
Set of LINQ-style extension methods that add useful functions not present in System.Linq

# Usage

Download, build, import.  Or you can just copy the extension methods you need striaight from [EnumerableExtensions.cs](JCharlesworth.Linq.Extra/EnumerableExtensions.cs) and [DictionaryExtensions.cs](JCharlesworth.Linq.Extra/DictionaryExtensions.cs)

![Linq.Extra code screenshot](https://raw.github.com/jcharlesworthuk/Linq.Extra/master/screenshot.PNG)

# Included Collection Extension Methods

## Page(pageNumber, pageSize)

Returns a chunk of the collection at a specific page number and size

## WithAction(action)

Returns the source enumerable with the supplied action invoked on each item.

## Enumerate()

Forces the collection to enumerate and does not return a value, Similar to IList.ForEach() without any operations

## Interweave(other)

Returns alternate elements of this and the supplied collections.

## EveryOther(every)

Returns a smaller collection consisting of every X elements in the source.

## ToCollection()

Creates a Collection<T> from an IEnumerable<T>.

## NotNull()

Returns only the elements that are not null

## Duplicates()

Returns the duplicate items

## Duplicates(comparer)

Returns the duplicate items using the specified comparer

## FirstOrDefault(compareTo, comparer)

FirstOrDefault overload that takes an equality comparer

## SingleOrDefault(compareTo, comparer)

SingleOrDefault overload that takes an equality comparer

## LastOrDefault(compareTo, comparer)

LastOrDefault overload that takes an equality comparer

## SelectManyMany(selectorA, selectorB)

Projects each element of a sequence to an IEnumerable<T> and flattens the resulting two sequences into one sequence.

## Expand(selector)

Performs a SelectMany() but returning a tuple containing both the parent and the child element.

## Distinct(selector)

Returns distinct elements from a sequence using a selector function


