# API.md

## 0. Executive Summary

Цей аудит виконано по фактичному стану репозиторію `GF-Dev` (read-only аналіз коду, без рефакторингу). Стан **не відповідає** описаному target-стану (`ASP.NET Core Web API + BLL + DAL + React`).

### Ключовий факт
- У solution знайдено **лише один .NET Framework WinForms-проєкт** `GF` (net48).
- Проєктів/папок `WebApi`, `BLL`, `DAL`, `WPFApp`, `Controllers`, `DbContext`, `Repositories`, `Middleware`, `Program.cs` для ASP.NET Core — **не знайдено**.
- Відповідно, backend-API (CRUD Employees/Shops/Containers/AvailabilityGroups, nested CRUD, generate endpoint `/api/containers/{containerId}/graphs/{graphId}/generate`) у цьому репозиторії відсутній.

### Backend completion (за наявним кодом)
- **Backend complete for React MVP:** **Ні**.
- **Backend complete for full parity with desktop app:** **Ні**.
- Поточний код містить desktop-бізнес-логіку генерації графіків, локальне JSON/file storage та Excel export без HTTP API.

---

## 1. Solution & Projects Map

## 1.1 Знайдені solution / projects

### Solution
- `GF.sln`

### Projects (*.csproj)
- `GF/GF.csproj`

Інших `.sln` / `.csproj` не знайдено.

## 1.2 Проєкт `GF`
- **Тип:** WinExe (desktop WinForms)
- **Target framework:** `.NET Framework v4.8`
- **Main entry point:** `GF/Program.cs` → `Application.Run(new FormMain())`

### ProjectReference
- `ProjectReference` у `GF.csproj` **відсутні**.

### Package/assembly dependencies (ключові)
- UI: `Guna.UI2.WinForms`
- Serialization: `Newtonsoft.Json`, `System.Text.Json`
- Excel/export: `ClosedXML`, `EPPlus`, `DocumentFormat.OpenXml` (+ related packages)

## 1.3 Dependency graph (фактичний)

```text
GF.sln
└── GF (net48 WinForms monolith)
    ├── UI forms (GF/UI/*)
    ├── scheduling domain + generator (GF/Scheduling/*)
    ├── file/json persistence helpers (GF/Scheduling/ScheduleFileService.cs, GF/Binds/BindStorage.cs)
    └── no Web API host / no BLL / no DAL projects
```

Очікуваний граф `WebApi -> BLL -> DAL` у репозиторії **Not found**.

---

## 2. Runtime architecture (WebApi host, BLL, DAL, SQLite)

## 2.1 Що очікувалось (за ТЗ)
- ASP.NET Core host (`Program.cs`, middleware pipeline)
- BLL layer (services/facades, DI)
- DAL layer (EF Core `DbContext`, repositories)
- SQLite connection/config

## 2.2 Що знайдено реально
- Runtime — це **desktop WinForms process**:
  - запуск через `Application.Run(new FormMain())`;
  - бізнес-операції тригеряться event handlers у формах;
  - збереження даних: JSON-файли в локальній файловій системі (`Schedules/`, `containers.json`, `binds.json`);
  - генерація графіка: in-process static class `ScheduleGenerator`.

## 2.3 Що не знайдено
- `WebApplication.CreateBuilder`, `UseSwagger`, `MapControllers`, middleware pipeline — **Not found**.
- `DbContext`, `DbSet`, EF Core migrations, SQLite provider — **Not found**.
- `IServiceCollection` DI stack (`AddBusinessLogicStack`, `AddDataAccess`) — **Not found**.

---

## 3. DAL (Entities, Repositories, DB invariants)

> Нижче — фактичний стан. DAL як окремий шар не існує.

## 3.1 AppDbContext
- `AppDbContext` / EF Core context: **Not found**.
- Пошук виконано по патернах: `DbContext`, `DbSet`, `OnModelCreating`, `UseSqlite`.

## 3.2 Entities (очікувані модулі)

### Employee, Shop, Container
- `Employee` (клас для генератора) знайдено: `GF/Scheduling/Employee.cs`.
- `Shop`, API-level `Container` entity (з id, ownership, relational links) — **Not found**.
- Є `MonthContainer` як desktop-модель контейнера: `GF/Month/MonthContainer.cs` (Month/Year/Name + Employees + Schedules).

### AvailabilityGroup, members, day/slot
- Усі AvailabilityGroup-related entities — **Not found**.

