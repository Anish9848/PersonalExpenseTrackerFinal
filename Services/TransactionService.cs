using Microsoft.Data.Sqlite;
using PersonalExpenseTracker.Models;
using System.Collections.Generic;

namespace PersonalExpenseTracker.Services
{
    public class TransactionService
    {
        private readonly SqliteConnection _dbConnection;

        // Constructor to initialize the database connection
        public TransactionService(SqliteConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Retrieve all transactions from the database
        public List<Transactionitem> GetTransactions()
        {
            var transactions = new List<Transactionitem>();

            // Create a command to fetch all transactions
            var command = _dbConnection.CreateCommand();
            command.CommandText = "SELECT transactionId, transactionTitle, transactionDescription, transactionAmount, transactionDate, transactionTag, transactionType FROM Transactions";

            // Execute the command and read the results
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Populate the list of transactions with data from the database
                    transactions.Add(new Transactionitem
                    {
                        transactionId = reader.GetInt32(0),
                        transactionTitle = reader.GetString(1),
                        transactionDescription = reader.GetString(2),
                        transactionAmount = reader.GetDecimal(3),
                        transactionDate = reader.GetDateTime(4),
                        transactionTag = reader.GetString(5),
                        transactionType = reader.GetString(6)
                    });
                }
            }
            return transactions; // Return the list of transactions
        }

        // Add a new transaction to the database
        public void AddTransaction(Transactionitem transaction)
        {
            var command = _dbConnection.CreateCommand();

            // Parameterized query to prevent SQL injection
            command.CommandText = @"
                INSERT INTO Transactions (transactionTitle, transactionDescription, transactionAmount, transactionDate, transactionTag, transactionType)
                VALUES ($title, $description, $amount, $date, $tag, $type)";

            // Bind parameters for each column of the transaction
            command.Parameters.AddWithValue("$title", transaction.transactionTitle);
            command.Parameters.AddWithValue("$description", transaction.transactionDescription);
            command.Parameters.AddWithValue("$amount", transaction.transactionAmount);
            command.Parameters.AddWithValue("$date", transaction.transactionDate);
            command.Parameters.AddWithValue("$tag", transaction.transactionTag);
            command.Parameters.AddWithValue("$type", transaction.transactionType);

            // Execute the query to insert the transaction
            command.ExecuteNonQuery();
        }

        // Update an existing transaction in the database
        public void UpdateTransaction(Transactionitem transaction)
        {
            var command = _dbConnection.CreateCommand();

            // Parameterized query to update the transaction's details
            command.CommandText = @"
                UPDATE Transactions
                SET transactionTitle = $title, transactionDescription = $description, transactionAmount = $amount, transactionDate = $date, transactionTag = $tag, transactionType = $type
                WHERE transactionId = $id";

            // Bind parameters for the transaction details and the transaction ID
            command.Parameters.AddWithValue("$title", transaction.transactionTitle);
            command.Parameters.AddWithValue("$description", transaction.transactionDescription);
            command.Parameters.AddWithValue("$amount", transaction.transactionAmount);
            command.Parameters.AddWithValue("$date", transaction.transactionDate);
            command.Parameters.AddWithValue("$tag", transaction.transactionTag);
            command.Parameters.AddWithValue("$type", transaction.transactionType);
            command.Parameters.AddWithValue("$id", transaction.transactionId);

            // Execute the query to update the transaction
            command.ExecuteNonQuery();
        }

        // Delete a transaction from the database by its ID
        public void DeleteTransaction(int id)
        {
            var command = _dbConnection.CreateCommand();

            // Parameterized query to delete a transaction by its ID
            command.CommandText = @"
                DELETE FROM Transactions
                WHERE transactionId = $id";

            // Bind the transaction ID parameter
            command.Parameters.AddWithValue("$id", id);

            // Execute the query to delete the transaction
            command.ExecuteNonQuery();
        }

        // New method to fetch the transaction type and transaction amount for all transactions
        public List<(string transactionType, decimal transactionAmount)> GetTransactionTypesAndAmounts()
        {
            var result = new List<(string transactionType, decimal transactionAmount)>();

            // Command to fetch transaction type and amount
            var command = _dbConnection.CreateCommand();
            command.CommandText = "SELECT transactionType, transactionAmount FROM Transactions";

            // Execute the query and read the results
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Add the transaction type and amount to the result list
                    var transactionType = reader.GetString(0);
                    var transactionAmount = reader.GetDecimal(1);
                    result.Add((transactionType, transactionAmount));
                }
            }
            return result; // Return the list of transaction types and amounts
        }
    }
}
