using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knerd.Work.Time.Tracker.Models {
    public static class SqlReaderHelpers {
        public static DateTime GetDateTime(this SqliteDataReader reader, string name) {
            var ordinal = reader.GetOrdinal(name);
            return reader.GetDateTime(ordinal);
        }
        public static string GetString(this SqliteDataReader reader, string name) {
            var ordinal = reader.GetOrdinal(name);
            return reader.GetString(ordinal);
        }

        public static TimeSpan GetTimeSpan(this SqliteDataReader reader, string name) {
            var ordinal = reader.GetOrdinal(name);
            var data = reader.GetString(ordinal);
            return TimeSpan.Parse(data);
        }
    }
}
