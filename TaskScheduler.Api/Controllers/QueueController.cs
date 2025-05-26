using Microsoft.AspNetCore.Mvc;
using TaskScheduler.Application.Interfaces;
using TaskScheduler.Domain.Request;

namespace TaskScheduler.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly IJobService _jobService;

    public TasksController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetJobStatus(Guid id)
    {
        var job = await _jobService.GetJobAsync(id);

        if (job is null)
        {
            return NotFound(new { message = "Job not found." });
        }

        return Ok(new
        {
            id = job.Id,
            type = job.Type,
            status = job.JobStatus.ToString(),
            created_at = job.CreatedAt,
            payload = job.Payload
        });
    }

    [HttpPost]
    public async Task<IActionResult> EnqueueJob([FromBody] JobDto job)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdJobId = await _jobService.CreateJobAsync(job);

        return Accepted(new { id = createdJobId });
    }

}
