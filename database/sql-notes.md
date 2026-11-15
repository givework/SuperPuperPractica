# SQL notes

---

## Task 01 — Создание базы и таблиц

### Запрос

```sql
CREATE DATABASE PracticeDB;
GO
USE PracticeDB;
GO

CREATE TABLE Departments (
    DepartmentId   INT PRIMARY KEY IDENTITY(1,1),
    DepartmentName NVARCHAR(100) NOT NULL
);

CREATE TABLE Employees (
    EmployeeId   INT PRIMARY KEY IDENTITY(1,1),
    EmployeeName NVARCHAR(100) NOT NULL,
    Salary       DECIMAL(10,2) NOT NULL,
    HireDate     DATE NOT NULL,
    DepartmentId INT NOT NULL,
    ManagerId    INT NULL,
    CONSTRAINT FK_Employees_Departments FOREIGN KEY (DepartmentId)
        REFERENCES Departments(DepartmentId),
    CONSTRAINT FK_Employees_Manager FOREIGN KEY (ManagerId)
        REFERENCES Employees(EmployeeId)
);

CREATE TABLE Projects (
    ProjectId   INT PRIMARY KEY IDENTITY(1,1),
    ProjectName NVARCHAR(100) NOT NULL,
    StartDate   DATE NOT NULL
);

CREATE TABLE EmployeeProjects (
    EmployeeId INT NOT NULL,
    ProjectId  INT NOT NULL,
    Hours      INT NOT NULL DEFAULT 0,
    CONSTRAINT PK_EmployeeProjects PRIMARY KEY (EmployeeId, ProjectId),
    CONSTRAINT FK_EP_Employees FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId),
    CONSTRAINT FK_EP_Projects  FOREIGN KEY (ProjectId)  REFERENCES Projects(ProjectId)
);
GO

-- Проверка: таблицы
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME;

-- Проверка: внешние ключи
SELECT fk.name AS ForeignKey,
       OBJECT_NAME(fk.parent_object_id)     AS ChildTable,
       OBJECT_NAME(fk.referenced_object_id) AS ParentTable
FROM sys.foreign_keys fk
ORDER BY ChildTable, ForeignKey;
```

### Результат

Таблицы:

| TABLE_NAME |
|---|
| Departments |
| EmployeeProjects |
| Employees |
| Projects |

Внешние ключи:

| ForeignKey | ChildTable | ParentTable |
|---|---|---|
| FK_EP_Employees | EmployeeProjects | Employees |
| FK_EP_Projects | EmployeeProjects | Projects |
| FK_Employees_Departments | Employees | Departments |
| FK_Employees_Manager | Employees | Employees |

### Вывод

Созданы все 4 таблицы и 4 внешних ключа. `FK_Employees_Manager` — самоссылка: `ManagerId` указывает на `EmployeeId` той же таблицы, поэтому `ManagerId` допускает `NULL` (у руководителей верхнего уровня менеджера нет). У `EmployeeProjects` составной первичный ключ `(EmployeeId, ProjectId)` — это защищает от дублей назначения.

---

## Task 02 — INSERT: заполнение данными

### Запрос

```sql
INSERT INTO Departments (DepartmentName) VALUES
    (N'Разработка'),
    (N'Аналитика'),
    (N'Тестирование'),
    (N'Менеджмент');

INSERT INTO Employees (EmployeeName, Salary, HireDate, DepartmentId, ManagerId) VALUES
    (N'Анна Иванова',     65000, '2020-03-15', 1, NULL),
    (N'Пётр Сидоров',     72000, '2019-07-01', 1, NULL),
    (N'Мария Кузнецова',  55000, '2021-11-20', 1, NULL),
    (N'Иван Петров',      48000, '2023-02-10', 1, NULL),
    (N'Ольга Смирнова',   61000, '2020-09-05', 2, NULL),
    (N'Дмитрий Орлов',    58000, '2022-04-18', 2, NULL),
    (N'Елена Волкова',    52000, '2023-06-01', 2, NULL),
    (N'Сергей Морозов',   45000, '2021-12-12', 3, NULL),
    (N'Наталья Соколова', 43000, '2024-01-15', 3, NULL),
    (N'Алексей Лебедев',  90000, '2018-05-25', 4, NULL);

-- Менеджеры проставляются после вставки (нужны уже существующие EmployeeId)
UPDATE Employees SET ManagerId = 1  WHERE EmployeeId = 4;
UPDATE Employees SET ManagerId = 2  WHERE EmployeeId = 3;
UPDATE Employees SET ManagerId = 5  WHERE EmployeeId = 6;
UPDATE Employees SET ManagerId = 10 WHERE EmployeeId = 9;

INSERT INTO Projects (ProjectName, StartDate) VALUES
    (N'CRM System',      '2023-01-10'),
    (N'Mobile App',      '2023-05-01'),
    (N'Data Warehouse',  '2022-11-20'),
    (N'AI Assistant',    '2024-02-01'),
    (N'Legacy Refactor', '2021-06-15');

INSERT INTO EmployeeProjects (EmployeeId, ProjectId, Hours) VALUES
    (1, 1, 120), (1, 3, 80),
    (2, 1, 150), (2, 4, 60),
    (3, 2, 100), (4, 2, 90),
    (4, 5, 40),  (5, 3, 110),
    (6, 3, 70),  (6, 4, 50),
    (7, 2, 95),  (8, 5, 130),
    (10, 4, 200),(10, 1, 30);

-- Проверка
SELECT 'Departments'      AS TableName, COUNT(*) AS Cnt FROM Departments
UNION ALL SELECT 'Employees',        COUNT(*) FROM Employees
UNION ALL SELECT 'Projects',         COUNT(*) FROM Projects
UNION ALL SELECT 'EmployeeProjects', COUNT(*) FROM EmployeeProjects;
```

### Результат

| TableName | Cnt |
|---|---|
| Departments | 4 |
| Employees | 10 |
| Projects | 5 |
| EmployeeProjects | 14 |

### Вывод

Для кириллицы строки нужно писать с префиксом `N'...'`, иначе возможны «?» вместо символов. Менеджеров нельзя указать в том же `INSERT`, где создаются сами сотрудники (ещё нет их `EmployeeId`), поэтому `ManagerId` проставлен отдельными `UPDATE`.

---

## Task 03 — SELECT + WHERE: базовые выборки

### Запрос

```sql
-- 3.1 Зарплата > 50000
SELECT EmployeeId, EmployeeName, Salary
FROM Employees
WHERE Salary > 50000;

-- 3.2 Нанятые в 2023 году
SELECT EmployeeId, EmployeeName, HireDate
FROM Employees
WHERE YEAR(HireDate) = 2023;

-- 3.3 Имя начинается на «А»
SELECT EmployeeId, EmployeeName
FROM Employees
WHERE EmployeeName LIKE N'А%';

-- 3.4 Отдел с DepartmentId = 2
SELECT EmployeeId, EmployeeName, DepartmentId
FROM Employees
WHERE DepartmentId = 2;

-- 3.5 Зарплата от 40000 до 60000
SELECT EmployeeId, EmployeeName, Salary
FROM Employees
WHERE Salary BETWEEN 40000 AND 60000;
```

### Результат

**3.1** — 7 строк

| EmployeeId | EmployeeName | Salary |
|---|---|---|
| 1 | Анна Иванова | 65000.00 |
| 2 | Пётр Сидоров | 72000.00 |
| 3 | Мария Кузнецова | 55000.00 |
| 5 | Ольга Смирнова | 61000.00 |
| 6 | Дмитрий Орлов | 58000.00 |
| 7 | Елена Волкова | 52000.00 |
| 10 | Алексей Лебедев | 90000.00 |

