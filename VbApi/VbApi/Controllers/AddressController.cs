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
    public class AddressController : ControllerBase
    {
        private readonly VbDbContext _dbContext; // Replace YourDbContext with the actual DbContext class you are using
        private readonly IMapper _mapper; // Inject IMapper

        public AddressController(VbDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AddressDTO>>> GetAddresses()
        {
             var addresses= await _dbContext.Set<Address>().ToListAsync();

             return  Ok(addresses);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AddressDTO>> GetAddressById(int id)
        {
            var address = await _dbContext.Set<Address>().FirstOrDefaultAsync(x => x.Id == id);

            if (address == null)
            {
                return NotFound();
            }

            var addresstDTO = _mapper.Map<AddressDTO>(address);

            return Ok(addresstDTO);
        }

        [HttpPost]
        public async Task<ActionResult<AddressDTO>> CreateAccount(AddressDTO addressDTO)
        {
            var address = _mapper.Map<Address>(addressDTO);
            

            _dbContext.Add(address);
            await _dbContext.SaveChangesAsync();

            return Ok();

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int id, AddressDTO addressDTO)
        {
            var addressFromDb = await _dbContext.Set<Address>().FirstOrDefaultAsync(x => x.Id == id);

            if (addressFromDb == null)
            {
                return NotFound();
            }

            _mapper.Map(addressDTO, addressFromDb);

            // Perform any additional logic or validation if needed

            _dbContext.Update(addressFromDb);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var address = await _dbContext.Set<Address>().FirstOrDefaultAsync(x => x.Id == id);

            if (address == null)
            {
                return NotFound();
            }

            address.IsActive = false;
            _dbContext.Update(address);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
