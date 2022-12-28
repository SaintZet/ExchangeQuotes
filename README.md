# Exchange Quotes

Test assignment on position .NET middle developer for a Ukrainian company [Profit Center FX](https://profitcenterfx.com).

The project used DepenedencyInjection, Modular Monolith design, xUnit tests for math logic, concurrent velues for thread safe math library.

## The task

### Server application

Infinitely generates random numbers in the range (to emulate the subject area - the flow of quotes from the stock exchange), sends multicast to
udp without delay.

The range and multicast group is configured through a separate xml config.

### Client application

It accepts data by udp, counts for all received: arithmetic mean, standard deviation, mode and median.
The total number of packets received can be from a trillion or more.

The calculated values are output to the console on demand (pressing enter).

The application must control the receipt of all packets, the number of lost packets must be displayed together with statistics (to force packet loss, you need to add a delay to receiving messages,
approximately once per second).

Receiving packets and counting should be implemented in different streams with minimal delays.

Multicast group and receive delay must be configured through a separate xml config (not in app.config).

**Important requirement**: the application must be optimized for speed as much as possible
work, taking into account the amount of data received and issue a solution as quickly as possible (in
within a few milliseconds) - for exchanges, every microsecond matters.

The application must work for a long time (week-month) without crashes
internal errors, as well as in case of network errors.

## Getting Started

### Dependencies

.NET 6.0 Runtime

### Installing

Just clone repo on PC.

### Executing program

Set up a client and server multi-launch, or just launch a server instance and a number of client instances.

## Authors

Chepets Serhii <br /> 
Contacts: [LinkedIn](https://www.linkedin.com/in/serhii-chepets-412b46223/) / [GitHub](https://github.com/SaintZet) / [Telegram](https://t.me/SaintZet)

## Version History

* v1.0.0
    * Initial Release

## License

This project is unlicense.

## Acknowledgments

* [Median in a stream of integers](https://www.geeksforgeeks.org/median-of-stream-of-integers-running-integers/)
* [Heaps: Find the Running Median](https://www.hackerrank.com/challenges/ctci-find-the-running-median/problem)
* [Standard Deviation on Streaming Data](https://nestedsoftware.com/2018/03/27/calculating-standard-deviation-on-streaming-data-253l.23919.html)
* [Simple Moving Average calculator](https://andrewlock.net/series/creating-a-simple-moving-average-calculator-in-csharp/)
* [Thread safety in C#](https://medium.com/@supriyaghevade77/thread-safety-in-c-b144a5d9731c#:~:text=Thread%20safety%20is%20the%20technique,run%20concurrently%20without%20break%20function.)
* [Thread-Safe Collections](https://learn.microsoft.com/en-us/dotnet/standard/collections/thread-safe/)
