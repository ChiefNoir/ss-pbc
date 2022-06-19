﻿using System.Diagnostics.CodeAnalysis;

namespace Abstractions.Models
{
    [ExcludeFromCodeCoverage]
    /// <summary> Project category</summary>
    public class Category
    {
        /// <summary> Unique id or <c>null</c> if category is brand new </summary>
        public Guid? Id { get; set; }

        /// <summary> Unique category code</summary>
        public string Code { get; set; }

        /// <summary> Friendly category name </summary>
        public string DisplayName { get; set; }

        /// <summary> Is category technical for filtering by all</summary>
        public bool IsEverything { get; set; }

        /// <summary> Get total projects in this category </summary>
        public int TotalProjects { get; set; }

        /// <summary> Entity version </summary>
        public long Version { get; set; }
    }
}
