using ArbaShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbaShop.DAL.Repos
{
    public class UserRepo
    {
        private ArbaShopDbEntities db;

        public UserRepo()
        {
            db = new ArbaShopDbEntities();
        }

        public IQueryable<User> GetAll()
        {
            return db.Users;
        }

        public void Create(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
        }

        public void Delete(int Id)
        {
            db.Users.Remove(db.Users.Find(Id));
            db.SaveChanges();
        }

        public User GetById(int Id)
        {
            return db.Users.Find(Id);
        }        

        public void Update(User user)
        {
            db.Users.Remove(db.Users.Find(user.Id));
            db.Users.Add(user);
            db.SaveChanges();
        }
    }
}
