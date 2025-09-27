using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment_12._3._2.Models;

namespace Assignment_12._3._2.Data
{
    public class PetContext : DbContext
    {
        public DbSet<Pet> PetList { get; set; }

        public PetContext(DbContextOptions<PetContext> options) : base(options) 
        { 
            
        } 
    }
}
