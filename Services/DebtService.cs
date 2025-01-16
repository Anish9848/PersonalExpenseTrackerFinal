using Microsoft.Data.Sqlite;
using PersonalExpenseTracker.Models;
using System.Collections.Generic;

namespace PersonalExpenseTracker.Services
{
    public class DebtService
    {
        private readonly SqliteConnection _dbConnection;

        public DebtService(SqliteConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public List<Debtitem> GetDebts()
        {
            var debts = new List<Debtitem>();
            var command = _dbConnection.CreateCommand();
            command.CommandText = "SELECT DebtId, DebtSource, DebtAmount, DebtDueDate, DebtTakenDate, Status FROM Debts";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    debts.Add(new Debtitem
                    {
                        DebtId = reader.GetInt32(0),
                        DebtSource = reader.GetString(1),
                        DebtAmount = reader.GetDecimal(2),
                        DebtDueDate = reader.GetDateTime(3),
                        DebtTakenDate = reader.GetDateTime(4),
                        Status = reader.GetString(5)
                    });
                }
            }
            return debts;
        }

        public void AddDebt(Debtitem debt)
        {
            var command = _dbConnection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Debts (DebtSource, DebtAmount, DebtDueDate, DebtTakenDate, Status)
                VALUES ($source, $amount, $dueDate, $takenDate, $status)";
            command.Parameters.AddWithValue("$source", debt.DebtSource);
            command.Parameters.AddWithValue("$amount", debt.DebtAmount);
            command.Parameters.AddWithValue("$dueDate", debt.DebtDueDate);
            command.Parameters.AddWithValue("$takenDate", debt.DebtTakenDate);
            command.Parameters.AddWithValue("$status", debt.Status);
            command.ExecuteNonQuery();
        }

        public void UpdateDebt(Debtitem debt)
        {
            var command = _dbConnection.CreateCommand();
            command.CommandText = @"
                UPDATE Debts
                SET DebtSource = $source, DebtAmount = $amount, DebtDueDate = $dueDate, DebtTakenDate = $takenDate, Status = $status
                WHERE DebtId = $id";
            command.Parameters.AddWithValue("$source", debt.DebtSource);
            command.Parameters.AddWithValue("$amount", debt.DebtAmount);
            command.Parameters.AddWithValue("$dueDate", debt.DebtDueDate);
            command.Parameters.AddWithValue("$takenDate", debt.DebtTakenDate);
            command.Parameters.AddWithValue("$status", debt.Status);
            command.Parameters.AddWithValue("$id", debt.DebtId);
            command.ExecuteNonQuery();
        }

        public void DeleteDebt(int id)
        {
            var command = _dbConnection.CreateCommand();
            command.CommandText = "DELETE FROM Debts WHERE DebtId = $id";
            command.Parameters.AddWithValue("$id", id);
            command.ExecuteNonQuery();
        }
    }
}