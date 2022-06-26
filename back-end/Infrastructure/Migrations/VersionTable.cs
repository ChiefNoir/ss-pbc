﻿using FluentMigrator.Runner.VersionTableInfo;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Migrations
{
    [VersionTableMetaData]
    [ExcludeFromCodeCoverage]
    public class VersionTable : IVersionTableMetaData
    {
        public string SchemaName => DataContext.SchemaHistory;
        public string TableName => "version";

        public string ColumnName => "version";
        public string AppliedOnColumnName => "date";
        public string DescriptionColumnName => "description";

        public string UniqueIndexName => "sspbc_version__version__idx";

        public object? ApplicationContext { get; set; }
        public bool OwnsSchema => true;
    }
}
