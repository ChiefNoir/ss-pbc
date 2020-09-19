using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.DataModel
{
    [Table("log")]
    internal class Log
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("date_time")]
        public DateTime DateTime { get; set; }

        [Column("source")]
        public string Source { get; set; }

        [Column("details")]
        public string Details { get; set; }
    }
}
