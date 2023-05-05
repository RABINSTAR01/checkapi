using Cards.API.Database;
using Cards.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Cards.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly CardDbContext cardDbContext1;
        public CardController(CardDbContext cardDbContext)
        {
          this.cardDbContext1 = cardDbContext;
        }
        //Get all cards details
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await cardDbContext1.Cards.ToListAsync();
            return Ok(cards);
        }

        //Get single cards details
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetSingleCard")]
        public async Task<IActionResult> GetSingleCard([FromRoute] Guid id)
        {
            var card = await cardDbContext1.Cards.FirstOrDefaultAsync(x => x.Id == id); 
            if (card != null)
            {
                return Ok(card);
            }
            else
            {
                return NotFound("Card not found");
            }
           
        }

        // add cards

        [HttpPost]
        public async Task<IActionResult> AddCard([FromBody] Card card)
        {
            card.Id= Guid.NewGuid();
          await  cardDbContext1.Cards.AddAsync(card);
            await cardDbContext1.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSingleCard),new {id = card.Id } ,card);
        }

        //update card
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> updateCard([FromRoute] Guid id, [FromBody] Card card)
        {
            var existingCard = await cardDbContext1.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCard != null)
            {
                existingCard.CardholderName = card.CardholderName;
                existingCard.CardNumber = card.CardNumber;
                existingCard.ExpiryMonth = card.ExpiryMonth;
                existingCard.ExpiryYear = card.ExpiryYear;
                existingCard.CVC = card.CVC;
                await cardDbContext1.SaveChangesAsync();
                return Ok(existingCard);
            }
            return NotFound("Card not found");
        }


        //delete card
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> deleteCard([FromRoute] Guid id)
        {
            var existingCard = await cardDbContext1.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCard != null)
            {
               cardDbContext1.Remove(existingCard);
                await cardDbContext1.SaveChangesAsync();
                return Ok(existingCard);
            }
            return NotFound("Card not found");
        }

    }
}
