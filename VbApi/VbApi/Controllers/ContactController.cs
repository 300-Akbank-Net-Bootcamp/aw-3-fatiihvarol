using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using VbApi.DTO;
using VbApi.Entity;


namespace VbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly VbDbContext _dbContext; // Replace YourDbContext with the actual DbContext class you are using
        private readonly IMapper _mapper; // Inject IMapper

        public ContactController(VbDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ContactDTO>>> GetContacts()
        {
             var contacts= await _dbContext.Set<Contact>().ToListAsync();

             return  Ok(contacts);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactDTO>> GetContactById(int id)
        {
            var contact = await _dbContext.Set<Contact>().FirstOrDefaultAsync(x => x.Id == id);

            if (contact == null)
            {
                return NotFound();
            }

            var contactDTO = _mapper.Map<ContactDTO>(contact);

            return Ok(contactDTO);
        }

        [HttpPost]
        public async Task<ActionResult<ContactDTO>> CreateContact(ContactDTO contactDto)
        {
            var contact = _mapper.Map<Contact>(contactDto);
            

            _dbContext.Add(contact);
            await _dbContext.SaveChangesAsync();

            return Ok();

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, ContactDTO contactDto)
        {
            var contactFromDb = await _dbContext.Set<Contact>().FirstOrDefaultAsync(x => x.Id == id);

            if (contactFromDb == null)
            {
                return NotFound();
            }

            _mapper.Map(contactDto, contactFromDb);

            // Perform any additional logic or validation if needed

            _dbContext.Update(contactFromDb);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _dbContext.Set<Contact>().FirstOrDefaultAsync(x => x.Id == id);

            if (contact == null)
            {
                return NotFound();
            }

            contact.IsActive = false;
            _dbContext.Update(contact);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
