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
    [Route("api/preorder")]
    public class PerOrderController : ControllerBase
    {
        private readonly ILogger<PerOrderController> _logger;
        InventoryControlContext _dbContext;

        public PerOrderController(ILogger<PerOrderController> logger, InventoryControlContext inventoryControlContext)
        {
            _logger = logger;
            _dbContext = inventoryControlContext;
        }

        [HttpGet]
        [Route("getheaders")]
        public PreOrderHeaderList GetHeaders([FromQuery] PageParameters parameters)
        {
            List<PreOrderHeaderInfo> _list = new List<PreOrderHeaderInfo>();
            var _products = _dbContext.PreOrderHeaders.Where(x => x.IsActive).OrderByDescending(x=>x.Id).ToList();
            var _warehouses = _dbContext.Warehouses.ToList();

            _products.ForEach(x =>
            {
                PreOrderHeaderInfo _info = new PreOrderHeaderInfo()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Deposit = x.Deposit,
                    OrderDate = x.CreatedDate,
                    Remark = x.Remark,
                    WaitingDays = x.WaitingDay,
                    SoldOut = x.SoldOut,
                    CustomerId = x.CustomerId,
                    CustomerName = _dbContext.Customers.Where(y => y.Id == x.CustomerId).Select(y => y.Name).First(),
                    WarehouseId = x.WarehouseId,
                    WarehouseName = _dbContext.Warehouses.Where(y => y.Id == x.WarehouseId).Select(y => y.Name).First()
                };
                _list.Add(_info);
            });

            var response = PagedList<PreOrderHeaderInfo>.ToPagedList(_list, parameters.PageNumber, parameters.PageSize);
            return new PreOrderHeaderList() { Items = response.ToList(), Meta = response.MetaData };
            //return _list;
        }

        [HttpGet]
        [Route("getheaderbyid/{id}")]
        public PreOrderHeaderInfo GetHeaderById(int id)
        {
            PreOrderHeaderInfo _info = new PreOrderHeaderInfo();

            if (_dbContext.PreOrderHeaders.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _product = _dbContext.PreOrderHeaders.Where(x => x.Id == id && x.IsActive).First();
                _info.Id = _product.Id;
                _info.Code = _product.Code;
                _info.Remark = _product.Remark;
                _info.Deposit = _product.Deposit;
                _info.SoldOut = _product.SoldOut;
                _info.OrderDate = _product.CreatedDate;
                _info.WaitingDays = _product.WaitingDay;
                _info.WarehouseId = _product.WarehouseId;
                _info.WarehouseName = _dbContext.Warehouses.Where(x => x.Id == _product.WarehouseId).Select(x => x.Name).First();
                _info.CustomerId = _product.CustomerId;
                _info.CustomerName = _dbContext.Customers.Where(x => x.Id == _product.CustomerId).Select(x=>x.Name).First();
            }
            return _info;
        }

        [HttpGet]
        [Route("getitemsbyid/{id}")]
        public List<PreOrderItemInfo> GetItemsById(int id)
        {
            List<PreOrderItemInfo> _list = new List<PreOrderItemInfo>();

            var _products = _dbContext.PreOrderItems.Where(x => x.HeaderId == id && x.IsActive).ToList();

            _products.ForEach(x =>
            {
                var _item = _dbContext.Products.Where(z => z.Id == x.ProductId).FirstOrDefault();

                PreOrderItemInfo _info = new PreOrderItemInfo()
                {
                    Id = x.Id,
                    HeaderId = x.HeaderId,
                    ProductId = x.ProductId,
                    ProductName = _item.Name,
                    ProductPrice =x.OrderPrice,
                    Quantity = x.Quantity,
                };
                _list.Add(_info);
            });

            return _list;
        }

        [HttpPost]
        [Route("save")]
        public int Save(PreOrderHeaderInfo info)
        {
            int _id = 0;

            if (!_dbContext.PreOrderHeaders.Where(x => x.Code == info.Code && x.IsActive).Any())
            {
                using var transaction = _dbContext.Database.BeginTransaction();

                try
                {
                    var _header = new PreOrderHeader()
                    {
                        Code = info.Code,
                        CustomerId = info.CustomerId,
                        WarehouseId = info.WarehouseId,
                        WaitingDay = info.WaitingDays,
                        Deposit = info.Deposit,
                        SoldOut = info.SoldOut,
                        Remark = info.Remark,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    };
                    _dbContext.PreOrderHeaders.Add(_header);
                    _dbContext.SaveChanges();

                    foreach (var item in info.Items)
                    {
                        var _item = new PreOrderItem()
                        {
                            HeaderId = _header.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            OrderPrice = item.ProductPrice,
                            IsActive = true,
                            CreatedDate = DateTime.Now
                        };
                        _dbContext.PreOrderItems.Add(_item);
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

        [HttpPut]
        [Route("update")]
        public int Update(PreOrderHeaderInfo info)
        {
            int _id = 0;

            if (_dbContext.PreOrderHeaders.Where(x => x.Id == info.Id && x.IsActive).Any())
            {
                using var transaction = _dbContext.Database.BeginTransaction();

                try
                {
                    var _header = _dbContext.PreOrderHeaders.Where(x => x.Id == info.Id && x.IsActive).FirstOrDefault();

                    _header.Code = info.Code;
                    _header.CustomerId = info.CustomerId;
                    _header.WarehouseId = info.WarehouseId;
                    _header.Deposit = info.Deposit;
                    _header.WaitingDay = info.WaitingDays;
                    _header.SoldOut = info.SoldOut;
                    _header.Remark = info.Remark;
                    _header.UpdatedDate = DateTime.Now;
                    _dbContext.SaveChanges();

                    var _oldItems = _dbContext.PreOrderItems.Where(x => x.HeaderId == _header.Id && x.IsActive).ToList();
                    _oldItems.ForEach(x => { x.IsActive = false; x.UpdatedDate = DateTime.Now; });
                    _dbContext.SaveChanges();

                    foreach (var item in info.Items)
                    {
                        var _item = new PreOrderItem()
                        {
                            HeaderId = _header.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            OrderPrice = item.ProductPrice,
                            IsActive = true,
                            CreatedDate = DateTime.Now
                        };
                        _dbContext.PreOrderItems.Add(_item);
                        _dbContext.SaveChanges();
                    }

                    transaction.Commit();
                    _id = _header.Id;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError("", ex);
                }
            }

            return _id;
        }

        [HttpGet]
        [Route("checkstatus/{id}")]
        public int CheckStatus(int id)
        {
            int _id = 0;

            if (_dbContext.PreOrderHeaders.Where(x => x.Id ==id && x.IsActive).Any())
            {
                var _header = _dbContext.PreOrderHeaders.Where(x => x.Id ==id && x.IsActive).First();
                var _items = _dbContext.PreOrderItems.Where(x => x.HeaderId == _header.Id && x.IsActive).ToList();

                foreach (PreOrderItem _item in _items)
                {
                    if (_dbContext.WarehouseProducts.Where(x => x.ProductId == _item.ProductId && x.WarehouseId == _header.WarehouseId && x.IsActive).Any())
                    {
                        var _product = _dbContext.WarehouseProducts.Where(x => x.ProductId == _item.ProductId && x.WarehouseId == _header.WarehouseId && x.IsActive).FirstOrDefault();

                        if (_product.Quantity >= _item.Quantity)
                            _id = _header.Id;
                        else
                        {
                            _id = 0;
                            break;
                        }
                    }
                    else
                    {
                        _id = 0;
                        break;
                    }
                }
            }

            return _id;
        }

        [HttpPut]
        [Route("updatestatus")]
        public int UpdateStatus(PreOrderHeaderInfo info)
        {
            int _id = 0;

            if (_dbContext.PreOrderHeaders.Where(x => x.Id == info.Id && x.IsActive).Any())
            {
                var _header = _dbContext.PreOrderHeaders.Where(x => x.Id == info.Id && x.IsActive).First();
                _id = _header.Id;
                _header.SoldOut = true;
                _header.UpdatedDate = DateTime.Now;
                _dbContext.SaveChanges();
            }

            return _id;
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public bool Delete(int id)
        {
            bool _result = false;

            if (_dbContext.PreOrderHeaders.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _info = _dbContext.PreOrderHeaders.Where(x => x.Id == id && x.IsActive).FirstOrDefault();
                _info.IsActive = false;
                _info.UpdatedDate = DateTime.Now;

                var _oldItems = _dbContext.PreOrderItems.Where(x => x.HeaderId == _info.Id && x.IsActive).ToList();
                _oldItems.ForEach(x => { x.IsActive = false; x.UpdatedDate = DateTime.Now; });

                _result = _dbContext.SaveChanges() > 0;
            }

            return _result;
        }

    }
}
