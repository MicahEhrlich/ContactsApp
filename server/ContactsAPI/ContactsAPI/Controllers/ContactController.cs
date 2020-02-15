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
using System.Web.Http.Cors;

namespace ContactsAPI.Controllers
{
    public class ContactController : ApiController
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public HttpResponseMessage Get()
        {
            DataTable dataTable = new DataTable();
            string query = @"SELECT IdNumber, FullName, Email, BirthDate, Gender, PhoneNum FROM Contacts";

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

                string query = "INSERT INTO Contacts (IdNumber ,FullName ,Email, BirthDate, Gender, PhoneNum) " +
                    "VALUES (@IdNumber, @FullName, @Email, @BirthDate, @Gender, @PhoneNum)";
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ContactsDB"].ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@IdNumber", contact.IdNumber);
                    command.Parameters.AddWithValue("@FullName", contact.FullName);
                    command.Parameters.AddWithValue("@Email", contact.Email);
                    command.Parameters.AddWithValue("@BirthDate", contact.BirthDate);
                    command.Parameters.AddWithValue("@Gender", contact.Gender);
                    command.Parameters.AddWithValue("@PhoneNum", contact.PhoneNum);
                    command.ExecuteNonQuery();
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

                string query = @"UPDATE Contacts SET
                 FullName = @FullName, " +
                 "Email = @Email, " +
                 "BirthDate = @BirthDate, " +
                 "Gender = @Gender, " +
                 "PhoneNum = @PhoneNum " +
                "WHERE IdNumber = @IdNumber";

                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ContactsDB"].ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@IdNumber", contact.IdNumber);
                    command.Parameters.AddWithValue("@FullName", contact.FullName);
                    command.Parameters.AddWithValue("@Email", contact.Email);
                    command.Parameters.AddWithValue("@BirthDate", contact.BirthDate);
                    command.Parameters.AddWithValue("@Gender", contact.Gender);
                    command.Parameters.AddWithValue("@PhoneNum", contact.PhoneNum);
                    command.ExecuteNonQuery();
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
                string query = @"DELETE from Contacts WHERE IdNumber = @IdNumber";
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ContactsDB"].ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@IdNumber", id);
                    command.ExecuteNonQuery();
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
            if (contact.PhoneNum != null && contact.PhoneNum.Length > 0 && !long.TryParse(contact.PhoneNum, out res)) return false;
            if (contact.Email != null && contact.Email.Length > 0)
            {
                var mail = new System.Net.Mail.MailAddress(contact.Email);
                if (mail.Address != contact.Email) return false;
            }
            return true;
        }
    }
}
