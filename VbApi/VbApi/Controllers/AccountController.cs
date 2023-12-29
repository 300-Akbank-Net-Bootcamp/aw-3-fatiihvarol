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
    public class AccountController : ControllerBase
    {
        private readonly VbDbContext _dbContext; // Replace YourDbContext with the actual DbContext class you are using
        private readonly IMapper _mapper; // Inject IMapper

        public AccountController(VbDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AccountDTO>>> GetAccounts()
        {
             var accounts= await _dbContext.Set<Account>().ToListAsync();

             return  Ok(accounts);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDTO>> GetAccountById(int id)
        {
            var account = await _dbContext.Set<Account>().FirstOrDefaultAsync(x => x.Id == id);

            if (account == null)
            {
                return NotFound();
            }

            var accountDTO = _mapper.Map<AccountDTO>(account);

            return Ok(accountDTO);
        }

        [HttpPost]
        public async Task<ActionResult<AccountDTO>> CreateAccount(AccountDTO accountDTO)
        {
            var account = _mapper.Map<Account>(accountDTO);
            

            _dbContext.Add(account);
            await _dbContext.SaveChangesAsync();

            return Ok();

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, AccountDTO accountDTO)
        {
            var accountFromDb = await _dbContext.Set<Account>().FirstOrDefaultAsync(x => x.Id == id);

            if (accountFromDb == null)
            {
                return NotFound();
            }

            _mapper.Map(accountDTO, accountFromDb);

            // Perform any additional logic or validation if needed

            _dbContext.Update(accountFromDb);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _dbContext.Set<Account>().FirstOrDefaultAsync(x => x.Id == id);

            if (account == null)
            {
                return NotFound();
            }

            account.IsActive = false;
            _dbContext.Update(account);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
