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
    [Route("api/damagedproduct")]
    public class DamagedProductController : ControllerBase
    {
        private readonly ILogger<DamagedProductController> _logger;
        InventoryControlContext _dbContext;

        public DamagedProductController(ILogger<DamagedProductController> logger, InventoryControlContext inventoryControlContext)
        {
            _logger = logger;
            _dbContext = inventoryControlContext;
        }

        [HttpGet]
        [Route("getheaders")]
        public DamagedProductHeaderList GetHeaders([FromQuery] PageParameters parameters)
        {
            List<DamagedProductHeaderInfo> _list = new List<DamagedProductHeaderInfo>();
            var _headers = _dbContext.DamagedProductHeaders.Where(x => x.IsActive).OrderByDescending(x => x.Id).ToList();
            var _warehouses = _dbContext.Warehouses.ToList();

            _headers.ForEach(x =>
            {
                DamagedProductHeaderInfo _info = new DamagedProductHeaderInfo()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Cost = x.Cost,
                    WarehouseId = x.WarehouseId,
                    WarehouseName = _warehouses.Where(y => y.Id == x.WarehouseId).Select(y => y.Name).First(),
                    Remark = x.Remark,
                    DamagedDate = x.CreatedDate
                };
                _list.Add(_info);
            });

            var response = PagedList<DamagedProductHeaderInfo>.ToPagedList(_list, parameters.PageNumber, parameters.PageSize);
            return new DamagedProductHeaderList() { Items = response.ToList(), Meta = response.MetaData };
            //return _list;
        }

        [HttpGet]
        [Route("getheaderbyid/{id}")]
        public DamagedProductHeaderInfo GetHeaderById(int id)
        {
            DamagedProductHeaderInfo _info = new DamagedProductHeaderInfo();

            if (_dbContext.DamagedProductHeaders.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _header = _dbContext.DamagedProductHeaders.Where(x => x.Id == id && x.IsActive).First();
                _info.Id = _header.Id;
                _info.Code = _header.Code;
                _info.Remark = _header.Remark;
                _info.Cost = _header.Cost;
                _info.DamagedDate = _header.CreatedDate;
                _info.WarehouseId = _header.WarehouseId;
                _info.WarehouseName = _dbContext.Warehouses.Where(x => x.Id == _header.WarehouseId).Select(x => x.Name).First();
            }
            return _info;
        }

        [HttpGet]
        [Route("getitemsbyid/{id}")]
        public List<DamagedProductItemInfo> GetItemsById(int id)
        {
            List<DamagedProductItemInfo> _list = new List<DamagedProductItemInfo>();

            var _items = _dbContext.DamagedProductItems.Where(x => x.HeaderId == id && x.IsActive).ToList();

            _items.ForEach(x =>
            {
                DamagedProductItemInfo _info = new DamagedProductItemInfo()
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
        public int Save(DamagedProductHeaderInfo info)
        {
            int _id = 0;

            if (!_dbContext.DamagedProductHeaders.Where(x => x.Code == info.Code && x.IsActive).Any())
            {
                using var transaction = _dbContext.Database.BeginTransaction();

                try
                {
                    var _header = new DamagedProductHeader()
                    {
                        Code = info.Code,
                        WarehouseId = info.WarehouseId,
                        Remark = info.Remark,
                        Cost = info.Cost,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    };
                    _dbContext.DamagedProductHeaders.Add(_header);
                    _dbContext.SaveChanges();

                    foreach (var item in info.Items)
                    {
                        var _item = new DamagedProductItem()
                        {
                            HeaderId = _header.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            IsActive = true,
                            CreatedDate = DateTime.Now
                        };
                        _dbContext.DamagedProductItems.Add(_item);

                        if (_dbContext.WarehouseProducts.Where(x => x.WarehouseId == info.WarehouseId && x.ProductId == item.ProductId && x.IsActive).Any())
                        {
                            var _product = _dbContext.WarehouseProducts.Where(x => x.WarehouseId == info.WarehouseId && x.ProductId == item.ProductId && x.IsActive).First();
                            _product.Quantity -= item.Quantity;
                            _product.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            throw new Exception("invalid item");
                        }

                        _dbContext.SaveChanges();
                    }

                    transaction.Commit();
                    _id = _header.Id;
                }
                catch
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

            if (_dbContext.DamagedProductHeaders.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _info = _dbContext.DamagedProductHeaders.Where(x => x.Id == id && x.IsActive).FirstOrDefault();
                _info.IsActive = false;
                _info.UpdatedDate = DateTime.Now;

                var _oldItems = _dbContext.DamagedProductItems.Where(x => x.HeaderId == _info.Id && x.IsActive).ToList();
                _oldItems.ForEach(x =>
                {
                    x.IsActive = false;
                    x.UpdatedDate = DateTime.Now;

                    if (_dbContext.WarehouseProducts.Where(y => y.WarehouseId == _info.WarehouseId && y.ProductId == x.ProductId && y.IsActive).Any())
                    {
                        var _product = _dbContext.WarehouseProducts.Where(y => y.WarehouseId == _info.WarehouseId && y.ProductId == x.ProductId && y.IsActive).First();
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
