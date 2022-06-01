using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ReportingServices.WebApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingServices.WebApi.Services
{
    public class ReportsService : IReportsService
    {
        private readonly ReportingContext _reportingContext;
        private readonly IStringLocalizer _localizer;
        public static string[] Rows = { "Header1", "Header2", "Header3" };
        public ReportsService(ReportingContext reportingContext, IStringLocalizer localizer)
        {
            _reportingContext = reportingContext;
            _localizer = localizer;
        }
        public async Task<IEnumerable<Report>> Get(int[] ids)
        {
            var reports = _reportingContext.Reports.AsQueryable();

            if (ids != null && ids.Any())
                reports = reports.Where(x => ids.Contains(x.Id));
            var result = await reports.ToListAsync();

            var translatedRows = Rows.Select(x => _localizer[x].Value).ToList();
            result.ForEach(x => x.Rows = translatedRows);

            return result;
        }
        public async Task<Report> Add(Report report)
        {
            await _reportingContext.Reports.AddAsync(report);

            await _reportingContext.SaveChangesAsync();
            return report;
        }
        public async Task<Report> Update(Report report)
        {
            var reportForChange = await _reportingContext.Reports.SingleAsync(x => x.Id == report.Id);

            reportForChange.Name = report.Name;

            _reportingContext.Reports.Update(reportForChange);
            await _reportingContext.SaveChangesAsync();
            return report;
        }
        public async Task<bool> Delete(Report report)
        {
            _reportingContext.Reports.Remove(report);
            await _reportingContext.SaveChangesAsync();

            return true;
        }
    }

    public interface IReportsService
    {
        Task<IEnumerable<Report>> Get(int[] ids);
        Task<Report> Add(Report report);
        Task<Report> Update(Report report);
        Task<bool> Delete(Report report);
    }
}
