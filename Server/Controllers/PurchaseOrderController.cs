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
    [Route("api/purchaseorder")]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly ILogger<PurchaseOrderController> _logger;
        InventoryControlContext _dbContext;

        public PurchaseOrderController(ILogger<PurchaseOrderController> logger, InventoryControlContext inventoryControlContext)
        {
            _logger = logger;
            _dbContext = inventoryControlContext;
        }

        [HttpGet]
        [Route("getheaders")]
        public PurchaseOrderHeaderList GetHeaders([FromQuery] PageParameters parameters)
        {
            List<PurchaseOrderHeaderInfo> _list = new List<PurchaseOrderHeaderInfo>();
            var _headers = _dbContext.PurchaseOrderHeaders.Where(x => x.IsActive).OrderByDescending(x=>x.Id).ToList();
            var _warehouses = _dbContext.Warehouses.ToList();

            _headers.ForEach(x =>
            {
                PurchaseOrderHeaderInfo _info = new PurchaseOrderHeaderInfo()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Source = x.Source,
                    PurchaseDate = x.CreatedDate,
                    Remark = x.Remark,
                    WarehouseId = x.WarehouseId,
                    WarehouseName = _warehouses.Where(y=>y.Id == x.WarehouseId).Select(y=>y.Name).First()
                };
                _list.Add(_info);
            });
                        
            var response = PagedList<PurchaseOrderHeaderInfo>.ToPagedList(_list, parameters.PageNumber, parameters.PageSize);
            return new PurchaseOrderHeaderList() { Items = response.ToList(), Meta = response.MetaData };
            //return _list;
        }

        [HttpGet]
        [Route("getheaderbyid/{id}")]
        public PurchaseOrderHeaderInfo GetHeaderById(int id)
        {
            PurchaseOrderHeaderInfo _info = new PurchaseOrderHeaderInfo();

            if (_dbContext.PurchaseOrderHeaders.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _header = _dbContext.PurchaseOrderHeaders.Where(x => x.Id == id && x.IsActive).First();
                _info.Id = _header.Id;
                _info.Code = _header.Code;
                _info.Source = _header.Source;
                _info.Remark = _header.Remark;                
                _info.PurchaseDate = _header.CreatedDate;
                _info.WarehouseId = _header.WarehouseId;
                _info.WarehouseName = _dbContext.Warehouses.Where(x => x.Id == _header.WarehouseId).Select(x => x.Name).First();
            }
            return _info;
        }

        [HttpGet]
        [Route("getitemsbyid/{id}")]
        public List<PurchaseOrderItemInfo> GetItemsById(int id)
        {
            List<PurchaseOrderItemInfo> _list = new List<PurchaseOrderItemInfo>();

            var _items = _dbContext.PurchaseOrderItems.Where(x => x.HeaderId == id && x.IsActive).ToList();

            _items.ForEach(x =>
            {
                PurchaseOrderItemInfo _info = new PurchaseOrderItemInfo()
                {
                    Id = x.Id,
                    HeaderId = x.HeaderId,
                    ProductId = x.ProductId,
                    ProductName = _dbContext.Products.Where(z => z.Id == x.ProductId).Select(z => z.Name).FirstOrDefault(),
                    BuyingPrice = x.BuyingPrice,
                    Currency = x.Currency,
                    Quantity = x.Quantity,
                };
                _list.Add(_info);
            });

            return _list;
        }

        [HttpPost]
        [Route("save")]
        public int Save(PurchaseOrderHeaderInfo info)
        {
            int _id = 0;

            if (!_dbContext.PurchaseOrderHeaders.Where(x => x.Code == info.Code && x.IsActive).Any())
            {
                using var transaction = _dbContext.Database.BeginTransaction();

                try
                {
                    var _header = new PurchaseOrderHeader()
                    {
                        Code = info.Code,
                        WarehouseId = info.WarehouseId,
                        Source = info.Source,
                        Remark = info.Remark,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    };
                    _dbContext.PurchaseOrderHeaders.Add(_header);
                    _dbContext.SaveChanges();

                    foreach (var item in info.Items)
                    {
                        var _item = new PurchaseOrderItem()
                        {
                            HeaderId = _header.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            BuyingPrice = item.BuyingPrice,
                            Currency = item.Currency,                            
                            IsActive = true,
                            CreatedDate = DateTime.Now
                        };
                        _dbContext.PurchaseOrderItems.Add(_item);

                        if (_dbContext.WarehouseProducts.Where(x => x.WarehouseId == info.WarehouseId && x.ProductId == item.ProductId && x.IsActive).Any())
                        {
                            var _product = _dbContext.WarehouseProducts.Where(x => x.WarehouseId == info.WarehouseId && x.ProductId == item.ProductId && x.IsActive).First();
                            _product.Quantity += item.Quantity;
                            _product.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            _dbContext.WarehouseProducts.Add(new WarehouseProduct() 
                            {
                                ProductId = item.ProductId,
                                Quantity = item.Quantity,
                                WarehouseId = info.WarehouseId,
                                CreatedDate = DateTime.Now,
                                IsActive = true
                            });
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

            if (_dbContext.PurchaseOrderHeaders.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _info = _dbContext.PurchaseOrderHeaders.Where(x => x.Id == id && x.IsActive).FirstOrDefault();
                _info.IsActive = false;
                _info.UpdatedDate = DateTime.Now;

                var _oldItems = _dbContext.PurchaseOrderItems.Where(x => x.HeaderId == _info.Id && x.IsActive).ToList();
                _oldItems.ForEach(x => 
                { 
                    x.IsActive = false; 
                    x.UpdatedDate = DateTime.Now;

                    if (_dbContext.WarehouseProducts.Where(y => y.WarehouseId == _info.WarehouseId && x.ProductId == x.ProductId && x.IsActive).Any())
                    {
                        var _product = _dbContext.WarehouseProducts.Where(y => y.WarehouseId == _info.WarehouseId && x.ProductId == x.ProductId && x.IsActive).First();
                        _product.Quantity -= x.Quantity;
                        _product.UpdatedDate = DateTime.Now;
                    }

                });

                _result = _dbContext.SaveChanges() > 0;
            }

            return _result;
        }

    }
}
