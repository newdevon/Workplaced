using EmployeeManagementServices.Pages.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;

namespace EmployeeManagementServices.Pages.Addresses
{
    public class IndexModel : PageModel
    {
        public List<AddressInfo> listAddresses = new List<AddressInfo>();
        
        public void OnGet()
        {
			// Get SQL connection
			try
			{
				String connectionString = "Data Source=localhost;Initial Catalog=EMS;Integrated Security=True";

				// Create SQL connection
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "SELECT * FROM Address";
					// Create SQL Command
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						// Use SQL Reader
						using (SqlDataReader reader = command.ExecuteReader())
						{
							// While there are objects to read save this data into an EmployeeInfo Object
							while (reader.Read())
							{
								AddressInfo addressInfo = new AddressInfo();
								addressInfo.id = "" + reader.GetInt32(0);
								addressInfo.street = reader.GetString(1);
								addressInfo.city = reader.GetString(2);
								addressInfo.state = reader.GetString(3);
								addressInfo.zip_code = "" + reader.GetInt32(4);
								addressInfo.country = reader.GetString(5);

								// Add Object to List
								listAddresses.Add(addressInfo);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception: " + ex.ToString());
			}
		}
    }

    public class AddressInfo
    {
        public String id;
        public String street;
        public String city;
        public String state;
        public String zip_code;
        public String country;
    }
}
