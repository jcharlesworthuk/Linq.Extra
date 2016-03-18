# Linq.Extra
Set of LINQ-style extension methods that add useful functions not present in System.Linq

## Page(pageNumber, pageSize)

Returns a chunk of the collection at a specific page number and size

## WithAction(action)

Returns the source enumerable with the supplied action invoked on each item.

## Enumerate()

Forces the collection to enumerate and does not return a value, Similar to IList.ForEach() without any operations