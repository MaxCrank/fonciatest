# Max Shnurenok Foncia test

## The task

The task can be read in a [separate document](/TASK.md).

## Decisions that were made

### Design

1. The flow was considered as following:
1.1. Add items to stock and set prices.
1.2. Integrate payment service.
1.3. Integrate scanning service.
1.4. Scan goods and get updated prices calculation for a check instantly.
1.5. Perform sale or cancel.
1.6. Go to p. 1.4.
I went with the POS terminal interface like this as a one which I assume to be more realistic. Thus, additional services were defined to corresponding with this decision.
2. POS terminal is assumed to work in a non-concurrent environment, while price management, payments, and stock management in a concurrent one. Code scanning is naturally stateless, so for it there's no difference.
3. I decided to add possibility of having as many volumes as required (i.e. not only one) - this was covered with tests. The decision was made since in the real solution it would be one of the most obvious future-proof advancements which might not be stated clearly, still should be done anyway.

### Code

1. All storages were emulated with in-memory data structures. 
2. The library was made to be a part of business domain.
3. All service implementations were made internal and assumed to be added to needed solutions using .NET Core DI container extension (e.g. ServiceCollection extension) from the same library where services reside (was not done in scope of this test task).

### Tests

Since I'm very low on time and mental resources, it was decided to go only with basic test listed in the task and additional set of use-cases for multi-volume discounts.

## Summary

My strong opinion is that the investment in  business analysis (and in non-functional requirements as well, just here it's not the case) is the most important in any design process, thus I consider the difference between task interface requirements and mine as an improvement. Hope your approach to problem solving is similar.

The solution can be tested with `dotnet test ./src/Foncia.POS.sln`.