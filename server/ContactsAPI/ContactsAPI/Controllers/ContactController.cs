using ContactsAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Mail;

namespace ContactsAPI.Controllers
{
    public class ContactController : ApiController
    {
        public HttpResponseMessage Get()
        {
            DataTable dataTable = new DataTable();
            string query = @"select IdNumber, FullName, Email, BirthDate, Gender, PhoneNum from Contacts";

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ContactsDB"].ConnectionString))
            using (var command = new SqlCommand(query, connection))
            using (var dataAdapter = new SqlDataAdapter(command))
            {
                command.CommandType = CommandType.Text;
                dataAdapter.Fill(dataTable);
            }

            return Request.CreateResponse(HttpStatusCode.OK, dataTable);
        }

        public HttpResponseMessage Post(Contact contact)
        {
            try
            {
                DataTable dataTable = new DataTable();

                if (!ValidateFields(contact)) throw Exception();

                string query = @"insert into Contacts (IdNumber, FullName, Email, BirthDate, Gender, PhoneNum)
                values('" + contact.IdNumber + @"',
                '" + contact.FullName + @"',
                '" + contact.Email + @"',
                '" + contact.BirthDate + @"',
                '" + contact.Gender + @"',
                '" + contact.PhoneNum + @"')";

                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ContactsDB"].ConnectionString))
                using (var command = new SqlCommand(query, connection))
                using (var dataAdapter = new SqlDataAdapter(command))
                {
                    command.CommandType = CommandType.Text;
                    dataAdapter.Fill(dataTable);
                }

                return Request.CreateResponse(HttpStatusCode.OK, "Added Successfully");
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Parameters");
            }
        }

        public HttpResponseMessage Put(Contact contact)
        {
            try
            {
                DataTable dataTable = new DataTable();

                if (!ValidateFields(contact)) throw Exception();

                string query = @"update Contacts set
                 FullName = '" + contact.FullName + @"',
                 Email = '" + contact.Email + @"',
                 BirthDate = '" + contact.BirthDate + @"',
                 Gender = '" + contact.Gender + @"',
                 PhoneNum = '" + contact.PhoneNum + @"'
                where IdNumber = " + contact.IdNumber + @"";


                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ContactsDB"].ConnectionString))
                using (var command = new SqlCommand(query, connection))
                using (var dataAdapter = new SqlDataAdapter(command))
                {
                    command.CommandType = CommandType.Text;
                    dataAdapter.Fill(dataTable);
                }

                return Request.CreateResponse(HttpStatusCode.OK, "Updated Successfully");
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Parameters");
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                DataTable dataTable = new DataTable();
                string query = @"delete from Contacts where IdNumber = " + id;
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ContactsDB"].ConnectionString))
                using (var command = new SqlCommand(query, connection))
                using (var dataAdapter = new SqlDataAdapter(command))
                {
                    command.CommandType = CommandType.Text;
                    dataAdapter.Fill(dataTable);
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Deleted Successfully");
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Parameters");
            }
        }


        private Exception Exception()
        {
            throw new NotImplementedException();
        }

        // validate contact fields
        bool ValidateFields(Contact contact)
        {
            long res = 0;
            if (!long.TryParse(contact.IdNumber, out res)) return false;
            if (!long.TryParse(contact.PhoneNum, out res)) return false;
            var mail = new System.Net.Mail.MailAddress(contact.Email);
            if (mail.Address != contact.Email) return false;
            return true;
        }
    }
}