### Schedule graph entities (ScheduleModel/SlotModel/EmployeeModel/CellStyleModel)
- EF/entities для backend graph model — **Not found**.
- Є `SavedSchedule` DTO для JSON-серіалізації desktop-таблиці: `GF/Scheduling/SavedSchedule.cs`.

## 3.3 Repositories
- `Abstractions`/`Repositories` інтерфейси і реалізації — **Not found**.
- Замість репозиторіїв використано static file services:
  - `ScheduleFileService` (save/load/delete JSON schedule files)
  - `ScheduleFileService.ContainerStorage` (load/save containers list у `containers.json`)
  - `BindStorage` (load/save keyboard binds у `binds.json`)

## 3.4 DB invariants
- DB-level інваріанти (indexes, unique constraints, FK cascade behavior) — **Not found**.
- Транзакції, bulk operations, `DbUpdateException` handling — **Not found** (бо немає EF/DB).

---

## 4. BLL (Services, Facades, Generator, Validation)

## 4.1 DI registration
- `BusinessLogicLayer/Extensions.cs`, `AddBusinessLogicStack` — **Not found**.
- Передача connection string (в т.ч. `GF3_CONNECTION_STRING`) — **Not found**.

## 4.2 Service/facade layer
- `IEmployeeFacade`, `IShopFacade`, `IContainerService`, `IAvailabilityGroupService` — **Not found**.
- CRUD service contracts — **Not found**.

## 4.3 Фактична бізнес-логіка (desktop)
- Генератор: `GF/Scheduling/ScheduleGenerator.cs`.
- Метод: `ScheduleGenerator.Generate(IReadOnlyList<Employee>, DataGridView, ScheduleParameters)`.
- Правила/обмеження закладені прямо в генераторі:
  - monthly hour limit;
  - priority/full-time добір до target;
  - max consecutive days;
  - max full shifts;
  - parsing availability `+`, `-`, `start-end`.
- Виклик генерації відбувається у `FormPageSecond.BtnCreateSchedule_Click`.

## 4.4 Validation & exceptions
- `ValidationException` domain model — **Not found**.
- Exception mapping `DbUpdateException -> ValidationException` — **Not found**.
- 404/ownership `KeyNotFoundException` для API — **Not found**.
- Є локальний `try/catch` в UI handlers з message boxes (desktop UX).

---

## 5. Web API (Pipeline, Middleware, Contracts/Mappers)

## 5.1 Program.cs pipeline (ASP.NET Core)
- **Not found**. Є лише WinForms startup (`GF/Program.cs`).

## 5.2 Middleware
- `ApiExceptionMiddleware` — **Not found**.
- Глобальний JSON error format (`400/404/500`) — **Not found**.

## 5.3 Contracts & Mappers
- `Contracts/*`, request/response DTO для API — **Not found**.
- Hand-written API mappers — **Not found**.

---

## 6. Endpoints Catalog (таблиця)

Через відсутність Web API layer усі endpoints — **Not found**.

| Module | Expected endpoint(s) | Status in repository |
|---|---|---|
| Employees CRUD | `GET/POST/PUT/DELETE /api/employees...` | Not found |
| Shops CRUD | `GET/POST/PUT/DELETE /api/shops...` | Not found |
| Containers CRUD | `GET/POST/PUT/DELETE /api/containers...` | Not found |
| Container graphs CRUD | `/api/containers/{id}/graphs...` | Not found |
| Graph slots/employees/cell-styles | nested routes under graphs | Not found |
| Generate graph | `POST /api/containers/{containerId}/graphs/{graphId}/generate` | Not found |
| AvailabilityGroups CRUD | `/api/availability-groups...` | Not found |
| Availability members/slots `/items` | nested routes | Not found |
| Health | `/health` | Not found |

---

## 7. Key Execution Flows (3–6 детальних flow-діаграм текстом)

Нижче — фактичні desktop-flows (замість HTTP).

## 7.1 “Create schedule” (аналог expected “Generate graph”)
1. User натискає `Create` у `FormPageOne`.
2. `FormPageOne.ButtonGenerate_Click` збирає:
   - трансформований `DataGridView` availability (`BuildDispoGridTransposed`),
   - список `Employee` (`BuildEmployeeList`),
   - selected `MonthContainer`.
