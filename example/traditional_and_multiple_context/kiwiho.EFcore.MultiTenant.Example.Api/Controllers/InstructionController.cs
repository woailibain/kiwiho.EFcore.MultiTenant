using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kiwiho.EFcore.MultiTenant.Example.DAL;
using kiwiho.EFcore.MultiTenant.Example.DAL.Entity.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kiwiho.EFcore.MultiTenant.Example.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class InstructionController : ControllerBase
    {
        private readonly CustomerDbContext customerDbContext;
        public InstructionController(CustomerDbContext customerDbContext)
        {
            this.customerDbContext = customerDbContext;
            this.customerDbContext.Database.EnsureCreated();
        }

        [HttpPost("")]
        public async Task<ActionResult<Instruction>> Create(Instruction instruction)
        {
            var rct = await this.customerDbContext.Instructions.AddAsync(instruction);

            await this.customerDbContext.SaveChangesAsync();

            return rct?.Entity;

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Instruction>> Get([FromRoute] int id)
        {

            var rct = await this.customerDbContext.Instructions.FindAsync(id);

            return rct;

        }

        [HttpGet("")]
        public async Task<ActionResult<List<Instruction>>> Search()
        {
            var rct = await this.customerDbContext.Instructions.ToListAsync();
            return rct;
        }
    }
}
