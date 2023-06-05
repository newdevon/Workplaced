using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;

namespace EmployeeManagementServices.Pages.Employees
{
    public class IndexModel : PageModel
    {
        public List<EmployeeInfo> listEmployees = new List<EmployeeInfo>();

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
					String sql = "SELECT * FROM Employee";
                    // Create SQL Command
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
						// Use SQL Reader
						using (SqlDataReader reader = command.ExecuteReader()) 
                        {
							// While there are objects to read save this data into an EmployeeInfo Object
							while (reader.Read())
                            {
								EmployeeInfo employeeInfo = new EmployeeInfo();
                                employeeInfo.id = "" + reader.GetInt32(0);
                                employeeInfo.first_name = reader.GetString(1);
                                employeeInfo.last_name = reader.GetString(2);
                                employeeInfo.email = reader.GetString(3);
                                employeeInfo.address_id = "" + reader.GetInt32(4);

                                // Add Object to List
                                listEmployees.Add(employeeInfo);
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

    public class EmployeeInfo
    {
        public String id;
        public String first_name;
        public String last_name;
        public String email;
        public String address_id;
    }
}
