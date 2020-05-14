﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ticketmaster.Models;
using ticketmaster.Services;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ticketmaster.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TicketController : ControllerBase

    {
        
        private readonly TicketsService _ticketService;
       
        public TicketController(TicketsService TicketService)
        {
            _ticketService = TicketService;
        }
        
        [Authorize]
        [HttpGet]
        public ActionResult<List<Ticket>> Get()
        {
            
            return _ticketService.Get();

        }
        
        [Authorize]
        [HttpGet("byUsername")]
        public async Task<ActionResult<List<Ticket>>> Get(string username)
        {
            var tickets = await _ticketService.GetByUsername(username);

            if (tickets == null)
            {
                return NotFound();
            }

            return tickets;
        }

        [HttpPost]
        public ActionResult<Ticket> Create(Ticket m)
        {
            _ticketService.Create(m);

            return CreatedAtRoute("GetTicket", new { id = m.Id.ToString() }, m);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Ticket min)
        {
            var contactform = _ticketService.Get(id);

            if (contactform == null)
            {
                return NotFound();
            }

            _ticketService.Update(id, min);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var ticket = _ticketService.Get(id);

            if (ticket == null)
            {
                return NotFound();
            }

            _ticketService.Remove(ticket);

            return NoContent();
        }
    }
}
