using InventoryControl.Server.Models;
using InventoryControl.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryControl.Server.Controllers
{
    [ApiController]
    [Route("api/producttype")]
    public class ProductTypeController : Controller
    {
        private readonly ILogger<WeatherForecastController> _logger;
        InventoryControlContext _dbContext;

        public ProductTypeController(ILogger<WeatherForecastController> logger, InventoryControlContext inventoryControlContext)
        {
            _logger = logger;
            _dbContext = inventoryControlContext;
        }

        [HttpGet]
        [Route("get")]
        public List<ProductTypeInfo> Get()
        {
            List<ProductTypeInfo> _productTypes = new List<ProductTypeInfo>();
            var _list = _dbContext.ProductTypes.Where(x => x.IsActive).ToList();
            _list.ForEach(x =>
            {
                ProductTypeInfo _info = new ProductTypeInfo()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                };
                _productTypes.Add(_info);
            });

            return _productTypes;
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public ProductTypeInfo GetById(int id)
        {
            ProductTypeInfo _productType = null;

            if (_dbContext.ProductTypes.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _info = _dbContext.ProductTypes.Where(x => x.Id == id && x.IsActive).First();
                _productType = new ProductTypeInfo() { Id = _info.Id, Code = _info.Code, Name = _info.Name };
            }

            return _productType;
        }

        [HttpGet]
        [Route("getbyname/{id}")]
        public List<ProductTypeInfo> GetByName(string name)
        {
            List<ProductType> _list = null;
            List<ProductTypeInfo> _productTypes = new List<ProductTypeInfo>();

            if (string.IsNullOrEmpty(name))
            {
                _list = _dbContext.ProductTypes.Where(x => x.IsActive).ToList();
            }
            else if (_dbContext.ProductTypes.Where(x => x.IsActive && (x.Code.ToUpper().Contains(name.ToUpper()) || x.Name.ToUpper().Contains(name.ToUpper()))).Any())
            {
                _list = _dbContext.ProductTypes.Where(x => x.IsActive && (x.Code.ToUpper().Contains(name.ToUpper()) || x.Name.ToUpper().Contains(name.ToUpper()))).ToList();
            }

            _list.ForEach(x =>
            {
                ProductTypeInfo _info = new ProductTypeInfo()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                };
                _productTypes.Add(_info);
            });

            return _productTypes;
        }

        [HttpPut]
        [Route("update")]
        public bool Update(ProductTypeInfo info)
        {
            bool _result = false;

            if (_dbContext.ProductTypes.Where(x => x.Id == info.Id && x.IsActive).Any())
            {
                var _info = _dbContext.ProductTypes.Where(x => x.Id == info.Id && x.IsActive).First();
                _info.Code = info.Code;
                _info.Name = info.Name;
                _info.UpdatedDate = DateTime.Now;
                _result = _dbContext.SaveChanges() > 0;
            }

            return _result;
        }

        [HttpPost]
        [Route("save")]
        public int Save(ProductTypeInfo info)
        {
            int _id = 0;

            if (!_dbContext.ProductTypes.Where(x => x.Code == info.Code && x.IsActive).Any())
            {
                var _productType = new ProductType() { Code = info.Code, Name = info.Name, IsActive = true, CreatedDate = DateTime.Now };
                _dbContext.ProductTypes.Add(_productType);
                _dbContext.SaveChanges();
                _id = _productType.Id;
            }

            return _id;
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public bool Delete(int id)
        {
            bool _result = false;

            if (_dbContext.ProductTypes.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _info = _dbContext.ProductTypes.Where(x => x.Id == id && x.IsActive).FirstOrDefault();
                _info.IsActive = false;
                _info.UpdatedDate = DateTime.Now;
                _result = _dbContext.SaveChanges() > 0;
            }

            return _result;
        }
    }
}
