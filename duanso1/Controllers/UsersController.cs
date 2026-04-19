using Microsoft.AspNetCore.Mvc;
using duanso1.Model;
using duanso1.Repositories;

namespace duanso1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _repo;

        public UsersController(UserRepository repo)
        {
            _repo = repo;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _repo.GetAllAsync();
            return Ok(users);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            var newId = await _repo.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = newId }, user);
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User user)
        {
            user.Id = id;
            var success = await _repo.UpdateAsync(user);
            if (!success) return NotFound();
            return NoContent();
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _repo.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
