using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Models;
using FitnessTracker.Services;
using Microsoft.AspNetCore.Http;

namespace FitnessTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TempData["ErrorMessage"] = "Username and password are required.";
                return View();
            }

            var user = await _authService.ValidateLoginAsync(username, password);
            if (user != null)
            {
                // Store user info in session
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("FullName", user.FullName ?? user.Username);

                // Send OTP for verification
                var otpSent = await _authService.SendOTPAsync(user.MobileNumber, "Login");
                if (otpSent)
                {
                    TempData["SuccessMessage"] = $"OTP sent to {user.MobileNumber}";
                    return RedirectToAction("VerifyOTP", new { mobileNumber = user.MobileNumber, purpose = "Login" });
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to send OTP. Please try again.";
                    return View();
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid username or password.";
                return View();
            }
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user, string confirmPassword)
        {
            if (user.Password != confirmPassword)
            {
                TempData["ErrorMessage"] = "Passwords do not match.";
                return View(user);
            }

            // Check if username already exists
            if (await _authService.IsUsernameExistsAsync(user.Username))
            {
                TempData["ErrorMessage"] = "Username already exists.";
                return View(user);
            }

            // Check if email already exists
            if (await _authService.IsEmailExistsAsync(user.Email))
            {
                TempData["ErrorMessage"] = "Email already exists.";
                return View(user);
            }

            // Check if mobile number already exists
            if (await _authService.IsMobileExistsAsync(user.MobileNumber))
            {
                TempData["ErrorMessage"] = "Mobile number already exists.";
                return View(user);
            }

            var success = await _authService.RegisterUserAsync(user);
            if (success)
            {
                // Send OTP for verification
                var otpSent = await _authService.SendOTPAsync(user.MobileNumber, "Registration");
                if (otpSent)
                {
                    TempData["SuccessMessage"] = $"Registration successful! OTP sent to {user.MobileNumber}";
                    return RedirectToAction("VerifyOTP", new { mobileNumber = user.MobileNumber, purpose = "Registration" });
                }
                else
                {
                    TempData["ErrorMessage"] = "Registration successful but failed to send OTP. Please contact support.";
                    return RedirectToAction("Login");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Registration failed. Please try again.";
                return View(user);
            }
        }

        // GET: Account/VerifyOTP
        public IActionResult VerifyOTP(string mobileNumber, string purpose)
        {
            ViewBag.MobileNumber = mobileNumber;
            ViewBag.Purpose = purpose;
            return View();
        }

        // POST: Account/VerifyOTP
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyOTP(string mobileNumber, string purpose, string otpCode)
        {
            if (string.IsNullOrEmpty(otpCode))
            {
                TempData["ErrorMessage"] = "OTP code is required.";
                ViewBag.MobileNumber = mobileNumber;
                ViewBag.Purpose = purpose;
                return View();
            }

            var isValid = await _authService.ValidateOTPAsync(mobileNumber, otpCode, purpose);
            if (isValid)
            {
                if (purpose == "Login")
                {
                    // Get user and update last login
                    var user = await _authService.ValidateLoginAsync(mobileNumber, mobileNumber); // This is a simplified approach
                    if (user != null)
                    {
                        await _authService.UpdateLastLoginAsync(user.UserId);
                        TempData["SuccessMessage"] = "Login successful!";
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                else if (purpose == "Registration")
                {
                    TempData["SuccessMessage"] = "Registration verified successfully! You can now login.";
                    return RedirectToAction("Login");
                }

                TempData["SuccessMessage"] = "OTP verified successfully!";
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid OTP code. Please try again.";
                ViewBag.MobileNumber = mobileNumber;
                ViewBag.Purpose = purpose;
                return View();
            }
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Logged out successfully.";
            return RedirectToAction("Login");
        }

        // GET: Account/ForgotPassword
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Account/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string mobileNumber)
        {
            if (string.IsNullOrEmpty(mobileNumber))
            {
                TempData["ErrorMessage"] = "Mobile number is required.";
                return View();
            }

            var otpSent = await _authService.SendOTPAsync(mobileNumber, "PasswordReset");
            if (otpSent)
            {
                TempData["SuccessMessage"] = $"OTP sent to {mobileNumber}";
                return RedirectToAction("VerifyOTP", new { mobileNumber = mobileNumber, purpose = "PasswordReset" });
            }
            else
            {
                TempData["ErrorMessage"] = "Mobile number not found or failed to send OTP.";
                return View();
            }
        }
    }
} 