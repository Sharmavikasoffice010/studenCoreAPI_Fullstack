using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApiCore.Data;
using StudentApiCore.Models;

namespace StudentApiCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/students
        //this is basically for comment
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Students.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Post(Student student)
        {
            student.Id = 0;  // Ensure Id is zero so EF treats it as new entity
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return Ok(student);
        }
        // PUT: api/students/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Student student)
        {
            if (id != student.Id) return BadRequest();

            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(student);
        }

        // DELETE: api/students/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
