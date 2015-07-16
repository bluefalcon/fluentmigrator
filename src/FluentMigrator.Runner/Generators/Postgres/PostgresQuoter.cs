using System.Linq;
using System.Text.RegularExpressions;
using FluentMigrator.Runner.Generators.Generic;

namespace FluentMigrator.Runner.Generators.Postgres
{
    public class PostgresQuoter : GenericQuoter
    {
        public override string FormatBool(bool value) { return value ? "true" : "false"; }

        public override string QuoteSchemaName(string schemaName)
        {
            if (string.IsNullOrEmpty(schemaName))
                schemaName = "public";

            return Quote(schemaName);
        }

        public string UnQuoteSchemaName(string quoted)
        {
            if (string.IsNullOrEmpty(quoted))
                return "public";

            return UnQuote(quoted);
        }

        public override string QuoteColumnName(string columnName)
        {
            // Quotes should only be included to retain case sensitivity.  Should only quote if user passes them in.
            return Quote(columnName);
        }

        public override string QuoteIndexName(string indexName)
        {
            // Quotes should only be included to retain case sensitivity.  Should only quote if user passes them in.
            return Quote(indexName);
        }

        public override string QuoteTableName(string tableName)
        {
            // Quotes should only be included to retain case sensitivity.  Should only quote if user passes them in.
            return Quote(tableName);
        }

        public override string Quote(string name)
        {
            // Quotes should only be included to retain case sensitivity.  Should only quote if user passes them in.
            if (IsQuoted(name) || IsValidName(name)) return name;
            
            return base.Quote(name);
        }

        private bool IsValidName(string name)
        {
            return new Regex("^[a-zA-Z_][a-zA-Z0-9_]*$").IsMatch(name);
        }
    }
}