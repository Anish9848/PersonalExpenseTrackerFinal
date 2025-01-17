using Microsoft.Data.Sqlite;
using PersonalExpenseTracker.Models;
using System.Collections.Generic;

namespace PersonalExpenseTracker.Services
{
    public class DebtService
    {
        private readonly SqliteConnection _dbConnection;

        // Constructor to initialize the database connection
        public DebtService(SqliteConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Retrieve all debts from the database
        public List<Debtitem> GetDebts()
        {
            var debts = new List<Debtitem>();

            // Create a command to fetch all debts
            var command = _dbConnection.CreateCommand();
            command.CommandText = "SELECT DebtId, DebtSource, DebtAmount, DebtDueDate, DebtTakenDate, Status FROM Debts";

            // Execute the command and read results
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Populate the Debtitem list with data from the database
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
            return debts; // Return the list of debts
        }

        // Add a new debt to the database
        public void AddDebt(Debtitem debt)
        {
            var command = _dbConnection.CreateCommand();

            // Parameterized query to prevent SQL injection
            command.CommandText = @"
                INSERT INTO Debts (DebtSource, DebtAmount, DebtDueDate, DebtTakenDate, Status)
                VALUES ($source, $amount, $dueDate, $takenDate, $status)";

            // Bind parameters to the query
            command.Parameters.AddWithValue("$source", debt.DebtSource);
            command.Parameters.AddWithValue("$amount", debt.DebtAmount);
            command.Parameters.AddWithValue("$dueDate", debt.DebtDueDate);
            command.Parameters.AddWithValue("$takenDate", debt.DebtTakenDate);
            command.Parameters.AddWithValue("$status", debt.Status);

            // Execute the query to insert the debt
            command.ExecuteNonQuery();
        }

        // Update an existing debt in the database
        public void UpdateDebt(Debtitem debt)
        {
            var command = _dbConnection.CreateCommand();

            // Parameterized query to update debt details
            command.CommandText = @"
                UPDATE Debts
                SET DebtSource = $source, DebtAmount = $amount, DebtDueDate = $dueDate, DebtTakenDate = $takenDate, Status = $status
                WHERE DebtId = $id";

            // Bind parameters to the query
            command.Parameters.AddWithValue("$source", debt.DebtSource);
            command.Parameters.AddWithValue("$amount", debt.DebtAmount);
            command.Parameters.AddWithValue("$dueDate", debt.DebtDueDate);
            command.Parameters.AddWithValue("$takenDate", debt.DebtTakenDate);
            command.Parameters.AddWithValue("$status", debt.Status);
            command.Parameters.AddWithValue("$id", debt.DebtId);

            // Execute the query to update the debt
            command.ExecuteNonQuery();
        }

        // Delete a debt from the database by its ID
        public void DeleteDebt(int id)
        {
            var command = _dbConnection.CreateCommand();

            // Parameterized query to delete a debt
            command.CommandText = "DELETE FROM Debts WHERE DebtId = $id";

            // Bind the debt ID parameter
            command.Parameters.AddWithValue("$id", id);

            // Execute the query to delete the debt
            command.ExecuteNonQuery();
        }
    }
}
