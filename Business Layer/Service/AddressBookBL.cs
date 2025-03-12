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

namespace Business_Layer.Service
{
    public class AddressBookBL:IAddressBookBL
    {
        private readonly IAddressBookRL _addressBookRL;

        public AddressBookBL(IAddressBookRL addressBookRL)
        {
            _addressBookRL = addressBookRL;
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
