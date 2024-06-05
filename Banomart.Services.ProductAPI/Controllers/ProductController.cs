using AutoMapper;
using Banomart.Services.ProductAPI.Data;
using Banomart.Services.ProductAPI.Models;
using Banomart.Services.ProductAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banomart.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly DatabaseContext db;
        private readonly IMapper mapper;
        private ResponseDto responseDto;

        public ProductController(DatabaseContext db, IMapper mapper) 
        {
            this.db = db;
            this.mapper = mapper;
            responseDto = new ResponseDto();
        }


        [HttpGet]
        public ResponseDto Get() 
        {
            try 
            {
                IEnumerable<Product> products = db.Products.ToList();
                responseDto.Result = this.mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex) 
            {
                responseDto.IsSuccessful = false;
                responseDto.Message = ex.Message;
            }

            return responseDto;
        }


        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Product product = db.Products.First(c => c.Id == id);
                responseDto.Result = this.mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccessful = false;
                responseDto.Message = ex.Message;
            }

            return responseDto;
        }


        [HttpGet]
        [Route("GetByCategory/{category}")]
        public ResponseDto GetByCode(string category)
        {
            try
            {
                IEnumerable<Product> product = db.Products.Where(c => c.CategoryName.ToLower() == category.ToLower()).ToList();

                if (product == null) 
                {
                    responseDto.IsSuccessful = true;
                    responseDto.Result = null;
                }
                responseDto.Result = this.mapper.Map<IEnumerable<ProductDto>>(product);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccessful = false;
                responseDto.Message = ex.Message;
            }

            return responseDto;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] ProductDto productDto)
        {
            try
            {
                Product product = mapper.Map<Product>(productDto);
                db.Products.Add(product);
                db.SaveChanges();

                responseDto.Result = this.mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccessful = false;
                responseDto.Message = ex.Message;
            }

            return responseDto;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] ProductDto productDto)
        {
            try
            {
                Product product = mapper.Map<Product>(productDto);
                db.Products.Update(product);
                db.SaveChanges();

                responseDto.Result = this.mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccessful = false;
                responseDto.Message = ex.Message;
            }

            return responseDto;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(long id)
        {
            try
            {
                Product product = db.Products.First(c => c.Id == id);
                db.Products.Remove(product);
                db.SaveChanges();

                responseDto.Result = this.mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccessful = false;
                responseDto.Message = ex.Message;
            }

            return responseDto;
        }
    }
}