**3.2** — 2 строки

| EmployeeId | EmployeeName | HireDate |
|---|---|---|
| 4 | Иван Петров | 2023-02-10 |
| 7 | Елена Волкова | 2023-06-01 |

**3.3** — 2 строки

| EmployeeId | EmployeeName |
|---|---|
| 1 | Анна Иванова |
| 10 | Алексей Лебедев |

**3.4** — 3 строки

| EmployeeId | EmployeeName | DepartmentId |
|---|---|---|
| 5 | Ольга Смирнова | 2 |
| 6 | Дмитрий Орлов | 2 |
| 7 | Елена Волкова | 2 |

**3.5** — 6 строк

| EmployeeId | EmployeeName | Salary |
|---|---|---|
| 3 | Мария Кузнецова | 55000.00 |
| 4 | Иван Петров | 48000.00 |
| 6 | Дмитрий Орлов | 58000.00 |
| 7 | Елена Волкова | 52000.00 |
| 8 | Сергей Морозов | 45000.00 |
| 9 | Наталья Соколова | 43000.00 |

### Вывод

`BETWEEN` включает обе границы. В `LIKE` буква «А» должна быть кириллической (и с `N`-префиксом) — латинская `A` визуально такая же, но не совпадёт. `YEAR(HireDate) = 2023` работает, но не использует индекс по `HireDate`; на больших данных лучше `HireDate >= '2023-01-01' AND HireDate < '2024-01-01'`.

---

## Task 04 — ORDER BY + TOP: сортировка и лимит

### Запрос

```sql
-- 4.1 Топ-3 по зарплате
SELECT TOP 3 EmployeeName, Salary
FROM Employees
ORDER BY Salary DESC;

-- 4.2 Топ-5 самых давних сотрудников
SELECT TOP 5 EmployeeName, HireDate
FROM Employees
ORDER BY HireDate ASC;

-- 4.3 По отделу (возр.), затем по зарплате (убыв.)
SELECT DepartmentId, EmployeeName, Salary
FROM Employees
ORDER BY DepartmentId ASC, Salary DESC;
```

### Результат

**4.1**

| EmployeeName | Salary |
|---|---|
| Алексей Лебедев | 90000.00 |
| Пётр Сидоров | 72000.00 |
| Анна Иванова | 65000.00 |

**4.2**

| EmployeeName | HireDate |
|---|---|
| Алексей Лебедев | 2018-05-25 |
| Пётр Сидоров | 2019-07-01 |
| Анна Иванова | 2020-03-15 |
| Ольга Смирнова | 2020-09-05 |
| Мария Кузнецова | 2021-11-20 |

**4.3**

| DepartmentId | EmployeeName | Salary |
|---|---|---|
| 1 | Пётр Сидоров | 72000.00 |
| 1 | Анна Иванова | 65000.00 |
| 1 | Мария Кузнецова | 55000.00 |
| 1 | Иван Петров | 48000.00 |
| 2 | Ольга Смирнова | 61000.00 |
| 2 | Дмитрий Орлов | 58000.00 |
| 2 | Елена Волкова | 52000.00 |
| 3 | Сергей Морозов | 45000.00 |
| 3 | Наталья Соколова | 43000.00 |
| 4 | Алексей Лебедев | 90000.00 |

### Вывод

`TOP` без `ORDER BY` возвращает произвольные строки, поэтому сортировка обязательна. При равных значениях на границе `TOP N` можно добавить `WITH TIES` (здесь повторов нет). Для сортировки по нескольким столбцам направление указывается у каждого отдельно.

---

## Task 05 — Агрегация: COUNT, SUM, AVG, MIN, MAX

### Запрос

```sql
SELECT
    COUNT(*)                   AS TotalEmployees,
    SUM(Salary)                AS TotalSalary,
    ROUND(AVG(Salary), 2)      AS AvgSalary,
    MIN(Salary)                AS MinSalary,
    MAX(Salary)                AS MaxSalary
FROM Employees;

SELECT COUNT(*) AS HighSalaryCount
FROM Employees
WHERE Salary > 50000;
```

### Результат

| TotalEmployees | TotalSalary | AvgSalary | MinSalary | MaxSalary |
|---|---|---|---|---|
| 10 | 589000.00 | 58900.000000 | 43000.00 | 90000.00 |

| HighSalaryCount |
|---|
| 7 |

Ручная проверка: 240000 (Разработка) + 171000 (Аналитика) + 88000 (Тестирование) + 90000 (Менеджмент) = 589000; 589000 / 10 = 58900.

### Вывод

`AVG` над `DECIMAL(10,2)` возвращает `DECIMAL` с 6 знаками после запятой, а `ROUND` не меняет масштаб типа — значение округлено, но выводятся нули. Для вывода ровно двух знаков можно использовать `CAST(AVG(Salary) AS DECIMAL(10,2))`. Агрегаты игнорируют `NULL`, а `COUNT(*)` считает все строки; на пустой таблице `SUM/AVG/MIN/MAX` вернут `NULL`.

---

## Task 06 — GROUP BY: статистика по отделам

### Запрос

```sql
SELECT
    DepartmentId,
    COUNT(*)                AS EmployeeCount,
    ROUND(AVG(Salary), 2)   AS AvgSalary,
    SUM(Salary)             AS TotalSalary
FROM Employees
GROUP BY DepartmentId
ORDER BY AvgSalary DESC;
```

### Результат

| DepartmentId | EmployeeCount | AvgSalary | TotalSalary |
|---|---|---|---|
| 4 | 1 | 90000.000000 | 90000.00 |
| 1 | 4 | 60000.000000 | 240000.00 |
| 2 | 3 | 57000.000000 | 171000.00 |
| 3 | 2 | 44000.000000 | 88000.00 |

### Вывод

Каждый столбец в `SELECT` должен быть либо в `GROUP BY`, либо внутри агрегата. По алиасу `AvgSalary` в `ORDER BY` сортировать можно (в отличие от `WHERE`/`HAVING`). Отделы без сотрудников в результат не попадают — для них нужен `LEFT JOIN` с `Departments`.

---

## Task 07 — GROUP BY + HAVING: фильтрация групп

### Запрос

```sql
-- 7.1 Отделы, где больше 2 сотрудников
SELECT DepartmentId, COUNT(*) AS EmployeeCount
FROM Employees
GROUP BY DepartmentId
HAVING COUNT(*) > 2;

-- 7.2 Отделы со средней зарплатой > 55000
SELECT DepartmentId, AVG(Salary) AS AvgSalary
FROM Employees
GROUP BY DepartmentId
HAVING AVG(Salary) > 55000;

-- 7.3 Отделы с суммарной зарплатой > 150000
SELECT DepartmentId, SUM(Salary) AS TotalSalary
FROM Employees
GROUP BY DepartmentId
HAVING SUM(Salary) > 150000;
```

### Результат

**7.1**

| DepartmentId | EmployeeCount |
|---|---|
| 1 | 4 |
| 2 | 3 |

**7.2**

| DepartmentId | AvgSalary |
|---|---|
| 1 | 60000.000000 |
| 2 | 57000.000000 |
| 4 | 90000.000000 |

**7.3**

| DepartmentId | TotalSalary |
|---|---|
| 1 | 240000.00 |
| 2 | 171000.00 |

### Вывод

