# backgroundmq

[English](README.md) | [Русский](README.ru.md)

The file message broker is best deployed as a background application to ensure that it is always running and processes messages in the background.

To ensure high performance of the file message broker, it is recommended to use multi-threaded/asynchronous programming. An asynchronous approach, in which tasks are processed in parallel, can significantly improve the speed of message processing.
