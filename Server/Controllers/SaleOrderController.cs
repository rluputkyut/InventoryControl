﻿using InventoryControl.Server.Models;
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
    [Route("api/saleorder")]
    public class SaleOrderController : ControllerBase
    {
        private readonly ILogger<SaleOrderController> _logger;
        InventoryControlContext _dbContext;

        public SaleOrderController(ILogger<SaleOrderController> logger, InventoryControlContext inventoryControlContext)
        {
            _logger = logger;
            _dbContext = inventoryControlContext;
        }

        [HttpGet]
        [Route("getheaders")]
        public SaleOrderHeaderList GetHeaders([FromQuery] PageParameters parameters)
        {
            List<SaleOrderHeaderInfo> _list = new List<SaleOrderHeaderInfo>();
            var _headers = _dbContext.SaleOrderHeaders.Where(x => x.IsActive).OrderByDescending(x => x.Id).ToList();
            var _customers = _dbContext.Customers.ToList();
            var _warehouses = _dbContext.Warehouses.ToList();

            _headers.ForEach(x =>
            {
                SaleOrderHeaderInfo _info = new SaleOrderHeaderInfo()
                {
                    Id = x.Id,
                    Code = x.Code,
                    CustomerId = x.CustomerId,
                    CustomerName = _customers.Where(y => y.Id == x.CustomerId).Select(y => y.Name).First(),
                    WarehouseId = x.WarehouseId,
                    WarehouseName = _warehouses.Where(y => y.Id == x.WarehouseId).Select(y => y.Name).First(),
                    SellingDate = x.CreatedDate,
                    Remark = x.Remark,
                    Delivered = x.Delivered,
                    IsCOD = x.CashOnDelivery,
                    IsAccountTransfer = x.AccountTransfer,
                    TransferInfo = x.TransferInformation,
                };
                _list.Add(_info);
            });

            var response = PagedList<SaleOrderHeaderInfo>.ToPagedList(_list, parameters.PageNumber, parameters.PageSize);
            return new SaleOrderHeaderList() { Items = response.ToList(), Meta = response.MetaData };
            //return _list;
        }

        [HttpGet]
        [Route("getheaderbyid/{id}")]
        public SaleOrderHeaderInfo GetHeaderById(int id)
        {
            SaleOrderHeaderInfo _info = new SaleOrderHeaderInfo();

            if (_dbContext.SaleOrderHeaders.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _header = _dbContext.SaleOrderHeaders.Where(x => x.Id == id && x.IsActive).First();
                _info.Id = _header.Id;
                _info.Code = _header.Code;
                _info.CustomerId = _header.CustomerId;
                _info.CustomerName = _dbContext.Customers.Where(x => x.Id == _header.CustomerId).Select(x => x.Name).First();
                _info.Remark = _header.Remark;
                _info.Delivered = _header.Delivered;
                _info.IsCOD = _header.CashOnDelivery;
                _info.IsAccountTransfer = _header.AccountTransfer;
                _info.TransferInfo = _header.TransferInformation;
                _info.Discount = _header.Discount;
                _info.SellingDate = _header.CreatedDate;
                _info.WarehouseId = _header.WarehouseId;
                _info.WarehouseName = _dbContext.Warehouses.Where(x => x.Id == _header.WarehouseId).Select(x => x.Name).First();
            }
            return _info;
        }

        [HttpGet]
        [Route("getitemsbyid/{id}")]
        public List<SaleOrderItemInfo> GetItemsById(int id)
        {
            List<SaleOrderItemInfo> _list = new List<SaleOrderItemInfo>();

            var _items = _dbContext.SaleOrderItems.Where(x => x.HeaderId == id && x.IsActive).ToList();

            _items.ForEach(x =>
            {
                SaleOrderItemInfo _info = new SaleOrderItemInfo()
                {
                    Id = x.Id,
                    HeaderId = x.HeaderId,
                    ProductId = x.ProductId,
                    ProductName = _dbContext.Products.Where(z => z.Id == x.ProductId).Select(z => z.Name).FirstOrDefault(),
                    SellingPrice = x.SellingPrice,
                    OtherExpense = x.OtherExpense == null ? 0 : (decimal)x.OtherExpense,
                    Quantity = x.Quantity,
                };
                _list.Add(_info);
            });

            return _list;
        }

        [HttpGet]
        [Route("getheaderfrompreorder/{id}")]
        public SaleOrderHeaderInfo GetHeaderFromPreOrder(int id)
        {
            SaleOrderHeaderInfo _info = new SaleOrderHeaderInfo();

            if (_dbContext.PreOrderHeaders.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _header = _dbContext.PreOrderHeaders.Where(x => x.Id == id && x.IsActive).First();
                _info.Id = _header.Id;
                _info.Code = _header.Code;
                _info.CustomerId = _header.CustomerId;
                _info.CustomerName = _dbContext.Customers.Where(x => x.Id == _header.CustomerId).Select(x => x.Name).First();
                _info.Remark = _header.Remark+ "From PreOrder";
                _info.Discount = _header.Deposit;
                _info.Delivered = false;
                _info.IsCOD = false;
                _info.IsAccountTransfer = false;
                _info.TransferInfo = string.Empty;
                _info.SellingDate = _header.CreatedDate;
                _info.WarehouseId = _header.WarehouseId;
                _info.WarehouseName = _dbContext.Warehouses.Where(x => x.Id == _header.WarehouseId).Select(x => x.Name).First();
            }
            return _info;
        }

        [HttpGet]
        [Route("getitemsfrompreorder/{id}")]
        public List<SaleOrderItemInfo> GetItemsFromPreOrder(int id)
        {
            List<SaleOrderItemInfo> _list = new List<SaleOrderItemInfo>();

            var _header = _dbContext.PreOrderHeaders.Where(x => x.Id == id && x.IsActive).First();
            var _items = _dbContext.PreOrderItems.Where(x => x.HeaderId == id && x.IsActive).ToList();

            _items.ForEach(x =>
            {
                SaleOrderItemInfo _info = new SaleOrderItemInfo()
                {
                    Id = x.Id,
                    HeaderId = x.HeaderId,
                    ProductId = x.ProductId,
                    ProductName = _dbContext.Products.Where(z => z.Id == x.ProductId).Select(z => z.Name).FirstOrDefault(),
                    SellingPrice = _dbContext.WarehouseProducts.Where(z=>z.WarehouseId == _header.WarehouseId && z.ProductId == x.ProductId && z.IsActive).Select(z=>z.Price).FirstOrDefault(),
                    OtherExpense = 0,
                    Quantity = x.Quantity,
                };
                _list.Add(_info);
            });

            return _list;
        }

        [HttpPost]
        [Route("save")]
        public int Save(SaleOrderHeaderInfo info)
        {
            int _id = 0;

            if (!_dbContext.SaleOrderHeaders.Where(x => x.Code == info.Code && x.IsActive).Any())
            {
                using var transaction = _dbContext.Database.BeginTransaction();

                try
                {
                    var _header = new SaleOrderHeader()
                    {
                        Code = info.Code,
                        WarehouseId = info.WarehouseId,
                        CustomerId = info.CustomerId,
                        CashOnDelivery = info.IsCOD,
                        AccountTransfer = info.IsAccountTransfer,
                        TransferInformation = info.TransferInfo,
                        Delivered = info.Delivered,
                        Remark = info.Remark,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    };
                    _dbContext.SaleOrderHeaders.Add(_header);
                    _dbContext.SaveChanges();

                    foreach (var item in info.Items)
                    {
                        var _item = new SaleOrderItem()
                        {
                            HeaderId = _header.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            SellingPrice = item.SellingPrice,
                            OtherExpense = item.OtherExpense,
                            IsActive = true,
                            CreatedDate = DateTime.Now
                        };
                        _dbContext.SaleOrderItems.Add(_item);

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

                    if (_dbContext.PreOrderHeaders.Where(x => x.Id == info.PreOrderId && x.IsActive).Any())
                    {
                        var _preOrder = _dbContext.PreOrderHeaders.Where(x => x.Id == info.PreOrderId && x.IsActive).First();
                        _id = _header.Id;
                        _preOrder.SoldOut = true;
                        _preOrder.UpdatedDate = DateTime.Now;
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

        [HttpGet]
        [Route("updatestatus/{id}/{delivered}")]
        public int UpdateStatus(int id, bool delivered)
        {
            int _id = 0;

            if (_dbContext.SaleOrderHeaders.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _header = _dbContext.SaleOrderHeaders.Where(x => x.Id == id && x.IsActive).First();
                _id = _header.Id;
                _header.Delivered = delivered;
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

            if (_dbContext.SaleOrderHeaders.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _info = _dbContext.SaleOrderHeaders.Where(x => x.Id == id && x.IsActive).FirstOrDefault();
                _info.IsActive = false;
                _info.UpdatedDate = DateTime.Now;

                var _oldItems = _dbContext.SaleOrderItems.Where(x => x.HeaderId == _info.Id && x.IsActive).ToList();
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
