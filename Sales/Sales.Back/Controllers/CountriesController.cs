﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.Backend.Data;
using Sales.Shared.Entities;

namespace Sales.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly DataContext _context;
        // Access to database for context
        public CountriesController(DataContext context)
        {
            _context = context;
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
            if(country == null)
            {
                return NotFound();
            }

            return Ok(country);

        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Countries.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Country country) //Paramter by body
        {
            _context.Add(country);
           await _context.SaveChangesAsync();
            return Ok(country);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, Country country)//Paramter by body and route
        {
            var currentCountry = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
            if (currentCountry == null)
            {
                return NotFound();
            }
            currentCountry.Name = country.Name;

            _context.Update(currentCountry);
            await _context.SaveChangesAsync();
            return Ok(currentCountry);

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)//Paramter by route
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            _context.Remove(country);
            await _context.SaveChangesAsync();
            return NoContent();

        }

    }
}
