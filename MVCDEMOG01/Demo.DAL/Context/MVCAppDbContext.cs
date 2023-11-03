using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Demo.DAL.Context
{
	public class MVCAppDbContext :IdentityDbContext<ApplicationUser, ApplicationRole ,string>
	{
		public MVCAppDbContext(DbContextOptions<MVCAppDbContext> options): base(options)
			{
			
			}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			
		}
		public DbSet<Department> Departments { get; set; }
		public DbSet<Employee> Employees { get; set; }
	}
}
