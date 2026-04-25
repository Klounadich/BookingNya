# 🏨 BookingNya

![.NET](https://img.shields.io/badge/.NET-10-black?logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-14+-blue?logo=postgresql)
![License](https://img.shields.io/badge/license-MIT-green)
![Status](https://img.shields.io/badge/status-alpha-orange)

> **WEB API для бронирования номеров в отеле**  
> Версия: `v1.2.0-alpha`  
> Современный бэкенд соответствующий паттерну SAGA  для управления номерами и системой бронирования.

---
> **Патч-ноут  [1.2.0-alpha] - 2026-04-25**
> ### Added✔️
> - Auth Система с использованием JWT токенов
> - FluentValidation-валидатор для Auth Endpoint

> ### Fixed🛠️
> - Убраны неиспользуемые элементы в коде
> - Исправлены мелкие недочёты


## 📑 Оглавление

- [✨ Возможности](#-возможности)
- [🛠 Технологический стек](#-технологический-стек)
- [🚀 Быстрый старт](#-быстрый-старт)
- [⚙️ Конфигурация](#️-конфигурация)
- [🗄️ Миграции базы данных](#️-миграции-базы-данных)
- [📖 Работа с API](#-работа-с-api)
- [📄 Лицензия](#-лицензия)
- [👤 Автор](#-автор)

---

## ✨ Возможности

- ✅ **Паттерн SAGA для оркестрации бронирования**  
  Гарантирует целостность данных при распределённых операциях:  
  `Резерв номера → Оплата → Подтверждение` с автоматическим откатом (`compensating transactions`) при сбое на любом этапе
- ✅ Просмотр доступных номеров и категорий
- ✅ Создание, изменение и отмена бронирований
- ✅ Проверка доступности номеров по датам
- ✅ Валидация входных данных и обработка ошибок
- ✅ Автоматическая документация через Scalar
- ✅  Уведомления на Email, интеграция с платежными системами
- ✅ Регистрация и авторизация пользователей (JWT)
---

## 🛠 Технологический стек

| Категория | Технология |
|-----------|------------|
| **Платформа** | .NET 10 (ASP.NET Core Web API) |
| **Язык** | C# 12 |
| **База данных** | PostgreSQL 14+ |
| **ORM** | Entity Framework Core |
| **Документация** | Scalar  |
| **Контейнеризация** | Docker |
| **Архитектура** | Clean Architecture / Layered |

---

## 🚀 Быстрый старт

### Предварительные требования

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL 14+](https://www.postgresql.org/download/)
- Git

### Установка и запуск

1. Клонируйте репозиторий:
   ```bash
   git clone https://github.com/Klounadich/BookingNya.git
   cd BookingNya
2. Перейдите в папку с проектом и запустите: 
 ```bash
 cd Web
dotnet restore
dotnet run
```
3. Сервер запустится по адресу :
```bash
http://localhost:5255
```
### ⚙️ Конфигурация 
Проект использует [appsettings.json](Web/appsettings.json) для хранения настроек.
Для корректной работы вам необходимо отредактировать его по ключевым параметрам . (Например : указать свои данные в строке подключения PostgreSQL)

### 🗄️ Миграции базы данных 
Проект использует Entity Framework Core Code-First.
### Применить миграции:
```bash
cd Web
dotnet ef database update
```
## 📖 Работа с API 
### Документация Scalar
Интерактивная документация доступна после запуска:
🔗 http://localhost:5255/api-docs
### Пример запроса на бронирование 
```bash
curl http://localhost:5255/api/booking \
  --request POST \
  --header 'Content-Type: application/json' \
  --data '{
  "room_id": "201",
  "guest_name": "Federiko Felini",
  "guest_email": "federik@gmail.com",
  "guest_phone": "+12347930",
  "check_in": "2026-06-15T00:00:00Z",
  "check_out": "2026-06-20T00:00:00Z",
  "total_price": 700,
  "currency": "usd",
  "payment_method": "card",
  "payment_reservation_id": "guid"
}'
```
### Ответ (201)
```json
{
  "message": "Booking saga initiated.",
  "sagaId": "e7b1f627-7c4c-43ae-a81a-a873748d1e09"
}
```

## 📄 Лицензия
Распространяется под лицензией __MIT__.
Вы можете свободно использовать, изменять и распространять код в учебных и коммерческих целях при условии указания авторства.
📄 См. файл [LICENSE](LICENSE) для деталей.

## 👤 Автор
Klounadich
🔗 [GitHub](https://github.com/Klounadich)
📧 kekers507y@gmail.com 
> Проект создан в рамках учебного портфолио / пет-проекта.
> Предложения и баг-репорты приветствуются — создавайте Issues или Pull Requests! 🙌
