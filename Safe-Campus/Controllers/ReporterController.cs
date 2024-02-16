using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCampus.Models;
using SafeCampus.Services;
using System.Data;

namespace SafeCampus.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly ReportService _reportService;

    public ReportController(ReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet]
   [ Authorize(Policy = "Admin")]
    public async Task<ActionResult<List<Report>>> Get()
    {
        var reports = await _reportService.GetAllReports();
        return Ok(reports);
    }

    [HttpGet("{id:length(24)}", Name = "GetReport"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<Report>> Get(string id)
    {
        var report = await _reportService.GetReportById(id);

        if (report == null)
        {
            return NotFound();
        }

        return Ok(report);
    }

    [HttpPost, Authorize("Student")]
    public async Task<ActionResult<Report>> Create(Report report)
    {
        var ownerId = _reportService.GetMyName();
        report.OwnerId = ownerId;
        await _reportService.CreateReport(report);
        return CreatedAtRoute("GetReport", new { id = report.Id }, report);
    }
    [HttpDelete("{id:length(24)}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string id)
    {
        var existingReport = await _reportService.GetReportById(id);

        if (existingReport == null)
        {
            return NotFound();
        }

        await _reportService.DeleteReport(id);

        return NoContent();
    }
}