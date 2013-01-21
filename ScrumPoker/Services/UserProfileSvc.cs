using System.Collections.Generic;
using System.Data;
using System.Linq;
using ScrumPoker.Models;
namespace ScrumPoker.Services
{
    public class UserProfileSvc
    {
        private ScrumPoker.Entities _entitiesDb = new Entities();
        private UsersContext _db = new UsersContext();

        public UserProfileSvc(Entities entitiesDb, UsersContext db)
        {
            _entitiesDb = entitiesDb;
            _db = db;
        }
        public List<UserProfile> GetAll()
        {
            List<UserProfile> profilesWithRoles;
         
                List<UserProfile> userProfiles = _db.UserProfiles.ToList();
                var Rolesquery = (from u in userProfiles


                                  select new UserProfile
                                  {
                                      UserId = u.UserId,
                                      UserName = u.UserName,
                                      Roles = ((from ur in _entitiesDb.UsersInRoles
                                                where ur.UserId == u.UserId
                                                select ur.webpages_Roles).ToList<Role>())
                                  }).ToList<UserProfile>();

                profilesWithRoles = Rolesquery;

            
            return profilesWithRoles;
        }

        public UserProfile Find(int id)
        {

            List<UserProfile> userProfiles = _db.UserProfiles.ToList();
            var Rolesquery = (from u in userProfiles
                                        where u.UserId == id
                                select new UserProfile
                                {
                                    UserId = u.UserId,
                                    UserName = u.UserName,
                                    Roles = ((from ur in _entitiesDb.UsersInRoles
                                            where ur.UserId == u.UserId
                                            select ur.webpages_Roles).ToList<Role>())
                                }).ToList<UserProfile>();

            return Rolesquery.FirstOrDefault<UserProfile>();
            
        }

        public bool InsertUserRole(int userId, int roleId)
        {

            UsersInRoles newUserRole = new UsersInRoles();
            newUserRole.UserId = userId;
            newUserRole.RoleId = roleId;
            _entitiesDb.UsersInRoles.Add(newUserRole);
            _entitiesDb.SaveChanges();
            return true;
            
        }

        public void DeleteUserRole(int userId, int roleId)
        {

            UsersInRoles userRole = (from ur in _entitiesDb.UsersInRoles
                            where ur.UserId == userId
                            && ur.RoleId == roleId
                            select ur).FirstOrDefault();

            _entitiesDb.Entry(userRole).State = EntityState.Deleted;
            _entitiesDb.SaveChanges();
            
        }

        public bool IsInRole(UserProfile userProfile, string roleName)
        {
            if (userProfile != null && !string.IsNullOrEmpty(roleName))
            {
                UserProfile currentProfile = Find(userProfile.UserId);

                var query = (from r in currentProfile.Roles
                             where r.RoleName == roleName
                             select r).FirstOrDefault();

                if (query != null)
                {
                    return true;
                }
            }
            return false;
        }


        public void Remove(int id)
        {
                UserProfile userprofile = _db.UserProfiles.Find(id);

                var userRoles = (from ur in _entitiesDb.UsersInRoles
                                where ur.UserId == userprofile.UserId
                                select ur).ToList();
                foreach (UsersInRoles userRole in userRoles)
                {
                    _entitiesDb.Entry(userRole).State = EntityState.Deleted;                       
                }

                _entitiesDb.SaveChanges();
                _db.UserProfiles.Remove(userprofile);
                _db.SaveChanges();
                
        }

        public static bool IsInRole(string userName, string roleName)
        {
            using (UsersContext db =  new UsersContext())
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
                                                  join r in entitiesDb.Roles1 on ur.RoleId equals r.RoleId
                                                  where ur.UserId == u.UserId
                                                  && r.RoleName.Equals(roleName, System.StringComparison.OrdinalIgnoreCase)
                                                  select ur.webpages_Roles).ToList<Role>())
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