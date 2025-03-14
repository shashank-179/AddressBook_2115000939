using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository_Layer.Context;
using Repository_Layer.Entity;
using Repository_Layer.Interface;
using Model_Layer.Model;
using Model_Layer.DTO;
using Microsoft.EntityFrameworkCore;
using Repository_Layer.Hashing;

namespace Repository_Layer.Service
{
    public class AddressBookRL: IAddressBookRL
    {
        private readonly AddressBookDbContext addressBookDbContext;

        public AddressBookRL(AddressBookDbContext addressBookDbContext)
        {
            this.addressBookDbContext=  addressBookDbContext;
        }

        public void Register(UserModel userModel)
        {
            // Map UserModel to UserEntity before saving
            var userEntity = new UserEntity
            {
                Name = userModel.Name,
                Email = userModel.Email,
                PasswordHash = userModel.PasswordHash
            };

            addressBookDbContext.Users.Add(userEntity);
            addressBookDbContext.SaveChanges();
        }
        public UserModel? GetUserByEmail(string email)
        {
            var userEntity = addressBookDbContext.Users.FirstOrDefault(u => u.Email == email);
            if (userEntity == null) return null;

            return new UserModel
            {
                Id = userEntity.Id,
                Name = userEntity.Name,
                Email = userEntity.Email,
                PasswordHash = userEntity.PasswordHash
            };
        }

        public UserEntity? GetUserEntityByEmail(string email)
        {
            return addressBookDbContext.Users.FirstOrDefault(u => u.Email == email);
        }

        // Get all contacts
        public List<AddressBookEntity> GetAllContacts()
        {
            return addressBookDbContext.AddressBook.ToList();
        }

        // Get contact by ID
        public AddressBookEntity GetContactById(int id)
        {
            return addressBookDbContext.AddressBook.FirstOrDefault(c => c.Id == id);
        }

        // Add a new contact
        public AddressBookEntity AddContact(AddressBookEntity contact)
        {
            contact.Id = 0;
            addressBookDbContext.AddressBook.Add(contact);
            addressBookDbContext.SaveChanges();
            return contact;
        }

        // Update a contact
        public AddressBookEntity UpdateContact(int id, AddressBookEntity updatedContact)
        {
            var existingContact = addressBookDbContext.AddressBook.FirstOrDefault(c => c.Id == id);
            if (existingContact != null)
            {

                existingContact.Name = updatedContact.Name;
                existingContact.Email = updatedContact.Email;
                existingContact.Address = updatedContact.Address;

                addressBookDbContext.SaveChanges();
            }
            return existingContact;
        }


        // Delete a contact
        public bool DeleteContact(int id)
        {
            var contact = addressBookDbContext.AddressBook.FirstOrDefault(c => c.Id == id);
            if (contact != null)
            {
                addressBookDbContext.AddressBook.Remove(contact);
                addressBookDbContext.SaveChanges();
                return true;
            }
            return false;
        }


    }
}
