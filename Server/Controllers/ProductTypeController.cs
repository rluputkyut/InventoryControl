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
    [Route("api/producttype")]
    public class ProductTypeController : Controller
    {
        private readonly ILogger<ProductTypeController> _logger;
        InventoryControlContext _dbContext;

        public ProductTypeController(ILogger<ProductTypeController> logger, InventoryControlContext inventoryControlContext)
        {
            _logger = logger;
            _dbContext = inventoryControlContext;
        }

        [HttpGet]
        [Route("get")]
        public List<ProductTypeInfo> Get()
        {
            List<ProductTypeInfo> _productTypes = new List<ProductTypeInfo>();
            var _list = _dbContext.ProductTypes.Where(x => x.IsActive).OrderByDescending(x=>x.Id).ToList();
            _list.ForEach(x =>
            {
                ProductTypeInfo _info = new ProductTypeInfo()
                {
                    Id = x.Id,
                    Name = x.Name
                };
                _productTypes.Add(_info);
            });

            return _productTypes;
        }

        [HttpGet]
        [Route("getbypage")]
        public ProductTypeList GetbyPage([FromQuery] PageParameters parameters)
        {
            List<ProductTypeInfo> _productTypes = new List<ProductTypeInfo>();
            var _list = _dbContext.ProductTypes.Where(x => x.IsActive).OrderByDescending(x => x.Id).ToList();
            _list.ForEach(x =>
            {
                ProductTypeInfo _info = new ProductTypeInfo()
                {
                    Id = x.Id,
                    Name = x.Name
                };
                _productTypes.Add(_info);
            });

            var response = PagedList<ProductTypeInfo>.ToPagedList(_productTypes, parameters.PageNumber, parameters.PageSize);
            return new ProductTypeList() { Items = response.ToList(), Meta = response.MetaData };
            //return _productTypes;
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public ProductTypeInfo GetById(int id)
        {
            ProductTypeInfo _productType = null;

            if (_dbContext.ProductTypes.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _info = _dbContext.ProductTypes.Where(x => x.Id == id && x.IsActive).First();
                _productType = new ProductTypeInfo() { Id = _info.Id, Name = _info.Name };
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
            else if (_dbContext.ProductTypes.Where(x => x.IsActive && x.Name.ToUpper().Contains(name.ToUpper())).Any())
            {
                _list = _dbContext.ProductTypes.Where(x => x.IsActive && x.Name.ToUpper().Contains(name.ToUpper())).OrderByDescending(x => x.Id).ToList();
            }

            _list.ForEach(x =>
            {
                ProductTypeInfo _info = new ProductTypeInfo()
                {
                    Id = x.Id,
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

            if (!_dbContext.ProductTypes.Where(x => x.Name == info.Name && x.IsActive).Any())
            {
                var _productType = new ProductType() { Name = info.Name, IsActive = true, CreatedDate = DateTime.Now };
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
