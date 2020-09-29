using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Abstractions.Model
{
    /// <summary> Introduction for the index page </summary>
    public class Introduction : IEquatable<Introduction>
    {
        /// <summary> Content </summary>
        public string Content { get; set; }

        /// <summary> Poster description </summary>
        public string PosterDescription { get; set; }

        /// <summary> Poster URL </summary>
        public string PosterUrl { get; set; }

        /// <summary> Introduction title </summary>
        public string Title { get; set; }

        /// <summary> Entity version </summary>
        public long Version { get; set; }

        /// <summary> External URLs </summary>
        public IEnumerable<ExternalUrl> ExternalUrls { get; set; }


        public bool Equals([AllowNull] Introduction other)
        {
            if (other == null)
                return false;

            if (Content != other.Content)
                return false;

            if (PosterDescription != other.PosterDescription)
                return false;

            if (PosterUrl != other.PosterUrl)
                return false;

            if (Title != other.Title)
                return false;

            if (Version != other.Version)
                return false;

            return Enumerable.SequenceEqual(ExternalUrls.OrderBy(x => x.Id), other.ExternalUrls.OrderBy(x => x.Id));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Introduction);
        }
    }
}