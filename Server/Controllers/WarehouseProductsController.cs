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
    [Route("api/warehouseproducts")]
    public class WarehouseProductsController : ControllerBase
    {
        private readonly ILogger<WarehouseProductsController> _logger;
        InventoryControlContext _dbContext;

        public WarehouseProductsController(ILogger<WarehouseProductsController> logger, InventoryControlContext inventoryControlContext)
        {
            _logger = logger;
            _dbContext = inventoryControlContext;
        }

        [HttpGet]
        [Route("get")]
        public List<WarehouseProductInfo> Get()
        {
            List<WarehouseProductInfo> _productTypes = new List<WarehouseProductInfo>();
            var _list = _dbContext.WarehouseProducts.Where(x => x.IsActive).ToList();
            _list.ForEach(x =>
            {
                WarehouseProductInfo _info = new WarehouseProductInfo()
                {
                    Id = x.Id,
                    WarehouseId = x.WarehouseId,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                };
                _productTypes.Add(_info);
            });

            return _productTypes;
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public WarehouseProductInfo GetById(int id)
        {
            WarehouseProductInfo _productType = null;

            if (_dbContext.WarehouseProducts.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _info = _dbContext.WarehouseProducts.Where(x => x.Id == id && x.IsActive).First();
                _productType = new WarehouseProductInfo() { Id = _info.Id, WarehouseId = _info.WarehouseId, ProductId = _info.ProductId, Quantity = _info.Quantity };
            }

            return _productType;
        }

        [HttpGet]
        [Route("getbywarehouseid/{id}")]
        public List<WarehouseProductInfo> GetByWarehouseId(int id)
        {
            List<WarehouseProductInfo> _productTypes = new List<WarehouseProductInfo>();

            if (_dbContext.WarehouseProducts.Where(x => x.WarehouseId == id && x.IsActive).Any())
            {
                var _list = _dbContext.WarehouseProducts.Where(x => x.WarehouseId == id && x.IsActive).ToList();

                foreach (WarehouseProduct _info in _list)
                {
                    var _product = _dbContext.Products.Where(x => x.Id == _info.ProductId && x.IsActive).FirstOrDefault();

                    _productTypes.Add(new WarehouseProductInfo()
                    {
                        Id = _info.Id,
                        WarehouseId = _info.WarehouseId,
                        ProductId = _info.ProductId,
                        ProductCode = _product.Code,
                        ProductName = _product.Name,
                        Quantity = _info.Quantity
                    });
                }
            }

            return _productTypes;
        }

        [HttpPut]
        [Route("update")]
        public bool Update(WarehouseProductInfo info)
        {
            bool _result = false;

            if (_dbContext.WarehouseProducts.Where(x => x.Id == info.Id && x.IsActive).Any())
            {
                var _info = _dbContext.WarehouseProducts.Where(x => x.Id == info.Id && x.IsActive).First();
                _info.ProductId = info.ProductId;
                _info.WarehouseId = info.WarehouseId;
                _info.Quantity = info.Quantity;
                _info.UpdatedDate = DateTime.Now;
                _result = _dbContext.SaveChanges() > 0;
            }

            return _result;
        }

        [HttpPost]
        [Route("save")]
        public int Save(WarehouseProductInfo info)
        {
            int _id = 0;

            if (!_dbContext.WarehouseProducts.Where(x => x.ProductId == info.ProductId && x.WarehouseId == info.WarehouseId && x.IsActive).Any())
            {
                var _productType = new WarehouseProduct() 
                { 
                    ProductId = info.ProductId, 
                    WarehouseId = info.WarehouseId, 
                    Quantity = info.Quantity,
                    IsActive = true, 
                    CreatedDate = DateTime.Now 
                };
                _dbContext.WarehouseProducts.Add(_productType);
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

            if (_dbContext.WarehouseProducts.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _info = _dbContext.WarehouseProducts.Where(x => x.Id == id && x.IsActive).FirstOrDefault();
                _info.IsActive = false;
                _info.UpdatedDate = DateTime.Now;
                _result = _dbContext.SaveChanges() > 0;
            }

            return _result;
        }

    }
}
