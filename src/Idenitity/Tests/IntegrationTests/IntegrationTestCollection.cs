using Tests.IntegrationTests.Fixtures;
using Xunit;

namespace Tests.IntegrationTests;

[CollectionDefinition("IntegrationTests", DisableParallelization = true)]
public class IntegrationTestCollection : ICollectionFixture<PostgreSqlFixture>
{
}
