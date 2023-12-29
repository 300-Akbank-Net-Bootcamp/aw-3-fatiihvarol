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
    public class EftTransactionController : ControllerBase
    {
        private readonly VbDbContext _dbContext; // Replace YourDbContext with the actual DbContext class you are using
        private readonly IMapper _mapper; // Inject IMapper

        public EftTransactionController(VbDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<EftTransactionDTO>>> GetEftTransactions()
        {
            var eftTransactions = await _dbContext.Set<EftTransaction>().ToListAsync();
            var eftTransactionDTOs = _mapper.Map<List<EftTransactionDTO>>(eftTransactions);

            return Ok(eftTransactionDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EftTransactionDTO>> GetEftTransactionById(int id)
        {
            var eftTransaction = await _dbContext.Set<EftTransaction>().FirstOrDefaultAsync(x => x.Id == id);

            if (eftTransaction == null)
            {
                return NotFound();
            }

            var eftTransactionDTO = _mapper.Map<EftTransactionDTO>(eftTransaction);

            return Ok(eftTransactionDTO);
        }

        [HttpPost]
        public async Task<ActionResult<EftTransactionDTO>> CreateEftTransaction(EftTransactionDTO eftTransactionDTO)
        {
            var eftTransaction = _mapper.Map<EftTransaction>(eftTransactionDTO);

            // Retrieve the account from the database including EftTransactions
            var account = await _dbContext.Set<Account>()
                .Include(a => a.EftTransactions)
                .FirstOrDefaultAsync(x => x.Id == eftTransaction.AccountId);

            if (account == null)
            {
                return BadRequest("Account not found");
            }

            // Add the new EftTransaction to the account
            account.EftTransactions.Add(eftTransaction);

            // Update the account in the database
            _dbContext.Update(account);

            // Save changes to the database
            await _dbContext.SaveChangesAsync();

            var createdEftTransactionDTO = _mapper.Map<EftTransactionDTO>(eftTransaction);

            // Return the created EftTransactionDTO
            return Ok();
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEftTransaction(int id, EftTransactionDTO eftTransactionDTO)
        {
            var eftTransactionFromDb = await _dbContext.Set<EftTransaction>().FirstOrDefaultAsync(x => x.Id == id);

            if (eftTransactionFromDb == null)
            {
                return NotFound();
            }

            _mapper.Map(eftTransactionDTO, eftTransactionFromDb);


            _dbContext.Update(eftTransactionFromDb);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEftTransaction(int id)
        {
            var eftTransaction = await _dbContext.Set<EftTransaction>().FirstOrDefaultAsync(x => x.Id == id);

            if (eftTransaction == null)
            {
                return NotFound();
            }

            eftTransaction.IsActive = false;
            _dbContext.Update(eftTransaction);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
