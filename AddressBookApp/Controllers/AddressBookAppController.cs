using AutoMapper;
using Business_Layer.Interface;
using Business_Layer.Service;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Model_Layer.DTO;
using Model_Layer.Model;
using Repository_Layer.Entity;

namespace AddressBookApp.Controllers
{
    [ApiController]
    [Route("api/addressbook")]
    public class AddressBookAppController : ControllerBase
    {
        private readonly IAddressBookBL _addressBookBL;
        private readonly IMapper _mapper;
        private readonly IValidator<AddressBookDTO> _validator;

        public AddressBookAppController(IAddressBookBL addressBookBL, IMapper mapper, IValidator<AddressBookDTO> validator)
        {
            _addressBookBL = addressBookBL;
            _mapper = mapper;
            _validator = validator;
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
        public ActionResult<AddressBookDTO> AddContact([FromBody] AddressBookDTO contactDTO)
        {
            var validationResult = _validator.Validate(contactDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            
            var contactEntity = _mapper.Map<AddressBookEntity>(contactDTO);
            var createdContact = _addressBookBL.AddContact(contactEntity);
            var createdContactDTO = _mapper.Map<AddressBookDTO>(createdContact);

            return CreatedAtAction(nameof(GetContactById), new { id = createdContactDTO.Id }, createdContactDTO);
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
