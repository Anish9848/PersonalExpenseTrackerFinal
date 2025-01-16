using Microsoft.Extensions.Logging;
using PersonalExpenseTracker.Database;
using PersonalExpenseTracker.Services;
using Microsoft.Data.Sqlite;

namespace PersonalExpenseTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddSingleton<SqliteConnection>(s =>
            {
                var projectDirectory = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
                var dbPath = Path.Combine(projectDirectory, "expenses.db");

                var connectionString = $"Data Source={dbPath}";
                var connection = new SqliteConnection(connectionString);
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Tags (
                        TagId INTEGER PRIMARY KEY AUTOINCREMENT,
                        TagName TEXT NOT NULL
                    );
                    CREATE TABLE IF NOT EXISTS Transactions (
                        transactionId INTEGER PRIMARY KEY AUTOINCREMENT,
                        transactionTitle TEXT NOT NULL,
                        transactionDescription TEXT,
                        transactionAmount REAL,
                        transactionDate TEXT,
                        transactionTag TEXT,
                        transactionType TEXT
                    );
                    CREATE TABLE IF NOT EXISTS Debts (
                        DebtId INTEGER PRIMARY KEY AUTOINCREMENT,
                        DebtSource TEXT NOT NULL,
                        DebtAmount REAL,
                        DebtDueDate TEXT,
                        DebtTakenDate TEXT,
                        Status TEXT
                    )";
                command.ExecuteNonQuery();

                return connection;
            });

            builder.Services.AddSingleton<TransactionService>();
            builder.Services.AddSingleton<TagService>();
            builder.Services.AddSingleton<DebtService>();
            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}