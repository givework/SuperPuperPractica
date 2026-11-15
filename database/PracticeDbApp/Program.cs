using System.Data;
using System.Globalization;
using System.Text;
using Microsoft.Data.SqlClient;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var cs = Environment.GetEnvironmentVariable("PRACTICE_DB_CONNECTION");
if (string.IsNullOrWhiteSpace(cs))
{
    Console.Error.WriteLine("Не задана переменная окружения PRACTICE_DB_CONNECTION.");
    Console.Error.WriteLine(@"Пример: Server=localhost\SQLEXPRESS;Database=PracticeDB;Trusted_Connection=True;TrustServerCertificate=True;");
    return 1;
}

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== PracticeDB ===");
    Console.WriteLine("1. Список сотрудников (задание 28)");
    Console.WriteLine("2. Добавить сотрудника (задание 29)");
    Console.WriteLine("3. Отчёт по отделам (задание 30)");
    Console.WriteLine("0. Выход");
    Console.Write("Выбор: ");

    switch (Console.ReadLine()?.Trim())
    {
        case "1": await Db.ListEmployeesAsync(cs); break;
        case "2": await AddDialogAsync(cs); break;
        case "3": await Db.DepartmentReportAsync(cs); break;
        case "0": return 0;
        default: Console.WriteLine("Неизвестный пункт меню."); break;
    }
}

static async Task AddDialogAsync(string cs)
{
    Console.Write("Имя: ");
    var name = Console.ReadLine() ?? "";

    Console.Write("Зарплата: ");
    var salaryText = (Console.ReadLine() ?? "").Trim().Replace(',', '.');
    if (!decimal.TryParse(salaryText, NumberStyles.Number, CultureInfo.InvariantCulture, out var salary))
    {
        Console.WriteLine("Ошибка: зарплата должна быть числом.");
        return;
    }

    Console.Write("Id отдела: ");
    if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var deptId))
    {
        Console.WriteLine("Ошибка: Id отдела должен быть целым числом.");
        return;
    }

    await Db.AddEmployeeAsync(cs, name, salary, deptId);
}

static class Db
{
    private const int ForeignKeyViolation = 547;
    private const int PrimaryKeyViolation = 2627;
    private const int UniqueIndexViolation = 2601;

    private static string Money(decimal v) => v.ToString("F2", CultureInfo.InvariantCulture);

    private static void ReportError(SqlException ex) =>
        Console.WriteLine($"Ошибка SQL Server #{ex.Number}: {ex.Message}");

    // Задание 28
    public static async Task ListEmployeesAsync(string cs)
    {
        try
        {
            await using var conn = new SqlConnection(cs);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand(
                "SELECT EmployeeName, Salary FROM Employees ORDER BY EmployeeId", conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            var count = 0;
            while (await reader.ReadAsync())
            {
                Console.WriteLine($"{reader.GetString(0),-20} {Money(reader.GetDecimal(1)),10}");
                count++;
            }
            Console.WriteLine(count == 0 ? "Сотрудников нет." : $"Всего: {count}");
        }
        catch (SqlException ex) { ReportError(ex); }
    }

    // Задание 29
    public static async Task<int?> AddEmployeeAsync(string cs, string name, decimal salary, int departmentId)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > 100)
        {
            Console.WriteLine("Ошибка: имя должно быть непустым и не длиннее 100 символов.");
            return null;
        }
        if (salary <= 0 || salary > 99_999_999.99m)
        {
            Console.WriteLine("Ошибка: зарплата должна быть больше 0 и не превышать 99 999 999.99.");
            return null;
        }

