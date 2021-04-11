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
    [Route("api/warehouse")]
    public class WarehouseController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        InventoryControlContext _dbContext;

        public WarehouseController(ILogger<WeatherForecastController> logger, InventoryControlContext inventoryControlContext)
        {
            _logger = logger;
            _dbContext = inventoryControlContext;
        }

        [HttpGet]
        [Route("get")]
        public List<WarehouseInfo> Get()
        {
            List<WarehouseInfo> _warehouses = new List<WarehouseInfo>();
            var _list = _dbContext.Warehouses.Where(x => x.IsActive).ToList();
            _list.ForEach(x =>
            {
                WarehouseInfo _info = new WarehouseInfo()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                };
                _warehouses.Add(_info);
            });

            return _warehouses;
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public WarehouseInfo GetById(int id)
        {
            WarehouseInfo _warehouse = null;

            if (_dbContext.Warehouses.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _info = _dbContext.Warehouses.Where(x => x.Id == id && x.IsActive).First();
                _warehouse = new WarehouseInfo() { Id = _info.Id, Code = _info.Code, Name = _info.Name };
            }

            return _warehouse;
        }

        [HttpGet]
        [Route("getbyname/{id}")]
        public List<WarehouseInfo> GetByName(string name)
        {
            List<Warehouse> _list = null;
            List<WarehouseInfo> _warehouses = new List<WarehouseInfo>();

            if (string.IsNullOrEmpty(name))
            {
                _list = _dbContext.Warehouses.Where(x => x.IsActive).ToList();
            }
            else if (_dbContext.Warehouses.Where(x => x.IsActive && (x.Code.ToUpper().Contains(name.ToUpper()) || x.Name.ToUpper().Contains(name.ToUpper()))).Any())
            {
                _list = _dbContext.Warehouses.Where(x => x.IsActive && (x.Code.ToUpper().Contains(name.ToUpper()) || x.Name.ToUpper().Contains(name.ToUpper()))).ToList();
            }

            _list.ForEach(x =>
            {
                WarehouseInfo _info = new WarehouseInfo()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                };
                _warehouses.Add(_info);
            });

            return _warehouses;
        }

        [HttpPut]
        [Route("update")]
        public bool Update(WarehouseInfo info)
        {
            bool _result = false;

            if (_dbContext.Warehouses.Where(x => x.Id == info.Id && x.IsActive).Any())
            {
                var _info = _dbContext.Warehouses.Where(x => x.Id == info.Id && x.IsActive).First();
                _info.Code = info.Code;
                _info.Name = info.Name;
                _info.UpdatedDate = DateTime.Now;
                _result = _dbContext.SaveChanges() > 0;
            }

            return _result;
        }

        [HttpPost]
        [Route("save")]
        public int Save(WarehouseInfo info)
        {
            int _id = 0;

            if (!_dbContext.Warehouses.Where(x => x.Code == info.Code && x.IsActive).Any())
            {
                var _warehouse = new Warehouse() { Code = info.Code, Name = info.Name, IsActive = true, CreatedDate = DateTime.Now };
                _dbContext.Warehouses.Add(_warehouse);
                _dbContext.SaveChanges();
                _id = _warehouse.Id;
            }

            return _id;
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public bool Delete(int id)
        {
            bool _result = false;

            if (_dbContext.Warehouses.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _info = _dbContext.Warehouses.Where(x => x.Id == id && x.IsActive).FirstOrDefault();
                _info.IsActive = false;
                _info.UpdatedDate = DateTime.Now;
                _result = _dbContext.SaveChanges() > 0;
            }

            return _result;
        }

    }
}