3. Подія `ScheduleRequested` передається в `FormMain.OnScheduleRequested`.
4. Відкривається `FormPageSecond` з dispo/employees/container.
5. Клік `Create Schedule` викликає `FormPageSecond.BtnCreateSchedule_Click`.
6. Усередині — `ScheduleGenerator.Generate(...)`.
7. Результат відображається в `dgvSchedule`, зберігається у `MonthContainer.Schedules`, потім `ContainerStorage.Save(_allContainers)`.

## 7.2 “Open existing schedules”
1. `ScheduleManagerForm.btnOpenSchedules_Click` завантажує `ContainerStorage.GetAll()`.
2. Відкриває `DialogOpenSchedule`.
3. Користувач обирає container + schedules.
4. `ScheduleManagerForm.LoadMany` створює card-forms (`FormPageSecond`) і робить `LoadSavedSchedule(dto)`.

## 7.3 “Swap shifts між графіками”
1. У `ScheduleManagerForm` активується swap mode (`ActivateSwapMode`).
2. Користувач обирає 2 клітинки (може в різних cards).
3. `DoSwap` міняє значення клітинок, формує `SwapInfo`.
4. Обидві cards отримують `MarkSwapped(...)` і `Recalc()`.

## 7.4 “Export to Excel”
1. У `FormPageSecond.btnExportExcel_Click` відкривається `SaveFileDialog`.
2. `ExportToExcel(path)` формує workbook (`ClosedXML`), переносить значення/styles/comments/borders.
3. `wb.SaveAs(path)` записує `.xlsx` на диск.

## 7.5 “Persist container/schedule metadata”
1. `ContainerStorage.Save(...)` серіалізує всі `MonthContainer` у `containers.json` в `Application.StartupPath`.
2. `ScheduleFileService.Save(dto)` серіалізує конкретний `SavedSchedule` у `Schedules/*.json`.

---

## 8. Backend Completion Audit (WPF decoupling analysis)

> Примітка: у репозиторії фактично WinForms (`System.Windows.Forms`), не WPF. Папка/проєкт `WPFApp` — **Not found**.

## 8.1 Що вже є як бізнес-функції (але НЕ через API)
- Створення/редагування розкладу через UI grids.
- Алгоритм генерації графіка з бізнес-правилами.
- Керування контейнерами місяців та співробітниками у контейнерах.
- Відкриття/видалення збережених графіків.
- Swap shifts між графіками.
- Коментарі/кольори/рамки клітинок.
- Export to Excel.
- Збереження/завантаження JSON-файлів.

## 8.2 Пошук “що лишилось у desktop і не винесено”

### Шукали
- `GF/UI/*`, `GF/Scheduling/*`, `GF/Binds/*`, `GF/Month/*`.

### Результат
- Практично вся доменна поведінка прив’язана до UI/DataGridView та локальних файлів.
- Немає незалежного backend-сервісного шару, який можна викликати з HTTP.

### Конкретні desktop-only блоки
- Генератор приймає `DataGridView`, а не доменні DTO (сильна UI-coupling).
- Import/export/filesystem ops — у формах і static file services.
- Schedule manager workflow (swap, open dialog, delete) — повністю UI-driven.
- Конфігурація bind keys (`binds.json`) — desktop local config.

## 8.3 Таблиця покриття

| Feature / Module | Де реалізовано зараз | Чи є endpoint | Що потрібно зробити |
|---|---|---|---|
| Employees management (в межах контейнера) | WinForms (`FormPageOne`, `MonthContainer`) | Ні | Виділити employee domain model + CRUD API |
| Containers (month containers) | WinForms + `ContainerStorage` JSON | Ні | DAL schema + container service + controllers |
| Schedule generation | `ScheduleGenerator` (desktop, DataGridView-based) | Ні | Перепакувати в pure domain service + generate endpoint |
| Availability/disposition grid | WinForms `DataGridView` | Ні | Спроєктувати API contracts для availability days/slots |
| Schedule persistence | JSON files (`Schedules/`, `containers.json`) | Ні | Перейти на DB (EF Core/SQLite), repositories |
| Schedule swap flow | `ScheduleManagerForm` | Ні | Винести як backend operation (optional for MVP) |
| Excel export | `FormPageSecond.ExportToExcel` | Ні | Або backend export endpoint, або frontend/export client-side |
| Keyboard bind config | `BindStorage` (`binds.json`) | Ні | Визначити: frontend local settings чи backend profile settings |
| Health check | Not implemented | Ні | Додати `/health` endpoint |

