using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScrumPoker.Models;
using ScrumPoker.Services;
using ScrumPoker.ViewModels.UserManagement;
using WebMatrix.WebData;
using ScrumPoker.Filters;

namespace ScrumPoker.Controllers
{
    [Authorize(Roles = "SiteAdmin")]
    [InitializeSimpleMembership]
    public class UserManagementController : Controller
    {
        private UserProfileSvc _userProfileSvc;
        private RoleSvc _roleSvc;

        public UserManagementController()
        {
            _userProfileSvc = new UserProfileSvc();
            _roleSvc = new RoleSvc();
        }

        private UsersContext db = new UsersContext();

        //
        // GET: /UserManagement/

        public ActionResult Index()
        {
            List<UserProfile> profilesWithRoles = _userProfileSvc.GetAll();

            return View(profilesWithRoles); //View(db.UserProfiles.ToList());
        }



        //
        // GET: /UserManagement/Details/5

        public ActionResult Details(int id = 0)
        {
            UserProfile userprofile = _userProfileSvc.Find(id);   //db.UserProfiles.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // GET: /UserManagement/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /UserManagement/Create

        [HttpPost]
        public ActionResult Create(UserProfile userprofile)
        {
            if (ModelState.IsValid)
            {
                db.UserProfiles.Add(userprofile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userprofile);
        }

        //
        // GET: /UserManagement/Edit/5
        [Authorize(Roles="SiteAdmin")]
        public ActionResult Edit(int id = 0)
        {
            UserProfile userprofile = _userProfileSvc.Find(id); // db.UserProfiles.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }

            List< Role> roles =  _roleSvc.GetAll();
            UserProfileEdit viewModel = new UserProfileEdit();
            viewModel.UserProfile = userprofile;
            viewModel.Roles = roles;
            
            return View(viewModel);
        }

        //
        // POST: /UserManagement/Edit/5

        [HttpPost]
        public ActionResult Edit(UserProfile userprofile, FormCollection collection)
        {
            WebSecurity.RequireRoles("SiteAdmin");
            //Do I need to change the incoming param to UserProfileEdit and then get the UserProfile from it?
            if (ModelState.IsValid)
            {
                List<Role> roles = _roleSvc.GetAll();
                List<Role> originalRoles = _userProfileSvc.Find(userprofile.UserId).Roles;
                var Query = (from r in roles                                   
                            select new
                            {
                                RoleName = r.RoleName,
                                RoleId = r.RoleId,
                                IsChecked = (
                                                    collection["Role_" + r.RoleName].Contains("true") ? true : false
                                              ),
                                
                                WasChecked = (
                                                            ((from or in originalRoles 
                                                            where or.RoleId == r.RoleId 
                                                            select or).FirstOrDefault<Role>()) == null ? false : true
                                                        )
                            }).ToList();

                foreach(var role in Query)
                {
                    if(role.IsChecked != role.WasChecked)
                    {
                        if(role.IsChecked == true && role.WasChecked == false)
                        {
                            //insert new role
                            _userProfileSvc.InsertUserRole(userprofile.UserId, role.RoleId);
                        }
                        else if(role.IsChecked == false && role.WasChecked == true)
                        {
                            //delete the role
                            _userProfileSvc.DeleteUserRole(userprofile.UserId, role.RoleId);
                        }
                    }
                }
                
                //collection["Role_" + 
                db.Entry(userprofile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userprofile);
        }

        //
        // GET: /UserManagement/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // POST: /UserManagement/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);           
            db.UserProfiles.Remove(userprofile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}