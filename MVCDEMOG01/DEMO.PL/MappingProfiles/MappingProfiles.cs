using AutoMapper;
using Demo.DAL.Entities;
using DEMO.PL.Models;

namespace DEMO.PL.MappingProfiles
{
	public class MappingProfiles:Profile
	{
        public MappingProfiles()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }
    }
}
