using System;
using System.Diagnostics.CodeAnalysis;

namespace Abstractions.Model
{
    /// <summary> URL to the external resources </summary>
    public class ExternalUrl : IEquatable<ExternalUrl>
    {
        /// <summary> Unique id or <c>null</c> if external URL is brand new </summary>
        public int? Id { get; set; }

        /// <summary> Friendly name for URL </summary>
        public string DisplayName { get; set; }

        /// <summary> URL to the external recourse </summary>
        public string Url { get; set; }

        /// <summary> Entity version </summary>
        public long Version { get; set; }


        public bool Equals([AllowNull] ExternalUrl other)
        {
            if (other == null)
                return false;

            if (!Id.HasValue)
                return false;

            if (Id != other.Id)
                return false;

            if (DisplayName != other.DisplayName)
                return false;

            if (Url != other.Url)
                return false;

            if (Version != other.Version)
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ExternalUrl);
        }
    }
}