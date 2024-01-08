using JobLists.Api.Models;
using JobLists.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobLists.Api.Controllers
{
    [Route("api/jobs")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public JobsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var jobs =await _dbContext.Jobs
                .AsNoTracking()
                .ToListAsync();

            return Ok(jobs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var job = await _dbContext.Jobs
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (job == null)
            {
                return NotFound();
            }

            return Ok(job);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Job job)
        {
            _dbContext.Jobs.Add(job);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = job.Id }, job);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Guid id, Job job)
        {
            var jobToUpdate =await _dbContext.Jobs.SingleOrDefaultAsync(x => x.Id == id);

            if (jobToUpdate == null)
            {
                return NotFound();
            }

            jobToUpdate.Update(job.Title, job.Description, job.Location, job.Salary);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var jobToDelete = await _dbContext.Jobs.SingleOrDefaultAsync(x => x.Id == id);

            if (jobToDelete == null)
            {
                return NotFound();
            }

            _dbContext.Jobs.Remove(jobToDelete);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
