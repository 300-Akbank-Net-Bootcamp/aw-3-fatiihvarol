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
    public class AccountTransactionController : ControllerBase
    {
        private readonly VbDbContext _dbContext; // Replace YourDbContext with the actual DbContext class you are using
        private readonly IMapper _mapper; // Inject IMapper

        public AccountTransactionController(VbDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AccountTransactionDTO>>> GetAccountTransactions()
        {
            var accountTransacitons = await _dbContext.Set<AccountTransaction>().ToListAsync();
            var accountTransactionDTOs = _mapper.Map<List<AccountTransactionDTO>>(accountTransacitons);

            return Ok(accountTransactionDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountTransactionDTO>> GetAccountTransactionById(int id)
        {
            var accountTransaciton = await _dbContext.Set<AccountTransaction>().FirstOrDefaultAsync(x => x.Id == id);

            if (accountTransaciton == null)
            {
                return NotFound();
            }

            var accountTransactionDTO = _mapper.Map<AccountTransactionDTO>(accountTransaciton);

            return Ok(accountTransactionDTO);
        }

        [HttpPost]
        public async Task<ActionResult<AccountTransactionDTO>> CreateEftTransaction(AccountTransactionDTO accountTransactionDto)
        {
            var accountTransaction = _mapper.Map<AccountTransaction>(accountTransactionDto);

            // Retrieve the account from the database including EftTransactions
            var account = await _dbContext.Set<Account>()
                .Include(a => a.AccountTransactions)
                .FirstOrDefaultAsync(x => x.Id == accountTransaction.AccountId);

            if (account == null)
            {
                return BadRequest("Account not found");
            }

            // Add the new EftTransaction to the account
            account.AccountTransactions.Add(accountTransaction);

            // Update the account in the database
            _dbContext.Update(account);

            // Save changes to the database
            await _dbContext.SaveChangesAsync();


            // Return the created EftTransactionDTO
            return Ok();
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccountTransaction(int id, AccountTransactionDTO accountTransactionDto)
        {
            var accountTransactionFromDb = await _dbContext.Set<AccountTransaction>().FirstOrDefaultAsync(x => x.Id == id);

            if (accountTransactionFromDb == null)
            {
                return NotFound();
            }

            _mapper.Map(accountTransactionDto, accountTransactionFromDb);


            _dbContext.Update(accountTransactionFromDb);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountTransaction(int id)
        {
            var accountTransaction = await _dbContext.Set<AccountTransaction>().FirstOrDefaultAsync(x => x.Id == id);

            if (accountTransaction == null)
            {
                return NotFound();
            }

            accountTransaction.IsActive = false;
            _dbContext.Update(accountTransaction);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
