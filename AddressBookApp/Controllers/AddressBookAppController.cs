using Business_Layer.Service;
using Microsoft.AspNetCore.Mvc;
using Repository_Layer.Entity;

namespace AddressBookApp.Controllers
{
    [ApiController]
    [Route("api/addressbook")]
    public class AddressBookAppController : ControllerBase
    {
        private readonly AddressBookBL _addressBookBL;

        public AddressBookAppController(AddressBookBL addressBookBL)
        {
            _addressBookBL = addressBookBL;
        }

        // GET: Fetch all contacts
        [HttpGet]
        public ActionResult<List<AddressBookEntity>> GetAllContacts()
        {
            return Ok(_addressBookBL.GetAllContacts());
        }

        // GET: Get contact by ID
        [HttpGet("{id}")]
        public ActionResult<AddressBookEntity> GetContactById(int id)
        {
            var contact = _addressBookBL.GetContactById(id);
            if (contact == null)
            {
                return NotFound("Contact not found.");
            }
            return Ok(contact);
        }

        // POST: Add a new contact
        [HttpPost]
        public ActionResult<AddressBookEntity> AddContact([FromBody] AddressBookEntity contact)
        {
            var createdContact = _addressBookBL.AddContact(contact);
            if (createdContact == null)
            {
                return BadRequest("Invalid contact details.");
            }
            return CreatedAtAction(nameof(GetContactById), new { id = createdContact.Id }, createdContact);
        }

        // PUT: Update contact
        [HttpPut("{id}")]
        public ActionResult<AddressBookEntity> UpdateContact(int id, [FromBody] AddressBookEntity contact)
        {
            var updatedContact = _addressBookBL.UpdateContact(id, contact);
            if (updatedContact == null)
            {
                return NotFound("Contact not found.");
            }
            return Ok(updatedContact);
        }

        // DELETE: Delete contact
        [HttpDelete("{id}")]
        public ActionResult DeleteContact(int id)
        {
            bool isDeleted = _addressBookBL.DeleteContact(id);
            if (!isDeleted)
            {
                return NotFound("Contact not found.");
            }
            return NoContent();
        }
    }
}
