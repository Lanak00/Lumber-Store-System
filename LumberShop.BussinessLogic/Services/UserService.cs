﻿using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Interfaces;
using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.BussinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) 
        { 
            _userRepository = userRepository;
        }
        public async Task Add(UserDTO userDTO)
        {
            var user = new User
            {
                Id = new int(),
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                Password = userDTO.Password,
                DateOfBirth = userDTO.DateOfBirth,
                PhoneNumber = userDTO.PhoneNumber,
                AddressId = userDTO.AddressId,
                UserRole = userDTO.Role
                
            };
            await _userRepository.Add(user);
        }

        public async Task Delete(int id)
        {
            var user = await _userRepository.GetById(id);
            await _userRepository.Delete(user);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User> GetById(int id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task Update(UserDTO userDTO)
        {
            var user = new User
            {
                Id = userDTO.Id,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                Password = userDTO.Password,
                DateOfBirth = userDTO.DateOfBirth,
                PhoneNumber = userDTO.PhoneNumber,
                AddressId = userDTO.AddressId,
                UserRole = userDTO.Role
            };
            await _userRepository.Update(user);
        }
    }
}