`WHERE` фильтрует строки до группировки, `HAVING` — группы после неё, поэтому условия на агрегаты пишутся только в `HAVING`. Алиас агрегата в `HAVING` использовать нельзя — выражение повторяется целиком.

---

## Task 08 — INNER JOIN: сотрудники и отделы

### Запрос

```sql
SELECT
    e.EmployeeName,
    d.DepartmentName,
    e.Salary
FROM Employees e
INNER JOIN Departments d ON d.DepartmentId = e.DepartmentId
ORDER BY d.DepartmentName ASC, e.Salary DESC;
```

### Результат

| EmployeeName | DepartmentName | Salary |
|---|---|---|
| Ольга Смирнова | Аналитика | 61000.00 |
| Дмитрий Орлов | Аналитика | 58000.00 |
| Елена Волкова | Аналитика | 52000.00 |
| Алексей Лебедев | Менеджмент | 90000.00 |
| Пётр Сидоров | Разработка | 72000.00 |
| Анна Иванова | Разработка | 65000.00 |
| Мария Кузнецова | Разработка | 55000.00 |
| Иван Петров | Разработка | 48000.00 |
| Сергей Морозов | Тестирование | 45000.00 |
| Наталья Соколова | Тестирование | 43000.00 |

10 строк — у каждого сотрудника есть отдел (`DepartmentId` — `NOT NULL` с внешним ключом).

### Вывод

Алиасы таблиц (`e`, `d`) делают запрос короче и однозначнее. Отделы здесь сортируются по алфавиту названия, а не по `DepartmentId`.

---

## Task 09 — LEFT JOIN: сотрудники без проектов

### Запрос

```sql
SELECT e.EmployeeId, e.EmployeeName
FROM Employees e
LEFT JOIN EmployeeProjects ep ON ep.EmployeeId = e.EmployeeId
WHERE ep.EmployeeId IS NULL;
```

### Результат

| EmployeeId | EmployeeName |
|---|---|
| 9 | Наталья Соколова |

Проверка: в `EmployeeProjects` есть сотрудники 1–8 и 10, отсутствует только 9.

### Вывод

`LEFT JOIN` сохраняет все строки `Employees`; для сотрудников без назначений столбцы `EmployeeProjects` равны `NULL`, на этом и построен фильтр. Проверять на `NULL` нужно столбец правой таблицы, который в самих данных `NULL` быть не может (здесь — ключ `EmployeeId`). Если все сотрудники назначены, запрос вернёт пустой результат — это корректно.

---

## Task 10 — Множественный JOIN: сотрудники, отделы, проекты

### Запрос

```sql
SELECT
    e.EmployeeName,
    d.DepartmentName,
    p.ProjectName,
    ep.Hours
FROM Employees e
INNER JOIN Departments      d  ON d.DepartmentId = e.DepartmentId
INNER JOIN EmployeeProjects ep ON ep.EmployeeId  = e.EmployeeId
INNER JOIN Projects         p  ON p.ProjectId    = ep.ProjectId
ORDER BY d.DepartmentName, e.EmployeeName, p.ProjectName;
```

### Результат

14 строк (по числу записей в `EmployeeProjects`):

| EmployeeName | DepartmentName | ProjectName | Hours |
|---|---|---|---|
| Дмитрий Орлов | Аналитика | AI Assistant | 50 |
| Дмитрий Орлов | Аналитика | Data Warehouse | 70 |
| Елена Волкова | Аналитика | Mobile App | 95 |
| Ольга Смирнова | Аналитика | Data Warehouse | 110 |
| Алексей Лебедев | Менеджмент | AI Assistant | 200 |
| Алексей Лебедев | Менеджмент | CRM System | 30 |
| Анна Иванова | Разработка | CRM System | 120 |
| Анна Иванова | Разработка | Data Warehouse | 80 |
| Иван Петров | Разработка | Legacy Refactor | 40 |
| Иван Петров | Разработка | Mobile App | 90 |
| Мария Кузнецова | Разработка | Mobile App | 100 |
| Пётр Сидоров | Разработка | AI Assistant | 60 |
| Пётр Сидоров | Разработка | CRM System | 150 |
| Сергей Морозов | Тестирование | Legacy Refactor | 130 |

### Вывод

Таблица `EmployeeProjects` — связующая (многие-ко-многим), поэтому цепочка `Employees → EmployeeProjects → Projects` обязательна. Наталья Соколова в результате отсутствует: `INNER JOIN` отбрасывает сотрудников без назначений (чтобы их показать, нужен `LEFT JOIN`, см. задание 9). Сортировка по `EmployeeName` идёт по полной строке «Имя Фамилия», т.е. по имени.

---

# Уровень 2 — Средний
---

## Task 11 — Self-join: иерархия руководителей

### Запрос

```sql
SELECT
    e.EmployeeName,
    m.EmployeeName AS ManagerName
FROM Employees e
LEFT JOIN Employees m ON m.EmployeeId = e.ManagerId
ORDER BY e.EmployeeId;
```

### Результат

| EmployeeName | ManagerName |
|---|---|
| Анна Иванова | NULL |
| Пётр Сидоров | NULL |
| Мария Кузнецова | Пётр Сидоров |
| Иван Петров | Анна Иванова |
| Ольга Смирнова | NULL |
| Дмитрий Орлов | Ольга Смирнова |
| Елена Волкова | NULL |
| Сергей Морозов | NULL |
| Наталья Соколова | Алексей Лебедев |
| Алексей Лебедев | NULL |

### Вывод

Одна и та же таблица подключается дважды под разными алиасами (`e` — сотрудник, `m` — руководитель). Нужен именно `LEFT JOIN`: с `INNER JOIN` пропали бы все сотрудники без руководителя (6 из 10).

---

## Task 12 — Подзапросы в WHERE

### Запрос

```sql
-- 12.1 Зарплата выше средней
SELECT EmployeeId, EmployeeName, Salary
FROM Employees
WHERE Salary > (SELECT AVG(Salary) FROM Employees)
ORDER BY Salary DESC;

-- 12.2 Максимальная зарплата
SELECT EmployeeId, EmployeeName, Salary
FROM Employees
WHERE Salary = (SELECT MAX(Salary) FROM Employees);
```

### Результат

Средняя зарплата = 58900.

**12.1**

| EmployeeId | EmployeeName | Salary |
|---|---|---|
| 10 | Алексей Лебедев | 90000.00 |
| 2 | Пётр Сидоров | 72000.00 |
| 1 | Анна Иванова | 65000.00 |
| 5 | Ольга Смирнова | 61000.00 |

**12.2**

| EmployeeId | EmployeeName | Salary |
|---|---|---|
| 10 | Алексей Лебедев | 90000.00 |

### Вывод

Подзапрос в сравнении должен возвращать ровно одно значение. Дмитрий Орлов (58000) в выборку не попал — он чуть ниже средней 58900. Если бы у нескольких сотрудников была максимальная зарплата, `= (SELECT MAX ...)` вернул бы их всех.

---

## Task 13 — Скалярные подзапросы в SELECT

### Запрос

```sql
SELECT
    e.EmployeeName,
    e.Salary,
    e.DepartmentId,
    CAST(ROUND(
        e.Salary - (SELECT AVG(e2.Salary)
                    FROM Employees e2
                    WHERE e2.DepartmentId = e.DepartmentId), 2)
        AS DECIMAL(10,2)) AS DiffFromDeptAvg
FROM Employees e
ORDER BY e.EmployeeId;
```

### Результат

