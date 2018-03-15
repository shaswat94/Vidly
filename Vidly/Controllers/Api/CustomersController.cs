using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using AutoMapper;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class CustomersController : ApiController
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        //GET /api/customers
        /// <summary>
        /// Method to return the list of customers to client
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetCustomers()
        {
            var customerDtos = _context.Customers
               .Include(c => c.MembershipType)
               .ToList()
               .Select(Mapper.Map<Customer, CustomerDto>);

            return Ok(customerDtos);
            //used as deegate, a referance to Mapper.Map<>(). Thus method braces are removed
        }

        //Get /api/customer/1
        /// <summary>
        /// Returns the details of a particular customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CustomerDto GetCustomer(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return Mapper.Map<Customer, CustomerDto>(customer);
        }

        //Post /api/customer
        [HttpPost]
        public IHttpActionResult CreateCustomer(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var customer = Mapper.Map<CustomerDto, Customer>(customerDto);
            _context.Customers.Add(customer);
            _context.SaveChanges();

            customerDto.Id = customer.Id;
            return Created(new Uri(Request.RequestUri+"/"+customer.Id), customerDto);
        }

        //PUT /api/customer/1
        /// <summary>
        /// Method to update the details of a customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customer"></param>
        [HttpPut]
        public void UpdateCustomer(int id, CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customerInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Mapper.Map(customerDto, customerInDb); 
            //Compiler automatically knows the type of arguments and copies values from source to target

            _context.SaveChanges();
        }

        //DELETE /api/customer/1
        /// <summary>
        /// Method to delete a customer from Database
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public void DeleteCustomer(int id)
        {
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customerInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Customers.Remove(customerInDb);
            _context.SaveChanges();
        }
    }
}
