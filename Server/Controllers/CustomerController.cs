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
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        InventoryControlContext _dbContext;

        public CustomerController(ILogger<CustomerController> logger, InventoryControlContext inventoryControlContext)
        {
            _logger = logger;
            _dbContext = inventoryControlContext;
        }

        [HttpGet]
        [Route("get")]
        public List<CustomerInfo> Get()
        {
            List<CustomerInfo> _customers = new List<CustomerInfo>();
            var _list = _dbContext.Customers.Where(x => x.IsActive).ToList();
            _list.ForEach(x =>
            {
                CustomerInfo _info = new CustomerInfo()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    NickName = x.NickName,
                    AccountInformation = x.AccountInformation,
                    PhoneNo = x.PhoneNo,
                    Address = x.Address
                };
                _customers.Add(_info);
            });

            return _customers;
        }

        [HttpGet]
        [Route("getbyPage")]
        public CustomerList GetByPage([FromQuery] PageParameters parameters)
        {
            List<CustomerInfo> _customers = new List<CustomerInfo>();
            var _list = _dbContext.Customers.Where(x => x.IsActive).ToList();
            _list.ForEach(x =>
            {
                CustomerInfo _info = new CustomerInfo()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    NickName = x.NickName,
                    AccountInformation = x.AccountInformation,
                    PhoneNo = x.PhoneNo,
                    Address = x.Address
                };
                _customers.Add(_info);
            });

            var response = PagedList<CustomerInfo>.ToPagedList(_customers, parameters.PageNumber, parameters.PageSize);
            return new CustomerList() { Items = response.ToList(), Meta = response.MetaData };
            //return _customers;
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public CustomerInfo GetById(int id)
        {
            CustomerInfo _customer = null;

            if (_dbContext.Customers.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _info = _dbContext.Customers.Where(x => x.Id == id && x.IsActive).First();
                _customer = new CustomerInfo() 
                { 
                    Id = _info.Id,
                    Code = _info.Code,
                    Name = _info.Name,
                    NickName = _info.NickName,
                    Address = _info.Address,
                    AccountInformation = _info.AccountInformation,
                    PhoneNo = _info.PhoneNo
                };
            }

            return _customer;
        }

        [HttpGet]
        [Route("getbyname/{id}")]
        public List<CustomerInfo> GetByName(string name)
        {
            List<Customer> _list = null;
            List<CustomerInfo> _customers = new List<CustomerInfo>();

            if (string.IsNullOrEmpty(name))
            {
                _list = _dbContext.Customers.Where(x => x.IsActive).ToList();
            }
            else if (_dbContext.Customers.Where(x => x.IsActive && (x.NickName.ToUpper().Contains(name.ToUpper()) || x.Name.ToUpper().Contains(name.ToUpper()))).Any())
            {
                _list = _dbContext.Customers.Where(x => x.IsActive && (x.NickName.ToUpper().Contains(name.ToUpper()) || x.Name.ToUpper().Contains(name.ToUpper()))).ToList();
            }

            _list.ForEach(x =>
            {
                CustomerInfo _info = new CustomerInfo()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    NickName = x.NickName,
                    AccountInformation = x.AccountInformation,
                    PhoneNo = x.PhoneNo,
                    Address = x.Address
                };
                _customers.Add(_info);
            });

            return _customers;
        }

        [HttpPut]
        [Route("update")]
        public bool Update(CustomerInfo info)
        {
            bool _result = false;

            if (_dbContext.Customers.Where(x => x.Id == info.Id && x.IsActive).Any())
            {
                var _info = _dbContext.Customers.Where(x => x.Id == info.Id && x.IsActive).First();
                _info.Code = info.Code;
                _info.Name = info.Name;
                _info.NickName = info.NickName;
                _info.AccountInformation = info.AccountInformation;
                _info.Address = info.Address;
                _info.PhoneNo = info.PhoneNo;
                _info.UpdatedDate = DateTime.Now;
                _result = _dbContext.SaveChanges() > 0;
            }

            return _result;
        }

        [HttpPost]
        [Route("save")]
        public int Save(CustomerInfo info)
        {
            int _id = 0;
            var _customer = new Customer()
            {
                Code = info.Code,
                Name = info.Name,
                NickName = info.NickName,
                Address = info.Address,
                AccountInformation = info.AccountInformation,
                PhoneNo = info.PhoneNo,
                IsActive = true, 
                CreatedDate = DateTime.Now 
            };
            _dbContext.Customers.Add(_customer);
            _dbContext.SaveChanges();
            _id = _customer.Id;

            return _id;
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public bool Delete(int id)
        {
            bool _result = false;

            if (_dbContext.Customers.Where(x => x.Id == id && x.IsActive).Any())
            {
                var _info = _dbContext.Customers.Where(x => x.Id == id && x.IsActive).FirstOrDefault();
                _info.IsActive = false;
                _info.UpdatedDate = DateTime.Now;
                _result = _dbContext.SaveChanges() > 0;
            }

            return _result;
        }

    }
}
