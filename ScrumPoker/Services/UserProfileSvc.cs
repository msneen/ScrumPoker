using System.Collections.Generic;
using System.Data;
using System.Linq;
using ScrumPoker.Models;
namespace ScrumPoker.Services
{
    public class UserProfileSvc
    {
        public List<UserProfile> GetAll()
        {
            List<UserProfile> profilesWithRoles;
         
            using(UsersContext db = new UsersContext())
            using (ScrumPoker.Entities entitiesDb = new Entities())
            {
                List<UserProfile> userProfiles = db.UserProfiles.ToList();
                var Rolesquery = (from u in userProfiles


                                  select new UserProfile
                                  {
                                      UserId = u.UserId,
                                      UserName = u.UserName,
                                      Roles = ((from ur in entitiesDb.webpages_UsersInRoles
                                                where ur.UserId == u.UserId
                                                select ur.webpages_Roles).ToList<webpages_Roles>())
                                  }).ToList<UserProfile>();

                profilesWithRoles = Rolesquery;

            }
            return profilesWithRoles;
        }

        public UserProfile Find(int id)
        {
            using (UsersContext db = new UsersContext())
            using (ScrumPoker.Entities entitiesDb = new Entities())
            {
                List<UserProfile> userProfiles = db.UserProfiles.ToList();
                var Rolesquery = (from u in userProfiles
                                            where u.UserId == id
                                  select new UserProfile
                                  {
                                      UserId = u.UserId,
                                      UserName = u.UserName,
                                      Roles = ((from ur in entitiesDb.webpages_UsersInRoles
                                                where ur.UserId == u.UserId
                                                select ur.webpages_Roles).ToList<webpages_Roles>())
                                  }).ToList<UserProfile>();

                return Rolesquery.FirstOrDefault<UserProfile>();
            }
        }

        public bool InsertUserRole(int userId, int roleId)
        {
            using (ScrumPoker.Entities entitiesDb = new Entities())
            {
                webpages_UsersInRoles newUserRole = new webpages_UsersInRoles();
                newUserRole.UserId = userId;
                newUserRole.RoleId = roleId;
                entitiesDb.webpages_UsersInRoles.Add(newUserRole);
                entitiesDb.SaveChanges();
                return true;
            }
        }

        public void DeleteUserRole(int userId, int roleId)
        {
            using (ScrumPoker.Entities entitiesDb = new Entities())
            {
                webpages_UsersInRoles userRole = (from ur in entitiesDb.webpages_UsersInRoles
                               where ur.UserId == userId
                               && ur.RoleId == roleId
                               select ur).FirstOrDefault();

                entitiesDb.Entry(userRole).State = EntityState.Deleted;
                entitiesDb.SaveChanges();
            }
        }

        public bool IsInRole(UserProfile userProfile, string roleName)
        {
            UserProfile currentProfile = Find(userProfile.UserId);

            var query = (from r in currentProfile.Roles
                        where r.RoleName == roleName
                        select r).FirstOrDefault();

            if (query != null)
            {
                return true;
            }
            return false;
        }


        public void Remove(int id)
        {
                using(UsersContext db = new UsersContext())
                using (ScrumPoker.Entities entitiesDb = new Entities())
                {
                    UserProfile userprofile = db.UserProfiles.Find(id);

                    var userRoles = (from ur in entitiesDb.webpages_UsersInRoles
                                    where ur.UserId == userprofile.UserId
                                    select ur).ToList();
                    foreach (webpages_UsersInRoles userRole in userRoles)
                    {
                        entitiesDb.Entry(userRole).State = EntityState.Deleted;                       
                    }

                    entitiesDb.SaveChanges();
                    db.UserProfiles.Remove(userprofile);
                    db.SaveChanges();
                }
        }
    }    
}