| EmployeeName | Salary | DepartmentId | DiffFromDeptAvg |
|---|---|---|---|
| Анна Иванова | 65000.00 | 1 | 5000.00 |
| Пётр Сидоров | 72000.00 | 1 | 12000.00 |
| Мария Кузнецова | 55000.00 | 1 | -5000.00 |
| Иван Петров | 48000.00 | 1 | -12000.00 |
| Ольга Смирнова | 61000.00 | 2 | 4000.00 |
| Дмитрий Орлов | 58000.00 | 2 | 1000.00 |
| Елена Волкова | 52000.00 | 2 | -5000.00 |
| Сергей Морозов | 45000.00 | 3 | 1000.00 |
| Наталья Соколова | 43000.00 | 3 | -1000.00 |
| Алексей Лебедев | 90000.00 | 4 | 0.00 |

Средние по отделам: 1 — 60000, 2 — 57000, 3 — 44000, 4 — 90000.

### Вывод

Это коррелированный подзапрос: он ссылается на `e.DepartmentId` внешнего запроса и выполняется для каждой строки. `CAST` к `DECIMAL(10,2)` нужен только для красивого вывода (иначе `ROUND` оставит 6 знаков). В отделе из одного человека отклонение всегда 0.

---

## Task 14 — EXISTS / NOT EXISTS

### Запрос

```sql
-- 14.1 Есть хотя бы один проект
SELECT e.EmployeeId, e.EmployeeName
FROM Employees e
WHERE EXISTS (SELECT 1 FROM EmployeeProjects ep WHERE ep.EmployeeId = e.EmployeeId)
ORDER BY e.EmployeeId;

-- 14.2 Нет проектов
SELECT e.EmployeeId, e.EmployeeName
FROM Employees e
WHERE NOT EXISTS (SELECT 1 FROM EmployeeProjects ep WHERE ep.EmployeeId = e.EmployeeId);
```

### Результат

**14.1** — 9 строк: сотрудники с `EmployeeId` 1, 2, 3, 4, 5, 6, 7, 8, 10 (Анна Иванова, Пётр Сидоров, Мария Кузнецова, Иван Петров, Ольга Смирнова, Дмитрий Орлов, Елена Волкова, Сергей Морозов, Алексей Лебедев).

**14.2**

| EmployeeId | EmployeeName |
|---|---|
| 9 | Наталья Соколова |

### Вывод

Результат 14.2 совпадает с заданием 9; 14.1 даёт тех же 9 сотрудников, что встречаются в задании 10 (там они повторяются по числу проектов, здесь — по одному разу). `EXISTS` останавливается на первой найденной строке, поэтому не плодит дубли, как `JOIN`. `SELECT 1` внутри — общепринятая форма: значения столбцов не важны.

---

## Task 15 — UNION / UNION ALL

### Запрос

```sql
-- 15.1 UNION (уникальные строки)
SELECT EmployeeId, EmployeeName FROM Employees WHERE Salary > 60000
UNION
SELECT EmployeeId, EmployeeName FROM Employees WHERE HireDate < '2022-01-01';

-- 15.2 UNION ALL (с дубликатами)
SELECT EmployeeId, EmployeeName FROM Employees WHERE Salary > 60000
UNION ALL
SELECT EmployeeId, EmployeeName FROM Employees WHERE HireDate < '2022-01-01';

-- 15.3 Сравнение количества строк
SELECT
    (SELECT COUNT(*) FROM (
        SELECT EmployeeId FROM Employees WHERE Salary > 60000
        UNION
        SELECT EmployeeId FROM Employees WHERE HireDate < '2022-01-01') u)     AS UnionCount,
    (SELECT COUNT(*) FROM (
        SELECT EmployeeId FROM Employees WHERE Salary > 60000
        UNION ALL
        SELECT EmployeeId FROM Employees WHERE HireDate < '2022-01-01') ua)    AS UnionAllCount;
```

### Результат

Запрос 1 (зарплата > 60000): сотрудники 1, 2, 5, 10 — 4 строки.
Запрос 2 (нанят до 2022): сотрудники 1, 2, 3, 5, 8, 10 — 6 строк.

**15.1 UNION** — 6 строк: 1 Анна Иванова, 2 Пётр Сидоров, 3 Мария Кузнецова, 5 Ольга Смирнова, 8 Сергей Морозов, 10 Алексей Лебедев.

**15.2 UNION ALL** — 10 строк: 4 + 6; сотрудники 1, 2, 5, 10 встречаются дважды.

**15.3**

| UnionCount | UnionAllCount |
|---|---|
| 6 | 10 |

### Вывод

Разница (4 строки) — это ровно пересечение двух выборок. `UNION` удаляет дубликаты (дорогая операция сортировки/хэширования), `UNION ALL` — нет и работает быстрее; если пересечений быть не может, лучше использовать `UNION ALL`. Количество и типы столбцов в обоих запросах должны совпадать.

---

## Task 16 — UPDATE: изменение данных

### Запрос

```sql
BEGIN TRANSACTION;

-- Состояние ДО
SELECT EmployeeId, EmployeeName, Salary, HireDate, DepartmentId
FROM Employees ORDER BY EmployeeId;

-- 16.1 +10% зарплаты отдела 1
UPDATE Employees SET Salary = Salary * 1.10 WHERE DepartmentId = 1;

-- 16.2 +5000 нанятым до 2022 года
UPDATE Employees SET Salary = Salary + 5000 WHERE HireDate < '2022-01-01';

-- 16.3 Изменить дату найма одного сотрудника
UPDATE Employees SET HireDate = '2024-02-01' WHERE EmployeeId = 9;

-- Состояние ПОСЛЕ
SELECT EmployeeId, EmployeeName, Salary, HireDate, DepartmentId
FROM Employees ORDER BY EmployeeId;

-- Возвращаем исходные данные, чтобы остальные задания считались на них
ROLLBACK TRANSACTION;

SELECT EmployeeId, Salary, HireDate FROM Employees ORDER BY EmployeeId;
```

### Результат

Сообщения: 16.1 — `(4 rows affected)`, 16.2 — `(6 rows affected)`, 16.3 — `(1 row affected)`.

Состояние ДО → ПОСЛЕ (внутри транзакции):

| EmployeeId | EmployeeName | Salary до | Salary после | Что изменилось |
|---|---|---|---|---|
| 1 | Анна Иванова | 65000.00 | 76500.00 | +10% (отдел 1), затем +5000 (найм до 2022) |
| 2 | Пётр Сидоров | 72000.00 | 84200.00 | +10%, +5000 |
| 3 | Мария Кузнецова | 55000.00 | 65500.00 | +10%, +5000 |
| 4 | Иван Петров | 48000.00 | 52800.00 | только +10% (нанят в 2023) |
| 5 | Ольга Смирнова | 61000.00 | 66000.00 | только +5000 (отдел 2) |
| 6 | Дмитрий Орлов | 58000.00 | 58000.00 | не менялась |
| 7 | Елена Волкова | 52000.00 | 52000.00 | не менялась |
| 8 | Сергей Морозов | 45000.00 | 50000.00 | только +5000 |
| 9 | Наталья Соколова | 43000.00 | 43000.00 | `HireDate`: 2024-01-15 → 2024-02-01 |
| 10 | Алексей Лебедев | 90000.00 | 95000.00 | только +5000 |

После `ROLLBACK` итоговый `SELECT` показывает исходные значения (65000, 72000, …, 90000; `HireDate` Натальи — 2024-01-15).

