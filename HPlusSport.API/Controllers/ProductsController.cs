using HPlusSport.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HPlusSport.API.Controllers
{
    [Route("api/[controller]")] //Route Attribute
    [ApiController]
    public class ProductsController : ControllerBase //Inheritated from :
    {
        //Constructor of the Product Controller and set the database context It will be injected)
        private readonly ShopContext _Context;

        public ProductsController(ShopContext context)
        {
            _Context = context;

            _Context.Database.EnsureCreated(); //Because we are seeding the database
        }

        [HttpGet] //HttpGet Attribute
        public  async Task<ActionResult> GetAllProducts()
        {
            return Ok(await _Context.Products.ToArrayAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(int id) {
            var product =await _Context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            /*
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }*/
            _Context.Products.Add(product); // _context - Database context
            await _Context.SaveChangesAsync();

            return CreatedAtAction(
                "GetProduct",
                new { id = product.Id },
                product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(int id, Product product)
        {
            if(id != product.Id)
            {
                return BadRequest();
            }
            //Saving the data
            _Context.Entry(product).State = EntityState.Modified;

            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) //Maybe some products have already being modified by other API calls
            { 
                if(!_Context.Products.Any(p => p.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw; //Throw the expection and then the Http requrest will just be serialized by Json
                }

            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _Context.Products.FindAsync(id);
            if (product == null) 
            {
                return NotFound();
            }

            _Context.Products.Remove(product);
            await _Context.SaveChangesAsync();

            return product;
        }

        [HttpPost("{id}")]
        [Route("Delete")]
        public async Task<ActionResult> DeleteMultiple([FromQuery]int [] ids)
        {
            var products = new List<Product>();
            foreach (var id in ids)
            {
                var product = await _Context.Products.FindAsync(id);

                if(product == null)
                {
                    return NotFound();
                }

                products.Add(product);
            }

            _Context.Products.RemoveRange(products);
            await _Context.SaveChangesAsync();

            return Ok(products);
        }
    }
}
