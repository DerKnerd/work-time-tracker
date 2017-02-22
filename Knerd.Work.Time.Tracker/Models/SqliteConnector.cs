using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Data;

namespace Knerd.Work.Time.Tracker.Models {
    public class SqliteConnector {

        private readonly string WorkItemTable = "WorkItems";
        public async Task SaveWorkItemEntryAsync(WorkItemEntryModel model) {
            await CreateIfNotExists();
            var query = $"INSERT INTO {WorkItemTable} (BeginTime, EndTime, Date, Call, Customer, WorkDone) VALUES (@beginTime, @endTime, @date, @call, @customer, @workDone)";
            var parameters = new List<SqliteParameter> {
                new SqliteParameter("@beginTime", model.BeginTime),
                new SqliteParameter("@endTime", model.EndTime),
                new SqliteParameter("@date", model.Date),
                new SqliteParameter("@call", model.Call),
                new SqliteParameter("@customer", model.Customer),
                new SqliteParameter("@workDone", model.WorkDone)
            };
            await ExecuteAsync(query, parameters);
        }

        public async Task<IEnumerable<WorkItemEntryModel>> GetWorkItems(DateTimeOffset date) {
            await CreateIfNotExists();
            var items = new List<WorkItemEntryModel>();
            using (var connection = await OpenConnectionAsync()) {
                await connection.OpenAsync();
                using (var cmd = new SqliteCommand($"SELECT BeginTime, EndTime, Date, Call, Customer, WorkDone FROM {WorkItemTable} ORDER BY Date DESC", connection)) {
                    var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync()) {
                        items.Add(new WorkItemEntryModel {
                            BeginTime = reader.GetTimeSpan("BeginTime"),
                            EndTime = reader.GetTimeSpan("EndTime"),
                            Date = reader.GetDateTime("Date"),
                            Call = reader.GetString("Call"),
                            Customer = reader.GetString("Customer"),
                            WorkDone = reader.GetString("WorkDone")
                        });
                    }
                }
            }
            return items;
        }

        private async Task<SqliteConnection> OpenConnectionAsync() {
            var connectionStringBuilder = new SqliteConnectionStringBuilder {
                DataSource = "workitems.db",
                Mode = SqliteOpenMode.ReadWriteCreate
            };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            return connection;
        }

        private async Task CreateIfNotExists() {
            var table = await ExecuteScalarAsync<string>($"SELECT name FROM sqlite_master WHERE type='table' AND name='{WorkItemTable}';");
            if (string.IsNullOrEmpty(table)) {
                await ExecuteAsync($"CREATE TABLE {WorkItemTable} (BeginTime TEXT NOT NULL, EndTime TEXT NOT NULL, Date DATETIME NOT NULL, Call TEXT, Customer TEXT NOT NULL, WorkDone TEXT NOT NULL)");
            }
        }

        private async Task<T> ExecuteScalarAsync<T>(string query) {
            using (var connection = await OpenConnectionAsync()) {
                await connection.OpenAsync();
                using (var cmd = new SqliteCommand(query, connection)) {
                    return (T)(await cmd.ExecuteScalarAsync());
                }
            }
        }

        private async Task ExecuteAsync(string query, List<SqliteParameter> parameter = null) {
            using (var connection = await OpenConnectionAsync()) {
                await connection.OpenAsync();
                using (var cmd = new SqliteCommand(query, connection)) {
                    if (parameter != null) {
                        cmd.Parameters.AddRange(parameter);
                    }
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
