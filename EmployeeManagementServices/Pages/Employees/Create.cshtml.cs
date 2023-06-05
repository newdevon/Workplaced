using EmployeeManagementServices.Pages.Addresses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Net.Security;

namespace EmployeeManagementServices.Pages.Employees
{
    public class CreateModel : PageModel
    {
		public AddressInfo addressInfo = new AddressInfo();
		public EmployeeInfo employeeInfo = new EmployeeInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost() 
        {
            addressInfo.street = Request.Form["street"];
			addressInfo.city = Request.Form["city"];
			addressInfo.state = Request.Form["state"];
            addressInfo.zip_code = Request.Form["zip_code"];
            addressInfo.country = Request.Form["country"];

			employeeInfo.first_name = Request.Form["first_name"];
			employeeInfo.last_name = Request.Form["last_name"];
			employeeInfo.email = Request.Form["email"];
			

            if (employeeInfo.first_name.Length == 0 || employeeInfo.last_name.Length == 0 || employeeInfo.email.Length == 0 || addressInfo.street.Length == 0 || addressInfo.city.Length == 0 || addressInfo.state.Length == 0 || addressInfo.zip_code.Length == 0 || addressInfo.country.Length == 0 )
            {
                errorMessage = "All the fields are required";
                return;
            }

			// Save new Employee into DB
            try
            {
				String connectionString = "Data Source=localhost;Initial Catalog=EMS;Integrated Security=True";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

                    // Insert SQL Address Command
                    String sql1 = "INSERT INTO Address " +
                                  "(street, city, state, zip_code, country) " +
                                  "OUTPUT INSERTED.ID " +
								  "VALUES (@street, @city, @state, @zip_code, @country)";

                    int address_id = 0;
                    using (SqlCommand command = new SqlCommand(sql1, connection))
                    {
                        command.Parameters.AddWithValue("@street", addressInfo.street);
						command.Parameters.AddWithValue("@city", addressInfo.city);
						command.Parameters.AddWithValue("@state", addressInfo.state);
						command.Parameters.AddWithValue("@zip_code", addressInfo.zip_code);
						command.Parameters.AddWithValue("@country", addressInfo.country);

                        address_id = (int)command.ExecuteScalar();
                        employeeInfo.address_id = address_id.ToString();
					}

					// Insert SQL Employee Command
					String sql2 = "INSERT INTO Employee " +
                                 "(first_name, last_name, email, address_id) VALUES " +
                                 "(@first_name, @last_name, @email, @address_id)";

					using (SqlCommand command = new SqlCommand(sql2, connection))
					{
						command.Parameters.AddWithValue("@first_name", employeeInfo.first_name);
                        command.Parameters.AddWithValue("@last_name", employeeInfo.last_name);
                        command.Parameters.AddWithValue("@email", employeeInfo.email);
                        command.Parameters.AddWithValue("@address_id", employeeInfo.address_id);

                        command.ExecuteNonQuery();

						if (connection.State == System.Data.ConnectionState.Open)
							connection.Close();
					}
				}
			}
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }


            // Reset and Empty form value after submit
			employeeInfo.first_name = "";
			employeeInfo.last_name = "";
			employeeInfo.email = "";
            employeeInfo.address_id = "";

            // After adding to DB
            successMessage = "New Employee Added Sucessfully";
            Response.Redirect("/Employees/Index");

		}
    }
}
