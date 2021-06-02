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
        public WarehouseProductList Get([FromQuery] PageParameters parameters)
        {
            List<WarehouseProductInfo> _products = new List<WarehouseProductInfo>();
            var _list = _dbContext.WarehouseProducts.Where(x => x.IsActive).ToList();
            _list.ForEach(x =>
            {
                WarehouseProductInfo _info = new WarehouseProductInfo()
                {
                    Id = x.Id,
                    WarehouseId = x.WarehouseId,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price
                };
                _products.Add(_info);
            });

            //return _productTypes;
            var response = PagedList<WarehouseProductInfo>.ToPagedList(_products, parameters.PageNumber, parameters.PageSize);
            return new WarehouseProductList() { Items = response.ToList(), Meta = response.MetaData };
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public WarehouseProductInfo GetById(int id)
        {
            WarehouseProductInfo _productType = null;

            if (_dbContext.WarehouseProducts.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _info = _dbContext.WarehouseProducts.Where(x => x.Id == id && x.IsActive).First();
                _productType = new WarehouseProductInfo() 
                { 
                    Id = _info.Id, 
                    WarehouseId = _info.WarehouseId, 
                    ProductId = _info.ProductId,
                    Quantity = _info.Quantity ,
                    Price = _info.Price,                    
                };
            }

            return _productType;
        }

        [HttpGet]
        [Route("getpgbywarehouseid")]
        public WarehouseProductList GetPgByWarehouseId([FromQuery] WarehouseProductListRequest request)
        {
            List<WarehouseProductInfo> _products = new List<WarehouseProductInfo>();

            if (_dbContext.WarehouseProducts.Where(x => x.WarehouseId == request.WarehouseId && x.IsActive).Any())
            {
                var _list = _dbContext.WarehouseProducts.Where(x => x.WarehouseId == request.WarehouseId && x.IsActive).ToList();

                foreach (WarehouseProduct _info in _list)
                {
                    var _product = _dbContext.Products.Where(x => x.Id == _info.ProductId && x.IsActive).FirstOrDefault();
                    var _brand = _dbContext.Brands.Where(x => x.Id == _product.BrandId && x.IsActive).FirstOrDefault();
                    var _productType = _dbContext.ProductTypes.Where(x => x.Id == _product.ProductTypeId && x.IsActive).FirstOrDefault();

                    _products.Add(new WarehouseProductInfo()
                    {
                        Id = _info.Id,
                        WarehouseId = _info.WarehouseId,
                        ProductId = _info.ProductId,
                        ProductName = _product.Name,
                        BrandName = _brand.Name,
                        ProductType = _productType.Name,
                        Size = _product.Size,
                        Quantity = _info.Quantity,
                        Price = _info.Price,
                        BatchCode = _product.BatchCode,
                        ManufactureDate = _product.ManufactureDate,
                        ExpiredDate = _product.ExpiredDate
                    }) ;
                }
            }

            if (!string.IsNullOrEmpty(request.Code))
                _products = _products.Where(x => x.ProductCode.ToLower().Contains(request.Code.ToLower())).ToList();
            if (!string.IsNullOrEmpty(request.Name))
                _products = _products.Where(x => x.ProductName.ToLower().Contains(request.Name.ToLower())).ToList();
            if (!string.IsNullOrEmpty(request.Brand))
                _products = _products.Where(x => x.BrandName.ToLower().Contains(request.Brand.ToLower())).ToList();
            if (!string.IsNullOrEmpty(request.Type))
                _products = _products.Where(x => x.ProductType.ToLower().Contains(request.Type.ToLower())).ToList();

            //return _productTypes;
            var response = PagedList<WarehouseProductInfo>.ToPagedList(_products, request.PageNumber, request.PageSize);
            return new WarehouseProductList() { Items = response.ToList(), Meta = response.MetaData };
        }

        [HttpGet]
        [Route("getbywarehouseid/{id}")]
        public List<WarehouseProductInfo> GetByWarehouseId(int id)
        {
            List<WarehouseProductInfo> _products = new List<WarehouseProductInfo>();

            if (_dbContext.WarehouseProducts.Where(x => x.WarehouseId == id && x.IsActive).Any())
            {
                var _list = _dbContext.WarehouseProducts.Where(x => x.WarehouseId == id && x.IsActive).ToList();

                foreach (WarehouseProduct _info in _list)
                {
                    var _product = _dbContext.Products.Where(x => x.Id == _info.ProductId && x.IsActive).FirstOrDefault();

                    _products.Add(new WarehouseProductInfo()
                    {
                        Id = _info.Id,
                        WarehouseId = _info.WarehouseId,
                        ProductId = _info.ProductId,
                        ProductName = _product.Name,
                        Quantity = _info.Quantity,
                        Price = _info.Price
                    });
                }
            }
            return _products;            
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
                _info.Price = info.Price;
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
                    Price = info.Price,
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
