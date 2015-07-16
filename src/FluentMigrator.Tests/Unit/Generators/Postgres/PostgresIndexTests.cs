﻿using FluentMigrator.Expressions;
using FluentMigrator.Model;
using FluentMigrator.Runner.Generators.Postgres;
using NUnit.Framework;
using NUnit.Should;

namespace FluentMigrator.Tests.Unit.Generators.Postgres
{
    [TestFixture]
    public class PostgresIndexTests : BaseIndexTests
    {
        protected PostgresGenerator Generator;

        [SetUp]
        public void Setup()
        {
            Generator = new PostgresGenerator();
        }

        [Test]
        public override void CanCreateIndexWithCustomSchema()
        {
            var expression = GeneratorTestHelper.GetCreateIndexExpression();
            expression.Index.SchemaName = "TestSchema";

            var result = Generator.Generate(expression);
            result.ShouldBe("CREATE INDEX TestIndex ON TestSchema.TestTable1 (TestColumn1 ASC)");
        }

        [Test]
        public override void CanCreateIndexWithDefaultSchema()
        {
            var expression = GeneratorTestHelper.GetCreateIndexExpression();

            var result = Generator.Generate(expression);
            result.ShouldBe("CREATE INDEX TestIndex ON public.TestTable1 (TestColumn1 ASC)");
        }

        [Test]
        public override void CanCreateMultiColumnIndexWithCustomSchema()
        {
            var expression = GeneratorTestHelper.GetCreateMultiColumnCreateIndexExpression();
            expression.Index.SchemaName = "TestSchema";

            var result = Generator.Generate(expression);
            result.ShouldBe("CREATE INDEX TestIndex ON TestSchema.TestTable1 (TestColumn1 ASC,TestColumn2 DESC)");
        }

        [Test]
        public override void CanCreateMultiColumnIndexWithDefaultSchema()
        {
            var expression = GeneratorTestHelper.GetCreateMultiColumnCreateIndexExpression();

            var result = Generator.Generate(expression);
            result.ShouldBe("CREATE INDEX TestIndex ON public.TestTable1 (TestColumn1 ASC,TestColumn2 DESC)");
        }

        [Test]
        public override void CanCreateMultiColumnUniqueIndexWithCustomSchema()
        {
            var expression = GeneratorTestHelper.GetCreateUniqueMultiColumnIndexExpression();
            expression.Index.SchemaName = "TestSchema";

            var result = Generator.Generate(expression);
            result.ShouldBe("CREATE UNIQUE INDEX TestIndex ON TestSchema.TestTable1 (TestColumn1 ASC,TestColumn2 DESC)");
        }

        [Test]
        public override void CanCreateMultiColumnUniqueIndexWithDefaultSchema()
        {
            var expression = GeneratorTestHelper.GetCreateUniqueMultiColumnIndexExpression();

            var result = Generator.Generate(expression);
            result.ShouldBe("CREATE UNIQUE INDEX TestIndex ON public.TestTable1 (TestColumn1 ASC,TestColumn2 DESC)");
        }

        [Test]
        public override void CanCreateUniqueIndexWithCustomSchema()
        {
            var expression = GeneratorTestHelper.GetCreateUniqueIndexExpression();
            expression.Index.SchemaName = "TestSchema";

            var result = Generator.Generate(expression);
            result.ShouldBe("CREATE UNIQUE INDEX TestIndex ON TestSchema.TestTable1 (TestColumn1 ASC)");
        }

        [Test]
        public override void CanCreateUniqueIndexWithDefaultSchema()
        {
            var expression = GeneratorTestHelper.GetCreateUniqueIndexExpression();

            var result = Generator.Generate(expression);
            result.ShouldBe("CREATE UNIQUE INDEX TestIndex ON public.TestTable1 (TestColumn1 ASC)");
        }

        [Test]
        public override void CanDropIndexWithCustomSchema()
        {
            var expression = GeneratorTestHelper.GetDeleteIndexExpression();
            expression.Index.SchemaName = "TestSchema";

            var result = Generator.Generate(expression);
            result.ShouldBe("DROP INDEX TestSchema.TestIndex");
        }

        [Test]
        public override void CanDropIndexWithDefaultSchema()
        {
            var expression = GeneratorTestHelper.GetDeleteIndexExpression();

            var result = Generator.Generate(expression);
            result.ShouldBe("DROP INDEX public.TestIndex");
        }

        [Test]
        public void CanCreateIndexWithUnderScoreNoQuotes()
        {
            var expression = new CreateIndexExpression();
            expression.Index.Name = "Test_Index";
            expression.Index.TableName = "TestTable1";
            expression.Index.IsUnique = false;
            expression.Index.Columns.Add(new IndexColumnDefinition { Direction = Direction.Ascending, Name = "TestColumn1" });

            var result = Generator.Generate(expression);
            result.ShouldBe("CREATE INDEX Test_Index ON public.TestTable1 (TestColumn1 ASC)");
        }

        [Test]
        public void CanCreateIndexInQuotesUsingSingleQuote()
        {
            var expression = new CreateIndexExpression();
            expression.Index.Name = "Test'Index";
            expression.Index.TableName = "TestTable1";
            expression.Index.IsUnique = false;
            expression.Index.Columns.Add(new IndexColumnDefinition { Direction = Direction.Ascending, Name = "TestColumn1" });

            var result = Generator.Generate(expression);
            result.ShouldBe("CREATE INDEX \"Test'Index\" ON public.TestTable1 (TestColumn1 ASC)");
        }
    }
}