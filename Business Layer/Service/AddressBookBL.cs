using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.Interface;
using Repository_Layer.Entity;
using Repository_Layer.Service;
using Repository_Layer.Interface;
using Model_Layer.Model;
using Model_Layer.DTO;
using Repository_Layer.Hashing;

namespace Business_Layer.Service
{
    public class AddressBookBL:IAddressBookBL
    {
        private readonly IAddressBookRL _addressBookRL;
        private readonly JwtService jwtService;

        public AddressBookBL(IAddressBookRL addressBookRL, JwtService jwtService)
        {
            _addressBookRL = addressBookRL;
            this.jwtService = jwtService;
        }

        public string Register(UserDTO userDto)
        {
            if (_addressBookRL.GetUserByEmail(userDto.Email) != null)
                return "User already exists";

            var user = new UserModel
            {
                Name = userDto.Name,
                Email = userDto.Email,
                PasswordHash = PasswordHashing.HashPassword(userDto.Password)
            };

            _addressBookRL.Register(user);
            return "User registered successfully";
        }

        public string? Login(UserDTO userDto)
        {
            var userEntity = _addressBookRL.GetUserEntityByEmail(userDto.Email);
            if (userEntity == null || !PasswordHashing.VerifyPassword(userDto.Password, userEntity.PasswordHash))
                return null; // Invalid login

            return jwtService.GenerateToken(userEntity);
        }

        public List<AddressBookEntity> GetAllContacts()
        {
            return _addressBookRL.GetAllContacts();
        }

        public AddressBookEntity GetContactById(int id)
        {
            return _addressBookRL.GetContactById(id);
        }

        public AddressBookEntity AddContact(AddressBookEntity contact)
        {
            if (string.IsNullOrEmpty(contact.Name) || string.IsNullOrEmpty(contact.Address))
            {
                return null; // Validation failed
            }
            return _addressBookRL.AddContact(contact);
        }

        public AddressBookEntity UpdateContact(int id, AddressBookEntity contact)
        {
            return _addressBookRL.UpdateContact(id, contact);
        }

        public bool DeleteContact(int id)
        {
            return _addressBookRL.DeleteContact(id);
        }

       
    }
}
