using Microsoft.Data.Sqlite;
using PersonalExpenseTracker.Models;
using System.Collections.Generic;

namespace PersonalExpenseTracker.Services
{
    public class TransactionService
    {
        private readonly SqliteConnection _dbConnection;

        public TransactionService(SqliteConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public List<Transactionitem> GetTransactions()
        {
            var transactions = new List<Transactionitem>();
            var command = _dbConnection.CreateCommand();
            command.CommandText = "SELECT transactionId, transactionTitle, transactionDescription, transactionAmount, transactionDate, transactionTag FROM Transactions";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    transactions.Add(new Transactionitem
                    {
                        transactionId = reader.GetInt32(0),
                        transactionTitle = reader.GetString(1),
                        transactionDescription = reader.GetString(2),
                        transactionAmount = reader.GetDecimal(3),
                        transactionDate = reader.GetDateTime(4),
                        transactionTag = reader.GetString(5)
                    });
                }
            }
            return transactions;
        }

        public void AddTransaction(Transactionitem transaction)
        {
            var command = _dbConnection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Transactions (transactionTitle, transactionDescription, transactionAmount, transactionDate, transactionTag)
                VALUES ($title, $description, $amount, $date, $tag)";
            command.Parameters.AddWithValue("$title", transaction.transactionTitle);
            command.Parameters.AddWithValue("$description", transaction.transactionDescription);
            command.Parameters.AddWithValue("$amount", transaction.transactionAmount);
            command.Parameters.AddWithValue("$date", transaction.transactionDate);
            command.Parameters.AddWithValue("$tag", transaction.transactionTag);
            command.ExecuteNonQuery();
        }

        public void UpdateTransaction(Transactionitem transaction)
        {
            var command = _dbConnection.CreateCommand();
            command.CommandText = @"
                UPDATE Transactions
                SET transactionTitle = $title, transactionDescription = $description, transactionAmount = $amount, transactionDate = $date, transactionTag = $tag
                WHERE transactionId = $id";
            command.Parameters.AddWithValue("$title", transaction.transactionTitle);
            command.Parameters.AddWithValue("$description", transaction.transactionDescription);
            command.Parameters.AddWithValue("$amount", transaction.transactionAmount);
            command.Parameters.AddWithValue("$date", transaction.transactionDate);
            command.Parameters.AddWithValue("$tag", transaction.transactionTag);
            command.Parameters.AddWithValue("$id", transaction.transactionId);
            command.ExecuteNonQuery();
        }

        public void DeleteTransaction(int id)
        {
            var command = _dbConnection.CreateCommand();
            command.CommandText = @"
                DELETE FROM Transactions
                WHERE transactionId = $id";
            command.Parameters.AddWithValue("$id", id);
            command.ExecuteNonQuery();
        }
    }
}