using FluentMigrator.Expressions;
using FluentMigrator.Runner.Generators.Postgres;
using NUnit.Framework;
using NUnit.Should;

namespace FluentMigrator.Tests.Unit.Generators.Postgres
{
    [TestFixture]
    public class PostgresSchemaTests : BaseSchemaTests
    {
        protected PostgresGenerator Generator;

        [SetUp]
        public void Setup()
        {
            Generator = new PostgresGenerator();
        }

        [Test]
        public override void CanAlterSchema()
        {
            var expression = GeneratorTestHelper.GetAlterSchemaExpression();

            var result = Generator.Generate(expression);
            result.ShouldBe("ALTER TABLE TestSchema1.TestTable SET SCHEMA TestSchema2");
        }

        [Test]
        public override void CanCreateSchema()
        {
            var expression = GeneratorTestHelper.GetCreateSchemaExpression();

            var result = Generator.Generate(expression);
            result.ShouldBe("CREATE SCHEMA TestSchema");
        }

        [Test]
        public override void CanDropSchema()
        {
            var expression = GeneratorTestHelper.GetDeleteSchemaExpression();

            var result = Generator.Generate(expression);
            result.ShouldBe("DROP SCHEMA TestSchema");
        }

        [Test]
        public void CanCreateSchemaWithUnderScoreNoQuotes()
        {
            var expression = new CreateSchemaExpression { SchemaName = "Test_Schema" };

            var result = Generator.Generate(expression);
            result.ShouldBe("CREATE SCHEMA Test_Schema");
        }

        [Test]
        public void CanCreateSchemaInQuotesUsingSingleQuote()
        {
            var expression = new CreateSchemaExpression { SchemaName = "Test'Schema" };

            var result = Generator.Generate(expression);
            result.ShouldBe("CREATE SCHEMA \"Test'Schema\"");
        }
    }
}