using System;
using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace API.Controllers;
[ApiController]
[Route("/api/[controller]")]
public class ProductsController(IUnitOfWork unit) : BaseApiController
{
    [Cache(600)]
    [HttpGet]
    public async Task<ActionResult<Pagination<Product>>> GetBySpec(
            [FromQuery] ProductSpecParams specParams) 
    {
        var spec = new ProductSpecification( specParams );
        
        return await  CreatePagedResult(unit.Repository<Product>(),spec,specParams.PageIndex,specParams.PageSize);
    }
    [Cache(600)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>>? GetProductById(int id) 
    {
        var product = await unit.Repository<Product>().GetByIdAsync(id);
        if (product == null) return NotFound();
        return product;
    }
    [Cache(6000)]
    [HttpGet("brands")] 
    public async Task<ActionResult<IReadOnlyList<string>>?> GetBrandList() 
    {
        var spec = new BrandListSpecification();
        var brands = await unit.Repository<Product>().ListAsync(spec);
        if (brands == null) return NotFound();
        return Ok(brands);
    }
    [Cache(6000)]
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>?> GetTypeList() {
        var spec = new TypeListSpecification();
        var types = await unit.Repository<Product>().ListAsync(spec);
        if (types == null) return NotFound();
        return Ok(types);
    } 


    [InvalidateCache("api/products|")]
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product) 
    {
        unit.Repository<Product>().Add(product);
        return await unit.Complete()? product : BadRequest("Cannot create this product");
    }


    [InvalidateCache("api/products|")]
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> UpdateProduct(int id, Product product) {
        if (product.Id != id || unit.Repository<Product>().IsExist(id)) return BadRequest("Cannot update this product");
        unit.Repository<Product>().Update(product);
        return await unit.Complete()? product : BadRequest("Cannot create this product");
    }


    [InvalidateCache("api/products|")]
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id) 
    {
        Product? product = await unit.Repository<Product>().GetByIdAsync(id);

        if (product == null) return NotFound();
        unit.Repository<Product>().Remove(product);
        return await unit.Complete() ? Ok() : BadRequest("Cannot delete this product");
    }
}
