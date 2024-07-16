using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Interfaces;
using LumberStoreSystem.DataAccess.Model;
using LumberStoreSystem.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.BussinessLogic.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public async Task Add(EmployeeDTO employeeDTO)
        {
            var employee = new Employee
            {
                Id = new int(),
                FirstName = employeeDTO.FirstName,
                LastName = employeeDTO.LastName,
                Email = employeeDTO.Email,
                Password = employeeDTO.Password,
                DateOfBirth = employeeDTO.DateOfBirth,
                PhoneNumber = employeeDTO.PhoneNumber,
                AddressId = employeeDTO.AddressId,
                UserRole = DataAccess.Model.Enummerations.UserRole.Employee,
                Image = employeeDTO.Image,
                Role = employeeDTO.Role,
                UMCN = employeeDTO.UMCN

            };
            await _employeeRepository.Add(employee);
        }

        public async Task Delete(int id)
        {
            var employee = await _employeeRepository.GetById(id);
            await _employeeRepository.Delete(employee);
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _employeeRepository.GetAll();
        }

        public async Task<Employee> GetById(int id)
        {
            return await _employeeRepository.GetById(id);
        }

        public async Task Update(EmployeeDTO employeeDTO)
        {
            var employee = new Employee
            {
                Id = employeeDTO.Id,
                FirstName = employeeDTO.FirstName,
                LastName = employeeDTO.LastName,
                Email = employeeDTO.Email,
                Password = employeeDTO.Password,
                DateOfBirth = employeeDTO.DateOfBirth,
                PhoneNumber = employeeDTO.PhoneNumber,
                AddressId = employeeDTO.AddressId,
                UserRole = 0,
                Image = employeeDTO.Image,
                Role = employeeDTO.Role,
                UMCN = employeeDTO.UMCN
            };
            await _employeeRepository.Update(employee);
        }
    }
}
