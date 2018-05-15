using ArbaShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbaShop.DAL.Repos
{
    public class LogRepo
    {
        private ArbaShopDbEntities db;

        public LogRepo()
        {
            db = new ArbaShopDbEntities();
        }

        public IQueryable<Log> GetAll()
        {
            return db.Logs;
        }

        public void Create(Log log)
        {
            db.Logs.Add(log);
            db.SaveChanges();
        }

        public void Delete(int Id)
        {
            db.Logs.Remove(db.Logs.Find(Id));
            db.SaveChanges();
        }

        public Log GetById(int Id)
        {
            return db.Logs.Find(Id);
        }

        public void Update(Log log)
        {
            db.Logs.Remove(db.Logs.Find(log.Id));
            db.Logs.Add(log);
            db.SaveChanges();
        }

    }
}