### Вывод

Первый `UPDATE` сработал на 4 строках, второй — на 6; у сотрудников 1, 2 и 3 изменения наложились друг на друга (порядок операций влияет на итог: сначала процент, потом фиксированная прибавка). `WHERE` обязателен: без него изменились бы все строки. Транзакция с откатом позволила показать «после» и не испортить данные для следующих заданий.

---

## Task 17 — DELETE: удаление данных

### Запрос

```sql
SELECT COUNT(*) AS EP_Count FROM EmployeeProjects;           -- ДО

-- 17.1 Назначения с Hours = 0 (если есть)
DELETE FROM EmployeeProjects WHERE Hours = 0;
SELECT COUNT(*) AS EP_Count FROM EmployeeProjects;           -- ПОСЛЕ

-- 17.2 Удаление сотрудника. Создаём тестового, чтобы не трогать основные данные.
DECLARE @NewId INT;
INSERT INTO Employees (EmployeeName, Salary, HireDate, DepartmentId, ManagerId)
VALUES (N'Тест Тестов', 30000, '2024-03-01', 3, NULL);
SET @NewId = SCOPE_IDENTITY();
INSERT INTO EmployeeProjects (EmployeeId, ProjectId, Hours) VALUES (@NewId, 1, 10);

SELECT COUNT(*) AS Emp_Count FROM Employees;                 -- 11
SELECT COUNT(*) AS EP_Count  FROM EmployeeProjects;          -- 15

-- Сначала дочерние строки (из-за FK), затем сотрудник
DELETE FROM EmployeeProjects WHERE EmployeeId = @NewId;
SELECT COUNT(*) AS EP_Count FROM EmployeeProjects;           -- 14

DELETE FROM Employees WHERE EmployeeId = @NewId;
SELECT COUNT(*) AS Emp_Count FROM Employees;                 -- 10

-- 17.3 DELETE с WHERE и без — на тестовой копии таблицы
SELECT * INTO Employees_Copy FROM Employees;
SELECT COUNT(*) AS Copy_Count FROM Employees_Copy;           -- 10

DELETE FROM Employees_Copy WHERE DepartmentId = 3;
SELECT COUNT(*) AS Copy_Count FROM Employees_Copy;           -- 8

DELETE FROM Employees_Copy;
SELECT COUNT(*) AS Copy_Count FROM Employees_Copy;           -- 0

DROP TABLE Employees_Copy;
```

### Результат

| Шаг | Сообщение | `COUNT(*)` |
|---|---|---|
| EmployeeProjects до | — | 14 |
| 17.1 `DELETE ... Hours = 0` | `(0 rows affected)` | 14 |
| Вставка тестового сотрудника | — | Employees: 11, EmployeeProjects: 15 |
| Удаление его назначений | `(1 row affected)` | EmployeeProjects: 14 |
| Удаление сотрудника | `(1 row affected)` | Employees: 10 |
| 17.3 копия до | — | 10 |
| `DELETE ... WHERE DepartmentId = 3` | `(2 rows affected)` | 8 |
| `DELETE` без `WHERE` | `(8 rows affected)` | 0 |

Попытка сначала удалить сотрудника (без удаления назначений) завершилась бы ошибкой 547 — конфликт с `FK_EP_Employees`.

### Вывод

В исходных данных нет назначений с `Hours = 0`, поэтому первый `DELETE` затронул 0 строк — это нормальный результат. Порядок удаления важен: сначала дочерняя таблица, затем родительская. `DELETE` без `WHERE` очищает всю таблицу, поэтому его показано только на копии (`SELECT ... INTO` не переносит ключи и FK).

---

## Task 18 — Транзакции: BEGIN TRAN / COMMIT / ROLLBACK

### Запрос

```sql
-- Исходное значение
SELECT EmployeeId, Salary FROM Employees WHERE EmployeeId = 1;

-- Часть 1: ROLLBACK
BEGIN TRANSACTION;
    UPDATE Employees SET Salary = Salary + 1000 WHERE EmployeeId = 1;
    SELECT EmployeeId, Salary AS SalaryInsideTran FROM Employees WHERE EmployeeId = 1;
ROLLBACK TRANSACTION;
SELECT EmployeeId, Salary AS SalaryAfterRollback FROM Employees WHERE EmployeeId = 1;

-- Часть 2: COMMIT
BEGIN TRANSACTION;
    UPDATE Employees SET Salary = Salary + 1000 WHERE EmployeeId = 1;
COMMIT TRANSACTION;
SELECT EmployeeId, Salary AS SalaryAfterCommit FROM Employees WHERE EmployeeId = 1;

-- Возвращаем исходное значение
UPDATE Employees SET Salary = 65000 WHERE EmployeeId = 1;
SELECT EmployeeId, Salary AS SalaryRestored FROM Employees WHERE EmployeeId = 1;
```

### Результат

| Шаг | Salary сотрудника 1 |
|---|---|
| Исходное | 65000.00 |
| Внутри транзакции (после `UPDATE`) | 66000.00 |
| После `ROLLBACK` | 65000.00 |
| После `COMMIT` | 66000.00 |
| После возврата исходного значения | 65000.00 |

### Вывод

Внутри транзакции изменение видно только её владельцу; `ROLLBACK` полностью его отменяет, `COMMIT` — фиксирует. Последний `UPDATE` — не часть задания, а возврат данных в исходное состояние. В реальном коде транзакцию стоит оборачивать в `TRY...CATCH` с `ROLLBACK` в блоке `CATCH`.

---

## Task 19 — Представления (VIEW)

### Запрос

```sql
CREATE VIEW vw_EmployeeDetails
AS
SELECT
    e.EmployeeName,
    d.DepartmentName,
    p.ProjectName,
    ep.Hours
FROM Employees e
INNER JOIN Departments      d  ON d.DepartmentId = e.DepartmentId
INNER JOIN EmployeeProjects ep ON ep.EmployeeId  = e.EmployeeId
INNER JOIN Projects         p  ON p.ProjectId    = ep.ProjectId;
GO

SELECT * FROM vw_EmployeeDetails
ORDER BY DepartmentName, EmployeeName, ProjectName;
GO

DROP VIEW vw_EmployeeDetails;
GO

-- Проверка, что представления больше нет
SELECT OBJECT_ID('vw_EmployeeDetails', 'V') AS ViewId;
```

### Результат

`SELECT * FROM vw_EmployeeDetails` — 14 строк, те же, что в задании 10:

| EmployeeName | DepartmentName | ProjectName | Hours |
|---|---|---|---|
| Дмитрий Орлов | Аналитика | AI Assistant | 50 |
| Дмитрий Орлов | Аналитика | Data Warehouse | 70 |
| Елена Волкова | Аналитика | Mobile App | 95 |
| Ольга Смирнова | Аналитика | Data Warehouse | 110 |
| Алексей Лебедев | Менеджмент | AI Assistant | 200 |
| Алексей Лебедев | Менеджмент | CRM System | 30 |
| Анна Иванова | Разработка | CRM System | 120 |
| Анна Иванова | Разработка | Data Warehouse | 80 |
| Иван Петров | Разработка | Legacy Refactor | 40 |
| Иван Петров | Разработка | Mobile App | 90 |
| Мария Кузнецова | Разработка | Mobile App | 100 |
| Пётр Сидоров | Разработка | AI Assistant | 60 |
| Пётр Сидоров | Разработка | CRM System | 150 |
| Сергей Морозов | Тестирование | Legacy Refactor | 130 |

