using Microsoft.Data.Sqlite;
using PersonalExpenseTracker.Models;
using System.Collections.Generic;

namespace PersonalExpenseTracker.Services
{
    public class TagService
    {
        private readonly SqliteConnection _dbConnection;

        // Constructor to initialize the database connection
        public TagService(SqliteConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Retrieve all tags from the database
        public List<Tagitem> GetTags()
        {
            var tags = new List<Tagitem>();

            // Create a command to fetch all tags
            var command = _dbConnection.CreateCommand();
            command.CommandText = "SELECT TagId, TagName FROM Tags";

            // Execute the command and read the results
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Populate the list of tags with data from the database
                    tags.Add(new Tagitem
                    {
                        TagId = reader.GetInt32(0),
                        TagName = reader.GetString(1)
                    });
                }
            }
            return tags; // Return the list of tags
        }

        // Add a new tag to the database
        public void AddTag(Tagitem tag)
        {
            var command = _dbConnection.CreateCommand();

            // Parameterized query to prevent SQL injection
            command.CommandText = @"
                INSERT INTO Tags (TagName)
                VALUES ($name)";

            // Bind the parameter for the tag name
            command.Parameters.AddWithValue("$name", tag.TagName);

            // Execute the query to insert the tag
            command.ExecuteNonQuery();
        }

        // Update an existing tag in the database
        public void UpdateTag(Tagitem tag)
        {
            var command = _dbConnection.CreateCommand();

            // Parameterized query to update the tag's name
            command.CommandText = @"
                UPDATE Tags
                SET TagName = $name
                WHERE TagId = $id";

            // Bind parameters for the tag name and tag ID
            command.Parameters.AddWithValue("$name", tag.TagName);
            command.Parameters.AddWithValue("$id", tag.TagId);

            // Execute the query to update the tag
            command.ExecuteNonQuery();
        }

        // Delete a tag from the database by its ID
        public void DeleteTag(int id)
        {
            var command = _dbConnection.CreateCommand();

            // Parameterized query to delete a tag by its ID
            command.CommandText = @"
                DELETE FROM Tags
                WHERE TagId = $id";

            // Bind the tag ID parameter
            command.Parameters.AddWithValue("$id", id);

            // Execute the query to delete the tag
            command.ExecuteNonQuery();
        }
    }
}
