using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly LittleJoyContext _context;

        public BlogRepository(LittleJoyContext context) 
        {
            _context = context;
        }
    }
}
