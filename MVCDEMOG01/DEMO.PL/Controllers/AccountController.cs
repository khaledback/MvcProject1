using Demo.DAL.Entities;
using DEMO.PL.Helper;
using DEMO.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DEMO.PL.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}
		#region SignUp
		public IActionResult SignUP()
		{
			return View(new RegisterViewModel());
		}
		[HttpPost]
		public async Task<IActionResult> SignUp(RegisterViewModel registerViewModel)
		{
			if(ModelState.IsValid)
			{
				var user = new ApplicationUser
				{
					Email = registerViewModel.Email,
					UserName = registerViewModel.Email.Split('@')[0],
					IsAgree = registerViewModel.IsAgree
				};
			var result=	await _userManager.CreateAsync(user,registerViewModel.Password);
				if(result.Succeeded)
				return RedirectToAction("SignIn");
				foreach(var error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);
			}
			return View(registerViewModel);
		}
		#endregion
		#region SignIn
		public IActionResult SignIn()
		{
			return View(new SignInViewModel());
		}
		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel signInViewModel)
		{
			if(ModelState.IsValid)
			{
				var user =await  _userManager.FindByEmailAsync(signInViewModel.Email);
				if (user is null)
					ModelState.AddModelError("", "Email Does Not Exist");
				var isCorrectPassword = await _userManager.CheckPasswordAsync(user, signInViewModel.Password);
				if (isCorrectPassword)
				{
					var result = await _signInManager.PasswordSignInAsync(user, signInViewModel.Password,signInViewModel.RememberMe,false);
					if (result.Succeeded)
						return RedirectToAction("Index", "Home");
				}
				
			}
			return View(signInViewModel);
		}
		#endregion
		public async Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(SignIn));
		}
		#region ForgetPassword
		public IActionResult ForgetPassword()
		{
			return View(new ForgetPasswordViewModel());
		}
		[HttpPost]
		public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
		{
			if(ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user != null)
				{
					var token =await _userManager.GeneratePasswordResetTokenAsync(user);
					var resetPasswordLink=Url.Action("ResetPassword","Account",new {Email=model.Email, Token=token},Request.Scheme);
					var email = new Email
					{
						Title = "Reset Password",
						Body = resetPasswordLink,
						To = model.Email
					};
					EmailSettings.SendEmail(email);
					return RedirectToAction("CompleteForgetPassword");
				}
				ModelState.AddModelError("", "Invalid Email");
			}
			return View(model);
		}
		public IActionResult CompleteForgetPassword()
		{
			return View();
		}
		#endregion
		public IActionResult ResetPassword(string email,string token)
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user != null)
				{
					var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
					if (result.Succeeded)
						return RedirectToAction(nameof(SignIn));
					foreach (var error in result.Errors)
						ModelState.AddModelError(string.Empty, error.Description); 
				}
			}
			return View(model);
		}
        public IActionResult AccessDenied()
        {
			return View();

    }
    }
	
}
