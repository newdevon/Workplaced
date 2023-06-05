using EmployeeManagementServices.Pages.Addresses;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace EmployeeManagementServices.Pages.Employees
{
    public class EditModel : PageModel
    {
        public EmployeeInfo employeeInfo = new EmployeeInfo();
        public AddressInfo addressInfo = new AddressInfo();

        public String errorMessage = "";
        public String successMessage = "";

        // GET Method used to read ID and fill form for editing
        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
				String connectionString = "Data Source=localhost;Initial Catalog=EMS;Integrated Security=True";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					String sql1 = "SELECT * FROM Employee WHERE id=@id";
					using (SqlCommand command = new SqlCommand(sql1, connection))
					{
						// Replace id with Query Data "id"
						command.Parameters.AddWithValue("@id", id);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								employeeInfo.id = "" + reader.GetInt32(0);
								employeeInfo.first_name = reader.GetString(1);
								employeeInfo.last_name = reader.GetString(2);
								employeeInfo.email = reader.GetString(3);
								employeeInfo.address_id = "" + reader.GetInt32(4);
							}
						}
					}

					String sql2 = "SELECT * FROM Address WHERE id=@id";
					using (SqlCommand command = new SqlCommand(sql2, connection))
					{
						// Replace id with employeeInfo.address_id
						command.Parameters.AddWithValue("@id", employeeInfo.address_id);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								addressInfo.id = "" + reader.GetInt32(0);
								addressInfo.street = reader.GetString(1);
								addressInfo.city = reader.GetString(2);
								addressInfo.state = reader.GetString(3);
								addressInfo.zip_code = "" + reader.GetInt32(4);
								addressInfo.country = reader.GetString(5);
							}
						}
					}

					if (connection.State == System.Data.ConnectionState.Open)
						connection.Close();
				}
			}
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
			// employeeInfo.id needs to be retrieved from the form in order to update
			employeeInfo.id = Request.Form["id"];
			employeeInfo.first_name = Request.Form["first_name"];
			employeeInfo.last_name = Request.Form["last_name"];
			employeeInfo.email = Request.Form["email"];

			// Get id from form
			addressInfo.id = Request.Form["address_id"];
			addressInfo.street = Request.Form["street"];
			addressInfo.city = Request.Form["city"];
			addressInfo.state = Request.Form["state"];
			addressInfo.zip_code = Request.Form["zip_code"];
			addressInfo.country = Request.Form["country"];

			if (employeeInfo.first_name.Length == 0 || employeeInfo.last_name.Length == 0 || employeeInfo.email.Length == 0 || addressInfo.street.Length == 0 || addressInfo.city.Length == 0 || addressInfo.state.Length == 0 || addressInfo.zip_code.Length == 0 || addressInfo.country.Length == 0)
			{
				errorMessage = "All the fields are required";
				return;
			}

			try
			{
				String connectionString = "Data Source=localhost;Initial Catalog=EMS;Integrated Security=True";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					string sql1 = "UPDATE Employee " +
								  "SET first_name=@first_name, last_name=@last_name, email=@email " +
								  "WHERE id=@id";

					using (SqlCommand command = new SqlCommand(sql1, connection))
					{
						command.Parameters.AddWithValue("@first_name", employeeInfo.first_name);
						command.Parameters.AddWithValue("@last_name", employeeInfo.last_name);
						command.Parameters.AddWithValue("@email", employeeInfo.email);
						command.Parameters.AddWithValue("@id", employeeInfo.id);

						command.ExecuteNonQuery();
					}

					string sql2 = "UPDATE Address " +
								  "SET street=@street, city=@city, state=@state, zip_code=@zip_code, country=@country " +
								  "WHERE id=@id";

					using (SqlCommand command = new SqlCommand(sql2, connection))
					{
						command.Parameters.AddWithValue("@street", addressInfo.street);
						command.Parameters.AddWithValue("@city", addressInfo.city);
						command.Parameters.AddWithValue("@state", addressInfo.state);
						command.Parameters.AddWithValue("@zip_code", addressInfo.zip_code);
						command.Parameters.AddWithValue("@country", addressInfo.country);
						command.Parameters.AddWithValue("@id", addressInfo.id);

						command.ExecuteNonQuery();
					}

					if (connection.State == System.Data.ConnectionState.Open)
						connection.Close();
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}

			Response.Redirect("/Employees/Index");
		}
	}
}
