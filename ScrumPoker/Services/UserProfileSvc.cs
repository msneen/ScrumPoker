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
                                      Roles = ((from ur in entitiesDb.UsersInRoles
                                                where ur.UserId == u.UserId
                                                select ur.webpages_Roles).ToList<Roles>())
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
                                      Roles = ((from ur in entitiesDb.UsersInRoles
                                                where ur.UserId == u.UserId
                                                select ur.webpages_Roles).ToList<Roles>())
                                  }).ToList<UserProfile>();

                return Rolesquery.FirstOrDefault<UserProfile>();
            }
        }

        public bool InsertUserRole(int userId, int roleId)
        {
            using (ScrumPoker.Entities entitiesDb = new Entities())
            {
                UsersInRoles newUserRole = new UsersInRoles();
                newUserRole.UserId = userId;
                newUserRole.RoleId = roleId;
                entitiesDb.UsersInRoles.Add(newUserRole);
                entitiesDb.SaveChanges();
                return true;
            }
        }

        public void DeleteUserRole(int userId, int roleId)
        {
            using (ScrumPoker.Entities entitiesDb = new Entities())
            {
                UsersInRoles userRole = (from ur in entitiesDb.UsersInRoles
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

                    var userRoles = (from ur in entitiesDb.UsersInRoles
                                    where ur.UserId == userprofile.UserId
                                    select ur).ToList();
                    foreach (UsersInRoles userRole in userRoles)
                    {
                        entitiesDb.Entry(userRole).State = EntityState.Deleted;                       
                    }

                    entitiesDb.SaveChanges();
                    db.UserProfiles.Remove(userprofile);
                    db.SaveChanges();
                }
        }

        public static bool IsInRole(string userName, string roleName)
        {
            using (UsersContext db = new UsersContext())
            using (ScrumPoker.Entities entitiesDb = new Entities())
            {

                List<UserProfile> userProfiles = db.UserProfiles.ToList();
                UserProfile user = (from u in userProfiles
                                    where u.UserName.Equals(userName, System.StringComparison.OrdinalIgnoreCase)
                                    select new UserProfile
                                    {
                                        UserId = u.UserId,
                                        UserName = u.UserName,
                                        Roles = ((from ur in entitiesDb.UsersInRoles
                                                  join r in entitiesDb.Roles on ur.RoleId equals r.RoleId
                                                  where ur.UserId == u.UserId
                                                  && r.RoleName.Equals(roleName, System.StringComparison.OrdinalIgnoreCase)
                                                  select ur.webpages_Roles).ToList<Roles>())
                                    }).FirstOrDefault<UserProfile>();

                if (user != null && user.Roles.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }    
}