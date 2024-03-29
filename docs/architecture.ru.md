# architecture

[English](architecture.md) | [Русский](architecture.ru.md)

## Основные детали

### На что нужно обращать внимание при проектировании 

При проектировании файлового брокера сообщений стоит обратить внимание на:
- Безопасность доступа к директории и файлам.
- Обработку ошибок при чтении/записи файлов.
- Механизмы синхронизации доступа к файлам, чтобы избежать конфликтов при одновременной записи/чтении.
- Оптимизацию процесса записи и чтения сообщений для обеспечения высокой производительности.

### Основные компоненты файлового брокера сообщений

Основные компоненты брокера сообщений, который сохраняет очередь сообщений в директорию, могут включать:
- Класс для работы с директорией и файлами.
- Механизмы для добавления сообщений в очередь и извлечения сообщений из очереди.
- Методы для сохранения запросов и ответов в файлах согласно требованиям.
- Логика для расчета ключа для сохранения файла по формуле MD5.

### Структурирование и именование директорий

Для структурирования и именования директорий можно использовать подход, где каждый запрос и соответствующий ответ хранятся в отдельной директории с уникальным идентификатором (например, по временной метке).

### Концепты для высокопроизводительного файлового брокера

Концепты для высокопроизводительного файлового брокера:
- Оптимизация работы с файловой системой
- Пул потоков для эффективного управления потоками
- Использование кэширования для быстрого доступа к данным
- Оптимизация алгоритмов обработки сообщений
- Механизмы мониторинга и управления нагрузкой
- Распределенная архитектура для горизонтального масштабирования
- Использование высокопроизводительных структур данных
- Оптимизация сетевого взаимодействия
- Обработка ошибок и восстановление после сбоев
- Тестирование и профилирование производительности

### Механизмы обеспечения отказоустойчивости 

Для предотвращения потери сообщений при падении сервиса, можно использовать механизмы персистентности, например, сохранять сообщения на диск перед отправкой на брокер сообщений или использовать очереди сообщений с поддержкой гарантированной доставки.

## Выбор базы данных 

### SQLite

Плюсы развертывания базы данных SQLite для файлового брокера сообщений:
1. Простота внедрения и использования, так как SQLite не требует установки отдельного сервера базы данных.
2. Легкость в поддержке и обслуживании, так как база данных SQLite представляет собой простой файл на диске.
3. Высокая производительность для небольших объемов данных и низкой нагрузки.
4. Поддержка транзакций и ACID-свойств.
5. Поддержка SQL-запросов и индексирования данных.
6. Портативность, так как файл базы данных SQLite можно легко перемещать между различными системами.
7. Низкие требования к ресурсам системы.
8. Встроенная поддержка шифрования данных.
9. Возможность использовать в памяти базу данных для повышения производительности.
10. Бесплатная и open-source лицензия.

Минусы развертывания базы данных SQLite для файлового брокера сообщений:
1. Ограничения по масштабируемости и производительности для больших объемов данных и высокой нагрузки.
2. Отсутствие поддержки удаленного доступа к базе данных.
3. Ограниченные возможности администрирования и мониторинга.
4. Не поддерживает некоторые продвинутые функции SQL.
5. Ограниченная поддержка хранимых процедур и триггеров.
6. Ограниченная возможность параллельной работы с несколькими потоками или процессами.
7. Ограниченная поддержка типов данных.
8. Не поддерживает репликацию данных.
9. Ограниченная возможность масштабирования горизонтально.
10. Не поддерживает распределенные транзакции.

### PostgreSQL

Плюсы развертывания базы данных PostgreSQL для файлового брокера сообщений:
1. Высокая производительность и масштабируемость для больших объемов данных и высокой нагрузки.
2. Полная поддержка SQL-стандартов и продвинутых функций SQL.
3. Поддержка хранимых процедур, триггеров, пользовательских функций и расширений.
4. Поддержка репликации данных для обеспечения отказоустойчивости и масштабируемости.
5. Поддержка партиционирования таблиц для улучшения производительности запросов.
6. Широкие возможности администрирования и мониторинга базы данных.
7. Поддержка распределенных транзакций и транзакций с точками сохранения.
8. Высокий уровень безопасности и возможность настройки прав доступа.
9. Поддержка индексирования данных для оптимизации запросов.
10. Активное сообщество пользователей и разработчиков, что обеспечивает быструю поддержку и развитие.

Минусы развертывания базы данных PostgreSQL для файлового брокера сообщений:
1. Большие требования к ресурсам системы.
2. Сложность внедрения и настройки, особенно для новичков.
3. Необходимость установки и настройки отдельного сервера базы данных.
4. Возможные проблемы с масштабированием вертикально из-за высоких требований к ресурсам.
5. Не самая лучшая производительность для небольших объемов данных и низкой нагрузки.
6. Возможные проблемы с производительностью при сложных запросах.
7. Необходимость регулярного обновления и управления версиями базы данных PostgreSQL.
8. Возможные проблемы с резервным копированием и восстановлением данных.
9. Не всегда подходит для небольших проектов из-за сложности использования.
10. Некоторые ограничения по портативности и переносимости между различными системами.
