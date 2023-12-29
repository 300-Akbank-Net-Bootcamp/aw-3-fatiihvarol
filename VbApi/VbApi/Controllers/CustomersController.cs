using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VbApi.Entity;
using VbApi.DTO;
using AutoMapper;

namespace VbApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly VbDbContext _dbContext;
    private readonly IMapper _mapper;

    public CustomersController(VbDbContext dbContext,IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<List<Customer>> Get()
    {

        return await _dbContext.Set<Customer>()
            .Include(z=>z.Contacts)
            .Include(x=>x.Addresses)
            .Include(x=>x.Accounts)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        

        var customer = await _dbContext.Set<Customer>()
            .Include(x => x.Accounts)
            .Include(x => x.Addresses)
            .Include(x => x.Contacts)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (customer is null)
        {
            return NotFound(); 
        }

      return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CustomerDTO newCustomer)
    {
        try
        {
            var customer = _mapper.Map<Customer>(newCustomer);

            var addresses = newCustomer.AddressDtos.Select(x => _mapper.Map<Address>(x)).ToList();
            var contacts = newCustomer.ContactDtos.Select(x => _mapper.Map<Contact>(x)).ToList();
            var accounts = newCustomer.AccountDtos.Select(x => _mapper.Map<Account>(x)).ToList();

            

            customer.Addresses = addresses;
            customer.Contacts = contacts;
            customer.Accounts = accounts;

            // Add and save to the database
            await _dbContext.AddAsync(customer);
            await _dbContext.SaveChangesAsync();

            // Return success response
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }





    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] CustomerDTO customer)
    {
        var fromdb = await _dbContext.Set<Customer>().Where(x => x.Id == id).FirstOrDefaultAsync();
        if (fromdb is null)
            return BadRequest("User not found");
        
        // Update the properties of the existing 'fromdb' object with values from 'customer'
        _mapper.Map(customer, fromdb);
        
        _dbContext.Update(fromdb);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var fromdb = await _dbContext.Set<Customer>().Where(x => x.Id == id).FirstOrDefaultAsync();
        if (fromdb is null)
            return BadRequest("User not found");
        fromdb.IsActive = false;
        _dbContext.Update(fromdb);
        await _dbContext.SaveChangesAsync();
        return Ok("Deleted");
    }
}
