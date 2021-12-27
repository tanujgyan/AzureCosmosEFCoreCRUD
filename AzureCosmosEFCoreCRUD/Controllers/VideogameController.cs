using AzureCosmosEFCoreCRUD.DBContext;
using AzureCosmosEFCoreCRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureCosmosEFCoreCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideogameController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IConfiguration configuration;

        public VideogameController(ApplicationDbContext applicationDbContext, IConfiguration configuration, ILogger<VideogameController> logger)
        {
            this.applicationDbContext = applicationDbContext;
            this.configuration = configuration;
            _logger = logger;
        }
        [HttpGet("GetAllVideogames")]
        public async Task<ActionResult<IList<Videogames>>> GetAllVideogames()
        {
            try
            {
                var videogames = await applicationDbContext.Videogames.ToListAsync();
                if (videogames != null)
                {
                    return Ok(videogames);
                }
                return NotFound("No results found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Some error occured while retrieving data");
            }
        }
        [HttpGet]
        public async Task<ActionResult<IList<Videogames>>> GetVideogamesByName(string name)
        {
            try
            {
                var videogames = await applicationDbContext.Videogames.FirstOrDefaultAsync(x => x.Name == name);
                if (videogames != null)
                {
                    return Ok(videogames);
                }
                return NotFound("No results found for "+name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Some error occured while retrieving data");
            }
        }
        [HttpGet("GetVideogameByCompanyName")]
        public async Task<ActionResult<IList<Videogames>>> GetVideogameByCompanyName(string name)
        {
            try
            {
                var videogames = await applicationDbContext.Videogames.Where(x => x.Company.Name == name).ToListAsync();
                if (videogames != null)
                {
                    return Ok(videogames);
                }
                return NotFound("No results found for " + name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Some error occured while retrieving data");
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddVideogame(Videogames videogame)
        {
            try
            {
                await applicationDbContext.Videogames.AddAsync(videogame);
                await applicationDbContext.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Some error occured while adding data");
            }
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteVideogameByName(string name)
        {
            try
            {
                var videogame = await applicationDbContext.Videogames.FirstOrDefaultAsync(x => x.Name == name);
                if (videogame != null)
                {
                    applicationDbContext.Remove(videogame);
                    await applicationDbContext.SaveChangesAsync();
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Some error occured while deleting data");
            }
        }
        [HttpPut]
        public async Task<ActionResult> UpdateVideogameByName(Videogames data)
        {
            try
            {
                var videogame = await applicationDbContext.Videogames.FirstOrDefaultAsync(x => x.Name == data.Name);
                if (videogame != null)
                {
                    var itemBody = applicationDbContext.Videogames.WithPartitionKey(videogame.Id);
                    itemBody.FirstOrDefault().Platform = "XBOX";
                    applicationDbContext.Update(itemBody.FirstOrDefault());
                    await applicationDbContext.SaveChangesAsync();
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Some error occured while updating data");
            }
        }


    }
}
