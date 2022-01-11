# TelegramSimpleService

---

### RU
* [Поддерживаемые платформы]
* [Руководство использования]
    * [Инициализация сервиса]
    * [Инициализация телеграм клиентов]
    * [Отправка логов]
    * [Реализованные типы]

---

[Поддерживаемые платформы]:ru_platforms
## Поддерживаемые платформы:
Проект нацелен как минимум на .NET Standard 2.0 и .NET Core 3.1.

[Руководство использования]:ru_getting_started
## Руководство использования:
Сервис написан на основе [TelegramBot](https://github.com/TelegramBots/Telegram.Bot)</br>
Для начала работы с ботом необходимо получить token от BotFather [Инструкция](https://core.telegram.org/bots#6-botfather)</br>

[Инициализация сервиса]:ru_service_init
### Инициализация сервиса
Для использования сервиса необходимо инициализировать его

```C#
namespace YourBot
{
    private ITelegramService telegramService;
  
    class Program
    {
      telegramService = new TelegramService();
    }
}
```

[Инициализация телеграм клиентов]:ru_clients_init
### Инициализация телеграм клиентов
Внутри сервиса лежит два статических объекта Client и Debugger с типом TelegramBotClient</br>
Для их инициализации используйте соответствующие методы

```C#
TelegramBotClient client;

bool isCorrect = await telegramService.CheckBotToken("YOUR_TOKEN");
if (!isCorrect)
  return;
client = await telegramService.CreateMainBot("YOUR_TOKEN");
```

```C#
TelegramBotClient debugger;

bool isCorrect = await telegramService.CheckBotToken("YOUR_TOKEN");
if (!isCorrect)
  return;
debugger = await telegramService.CreateDebugBot("YOUR_TOKEN");
```

Для того чтобы прослушивать сообщения ботом необходимо создать класс имлементирующий интерфейс IUpdateHandler
и вызвать метод StartMainBotReceiving - для прослушивания с основного бота
или метод StartBotReceiving - для прослушивания с другого объекта TelegramBotClient

```C#
public class Handler : IUpdateHandler
{
    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        //exceptions
    }
    public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        //your updates
    }
}
```
```C#
CancelationTokenSource cts = await telegramService.StartMainBotReceiving(handler, new UpdateType[] { });
```
```C#
CancelationTokenSource cts = await telegramService.StartBotReceiving(client, handler, new UpdateType[] { });
```
При необходимости можно ограничить принимаемые типы указав их в параметре
Доступные типы [здесь](https://github.com/TelegramBots/Telegram.Bot/blob/master/src/Telegram.Bot/Types/Enums/UpdateType.cs)

Пример:
```C#
CancelationTokenSource cts = await telegramService.StartMainBotReceiving(handler,
    new UpdateType[]
    {
      UpdateType.Message,
      UpdateType.CallbackQuery
    });
```

[Отправка логов]:ru_send_logs
### Отправка логов
После инициализации дебагера CreateDebugBot можно использовать метод SendLog(uid, string)
Этот метод позволяет отправлять лог с другого бота имеющего доступ к сообщениям
Пример реализации

```C#
try
{
    //some code
}
catch (Exception ex)
{
    await telegramService.SendLog(id_for_debug, ex.Message); 
}
```
[Реализованные типы]:ru_implemented_types
### Реализованные типы
Ниже указаны реализованные в сервисе типы сообщений (если последним параметром добавить клиент TelegramBotClient сообщение будет отправлено от него)

DeleteMessage - метод удаляет сообщение из чата
```C#
await telegramService.DeleteMessage(chat_id, message_id)
```

SendRemoveMessage - метод отправляет сообщение и удаляет клавиатуру
```C#
await telegramService.SendRemoveMessage(target_id, "text")
```

SendMessage - метод отправляет сообщение
```C#
await telegramService.SendMessage(target_id, "text")
```

SendMessageWithFile - метод отправляет сообщение с вложением, если указать параметр deleteFileWhenComplete = false файл остается на компьютере,
по умолчанию = true, файл удаляется после отправки
```C#
await telegramService.SendMessageWithFile(target_id, "text", new FileStream("example.txt", FileMode.Open))
```

SendMessageWithKeyboard - метод отправляет сообщение с клавиатурой
Метод перегружен и принимает ReplyKeyboardMarkup или InlineKeyboardMarkup
```C#
await telegramService.SendMessageWithKeyboard(target_id, "text", markup)
```