        try
        {
            await using var conn = new SqlConnection(cs);
            await conn.OpenAsync();

            int newId;
            await using (var insert = new SqlCommand("""
                INSERT INTO Employees (EmployeeName, Salary, HireDate, DepartmentId, ManagerId)
                OUTPUT INSERTED.EmployeeId
                VALUES (@name, @salary, CAST(GETDATE() AS DATE), @deptId, NULL);
                """, conn))
            {
                insert.Parameters.Add("@name", SqlDbType.NVarChar, 100).Value = name.Trim();
                var p = insert.Parameters.Add("@salary", SqlDbType.Decimal);
                p.Precision = 10;
                p.Scale = 2;
                p.Value = salary;
                insert.Parameters.Add("@deptId", SqlDbType.Int).Value = departmentId;

                newId = (int)(await insert.ExecuteScalarAsync())!;
            }

            await using var select = new SqlCommand("""
                SELECT e.EmployeeId, e.EmployeeName, e.Salary, e.HireDate, d.DepartmentName
                FROM Employees e
                INNER JOIN Departments d ON d.DepartmentId = e.DepartmentId
                WHERE e.EmployeeId = @id;
                """, conn);
            select.Parameters.Add("@id", SqlDbType.Int).Value = newId;

            await using var reader = await select.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                Console.WriteLine("Сотрудник добавлен:");
                Console.WriteLine($"  Id:       {reader.GetInt32(0)}");
                Console.WriteLine($"  Имя:      {reader.GetString(1)}");
                Console.WriteLine($"  Зарплата: {Money(reader.GetDecimal(2))}");
                Console.WriteLine($"  Принят:   {reader.GetDateTime(3):dd.MM.yyyy}");
                Console.WriteLine($"  Отдел:    {reader.GetString(4)}");
            }
            return newId;
        }
        catch (SqlException ex) when (ex.Number == ForeignKeyViolation)
        {
            Console.WriteLine($"Ошибка: отдела с Id = {departmentId} не существует.");
        }
        catch (SqlException ex) when (ex.Number is PrimaryKeyViolation or UniqueIndexViolation)
        {
            Console.WriteLine("Ошибка: такая запись уже существует (дубликат ключа).");
        }
        catch (SqlException ex) { ReportError(ex); }
        return null;
    }

    // Задание 30
    public static async Task DepartmentReportAsync(string cs)
    {
        try
        {
            await using var conn = new SqlConnection(cs);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand("""
                SELECT d.DepartmentName,
                       COUNT(e.EmployeeId) AS EmpCount,
                       CAST(AVG(e.Salary) AS DECIMAL(10,2)) AS AvgSalary
                FROM Departments d
                LEFT JOIN Employees e ON d.DepartmentId = e.DepartmentId
                GROUP BY d.DepartmentName
                ORDER BY d.DepartmentName;
                """, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            var rows = new List<(string Name, string Count, string Avg)>();
            while (await reader.ReadAsync())
            {
                rows.Add((reader.GetString(0),
                          reader.GetInt32(1).ToString(CultureInfo.InvariantCulture),
                          reader.IsDBNull(2) ? "N/A" : Money(reader.GetDecimal(2))));
            }

            if (rows.Count == 0)
            {
                Console.WriteLine("Данных нет: в базе не найдено ни одного отдела.");
                return;
            }

            const string h1 = "Отдел", h2 = "Сотрудников", h3 = "Средняя зарплата";
            var w1 = Math.Max(h1.Length, rows.Max(r => r.Name.Length));
            var w2 = Math.Max(h2.Length, rows.Max(r => r.Count.Length));
            var w3 = Math.Max(h3.Length, rows.Max(r => r.Avg.Length));

            Console.WriteLine($"{h1.PadRight(w1)}  {h2.PadLeft(w2)}  {h3.PadLeft(w3)}");
            Console.WriteLine($"{new string('-', w1)}  {new string('-', w2)}  {new string('-', w3)}");
            foreach (var r in rows)
                Console.WriteLine($"{r.Name.PadRight(w1)}  {r.Count.PadLeft(w2)}  {r.Avg.PadLeft(w3)}");
        }
        catch (SqlException ex) { ReportError(ex); }
    }
}
