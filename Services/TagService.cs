using Microsoft.Data.Sqlite;
using PersonalExpenseTracker.Models;
using System.Collections.Generic;

namespace PersonalExpenseTracker.Services
{
    public class TagService
    {
        private readonly SqliteConnection _dbConnection;

        public TagService(SqliteConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public List<Tagitem> GetTags()
        {
            var tags = new List<Tagitem>();
            var command = _dbConnection.CreateCommand();
            command.CommandText = "SELECT TagId, TagName FROM Tags";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tags.Add(new Tagitem
                    {
                        TagId = reader.GetInt32(0),
                        TagName = reader.GetString(1)
                    });
                }
            }
            return tags;
        }

        public void AddTag(Tagitem tag)
        {
            var command = _dbConnection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Tags (TagName)
                VALUES ($name)";
            command.Parameters.AddWithValue("$name", tag.TagName);
            command.ExecuteNonQuery();
        }

        public void UpdateTag(Tagitem tag)
        {
            var command = _dbConnection.CreateCommand();
            command.CommandText = @"
                UPDATE Tags
                SET TagName = $name
                WHERE TagId = $id";
            command.Parameters.AddWithValue("$name", tag.TagName);
            command.Parameters.AddWithValue("$id", tag.TagId);
            command.ExecuteNonQuery();
        }

        public void DeleteTag(int id)
        {
            var command = _dbConnection.CreateCommand();
            command.CommandText = @"
                DELETE FROM Tags
                WHERE TagId = $id";
            command.Parameters.AddWithValue("$id", id);
            command.ExecuteNonQuery();
        }
    }
}