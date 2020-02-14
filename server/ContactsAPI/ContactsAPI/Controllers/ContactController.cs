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

                long res = 0;
                if (!long.TryParse(contact.IdNumber,out res)) throw Exception();
                if (!long.TryParse(contact.PhoneNum, out res)) throw Exception();
                var mail = new System.Net.Mail.MailAddress(contact.Email);
                if (mail.Address != contact.Email) throw Exception();

                string query = @"insert into Contacts (IdNumber, FullName, Email, BirthDate, Gender, PhoneNum)
                values('" + contact.IdNumber+ @"',
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
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Parameters");
            }
        }

        private Exception Exception()
        {
            throw new NotImplementedException();
        }
    }
}