После `DROP VIEW`: `ViewId = NULL`.

### Вывод

`Employees` и `Projects` напрямую не связаны — связь идёт через `EmployeeProjects`, поэтому в представление включена и она. `CREATE VIEW` должен быть первой командой в пакете (отсюда `GO`). `ORDER BY` внутри представления без `TOP` недопустим, сортировка делается в обращающемся запросе.

---

## Task 20 — Индексы: создание и анализ

### Запрос

```sql
SET STATISTICS IO ON;
SET STATISTICS TIME ON;

-- ДО индексов (включить Actual Execution Plan: Ctrl+M)
SELECT EmployeeId, EmployeeName, Salary
FROM Employees
WHERE DepartmentId = 1 AND Salary > 50000;

-- Индексы
CREATE NONCLUSTERED INDEX IX_Employees_DepartmentId ON Employees(DepartmentId);
CREATE NONCLUSTERED INDEX IX_Employees_Salary       ON Employees(Salary);

-- ПОСЛЕ индексов
SELECT EmployeeId, EmployeeName, Salary
FROM Employees
WHERE DepartmentId = 1 AND Salary > 50000;

-- Для наглядности: то же на большой тестовой таблице (100 000 строк)
CREATE TABLE EmployeesBig (
    EmployeeId   INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeName NVARCHAR(100) NOT NULL,
    Salary       DECIMAL(10,2) NOT NULL,
    DepartmentId INT NOT NULL,
    Filler       CHAR(200) NOT NULL DEFAULT 'x'
);

INSERT INTO EmployeesBig (EmployeeName, Salary, DepartmentId)
SELECT TOP (100000)
    N'Employee ' + CAST(ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS NVARCHAR(10)),
    30000 + ABS(CHECKSUM(NEWID())) % 70000,
    ABS(CHECKSUM(NEWID())) % 4 + 1
FROM sys.all_objects a CROSS JOIN sys.all_objects b;

-- 20.1 Без индексов
SELECT EmployeeId, EmployeeName, Salary FROM EmployeesBig
WHERE DepartmentId = 1 AND Salary > 50000;

-- 20.2 С индексами (по заданию)
CREATE NONCLUSTERED INDEX IX_Big_DepartmentId ON EmployeesBig(DepartmentId);
CREATE NONCLUSTERED INDEX IX_Big_Salary       ON EmployeesBig(Salary);
SELECT EmployeeId, EmployeeName, Salary FROM EmployeesBig
WHERE DepartmentId = 1 AND Salary > 50000;

-- 20.3 Составной покрывающий индекс
CREATE NONCLUSTERED INDEX IX_Big_Dept_Salary ON EmployeesBig(DepartmentId, Salary)
    INCLUDE (EmployeeName);
SELECT EmployeeId, EmployeeName, Salary FROM EmployeesBig
WHERE DepartmentId = 1 AND Salary > 50000;

-- Уборка
DROP TABLE EmployeesBig;
DROP INDEX IX_Employees_DepartmentId ON Employees;
DROP INDEX IX_Employees_Salary ON Employees;
```

### Результат

Результат запроса на `Employees` — 3 строки (Пётр Сидоров, Анна Иванова, Мария Кузнецова).

| Сценарий | Оператор в плане | logical reads |
|---|---|---|
| `Employees`, без индексов | _…_ | _…_ |
| `Employees`, с индексами | _…_ | _…_ |
| `EmployeesBig`, без индексов | _…_ | _…_ |
| `EmployeesBig`, 2 отдельных индекса | _…_ | _…_ |
| `EmployeesBig`, составной индекс | _…_ | _…_ |

Чего ожидать:

- `Employees` (10 строк помещаются в одну страницу): оптимизатор, скорее всего, оставит `Clustered Index Scan` и в обоих вариантах будет около 1 logical read — индексы на такой таблице ничего не дают.
- `EmployeesBig`: без индексов — `Clustered Index Scan` и тысячи logical reads. С двумя отдельными индексами оптимизатор может всё равно выбрать scan: условие `DepartmentId = 1` возвращает ~25% строк, а `Salary > 50000` — ~70%, то есть оба индекса малоселективны, а `Key Lookup` за остальными столбцами дорог.
- Составной индекс `(DepartmentId, Salary) INCLUDE (EmployeeName)` покрывает запрос полностью: `Index Seek` без `Key Lookup` и заметно меньше logical reads.

### Вывод

Индекс помогает, только если условие достаточно селективно, а запрос не требует дорогих обращений за недостающими столбцами. Два отдельных индекса на столбцы одного условия `AND` — обычно хуже, чем один составной в правильном порядке (сначала столбец с равенством, затем диапазон). Каждый индекс замедляет `INSERT/UPDATE/DELETE` и занимает место, поэтому после эксперимента индексы удалены.

---

# Уровень 3 — Продвинутый

---

## Task 21 — Хранимые процедуры

### Запрос

```sql
CREATE OR ALTER PROCEDURE sp_GetEmployeesByDepartment
    @DeptId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT EmployeeId, EmployeeName, Salary, HireDate
    FROM Employees
    WHERE DepartmentId = @DeptId
    ORDER BY EmployeeId;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetAvgSalaryByDepartment
    @DeptId    INT,
    @AvgSalary DECIMAL(10,2) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @AvgSalary = CAST(AVG(Salary) AS DECIMAL(10,2))
    FROM Employees
    WHERE DepartmentId = @DeptId;      -- если сотрудников нет, будет NULL
END;
GO

-- Вызовы
EXEC sp_GetEmployeesByDepartment @DeptId = 1;

DECLARE @Avg DECIMAL(10,2);
EXEC sp_GetAvgSalaryByDepartment @DeptId = 1, @AvgSalary = @Avg OUTPUT;
SELECT @Avg AS AvgSalaryDept1;

-- Крайний случай: отдела (или сотрудников) нет
EXEC sp_GetAvgSalaryByDepartment @DeptId = 99, @AvgSalary = @Avg OUTPUT;
SELECT ISNULL(CONVERT(VARCHAR(20), @Avg), 'N/A') AS AvgSalaryDept99;
```

### Результат

`EXEC sp_GetEmployeesByDepartment @DeptId = 1`:

| EmployeeId | EmployeeName | Salary | HireDate |
|---|---|---|---|
| 1 | Анна Иванова | 65000.00 | 2020-03-15 |
| 2 | Пётр Сидоров | 72000.00 | 2019-07-01 |
| 3 | Мария Кузнецова | 55000.00 | 2021-11-20 |
| 4 | Иван Петров | 48000.00 | 2023-02-10 |

| AvgSalaryDept1 |
|---|
| 60000.00 |

Для отдела 99 — `N/A` (пустой результат первой процедуры, `NULL` во второй).

### Вывод

`OUTPUT`-параметр нужно указывать и при вызове. `SET NOCOUNT ON` убирает лишние сообщения «rows affected». Префикс `sp_` Microsoft не рекомендует для пользовательских процедур (SQL Server сначала ищет такие имена в `master`), но он задан условием задания; в реальных проектах лучше `usp_` или `Get...`.

---

## Task 22 — Функции: скалярная и табличная

### Запрос

