using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Generators;
using FluentMigrator.Runner.Generators.Postgres;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.Postgres;
using FluentMigrator.Tests.Helpers;
using NUnit.Framework;
using NUnit.Should;
using Npgsql;

namespace FluentMigrator.Tests.Integration.Processors.Postgres
{
    [TestFixture]
    [Category("Integration")]
    public class PostgresSchemaExtensionsTests : BaseSchemaExtensionsTests
    {
        public NpgsqlConnection Connection { get; set; }
        public PostgresProcessor Processor { get; set; }
        public IQuoter Quoter { get; set; }

        [SetUp]
        public void SetUp()
        {
            Connection = new NpgsqlConnection(IntegrationTestOptions.Postgres.ConnectionString);
            Processor = new PostgresProcessor(Connection, new PostgresGenerator(), new TextWriterAnnouncer(System.Console.Out), new ProcessorOptions(), new PostgresDbFactory());
            Quoter = new PostgresQuoter();
            Connection.Open();
        }

        [TearDown]
        public void TearDown()
        {
            Processor.CommitTransaction();
            Processor.Dispose();
        }

        [Test]
        public override void CallingColumnExistsCanAcceptSchemaNameWithSingleQuote()
        {
            var schemaNameSingleQuote = Quoter.QuoteSchemaName("Test'Schema");
            using (var table = new PostgresTestTable(Processor, schemaNameSingleQuote, "id int"))
                Processor.ColumnExists(schemaNameSingleQuote, table.Name, "id").ShouldBeTrue();
        }

        [Test]
        public override void CallingConstraintExistsCanAcceptSchemaNameWithSingleQuote()
        {
            var schemaNameSingleQuote = Quoter.QuoteSchemaName("Test'Schema");
            using (var table = new PostgresTestTable(Processor, schemaNameSingleQuote, "id int", "wibble int CONSTRAINT c1 CHECK(wibble > 0)"))
                Processor.ConstraintExists(schemaNameSingleQuote, table.Name, "c1").ShouldBeTrue();
        }

        [Test]
        public override void CallingIndexExistsCanAcceptSchemaNameWithSingleQuote()
        {
            var schemaNameSingleQuote = Quoter.QuoteSchemaName("Test'Schema");
            using (var table = new PostgresTestTable(Processor, schemaNameSingleQuote, "id int"))
            {
                var idxName = string.Format("\"idx_{0}\"", Quoter.UnQuote(table.Name));

                var cmd = table.Connection.CreateCommand();
                cmd.Transaction = table.Transaction;
                cmd.CommandText = string.Format("CREATE INDEX {0} ON {1} (id)", idxName, table.NameWithSchema);
                cmd.ExecuteNonQuery();

                Processor.IndexExists(schemaNameSingleQuote, table.Name, idxName).ShouldBeTrue();
            }
        }

        [Test]
        public override void CallingSchemaExistsCanAcceptSchemaNameWithSingleQuote()
        {
            var schemaNameSingleQuote = Quoter.QuoteSchemaName("Test'Schema");
            using (new PostgresTestTable(Processor, schemaNameSingleQuote, "id int"))
                Processor.SchemaExists(schemaNameSingleQuote).ShouldBeTrue();
        }

        [Test]
        public override void CallingTableExistsCanAcceptSchemaNameWithSingleQuote()
        {
            var schemaNameSingleQuote = Quoter.QuoteSchemaName("Test'Schema");
            using (var table = new PostgresTestTable(Processor, schemaNameSingleQuote, "id int"))
                Processor.TableExists(schemaNameSingleQuote, table.Name).ShouldBeTrue();
        }

        [Test]
        public void CallingDefaultValueExistsCanAcceptSchemaNameWithSingleQuote()
        {
            var schemaNameSingleQuote = Quoter.QuoteSchemaName("Test'Schema");
            using (var table = new PostgresTestTable(Processor, schemaNameSingleQuote, "id int"))
            {
                table.WithDefaultValueOn("id");
                Processor.DefaultValueExists(schemaNameSingleQuote, table.Name, "id", 1).ShouldBeTrue();
            }
        }
    }
}