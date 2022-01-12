# TelegramSimpleService

---

### RU
1. Поддерживаемые платформы</br>
2. Руководство использования</br>
   2.1. Инициализация сервиса</br>
   2.2. Инициализация телеграм клиентов</br>
   2.3. Отправка логов</br>
   2.4. Реализованные типы</br>
   2.5. Клавиатуры</br>

---

[Поддерживаемые платформы]:ru_platforms
## 1. Поддерживаемые платформы:
Проект нацелен как минимум на .NET Standard 2.0 и .NET Core 3.1.

[Руководство использования]:ru_getting_started
## 2. Руководство использования:
Сервис написан на основе [TelegramBot](https://github.com/TelegramBots/Telegram.Bot)</br>
Для начала работы с ботом необходимо получить token от BotFather [Инструкция](https://core.telegram.org/bots#6-botfather)</br>

[Инициализация сервиса]:ru_service_init
## 2.1 Инициализация сервиса
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
## 2.2 Инициализация телеграм клиентов
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

Для того чтобы прослушивать сообщения ботом необходимо создать класс имплементирующий интерфейс IUpdateHandler
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
## 2.3 Отправка логов
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
## 2.4 Реализованные типы
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

[Клавиатуры]:ru_keyboards
## 2.5 Клавиатуры
Для иницилизации сервиса клавиатур используйте интерфейс IKeyboardService

```C#
namespace YourBot
{
    private IKeyboardService keyboardService;
  
    class Program
    {
      keyboardService = new KeyboardService();
    }
}
```

По умолчанию метод задает имена файлов для хранимых клавиатур r_keys.json и i_keys.json</br>
Изменить имена можно с помощью метода SetStoreFileName(string, string) в параметрах указываются новые имена для файлов</br>
Метод возвращает true при успешном выполнении

```C#
await keyboardService.SetStoreFileName("new_reply_keyboards_name.json", "new_inline_keyboards_name.json")
```

Для сохранения клавиатур используется метод SaveKeyboards(keyboards) возвращает true в случае успеха

```C#
await keyboardService.SaveKeyboards(keyboards);
```

Для загрузки сохраненных клавиатур используется метод LoadKeyboards(KeyboardType) и выбирается какую клавиатуру необходимо загрузить</br>
Метод возвращает object который нужно привести к типу нужной клавиатуры

```C#
Dictionary<string, ReplyKeyboardMarkup> loaded_r_keys = (Dictionary<string, ReplyKeyboardMarkup>)await keyboardService.LoadKeyboards(KeyboardType.Reply);
Dictionary<string, InlineKeyboardMarkup> loaded_i_keys = (Dictionary<string, InlineKeyboardMarkup>)await keyboardService.LoadKeyboards(KeyboardType.Inline);
```

Для генерации inline клавиатуры используйте метод GenerateInlineKeyboard(List<Tuple<string, string>>) </br>
Для каждого элемента списка используется тип Tuple где 1 элемент текст кнопки 2 элемент CallbackData

```C#
InlineKeyboardMarkup keyboard = await keyboardService.GenerateInlineKeyboard(myCollection);
```

Для генерации так же доступен расширенный метод GeneratePagedInlineKeyboard, работает по принципу GenerateInlineKeyboard с разницей в параметрах</br>
В параметрах указывается номер текущей страницы, максимальное кол-во элементов на странице, данные для кнопок назад и вперед</br>
При указании страницы вне допустимого значения выбирается первая или последняя страница, к примеру страниц 5 при вводе страницы 7 будет загрузка страницы 5

```C#
//Загрузка страницы 1, кол-во элементов на странице 5
InlineKeyboardMarkup keyboard = await keyboardService.GeneratePagedInlineKeyboard(myCollection, 1, 5, new Tuple<string, string>("Btn1", "Back"), new Tuple<string, string>("Btn2", "Forward"));

//Загрузка страницы 3, кол-во элементов на странице 5
InlineKeyboardMarkup keyboard = await keyboardService.GeneratePagedInlineKeyboard(myCollection, 3, 5, new Tuple<string, string>("Btn1", "Back"), new Tuple<string, string>("Btn2", "Forward"));
```