using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using UsaPopulation.Domain.Entities;
using UsaPopulation.Domain;

namespace UsaPopulation.Data.Configurations
{
    class QueryLogsConfiguration : IEntityTypeConfiguration<QueryLog>
    {
        public void Configure(EntityTypeBuilder<QueryLog> builder)
        {
            builder.Property(e => e.DateTime).IsRequired();
            builder.Property(e => e.Endpoint).IsRequired();
            builder.Property(e => e.PathAndQuery).IsRequired();
        }
    }
}
