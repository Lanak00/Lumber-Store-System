using LumberStoreSystem.DataAccess.Model.Enummerations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.DataAccess.Model
{

    public class Employee : User
    {
        public string UMCN { get; set; }
        public string? Image { get; set; } = null;
        public EmployeeRole Role { get; set; }
    }

}
