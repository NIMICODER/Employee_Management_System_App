using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ems_Models.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }

        //Relationship : one to many
        [JsonIgnore]
        public List<Employee>? Employees { get; set; }
    }
}
