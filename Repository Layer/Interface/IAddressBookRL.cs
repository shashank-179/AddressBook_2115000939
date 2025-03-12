using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository_Layer.Entity;

namespace Repository_Layer.Interface
{
    public interface IAddressBookRL
    {
        public List<AddressBookEntity> GetAllContacts();
        public AddressBookEntity GetContactById(int id);
        public AddressBookEntity AddContact(AddressBookEntity contact);
        public AddressBookEntity UpdateContact(int id, AddressBookEntity updatedContact);
        public bool DeleteContact(int id);

    }
}
