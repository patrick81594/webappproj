
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testproj.Models;

namespace testproj.Data
{
    public class DataContexT : Microsoft.EntityFrameworkCore.DbContext
    {
        public DataContexT(DbContextOptions<DataContexT> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
    }
}