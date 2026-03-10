using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;

using Microsoft.AspNetCore.Authorization;


using vaulin_up.Data;
using vaulin_up.Models;
using vaulin_up.Dtos;

namespace vaulin_up.Controllers;

[ApiController]
[Route("api/[controller]")]

[Authorize]

public class ProductController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public ProductController(ApplicationDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _db.Set<Product>()
            .AsNoTracking()
            .Select(x => new ProductDto
            {

                ProductId = x.ProductId,

                Article = x.Article,

                Name = x.Name,

                ProductTypeId = x.ProductTypeId,

                MinPartnerPrice = x.MinPartnerPrice,

                MainMaterialTypeId = x.MainMaterialTypeId,

            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var item = await _db.Set<Product>()
            .AsNoTracking()
            .Where(x => x.ProductId == id)
            .Select(x => new ProductDto
            {

                ProductId = x.ProductId,

                Article = x.Article,

                Name = x.Name,

                ProductTypeId = x.ProductTypeId,

                MinPartnerPrice = x.MinPartnerPrice,

                MainMaterialTypeId = x.MainMaterialTypeId,

            })
            .FirstOrDefaultAsync();

        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
    {
        var entity = new Product
        {




            Article = dto.Article,



            Name = dto.Name,



            ProductTypeId = dto.ProductTypeId,



            MinPartnerPrice = dto.MinPartnerPrice,



            MainMaterialTypeId = dto.MainMaterialTypeId,


        };

        _db.Set<Product>().Add(entity);
        await _db.SaveChangesAsync();

        return Ok(new ProductDto
        {

            ProductId = entity.ProductId,

            Article = entity.Article,

            Name = entity.Name,

            ProductTypeId = entity.ProductTypeId,

            MinPartnerPrice = entity.MinPartnerPrice,

            MainMaterialTypeId = entity.MainMaterialTypeId,

        });
    }

    [HttpPatch("{id:long}")]
    public async Task<IActionResult> Patch(long id, [FromBody] JsonPatchDocument<ProductUpdateDto> patch)
    {
        if (patch is null) return BadRequest("Patch document is required.");

        var entity = await _db.Set<Product>().FirstOrDefaultAsync(x => x.ProductId == id);
        if (entity is null) return NotFound();

        var dto = new ProductUpdateDto
        {




            Article = entity.Article,



            Name = entity.Name,



            ProductTypeId = entity.ProductTypeId,



            MinPartnerPrice = entity.MinPartnerPrice,



            MainMaterialTypeId = entity.MainMaterialTypeId,


        };

        patch.ApplyTo(dto, ModelState);
        if (!ModelState.IsValid) return ValidationProblem(ModelState);





        entity.Article = dto.Article;



        entity.Name = dto.Name;



        entity.ProductTypeId = dto.ProductTypeId;



        entity.MinPartnerPrice = dto.MinPartnerPrice;



        entity.MainMaterialTypeId = dto.MainMaterialTypeId;



        await _db.SaveChangesAsync();

        return Ok(new ProductDto
        {

            ProductId = entity.ProductId,

            Article = entity.Article,

            Name = entity.Name,

            ProductTypeId = entity.ProductTypeId,

            MinPartnerPrice = entity.MinPartnerPrice,

            MainMaterialTypeId = entity.MainMaterialTypeId,

        });
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var entity = await _db.Set<Product>().FirstOrDefaultAsync(x => x.ProductId == id);
        if (entity is null) return NotFound();

        _db.Remove(entity);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