```sql
CREATE OR ALTER FUNCTION dbo.fn_GetAnnualSalary (@MonthlySalary DECIMAL(10,2))
RETURNS DECIMAL(12,2)
AS
BEGIN
    RETURN @MonthlySalary * 12;
END;
GO

CREATE OR ALTER FUNCTION dbo.fn_GetEmployeesBySalaryRange (@Min DECIMAL(10,2), @Max DECIMAL(10,2))
RETURNS TABLE
AS
RETURN
(
    SELECT EmployeeId, EmployeeName, Salary, DepartmentId
    FROM Employees
    WHERE Salary BETWEEN @Min AND @Max
);
GO

-- Скалярная — в SELECT
SELECT EmployeeName, Salary, dbo.fn_GetAnnualSalary(Salary) AS AnnualSalary
FROM Employees
ORDER BY EmployeeId;

-- Табличная — в FROM
SELECT * FROM dbo.fn_GetEmployeesBySalaryRange(50000, 60000)
ORDER BY Salary DESC;
```

### Результат

Скалярная (предполагается, что `Salary` — месячная):

| EmployeeName | Salary | AnnualSalary |
|---|---|---|
| Анна Иванова | 65000.00 | 780000.00 |
| Пётр Сидоров | 72000.00 | 864000.00 |
| Мария Кузнецова | 55000.00 | 660000.00 |
| Иван Петров | 48000.00 | 576000.00 |
| Ольга Смирнова | 61000.00 | 732000.00 |
| Дмитрий Орлов | 58000.00 | 696000.00 |
| Елена Волкова | 52000.00 | 624000.00 |
| Сергей Морозов | 45000.00 | 540000.00 |
| Наталья Соколова | 43000.00 | 516000.00 |
| Алексей Лебедев | 90000.00 | 1080000.00 |

Табличная `(50000, 60000)`:

| EmployeeId | EmployeeName | Salary | DepartmentId |
|---|---|---|---|
| 6 | Дмитрий Орлов | 58000.00 | 2 |
| 3 | Мария Кузнецова | 55000.00 | 1 |
| 7 | Елена Волкова | 52000.00 | 2 |

### Вывод

В условии параметр записан как `DECIMAL` без точности — в T-SQL это `DECIMAL(18,0)`, и копейки отбрасывались бы, поэтому указана точность `(10,2)`. Табличная функция в виде одного `RETURN (SELECT ...)` (inline TVF) работает как параметризованное представление и оптимизируется лучше многооператорной. Скалярные функции вызываются для каждой строки — на больших таблицах это может быть медленно.

---

## Task 23 — Оконные функции: ROW_NUMBER, RANK, DENSE_RANK

### Запрос

```sql
-- 23.1 Основное задание
SELECT
    EmployeeName,
    DepartmentId,
    Salary,
    ROW_NUMBER() OVER (ORDER BY Salary DESC)                           AS RowNum,
    RANK()       OVER (PARTITION BY DepartmentId ORDER BY Salary DESC) AS RankInDept,
    DENSE_RANK() OVER (PARTITION BY DepartmentId ORDER BY Salary DESC) AS DenseRankInDept
FROM Employees
ORDER BY RowNum;

-- 23.2 Разница RANK / DENSE_RANK при равных значениях
--      (зарплаты в данных уникальны, поэтому ранжируем по DepartmentId)
SELECT
    EmployeeName,
    DepartmentId,
    RANK()       OVER (ORDER BY DepartmentId) AS Rnk,
    DENSE_RANK() OVER (ORDER BY DepartmentId) AS DenseRnk
FROM Employees
ORDER BY DepartmentId, EmployeeId;
```

### Результат

**23.1**

| EmployeeName | DepartmentId | Salary | RowNum | RankInDept | DenseRankInDept |
|---|---|---|---|---|---|
| Алексей Лебедев | 4 | 90000.00 | 1 | 1 | 1 |
| Пётр Сидоров | 1 | 72000.00 | 2 | 1 | 1 |
| Анна Иванова | 1 | 65000.00 | 3 | 2 | 2 |
| Ольга Смирнова | 2 | 61000.00 | 4 | 1 | 1 |
| Дмитрий Орлов | 2 | 58000.00 | 5 | 2 | 2 |
| Мария Кузнецова | 1 | 55000.00 | 6 | 3 | 3 |
| Елена Волкова | 2 | 52000.00 | 7 | 3 | 3 |
| Иван Петров | 1 | 48000.00 | 8 | 4 | 4 |
| Сергей Морозов | 3 | 45000.00 | 9 | 1 | 1 |
| Наталья Соколова | 3 | 43000.00 | 10 | 2 | 2 |

**23.2** (внутри одного отдела порядок строк при равных значениях не гарантирован, здесь отсортировано по `EmployeeId`)

| DepartmentId | Сотрудников | Rnk | DenseRnk |
|---|---|---|---|
| 1 | 4 | 1 | 1 |
| 2 | 3 | 5 | 2 |
| 3 | 2 | 8 | 3 |
| 4 | 1 | 10 | 4 |

### Вывод

В 23.1 `RANK` и `DENSE_RANK` совпадают, потому что зарплаты у всех разные. 23.2 показывает разницу: после группы из 4 равных значений `RANK` перескакивает на 5 (пропуски), а `DENSE_RANK` идёт подряд — 2. `ROW_NUMBER` всегда уникален, даже при равных значениях; чтобы порядок среди равных был детерминированным, в `ORDER BY` нужен дополнительный ключ.

---

## Task 24 — Оконные агрегаты: SUM OVER, AVG OVER

### Запрос

```sql
SELECT
    EmployeeName,
    DepartmentId,
    Salary,
    SUM(Salary) OVER (PARTITION BY DepartmentId)                                   AS DeptTotal,
    CAST(AVG(Salary) OVER (PARTITION BY DepartmentId) AS DECIMAL(10,2))            AS DeptAvg,
    CAST(Salary - AVG(Salary) OVER (PARTITION BY DepartmentId) AS DECIMAL(10,2))   AS DiffFromAvg,
    SUM(Salary) OVER (PARTITION BY DepartmentId
                      ORDER BY Salary DESC
                      ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW)            AS RunningTotal
FROM Employees
ORDER BY DepartmentId, Salary DESC;
```

### Результат

| EmployeeName | DepartmentId | Salary | DeptTotal | DeptAvg | DiffFromAvg | RunningTotal |
|---|---|---|---|---|---|---|
| Пётр Сидоров | 1 | 72000.00 | 240000.00 | 60000.00 | 12000.00 | 72000.00 |
| Анна Иванова | 1 | 65000.00 | 240000.00 | 60000.00 | 5000.00 | 137000.00 |
| Мария Кузнецова | 1 | 55000.00 | 240000.00 | 60000.00 | -5000.00 | 192000.00 |
| Иван Петров | 1 | 48000.00 | 240000.00 | 60000.00 | -12000.00 | 240000.00 |
| Ольга Смирнова | 2 | 61000.00 | 171000.00 | 57000.00 | 4000.00 | 61000.00 |
| Дмитрий Орлов | 2 | 58000.00 | 171000.00 | 57000.00 | 1000.00 | 119000.00 |
| Елена Волкова | 2 | 52000.00 | 171000.00 | 57000.00 | -5000.00 | 171000.00 |
| Сергей Морозов | 3 | 45000.00 | 88000.00 | 44000.00 | 1000.00 | 45000.00 |
| Наталья Соколова | 3 | 43000.00 | 88000.00 | 44000.00 | -1000.00 | 88000.00 |
| Алексей Лебедев | 4 | 90000.00 | 90000.00 | 90000.00 | 0.00 | 90000.00 |

`DeptTotal` и `DeptAvg` совпадают с результатом `GROUP BY` из задания 6 (240000/60000, 171000/57000, 88000/44000, 90000/90000); последнее значение `RunningTotal` в каждом отделе равно `DeptTotal`.

