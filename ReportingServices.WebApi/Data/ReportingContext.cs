using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingServices.WebApi.Data
{
    public class ReportingContext : DbContext
    {
        public ReportingContext(DbContextOptions<ReportingContext> options) : base(options) { }
        public DbSet<Report> Reports { get; set; }
    }
    public class Report
    {
        public Report()
        {
            Rows = new List<string>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        [NotMapped]
        public List<string> Rows { get; set; }

    }
}
