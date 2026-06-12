using Microsoft.EntityFrameworkCore;
using StudyFlow.Domain.Entities;

namespace StudyFlow.Tests;

public static class TestDbContextFactory
{
    public static StudyFlowDbContext Create()
    {
        var options = new DbContextOptionsBuilder<StudyFlowDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new StudyFlowDbContext(options);
    }
}