### Вывод

В отличие от `GROUP BY`, оконные агрегаты не схлопывают строки — рядом с данными сотрудника видна статистика отдела, а отдельный коррелированный подзапрос (задание 13) не нужен. Для накопительного итога указан явный фрейм `ROWS ...`: без него по умолчанию используется `RANGE`, который при равных значениях `ORDER BY` объединяет строки.

---

## Task 25 — CTE

### Запрос

```sql
-- 25.1 Сотрудники выше средней — через CTE
WITH AvgSalary AS (
    SELECT AVG(Salary) AS AvgValue FROM Employees
)
SELECT e.EmployeeId, e.EmployeeName, e.Salary
FROM Employees e
CROSS JOIN AvgSalary a
WHERE e.Salary > a.AvgValue
ORDER BY e.Salary DESC;

-- 25.2 Топ-3 отдела по средней зарплате
WITH DeptAvg AS (
    SELECT DepartmentId, AVG(Salary) AS AvgSalary
    FROM Employees
    GROUP BY DepartmentId
)
SELECT TOP 3
    d.DepartmentName,
    CAST(da.AvgSalary AS DECIMAL(10,2)) AS AvgSalary
FROM DeptAvg da
INNER JOIN Departments d ON d.DepartmentId = da.DepartmentId
ORDER BY da.AvgSalary DESC;
```

### Результат

**25.1** — те же 4 строки, что и в задании 12.1:

| EmployeeId | EmployeeName | Salary |
|---|---|---|
| 10 | Алексей Лебедев | 90000.00 |
| 2 | Пётр Сидоров | 72000.00 |
| 1 | Анна Иванова | 65000.00 |
| 5 | Ольга Смирнова | 61000.00 |

**25.2**

| DepartmentName | AvgSalary |
|---|---|
| Менеджмент | 90000.00 |
| Разработка | 60000.00 |
| Аналитика | 57000.00 |

### Вывод

Читаемость: CTE даёт запросу «имена» (`AvgSalary`, `DeptAvg`) и читается сверху вниз — сначала что считаем, потом как используем. Для одного скалярного значения вариант с подзапросом (задание 12) короче и не хуже по производительности; CTE выигрывает, когда промежуточный результат используется несколько раз или шагов несколько. CTE — не материализованная таблица: оптимизатор разворачивает его в основной запрос.

---

## Task 26 — CASE: условная логика в SELECT

### Запрос

```sql
SELECT
    EmployeeName,
    Salary,
    CASE
        WHEN Salary > 60000              THEN 'High'
        WHEN Salary BETWEEN 40000 AND 60000 THEN 'Medium'
        ELSE 'Low'
    END AS SalaryLevel,
    HireDate,
    CASE
        WHEN HireDate <  '2020-01-01' THEN 'Senior'
        WHEN HireDate <  '2023-01-01' THEN 'Mid'      -- 2020–2022
        ELSE 'Junior'
    END AS Tenure
FROM Employees
ORDER BY EmployeeId;
```

### Результат

| EmployeeName | Salary | SalaryLevel | HireDate | Tenure |
|---|---|---|---|---|
| Анна Иванова | 65000.00 | High | 2020-03-15 | Mid |
| Пётр Сидоров | 72000.00 | High | 2019-07-01 | Senior |
| Мария Кузнецова | 55000.00 | Medium | 2021-11-20 | Mid |
| Иван Петров | 48000.00 | Medium | 2023-02-10 | Junior |
| Ольга Смирнова | 61000.00 | High | 2020-09-05 | Mid |
| Дмитрий Орлов | 58000.00 | Medium | 2022-04-18 | Mid |
| Елена Волкова | 52000.00 | Medium | 2023-06-01 | Junior |
| Сергей Морозов | 45000.00 | Medium | 2021-12-12 | Mid |
| Наталья Соколова | 43000.00 | Medium | 2024-01-15 | Junior |
| Алексей Лебедев | 90000.00 | High | 2018-05-25 | Senior |

`NULL` нет; уровня `Low` нет — минимальная зарплата 43000.

### Вывод

`CASE` проверяет условия по порядку и берёт первое совпавшее, поэтому границы перекрывать не нужно. Диапазон дат лучше задавать как `< '2023-01-01'`, а не `<= '2022-12-31'` — так не будет проблем с типами, содержащими время. `ELSE` защищает от `NULL`, если значение не подошло ни под одно условие. Зарплата ровно 60000 попадает в `Medium` (по условию «> 60000» для `High`).

---

## Task 27 — Работа с датами: DATEADD, DATEDIFF, FORMAT

### Запрос

```sql
SELECT
    EmployeeName,
    HireDate,
    DATEDIFF(YEAR, HireDate, GETDATE())  AS YearsDiff,         -- как в задании
    DATEDIFF(YEAR, HireDate, GETDATE())
      - CASE WHEN DATEADD(YEAR, DATEDIFF(YEAR, HireDate, GETDATE()), HireDate) > CAST(GETDATE() AS DATE)
             THEN 1 ELSE 0 END                                  AS FullYears,   -- полных лет
    DATEDIFF(MONTH, HireDate, GETDATE()) AS MonthsDiff,
    DATEADD(YEAR, 1, HireDate)           AS OneYearLater,
    FORMAT(HireDate, 'dd.MM.yyyy')       AS HireDateFmt
FROM Employees
ORDER BY EmployeeId;
```

### Результат

Расчёт на дату 14.11.2026 (при другой дате запуска цифры изменятся):

| EmployeeName | HireDate | YearsDiff | FullYears | MonthsDiff | OneYearLater | HireDateFmt |
|---|---|---|---|---|---|---|
| Анна Иванова | 2020-03-15 | 6 | 6 | 80 | 2021-03-15 | 15.03.2020 |
| Пётр Сидоров | 2019-07-01 | 7 | 7 | 88 | 2020-07-01 | 01.07.2019 |
| Мария Кузнецова | 2021-11-20 | 5 | **4** | 60 | 2022-11-20 | 20.11.2021 |
| Иван Петров | 2023-02-10 | 3 | 3 | 45 | 2024-02-10 | 10.02.2023 |
| Ольга Смирнова | 2020-09-05 | 6 | 6 | 74 | 2021-09-05 | 05.09.2020 |
| Дмитрий Орлов | 2022-04-18 | 4 | 4 | 55 | 2023-04-18 | 18.04.2022 |
| Елена Волкова | 2023-06-01 | 3 | 3 | 41 | 2024-06-01 | 01.06.2023 |
| Сергей Морозов | 2021-12-12 | 5 | **4** | 59 | 2022-12-12 | 12.12.2021 |
| Наталья Соколова | 2024-01-15 | 2 | 2 | 34 | 2025-01-15 | 15.01.2024 |
| Алексей Лебедев | 2018-05-25 | 8 | 8 | 102 | 2019-05-25 | 25.05.2018 |

### Вывод

`DATEDIFF` считает не прошедшие периоды, а количество пересечённых границ (года/месяца). Поэтому `DATEDIFF(YEAR, ...)` для Марии Кузнецовой и Сергея Морозова даёт 5, хотя в этом году годовщина ещё не наступила и полных лет 4. Для точного стажа добавлена поправка через `DATEADD`; то же верно и для месяцев (у Марии полных месяцев 59, `DATEDIFF` покажет 60). `FORMAT` удобен, но заметно медленнее `CONVERT` на больших объёмах и возвращает `NVARCHAR`.

