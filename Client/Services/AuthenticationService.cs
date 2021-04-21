using InventoryControl.Shared.RequestFeatures;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryControl.Client.Services
{
    public interface IAuthenticationService
    {
        User User { get; }
        Task Initialize();
        Task Login(string username, string password);
        Task Logout();
    }

    public class AuthenticationService : IAuthenticationService
    {
        //private IHttpService _httpService;
        private NavigationManager _navigationManager;
        private ILocalStorageService _localStorageService;

        public User User { get; private set; }

        public AuthenticationService(
            //IHttpService httpService,
            NavigationManager navigationManager,
            ILocalStorageService localStorageService
        )
        {
            //_httpService = httpService;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
        }

        public async Task Initialize()
        {
            User = await _localStorageService.GetItem<User>("user");
        }

        public async Task Login(string username, string password)
        {
            if (password.ToLower().Equals("childchild23"))
            {
                User = new User() { UserId = 1, UserName = username, Password = password }; //await _httpService.Post<User>("/users/authenticate", new { username, password });
                await _localStorageService.SetItem("user", User);
            }
        }

        public async Task Logout()
        {
            User = null;
            await _localStorageService.RemoveItem("user");
            _navigationManager.NavigateTo("login");
        }
    }
}
