data-structures-net
===================

Data structures that I use in my .Net projects.

**Immutable tree and Continuous set.**

Immutable tree
--------------

Immutable structures cannot be changed after they are created,
you must replace the entire thing when changes must be made.
Good for multi-threading, functional programming, few-writes/many-reads approaches.
Tree builders are available (e.g. convert flat data to tree, e.g. DB table).
Uses tree visitors to change tree.

Continuous set
--------------

Continuous set is a set that is not discrete. It's better explained with an example:
a set with all values between 1.1 (inclusive) and 2.7 (exclusive).
Can have multiple sequences, actually can represent a set of any type
for which an IComparable<T> can be implemented (Double, String, DateTime, Int32).
Set operations available (e.g. union, exclusion, intersection)
