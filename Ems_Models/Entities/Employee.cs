using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems_Models.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }

        public string? CiviIld {  get; set; }
        public string? FileNumber { get; set; } 
        public string? FullName { get; set; }
        public string? JobTitle { get; set; }
        public string? JobDescription { get; set;}
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Photo { get; set; }
        public string? Other { get; set; }
        public string? City { get; set; } = null;
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }

        //Relatioship : Many to One
        public GeneralDepartment? GeneralDepartment { get; set; }
        public int GeneralDepartmetId { get; set; }

        public Department? Department { get; set; }
        public int DepartmetId { get;set; }
        public Branch? Branch { get; set; }
        public int BranchId { get; set; }
        public Town? Town { get; set; }
        public int TownId { get; set;}
    }
}
