# concurrency-mastering
Repository for tasks that help me to play with concurrency, multithreading, asynchronous in .net

## ReadWrite Lock
### Problem statement
Imagine you have an application where you have multiple readers and a single writer. You are asked to design a lock which lets multiple readers read at the same time, but only one writer write at a time.

## Dining Philosopher
### Problem statement
Imagine you have five philosophers sitting around a roundtable. The philosophers do only two kinds of activities. One: they contemplate, and two: they eat.
However, they have only five forks between themselves to eat their food with. Each philosopher requires both the fork to his left and the fork to his right to eat his food.
Design a solution where each philosopher gets a chance to eat his food without causing a deadlock.

### Sources
[Educative](https://www.educative.io/blog/top-five-concurrency-interview-questions-for-software-engineers)
