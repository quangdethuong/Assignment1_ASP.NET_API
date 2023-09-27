using AutoMapper;
using BusinessObject;
using eStoreWebAPI.DTO;
using eStoreWebAPI.DTO.Products;
using Microsoft.AspNetCore.Mvc;
using Repository.Services.Products;
using System.Collections;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace eStoreWebAPI.Controllers.ProductController
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {

  

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public ActionResult GetProducts()
        {
            var p = _productRepository.GetProducts();
            if (p == null)
            {
                return NotFound();
            }
            var pDTO = _mapper.Map<IEnumerable<ProductDTO>>(p);
            return Ok(pDTO);
        }


        // GET api/<ProductController>/5

        [HttpGet("{id}")]
        public ActionResult FindProductById(int id)
        {
            var p = _productRepository.GetProductById(id);
            if (p == null)
            {
                return NotFound();
            }
            var pDTO = _mapper.Map<ProductDTO>(p);
            return Ok(pDTO);
        }

        // POST api/<ProductController>


        [HttpPost]
        public ActionResult SaveProduct(ProductCreateDTO p)
        {
            var product = _mapper.Map<Product>(p);
            _productRepository.SaveProduct(product);
            return Ok(p);
        }

        // PUT api/<ProductController>/5
        [HttpPost("{id}")]
        public ActionResult UpdateProduct(int id, ProductCreateDTO p)
        {
            var pId = _productRepository.GetProductById(id);

            if (pId == null)
                return NotFound();
            var product = _mapper.Map(p , pId);
            _productRepository.UpdateProduct(product);
            return Ok(product);
        }

        // DELETE api/<ProductController>/5


        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null)
                return NotFound();
            _productRepository.DeleteProduct(product);
            return Ok(product);
        }

        [HttpGet("Search")]
        public ActionResult<List<ProductDTO>> SearchProduct(string search)
        {
            var listProducts = _productRepository.SearchProduct(search);
            var listProductsDTO = _mapper.Map<List<ProductDTO>>(listProducts);
            return Ok(listProductsDTO);
        }

    }
}
