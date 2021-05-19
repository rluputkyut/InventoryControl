using InventoryControl.Server.Models;
using InventoryControl.Shared;
using InventoryControl.Shared.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryControl.Server.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        InventoryControlContext _dbContext;

        public ProductController(ILogger<ProductController> logger, InventoryControlContext inventoryControlContext)
        {
            _logger = logger;
            _dbContext = inventoryControlContext;
        }

        [HttpGet]
        [Route("get")]
        public List<ProductInfo> Get()
        {
            List<ProductInfo> _products = new List<ProductInfo>();
            var _productList = _dbContext.Products.Where(x => x.IsActive).ToList();
                        
            _productList.ForEach(x =>
            {
                ProductInfo _info = new ProductInfo()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,
                    BrandId = x.BrandId,
                    BatchCode = x.BatchCode,
                    BrandName = _dbContext.Brands.Where(y => y.Id == x.BrandId).Select(y=>y.Name).FirstOrDefault(),
                    ProductTypeId = x.ProductTypeId,
                    ProductTypeName = _dbContext.ProductTypes.Where(y => y.Id == x.ProductTypeId).Select(y => y.Name).FirstOrDefault(),
                    Photo = x.Photo  == null? string.Empty : Convert.ToBase64String(x.Photo),
                    Size = x.Size,
                    ExpiredDate = x.ExpiredDate,
                    ManufactureDate = x.ManufactureDate
                };
                _products.Add(_info);
            });

            return _products;
        }

        [HttpGet]
        [Route("getbyPage")]
        public ProductList GetByPage([FromQuery] ProductListRequest request)
        {
            List<ProductInfo> _products = new List<ProductInfo>();
            var _productList = _dbContext.Products.Where(x => x.IsActive).ToList();

            _productList.ForEach(x =>
            {
                ProductInfo _info = new ProductInfo()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,
                    BrandId = x.BrandId,
                    BatchCode = x.BatchCode,
                    BrandName = _dbContext.Brands.Where(y => y.Id == x.BrandId).Select(y => y.Name).FirstOrDefault(),
                    ProductTypeId = x.ProductTypeId,
                    ProductTypeName = _dbContext.ProductTypes.Where(y => y.Id == x.ProductTypeId).Select(y => y.Name).FirstOrDefault(),
                    Photo = x.Photo == null ? string.Empty : Convert.ToBase64String(x.Photo),
                    Size = x.Size,
                    ExpiredDate = x.ExpiredDate,
                    ManufactureDate = x.ManufactureDate
                };
                _products.Add(_info);
            });

            if (!string.IsNullOrEmpty(request.Code))
                _products = _products.Where(x => x.Code.ToLower().Contains(request.Code.ToLower())).ToList();
            if (!string.IsNullOrEmpty(request.Name))
                _products = _products.Where(x => x.Name.ToLower().Contains(request.Name.ToLower())).ToList();
            if (!string.IsNullOrEmpty(request.Brand))
                _products = _products.Where(x => x.BrandName.ToLower().Contains(request.Brand.ToLower())).ToList();
            if (!string.IsNullOrEmpty(request.ProductType))
                _products = _products.Where(x => x.ProductTypeName.ToLower().Contains(request.ProductType.ToLower())).ToList();

            var response = PagedList<ProductInfo>.ToPagedList(_products, request.PageNumber, request.PageSize);
            return new ProductList() { Items = response.ToList(), Meta = response.MetaData };
            //return _products;
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public ProductInfo GetById(int id)
        {
            ProductInfo _product = new ProductInfo();

            if (_dbContext.Products.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _info = _dbContext.Products.Where(x => x.Id == id && x.IsActive).First();
                _product = new ProductInfo() 
                {
                    Id = _info.Id,
                    Code = _info.Code,
                    Name = _info.Name,
                    Description = _info.Description,
                    BrandId = _info.BrandId,
                    BatchCode = _info.BatchCode,
                    BrandName = _dbContext.Brands.Where(y => y.Id == _info.BrandId).Select(y => y.Name).FirstOrDefault(),
                    ProductTypeId = _info.ProductTypeId,
                    ProductTypeName = _dbContext.ProductTypes.Where(y => y.Id == _info.ProductTypeId).Select(y => y.Name).FirstOrDefault(),
                    Photo = _info.Photo == null ? string.Empty : Convert.ToBase64String(_info.Photo),
                    Size = _info.Size,
                    ExpiredDate = _info.ExpiredDate,
                    ManufactureDate = _info.ManufactureDate
                };
            }

            return _product;
        }

        [HttpGet]
        [Route("getbyname/{id}")]
        public List<ProductInfo> GetByName(string name)
        {
            List<Product> _list = null;
            List<ProductInfo> _products = new List<ProductInfo>();

            if (string.IsNullOrEmpty(name))
            {
                _list = _dbContext.Products.Where(x => x.IsActive).ToList();
            }
            else if (_dbContext.Products.Where(x => x.IsActive && (x.Code.ToUpper().Contains(name.ToUpper()) || x.Name.ToUpper().Contains(name.ToUpper()))).Any())
            {
                _list = _dbContext.Products.Where(x => x.IsActive && (x.Code.ToUpper().Contains(name.ToUpper()) || x.Name.ToUpper().Contains(name.ToUpper()))).ToList();
            }

            _list.ForEach(x =>
            {
                ProductInfo _info = new ProductInfo()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,
                    BrandId = x.BrandId,
                    BatchCode = x.BatchCode,
                    BrandName = _dbContext.Brands.Where(y => y.Id == x.BrandId).Select(y => y.Name).FirstOrDefault(),
                    ProductTypeId = x.ProductTypeId,
                    ProductTypeName = _dbContext.ProductTypes.Where(y => y.Id == x.ProductTypeId).Select(y => y.Name).FirstOrDefault(),
                    Photo = Convert.ToBase64String(x.Photo),
                    Size = x.Size,
                    ExpiredDate = x.ExpiredDate,
                    ManufactureDate = x.ManufactureDate
                };
                _products.Add(_info);
            });

            return _products;
        }

        [HttpPut]
        [Route("update")]
        public bool Update(ProductInfo info)
        {
            bool _result = false;

            if (_dbContext.Products.Where(x => x.Id == info.Id && x.IsActive).Any())
            {
                var _info = _dbContext.Products.Where(x => x.Id == info.Id && x.IsActive).First();
                _info.Code = info.Code;
                _info.Name = info.Name;
                _info.Description = info.Description;
                _info.BrandId = info.BrandId;
                _info.ProductTypeId = info.ProductTypeId;                
                _info.BatchCode = info.BatchCode;
                _info.Photo = string.IsNullOrEmpty(info.Photo) ? _info.Photo : Convert.FromBase64String(info.Photo);
                _info.Size = info.Size == null ? info.Size : _info.Size;
                _info.ExpiredDate = info.ExpiredDate != null ? info.ExpiredDate : _info.ExpiredDate;
                _info.ManufactureDate = info.ManufactureDate != null ? info.ManufactureDate : _info.ManufactureDate;
                _info.UpdatedDate = DateTime.Now;
                _result = _dbContext.SaveChanges() > 0;
            }

            return _result;
        }

        [HttpPost]
        [Route("save")]
        public int Save(ProductInfo info)
        {
            int _id = 0;

            if (!_dbContext.Products.Where(x => x.Code == info.Code && x.IsActive).Any())
            {
                var _product = new Product() 
                {
                    Code = info.Code,
                    Name = info.Name,
                    Description = info.Description,
                    BrandId = info.BrandId,
                    ProductTypeId = info.ProductTypeId,
                    Photo = string.IsNullOrEmpty(info.Photo) ? null : Convert.FromBase64String(info.Photo),
                    BatchCode = info.BatchCode,
                    Size = info.Size,
                    ExpiredDate = info.ExpiredDate,
                    ManufactureDate = info.ManufactureDate,
                    IsActive = true, 
                    CreatedDate = DateTime.Now 
                };
                _dbContext.Products.Add(_product);
                _dbContext.SaveChanges();
                _id = _product.Id;
            }

            return _id;
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public bool Delete(int id)
        {
            bool _result = false;

            if (_dbContext.Products.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _info = _dbContext.Products.Where(x => x.Id == id && x.IsActive).FirstOrDefault();
                _info.IsActive = false;
                _info.UpdatedDate = DateTime.Now;
                _result = _dbContext.SaveChanges() > 0;
            }

            return _result;
        }
    }
}
