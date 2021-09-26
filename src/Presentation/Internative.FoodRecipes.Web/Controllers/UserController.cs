using Internative.FoodRecipes.Application.Common.Interfaces;
using Internative.FoodRecipes.Infrastructure.Identity;
using Internative.FoodRecipes.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Web.Controllers
{
    [Authorize(Roles=RoleNames.User)]
    public class UserController : BasePublicController
    {
        #region Fields

        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;


        #endregion

        #region Ctor

        public UserController(ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        #endregion

        #region Methos

        [AllowAnonymous]
        public virtual async Task<IActionResult> Register(string returnUrl)
        {
            return View(new RegisterModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public virtual async Task<IActionResult> Register(RegisterModel model, string returnUrl)
        {
            if (_currentUserService.UserId > 0)
                return RedirectToAction("Index", "Home");

            // I were to use Fluent Validation but time was limited, so
            if(model.Password != model.ConfirmPassword)
                return View(new RegisterModel());

            if (ModelState.IsValid)
            {
                var username = model.Username?.Trim();

                await _identityService.CreateUserAsync(username, model.Password);

                return RedirectToAction("Index", "Home");
            }

            return View(new RegisterModel());
        }

        [AllowAnonymous]
        public virtual async Task<IActionResult> Login(string returnUrl)
        {
            return View(new LoginModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public virtual async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            if (_currentUserService.UserId > 0)
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var token = await _identityService.GetTokenAsync(model.Username, model.Password);

                if(token != null)
                {

                }

                return RedirectToAction("Index", "Home");
            }

            return View(new LoginModel());
        }

        #endregion
    }
}
