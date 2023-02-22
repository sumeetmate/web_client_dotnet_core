using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebClient_For_Microservice.Models.Accounts;

namespace WebClient_For_Microservice.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<CognitoUser> signInManager;
        private readonly UserManager<CognitoUser> userManger;
        private readonly CognitoUserPool pool;

        public AccountController(SignInManager<CognitoUser> signInManager, UserManager<CognitoUser> userManger, CognitoUserPool pool)
        {
            this.signInManager = signInManager;
            this.userManger = userManger;
            this.pool = pool;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            var model = new SignupModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignupModel model)
        {
            if (ModelState.IsValid)
            {
                var user = pool.GetUser(model.Email);
                if (user.Status != null)
                {
                    ModelState.AddModelError("User Exists", "user with thie email already exists.");
                    return View(model);
                }

                user.Attributes.Add("Name", model.Email);
                var new_user = await userManger.CreateAsync(user, model.Password).ConfigureAwait(false);

                if (new_user.Succeeded)
                    RedirectToAction("confirm");
            }
            return View();
        }


        public IActionResult Confirm()
        {
            var model = new ConfirmModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(ConfirmModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await this.userManger.FindByEmailAsync(model.Email).ConfigureAwait(false);
                if (user == null)
                {
                    ModelState.AddModelError("Not Found ", "Not found");
                    return View(model);
                }

                var result = await this.userManger.ConfirmEmailAsync(user, model.Code);
                if (result.Succeeded)
                    return RedirectToAction("Index", "HOme");
                else
                {
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Login(LoginModel model)
        {
            return View(model);
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> Login_Post(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                    ModelState.AddModelError("LoginError", "Invlaid Credentials");
            }
            return View("Login", model);
        }
    }
}
