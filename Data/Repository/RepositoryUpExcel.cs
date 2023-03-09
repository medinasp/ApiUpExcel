using Data.Config;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class RepositoryUpExcel : RepositoryGenerics<UpExcel>, IUpExcel
    {
        public RepositoryUpExcel(DbContextOptions<ContextBase> optionsBuilder) : base(optionsBuilder)
        {

        }
    }
}
