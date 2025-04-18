﻿using E_Ticket_System.Models;
using E_Ticket_System.Repositries.Irepostries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Ticket_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]


    public class UserController : Controller
    {
        private readonly IApplicationUserReposatory _UserReposatory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPendingTicketRepository _pendingTicketRepository;
        public UserController(IApplicationUserReposatory userReposatory, UserManager<ApplicationUser> userManager, IPendingTicketRepository pendingTicketRepository)
        {
            this._UserReposatory = userReposatory;
            this._userManager = userManager;
            this._pendingTicketRepository = pendingTicketRepository;
        }
        public IActionResult Index(string? query, int page = 1)
        {

            IEnumerable<ApplicationUser> users = _UserReposatory.Get();

            if (query != null)
            {
                users = _UserReposatory.Get(e => e.UserName.Contains(query) || e.Email.Contains(query));
            }
            var totalpages = Math.Ceiling((decimal)users.ToList().Count() / 5);
            if (totalpages < page - 1)
            {
                return RedirectToAction("Index", "Home", new { area = "Customer" });

            }
            users = users.Skip((page - 1) * 5).Take(5);
            ViewBag.totalpages = totalpages;
            return View(users.ToList());
        }
        public async Task<IActionResult> Delete(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            var currentuser = _userManager.GetUserId(User);
            if (UserId == currentuser)
            {
                TempData["Error"] = "You cannot delete your own account!";
                return RedirectToAction("Index", "User", new { area = "Admin" });
            }
            if (user != null)
            { 
                var pendingticket=_pendingTicketRepository.Get(e => e.UserId == user.Id);
                if (pendingticket != null)
                {
                    foreach (var item in pendingticket)
                    {
                        _pendingTicketRepository.Delete(item);
                    }
                    _pendingTicketRepository.comit();
                }
                var deleteuser = _UserReposatory.GetOne(e => e.Id == user.Id);
                _UserReposatory.Delete(deleteuser);
                _UserReposatory.comit();
            }
            return RedirectToAction("Index", "User", new { area = "Admin" });
        }
        public async Task<IActionResult> Block(string UserId)
        {   
            var user = await _userManager.FindByIdAsync(UserId);
            var currentuser = _userManager.GetUserId(User);
            if (UserId == currentuser)
            {
                TempData["Error"] = "You cannot Block your own account!";
                return RedirectToAction("Index", "User", new { area = "Admin" });
            }
            if (user != null)
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddDays(30));
                TempData["Notification"] = $"User {user.UserName} has been blocked for 30 days.";
            }
            return RedirectToAction("Index", "User", new { area = "Admin" });
        }
        public async Task<IActionResult> Unblock(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            var currentuser = _userManager.GetUserId(User);
            var result = await _userManager.SetLockoutEndDateAsync(user, null);

            if (result.Succeeded)
            {
                TempData["Success"] = "User has been unblocked successfully!";
                return RedirectToAction("Index", "User", new { area = "Admin" });

            }
            else
            {
                TempData["Failed"] = "Failed to unblock user!";
                return RedirectToAction("Index", "User", new { area = "Admin" });

            }

        }


    }
}