## 8.4 Висновок
- **Backend complete for React MVP:** **Ні**.
- **Backend complete for full parity with WPF/desktop:** **Ні**.

### Основні блокери
1. Відсутній ASP.NET Core Web API host.
2. Відсутній DAL/BLL як проєкти/шари.
3. Відсутня БД (schema/migrations/invariants).
4. Критична бізнес-логіка прив’язана до WinForms controls (`DataGridView`).

### Ризики
- Перенесення генератора без декомпозиції input model може змінити поведінку.
- Непокриті edge-cases при переході з файлів на DB (цілісність і конкурентність).
- Відсутність API contracts ускладнює паралельну розробку React.

---

## 9. Next Steps (React hosting + exe launcher + remaining backend work)

## 9.1 Що треба винести в backend в першу чергу
1. Створити solution-структуру:
   - `GF.WebApi` (ASP.NET Core)
   - `GF.BLL`
   - `GF.DAL` (EF Core + SQLite)
2. Виділити доменні моделі без UI-dependencies (`DataGridView` прибрати з сигнатур).
3. Перенести генератор у BLL як pure service (input DTO -> output DTO).
4. Додати CRUD endpoints:
   - Employees, Shops (якщо бізнес-вимога), Containers,
   - availability groups/members/slots,
   - graphs/slots/employees/cell styles,
   - generate endpoint з `dryRun/overwrite` semantics.
5. Додати middleware error contract + validation model.
6. Додати `/health`.

## 9.2 Для React
- Після появи API:
  - якщо frontend окремим dev-server → увімкнути CORS;
  - для single-binary deploy розглянути hosting static build в `wwwroot` + fallback route (`MapFallbackToFile("index.html")`).

## 9.3 Для exe launcher
- Publish backend host як self-contained executable.
- Startup flow launcher:
  1) обрати/перевірити порт,
  2) запустити backend process,
  3) poll `/health` до ready,
  4) відкрити browser на frontend URL.
- Port selection strategy:
  - preferred fixed port + fallback scan free port;
  - передавати фактичний порт у frontend config/runtime bootstrap.

## 9.4 Тестування (рекомендації)
- Мінімум:
  - `ApiSamples.http` smoke сценарії для всіх CRUD + generate.
- Інтеграційні тести:
  - in-memory або test SQLite DB;
  - перевірка ownership/404,
  - duplicate constraints -> validation errors,
  - dryRun/overwrite flows,
  - паралельні generate requests (locking behavior).
- Поки Web API відсутній — ці тести не можуть бути реалізовані в поточному репозиторії.

---

## 10. Appendix (external packages, entry points, config keys, env vars)

## 10.1 External packages (фактично у repo)
- З `GF/packages.config`:
  - ClosedXML / ClosedXML.Parser
  - DocumentFormat.OpenXml / Framework
  - EPPlus / EPPlus.Interfaces
  - Newtonsoft.Json
  - Guna.UI2.WinForms
  - System.Text.Json + supporting packages
  - інші dependency packages (buffers/memory/crypto/fonts/...)

## 10.2 Entry points
- `GF/Program.cs` — desktop app startup (`Application.Run(new FormMain())`).

## 10.3 Config keys / env vars
- `GF3_CONNECTION_STRING` — **Not found**.
- `appsettings.json` / `appsettings.*.json` — **Not found**.
- ASP.NET Core configuration binding — **Not found**.

---

## Backend ready checklist

### ✅ Must-have done (у поточному desktop застосунку)
- Є робоча desktop-генерація графіка.
- Є локальне збереження/відкриття графіків.
- Є керування контейнерами та базовими даними співробітників.
- Є Excel export.

### ⚠️ Should-do soon
- Формалізувати domain contracts (без WinForms типів).
- Зафіксувати API contracts для React команди (OpenAPI/DTO).
- Визначити migration strategy JSON -> SQLite.

### ❌ Missing for full parity (у backend API)
- Web API host, middleware, swagger, health.
- BLL/DAL layers.
- EF Core schema + repositories + invariants.
- Повний endpoint catalog з nested CRUD і generate endpoint.
- Інтеграційні API тести.

---

## Console summary

### Що знайдено
- 1 solution (`GF.sln`), 1 project (`GF/GF.csproj`, net48 WinForms).
- Desktop scheduling engine, file-based persistence, Excel export.

### Що missing
- Увесь заявлений backend stack (`ASP.NET Core Web API + BLL + DAL + SQLite + endpoints`) — Not found у цьому репозиторії.
