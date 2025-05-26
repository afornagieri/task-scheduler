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

    /// <summary>
    /// Get a job status given id
    /// </summary>
    /// <param name="id">Unique identifier</param>
    /// <returns>
    /// 200 OK
    /// 404 Not Found
    /// 500 Internal Server Error
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Enqueue a new job
    /// </summary>
    /// <param name="job">Job object to be created</param>
    /// <returns>
    /// 202 Accepted
    /// 400 Bad Request in ArgumentNullException or ArgumentException
    /// 500 Internal Server Error
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EnqueueJob([FromBody] JobDto job)
    {
        var createdJobId = await _jobService.CreateJobAsync(job);
        return Accepted(new { id = createdJobId });
    }

}
