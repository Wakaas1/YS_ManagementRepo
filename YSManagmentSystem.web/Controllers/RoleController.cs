﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using YSManagmentSystem.BLL.User;
using YSManagmentSystem.BLL.UserRole;
using YSManagmentSystem.Domain.User;

namespace YSManagmentSystem.web.Controllers
{
   public class RoleController : Controller
    {
        private readonly IRoleServices _role;
        private readonly IUserServices _user;
        
        const string SessionId = "_Id";
       
        public RoleController(IRoleServices role, IUserServices user)
        {
            _role = role;
            _user = user;
        }

        //[Authorize(Roles = "Admin,Ceo")]
        public IActionResult Index(UserDetail uD)
        {
            var user = _user.GetAllUsers(uD);
            return View(user);
        }
        //[Authorize(Roles = "Admin,Ceo")]
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        //[Authorize(Roles = "Admin,Ceo")]
        [HttpPost]
        public IActionResult CreateRole(AppRole role)
        {
            //if (ModelState.IsValid)
            //{
                _role.AddRole(role);
            //}
            //else
            //{
            //    ModelState.AddModelError("", "Wrong Detail Add.");
            //}
            return View("Index");
        }

        //[Authorize(Roles = "Admin,Ceo")]
        public IActionResult EditRole(int uId)
        {            
            var user = _user.GetUserByID(uId).Email;
            ViewBag.Email = user.ToString();

            HttpContext.Session.SetInt32(SessionId, uId);
            
                return View(_role.GetAllRole(uId));
        }

        //[Authorize(Roles = "Admin,Ceo")]
        [HttpPost]
        public IActionResult EditRole(List<RoleEdit> RoleEdit)
        {
            long result = 0;
            int Status;
            string Value;
            int UId = (int)HttpContext.Session.GetInt32(SessionId);
            var roleChk = RoleEdit.Where(x => x.Checked == true);
            if (ModelState.IsValid)
            {
                foreach (var item in roleChk)
                {
                    if (item.Checked == true)
                    {
                        _role.RemoveRole(UId, item.RoleId);
                        result = _role.AddUserRole(UId, item.RoleId);
                    }
                };
                var roleUchk = RoleEdit.Where(x => x.Checked == false);
                foreach (var item in roleUchk)
                {
                    _role.RemoveRole(UId, item.RoleId);

                };
                if (result > 0)
                {
                    Status = 200;
                    Value = Url.Content("~/Design/View/");
                }
                else
                {
                    Status = 500;
                    Value = "There is some error at server side";
                }
            }
            else
            {
                Status = 500;
                Value = "There is some error at client side";
            }
            return Json(new { status = Status, value = Value }); 
        }

    }
}
