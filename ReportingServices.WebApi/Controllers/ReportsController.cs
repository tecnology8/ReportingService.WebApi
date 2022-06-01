using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ReportingServices.WebApi.Data;
using ReportingServices.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ReportingServices.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService _reportsService;
        private readonly IStringLocalizer<ReportsController> _localizer;

        public ReportsController(IReportsService reportsService, IStringLocalizer<ReportsController> localizer)
        {
            _reportsService = reportsService;
            _localizer = localizer;
        }

        [HttpGet("{reportId}")]
        public async Task<IActionResult> Get(int reportId)
        {
            var titulo1 = _localizer["Titulo 1"];
            var titulo2 = _localizer["Titulo 2"];
            var titulo3 = _localizer["Titulo 3"];

            var report = (await _reportsService.Get(new[] { reportId })).FirstOrDefault();
            if (report == null)
                return NotFound("Report");

            return Ok(new { report, titulo1, titulo2, titulo3 });
        }

        [HttpGet]
        [Route("Prueba")]
        public IActionResult GetUsingSharedResource()
        {
            var article = _localizer["Titulo 1"];
            var postName = _localizer.GetString("Titulo 1").Value ?? "";

            return Ok(new { PostType = article.Value, PostName = postName });
        }

        [HttpPost]
        public async Task<IActionResult> Add(Report report)
        {
            await _reportsService.Add(report);
            return Ok(report);
        }
    }
}
