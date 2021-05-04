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
    [Route("api/transferproduct")]
    public class TransferProductController : ControllerBase
    {
        private readonly ILogger<TransferProductController> _logger;
        InventoryControlContext _dbContext;

        public TransferProductController(ILogger<TransferProductController> logger, InventoryControlContext inventoryControlContext)
        {
            _logger = logger;
            _dbContext = inventoryControlContext;
        }

        [HttpGet]
        [Route("getheaders")]
        public TransferProductHeaderList GetHeaders([FromQuery] PageParameters parameters)
        {
            List<TransferProductHeaderInfo> _list = new List<TransferProductHeaderInfo>();
            var _products = _dbContext.TransferProductHeaders.Where(x => x.IsActive).OrderByDescending(x=>x.Id).ToList();
            var _warehouses = _dbContext.Warehouses.ToList();

            _products.ForEach(x =>
            {
                TransferProductHeaderInfo _info = new TransferProductHeaderInfo()
                {
                    Id = x.Id,
                    Code = x.Code,
                    FromWarehouseId = x.FromWarehouseId,
                    FromWarehouseName = _warehouses.Where(z => z.Id == x.FromWarehouseId).Select(z => z.Name).FirstOrDefault(),
                    ToWarehouseId = x.ToWarehouseId,
                    ToWarehouseName = _warehouses.Where(z => z.Id == x.ToWarehouseId).Select(z => z.Name).FirstOrDefault(),
                    Remark = x.Remark,
                    Cost = x.Cost,
                    TransferDate = x.CreatedDate
                };
                _list.Add(_info);
            });

            var response = PagedList<TransferProductHeaderInfo>.ToPagedList(_list, parameters.PageNumber, parameters.PageSize);
            return new TransferProductHeaderList() { Items = response.ToList(), Meta = response.MetaData };
            //return _list;
        }

        [HttpGet]
        [Route("getheaderbyid/{id}")]
        public TransferProductHeaderInfo GetHeaderById(int id)
        {
            TransferProductHeaderInfo _info = new TransferProductHeaderInfo();
            var _warehouses = _dbContext.Warehouses.ToList();

            if (_dbContext.TransferProductHeaders.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _product = _dbContext.TransferProductHeaders.Where(x => x.Id == id && x.IsActive).FirstOrDefault();
                
                _info.Id = _product.Id;
                _info.Code = _product.Code;
                _info.FromWarehouseId = _product.FromWarehouseId;
                _info.FromWarehouseName = _warehouses.Where(z => z.Id == _product.FromWarehouseId).Select(z => z.Name).FirstOrDefault();
                _info.ToWarehouseId = _product.ToWarehouseId;
                _info.ToWarehouseName = _warehouses.Where(z => z.Id == _product.ToWarehouseId).Select(z => z.Name).FirstOrDefault();
                _info.Remark = _product.Remark;
                _info.Cost = _product.Cost;
                _info.TransferDate = _product.CreatedDate;
            }
            return _info;
        }

        [HttpGet]
        [Route("getitemsbyid/{id}")]
        public List<TransferProductItemInfo> GetItemsById(int id)
        {
            List<TransferProductItemInfo> _list = new List<TransferProductItemInfo>();

            var _transferProducts = _dbContext.TransferProductItems.Where(x => x.HeaderId == id && x.IsActive).ToList();

            _transferProducts.ForEach(x =>
            {
                TransferProductItemInfo _info = new TransferProductItemInfo()
                {
                    Id = x.Id, 
                    HeaderId = x.HeaderId,
                    ProductId = x.ProductId,
                    ProductName = _dbContext.Products.Where(z => z.Id == x.ProductId).Select(z => z.Name).FirstOrDefault(),
                    Quantity = x.Quantity,
                };
                _list.Add(_info);
            });

            return _list;
        }

        [HttpPost]
        [Route("save")]
        public int Save(TransferProductHeaderInfo info)
        {
            int _id = 0;

            if (!_dbContext.TransferProductHeaders.Where(x => x.Code == info.Code && x.IsActive).Any())
            {
                using var transaction = _dbContext.Database.BeginTransaction();

                try
                {
                    var _transferproduct = new TransferProductHeader()
                    {
                        Code = info.Code,
                        FromWarehouseId = info.FromWarehouseId,
                        ToWarehouseId = info.ToWarehouseId,
                        Cost = info.Cost,
                        Remark = info.Remark,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    };
                    _dbContext.TransferProductHeaders.Add(_transferproduct);
                    _dbContext.SaveChanges();

                    foreach (var item in info.Items)
                    {
                        var _transferproductItem = new TransferProductItem()
                        {
                            HeaderId = _transferproduct.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            IsActive = true,
                            CreatedDate = DateTime.Now
                        };
                        _dbContext.TransferProductItems.Add(_transferproductItem);

                        if (_dbContext.WarehouseProducts.Where(x => x.WarehouseId == info.FromWarehouseId && x.ProductId == item.ProductId && x.IsActive).Any())
                        {
                            var _product = _dbContext.WarehouseProducts.Where(x => x.WarehouseId == info.FromWarehouseId && x.ProductId == item.ProductId && x.IsActive).First();
                            _product.Quantity -= item.Quantity;
                            _product.UpdatedDate = DateTime.Now;
                        }

                        _dbContext.SaveChanges();
                    }

                    transaction.Commit();
                    _id = _transferproduct.Id;
                }
                catch
                {
                    transaction.Rollback();
                }
            }

            return _id;
        }

        [HttpPut]
        [Route("update")]
        public int Update(TransferProductHeaderInfo info)
        {
            int _id = 0;

            if (_dbContext.TransferProductHeaders.Where(x => x.Id == info.Id && x.IsActive).Any())
            {
                using var transaction = _dbContext.Database.BeginTransaction();

                try
                {
                    var _transferproduct = _dbContext.TransferProductHeaders.Where(x => x.Id == info.Id && x.IsActive).FirstOrDefault();

                    _transferproduct.Code = info.Code;
                    _transferproduct.FromWarehouseId = info.FromWarehouseId;
                    _transferproduct.ToWarehouseId = info.ToWarehouseId;
                    _transferproduct.Cost = info.Cost;
                    _transferproduct.Remark = info.Remark;
                    _transferproduct.UpdatedDate = DateTime.Now;                    
                    _dbContext.SaveChanges();

                    var _oldItems = _dbContext.TransferProductItems.Where(x => x.HeaderId == _transferproduct.Id && x.IsActive).ToList();
                    _oldItems.ForEach(x => { x.IsActive = false; x.UpdatedDate = DateTime.Now; });
                    _dbContext.SaveChanges();

                    foreach (var item in info.Items)
                    {
                        var _transferproductItem = new TransferProductItem()
                        {
                            HeaderId = _transferproduct.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            IsActive = true,
                            CreatedDate = DateTime.Now
                        };
                        _dbContext.TransferProductItems.Add(_transferproductItem);
                        _dbContext.SaveChanges();
                    }

                    transaction.Commit();
                    _id = _transferproduct.Id;
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                }
            }

            return _id;
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public bool Delete(int id)
        {
            bool _result = false;

            if (_dbContext.TransferProductHeaders.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _info = _dbContext.TransferProductHeaders.Where(x => x.Id == id && x.IsActive).FirstOrDefault();
                _info.IsActive = false;
                _info.UpdatedDate = DateTime.Now;

                var _oldItems = _dbContext.TransferProductItems.Where(x => x.HeaderId == _info.Id && x.IsActive).ToList();
                _oldItems.ForEach(x => 
                { 
                    x.IsActive = false; 
                    x.UpdatedDate = DateTime.Now;

                    if (_dbContext.WarehouseProducts.Where(y => y.WarehouseId == _info.FromWarehouseId && x.ProductId == x.ProductId && x.IsActive).Any())
                    {
                        var _product = _dbContext.WarehouseProducts.Where(y => y.WarehouseId == _info.FromWarehouseId && x.ProductId == x.ProductId && x.IsActive).First();
                        _product.Quantity += x.Quantity;
                        _product.UpdatedDate = DateTime.Now;
                    }

                });

                _result = _dbContext.SaveChanges() > 0;
            }

            return _result;
        }

    }
}
