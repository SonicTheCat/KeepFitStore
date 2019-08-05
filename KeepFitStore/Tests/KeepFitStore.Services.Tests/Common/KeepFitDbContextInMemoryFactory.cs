namespace KeepFitStore.Services.Tests.Common
{
    using System;

    using Microsoft.EntityFrameworkCore;

    using KeepFitStore.Data;

    public static class KeepFitDbContextInMemoryFactory
    {
        public static KeepFitDbContext Initialize()
        {
            var optionsBuilder = new DbContextOptionsBuilder<KeepFitDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());

            return new KeepFitDbContext(optionsBuilder.Options);
        }
    }
}