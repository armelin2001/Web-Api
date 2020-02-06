using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Api_dotnet.Models;
namespace Api_dotnet.Data
{
    public class AplicationDBContext:DbContext
    {
        public DbSet<Produto> produtos{get;set;}
        public AplicationDBContext(DbContextOptions<AplicationDBContext> options) : base(options){

        }
    }
}