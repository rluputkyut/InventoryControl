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
    [Route("api/brand")]
    public class BrandController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        InventoryControlContext _dbContext;

        public BrandController(ILogger<WeatherForecastController> logger, InventoryControlContext inventoryControlContext)
        {
            _logger = logger;
            _dbContext = inventoryControlContext;
        }

        [HttpGet]
        [Route("get")]
        public List<BrandInfo> Get([FromQuery] PageParameters parameters)
        {
            List<BrandInfo> _brands = new List<BrandInfo>();
            var _brandList = _dbContext.Brands.Where(x=>x.IsActive).ToList();
            _brandList.ForEach(x =>
            {
                BrandInfo _info = new BrandInfo()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                };
                _brands.Add(_info);
            });

            var response = PagedList<BrandInfo>.ToPagedList(_brands, parameters.PageNumber, parameters.PageSize);
            response.ForEach(x => 
            {
                x.CurrentPage = response.MetaData.CurrentPage;
                x.PageSize = response.MetaData.PageSize;
                x.TotalCount = response.MetaData.TotalCount;
                x.TotalPages = response.MetaData.TotalPages;
            });

            return response.ToList();
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public BrandInfo GetById(int id)
        {
            BrandInfo _brand = null;

            if (_dbContext.Brands.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _info = _dbContext.Brands.Where(x => x.Id == id && x.IsActive).First();
                _brand = new BrandInfo() { Id = _info.Id, Code = _info.Code, Name = _info.Name };
            }

            return _brand;
        }

        [HttpGet]
        [Route("getbyname/{id}")]
        public List<BrandInfo> GetByName(string name)
        {
            List<Brand> _list = null;
            List<BrandInfo> _brands = new List<BrandInfo>();

            if (string.IsNullOrEmpty(name))
            {
                _list = _dbContext.Brands.Where(x=> x.IsActive).ToList();
            }
            else if (_dbContext.Brands.Where(x => x.IsActive && (x.Code.ToUpper().Contains(name.ToUpper()) || x.Name.ToUpper().Contains(name.ToUpper()))).Any())
            {
                _list = _dbContext.Brands.Where(x => x.IsActive && (x.Code.ToUpper().Contains(name.ToUpper()) || x.Name.ToUpper().Contains(name.ToUpper()))).ToList();
            }

            _list.ForEach(x =>
            {
                BrandInfo _info = new BrandInfo()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                };
                _brands.Add(_info);
            });

            return _brands;
        }

        [HttpPut]
        [Route("update")]
        public bool Update(BrandInfo info)
        {
            bool _result = false;

            if (_dbContext.Brands.Where(x => x.Id == info.Id && x.IsActive).Any())
            {
                var _info = _dbContext.Brands.Where(x => x.Id == info.Id && x.IsActive).First();
                _info.Code = info.Code;
                _info.Name = info.Name;
                _info.UpdatedDate = DateTime.Now;
                _result = _dbContext.SaveChanges() > 0;
            }

            return _result;
        }

        [HttpPost]
        [Route("save")]
        public int Save(BrandInfo info)
        {
            int _id = 0;

            if (!_dbContext.Brands.Where(x => x.Code == info.Code && x.IsActive).Any())
            {
                var _brand = new Brand() { Code = info.Code, Name = info.Name, IsActive = true, CreatedDate = DateTime.Now };
                _dbContext.Brands.Add(_brand);
                _dbContext.SaveChanges();
                _id = _brand.Id;
            }

            return _id;
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public bool Delete(int id)
        {
            bool _result = false;

            if (_dbContext.Brands.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _info = _dbContext.Brands.Where(x => x.Id == id && x.IsActive).FirstOrDefault();
                _info.IsActive = false;
                _info.UpdatedDate = DateTime.Now;
                _result = _dbContext.SaveChanges() > 0;
            }

            return _result;
        }

    }
}
