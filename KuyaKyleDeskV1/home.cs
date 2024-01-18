using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KuyaKyleDeskV1
{
    public partial class home : Form
    {
      
        public home()
        {
            InitializeComponent();
            dataGridView1.Visible = true; // Set the DataGridView to be visible
            dataGridView3.Visible = true; // Set the DataGridView to be visible
            LoadDataGridView(); // Call a method to load data into the DataGridView
            LoadDataGridViewProduct();
            LoadDataGridView8();
            LoadDataGridView7();
            LoadDataGridView6();
            LoadDataGridView5();
            dataGridView1.CellClick += dataGridView1_CellClick;
            printButton.Click += printButton_Click;
            PopulateProductImages();
            timer1.Start();
        }



        private void LoadDataGridView()
        {
            // Connection string for your MySQL database
            string connectionString = "server=localhost;user=root;password=;database=kyle";

            // Create a connection to the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Create a SQL command to select all records from tbl_order
                    string query = "SELECT customer_id,amount,name FROM tbl_order";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    // Create a DataAdapter to fill a DataTable
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataTable to the DataGridView
                        dataGridView1.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void LoadDataGridViewProduct()
        {
            // Connection string for your MySQL database
            string connectionString = "server=localhost;user=root;password=;database=kyle";

            // Create a connection to the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Create a SQL command to select all records from tbl_order
                    string query = "SELECT * FROM tbl_product";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    // Create a DataAdapter to fill a DataTable
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataTable to the DataGridView
                        dataGridView3.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a cell in the ID column (assuming it's the first column) was clicked
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                // Get the value of the clicked cell (customer ID)
                int customer_id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());

                // Call a method to display customer details based on the customer ID
                DisplayCustomerDetails(customer_id);
            }
        }


        private void DisplayCustomerDetails(int customerId)
        {
            // Use the customerId to fetch the details of the selected customer
            // You can perform a database query to retrieve customer details
            // and then display them in another section of your form, e.g., labels or text boxes.
            // Example:
            string connectionString = "server=localhost;user=root;password=;database=kyle";

            // Create a connection to the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT TP.name,TOI.quantity,TP.price as ProductPrice,(TP.price * TOI.quantity) as Amount FROM tbl_order O LEFT JOIN tbl_order_item TOI ON O.id = TOI.order_id LEFT JOIN tbl_product TP ON TOI.product_id = TP.id WHERE O.customer_id = @CustomerId GROUP BY  TP.name";
                MySqlCommand commandFetch = new MySqlCommand(query, connection);
                commandFetch.Parameters.AddWithValue("@CustomerId", customerId);
                // Create a DataAdapter to fill a DataTable
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(commandFetch))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Bind the DataTable to the DataGridView
                    dataGridView2.DataSource = dataTable;
                }
            }
            // Execute the query, retrieve the details, and display them.
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(PrintPage);
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = pd;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
            }
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            // Define the content to be printed
            string contentToPrint = "Customer Details:\n\n";

            // Append the data from dataGridView2 to the content
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    contentToPrint += cell.Value.ToString() + "\t";
                }
                contentToPrint += "\n";
            }

            // Set up the font and location for printing
            Font font = new Font("Arial", 12);
            PointF location = new PointF(100, 100);

            // Print the content
            e.Graphics.DrawString(contentToPrint, font, Brushes.Black, location);
        }

     

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;password=;database=kyle";

            // Create a connection to the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                try
                {
                    connection.Open();

                    // Create a SQL command to search for products based on the criteria
                    string query = "SELECT customer_id,amount,name FROM tbl_order";
                    MySqlCommand command = new MySqlCommand(query, connection);
                   

                    // Create a DataAdapter to fill a DataTable
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataTable to dataGridView3
                        dataGridView1.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Update the time in the Label every second
            // Get the current date and time
            DateTime now = DateTime.Now;

            // Format the date and time as desired
            string formattedDateTime = $"{now:MMMM dd, yyyy} {now:hh:mm:ss tt} | {now:dddd}";

            // Update the Label with the formatted date and time
            lblTimer.Text = formattedDateTime;
        }



        private const string BaseImageUrl = "http://localhost/KYL/";

        private Random random = new Random(); // Initialize a Random object

        private int currentMemberId = -1;
        private void PopulateProductImages()
        {
            string connectionString = "server=localhost;user=root;password=;database=kyle";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM tbl_product";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            string productName = row["name"].ToString();
                            string relativeImagePath = row["image"].ToString();
                            int product_id = int.Parse(row["id"].ToString());

                            PictureBox productPictureBox = new PictureBox();
                            productPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                            productPictureBox.Size = new Size(100, 100);

                            string completeImageUrl = BaseImageUrl + relativeImagePath;
                            productPictureBox.ImageLocation = completeImageUrl;

                            productPictureBox.Click += (sender, e) =>
                            {
                                int memberId = GetMemberId();

                                if (IsMemberIdExists(memberId, product_id))
                                {
                                    UpdateMemberOrder(memberId, product_id);
                                }
                                else
                                {
                                    InsertMemberOrder(memberId, product_id);
                                }

                                DisplayOrders(memberId);
                            };

                            flowLayoutPanel1.Controls.Add(productPictureBox);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private int GetMemberId()
        {
            if (currentMemberId == -1)
            {
                // Generate a new memberId for a new session
                currentMemberId = random.Next(6666, 9999);
            }
            return currentMemberId;
        }

        private bool IsMemberIdExists(int memberId, int productId)
        {
            string connectionString = "server=localhost;user=root;password=;database=kyle";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM tbl_cart WHERE member_id = @MemberId AND product_id = @ProductId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@MemberId", memberId);
                command.Parameters.AddWithValue("@ProductId", productId);
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        private void InsertMemberOrder(int memberId, int productId)
        {
            string connectionString = "server=localhost;user=root;password=;database=kyle";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO tbl_cart (product_id, quantity, member_id) VALUES (@ProductId, 1, @MemberId)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductId", productId);
                command.Parameters.AddWithValue("@MemberId", memberId);
                command.ExecuteNonQuery();
            }
        }

        private void UpdateMemberOrder(int memberId, int productId)
        {
            string connectionString = "server=localhost;user=root;password=;database=kyle";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE tbl_cart SET quantity = quantity + 1 WHERE product_id = @ProductId AND member_id = @MemberId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductId", productId);
                command.Parameters.AddWithValue("@MemberId", memberId);
                command.ExecuteNonQuery();
            }
        }

        private void DisplayOrders(int memberId)
        {
            string connectionString = "server=localhost;user=root;password=;database=kyle";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT TP.name,TP.price,TC.quantity FROM tbl_cart TC LEFT JOIN tbl_product TP ON TC.product_id = TP.id WHERE TC.member_id = @MemberId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@MemberId", memberId);

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView4.DataSource = dataTable;
                }
            }
        }

        private string customerName = "";

        private void btnOrder_Click(object sender, EventArgs e)
        {
            // Check if a customer name is entered
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a customer name.");
                return;
            }

            // Set the customer name
            customerName = txtName.Text;

            // Insert the order into tbl_order
            int orderId = InsertOrder();

            // Insert order items into tbl_order_item
            InsertOrderItems(orderId);

            // Clear the cart (tbl_cart) for the current memberId
            ClearCart(currentMemberId);

            // Clear the customer name text box
            txtName.Text = "";

            // Display the updated cart (empty)
            DisplayOrders(currentMemberId);

            // Generate a new memberId for the next session
            currentMemberId = -1;
        }

        private int InsertOrder()
        {
            // Insert the order into tbl_order
            string connectionString = "server=localhost;user=root;password=;database=kyle";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO tbl_order (customer_id, amount, name, order_status, order_at) VALUES (@CustomerId, @TotalAmount, @Name, 'PENDING', NOW())";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerId", currentMemberId);
                command.Parameters.AddWithValue("@TotalAmount", CalculateTotalAmount(currentMemberId));
                command.Parameters.AddWithValue("@Name", customerName);
                command.ExecuteNonQuery();

                // Return the generated order ID
                return (int)command.LastInsertedId;
            }
        }

        private decimal CalculateTotalAmount(int memberId)
        {
            // Calculate the total amount based on the cart items
            string connectionString = "server=localhost;user=root;password=;database=kyle";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT SUM(TP.price * TC.quantity) FROM tbl_cart TC LEFT JOIN tbl_product TP ON TC.product_id = TP.id WHERE TC.member_id = @MemberId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@MemberId", memberId);
                object result = command.ExecuteScalar();

                if (result != DBNull.Value)
                {
                    return Convert.ToDecimal(result);
                }
                else
                {
                    return 0;
                }
            }
        }

        private void InsertOrderItems(int orderId)
        {
            // Insert order items into tbl_order_item
            string connectionString = "server=localhost;user=root;password=;database=kyle";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO tbl_order_item (order_id, product_id, item_price, quantity) SELECT @OrderId, TC.product_id, TP.price, TC.quantity FROM tbl_cart TC LEFT JOIN tbl_product TP ON TC.product_id = TP.id WHERE TC.member_id = @MemberId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@OrderId", orderId);
                command.Parameters.AddWithValue("@MemberId", currentMemberId);
                command.ExecuteNonQuery();
            }
        }

        private void ClearCart(int memberId)
        {
            // Clear the cart for the current memberId
            string connectionString = "server=localhost;user=root;password=;database=kyle";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM tbl_cart WHERE member_id = @MemberId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@MemberId", memberId);
                command.ExecuteNonQuery();
            }
        }

        private int selectedCustomerId = -1;
        private void btnProceed_Click(object sender, EventArgs e)
        {
            if (selectedCustomerId != -1)
            {
                // Update tbl_order to set order_status to 'PROCEED' for the selected customer
                string connectionString = "server=localhost;user=root;password=;database=kyle";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE tbl_order SET order_status = 'PROCEED' WHERE customer_id = @CustomerId";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CustomerId", selectedCustomerId);
                    command.ExecuteNonQuery();
                }

                // Reset the selected customer ID
                selectedCustomerId = -1;
            }
        }

        private void LoadDataGridView5()
        {
            // Connection string for your MySQL database
            string connectionString = "server=localhost;user=root;password=;database=kyle";

            // Create a connection to the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Create a SQL command to select all records from tbl_order
                    string query = "SELECT customer_id,amount,name FROM tbl_order WHERE order_status = 'PENDING'";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    // Create a DataAdapter to fill a DataTable
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataTable to the DataGridView
                        dataGridView5.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private int? selectedCustomerId5;

        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a cell in the customer_id column (assuming it's the first column) was clicked
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                // Get the value of the clicked cell (customer_id) as a string
                string customer_idString = dataGridView5.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                // Try to parse the string to an integer and store it in selectedCustomerId5
                if (int.TryParse(customer_idString, out int customerId))
                {
                    selectedCustomerId5 = customerId;
                }
                else
                {
                    MessageBox.Show("Invalid customer ID format.");
                    selectedCustomerId5 = null;
                }
            }
        }


        private void UpdateOrderStatus(int customer_id)
        {
            // Connection string for your MySQL database
            string connectionString = "server=localhost;user=root;password=;database=kyle";

            // Create a connection to the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Create an UPDATE query to set the order_status to 'IN-PROGRESS'
                    string updateQuery = "UPDATE tbl_order SET order_status = 'IN-PROGRESS' WHERE customer_id = @CustomerId";
                    MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@CustomerId", customer_id);

                    // Execute the UPDATE query
                    updateCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while updating the order status: {ex.Message}");
                }
            }
        }


        private void LoadDataGridView6()
        {
            // Connection string for your MySQL database
            string connectionString = "server=localhost;user=root;password=;database=kyle";

            // Create a connection to the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Create a SQL command to select all records from tbl_order
                    string query = "SELECT customer_id,amount,name FROM tbl_order WHERE order_status = 'IN-PROGRESS'";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    // Create a DataAdapter to fill a DataTable
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataTable to the DataGridView
                        dataGridView6.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void dataGridView6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a cell in the customer_id column (assuming it's the first column) was clicked
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                // Get the value of the clicked cell (customer_id)
                string customer_id = dataGridView5.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                // Update the order status to 'IN-PROGRESS' for the clicked customer_id
                UpdateOrderStatus6(customer_id);

                // Reload the DataGridView to reflect the updated data
                LoadDataGridView6();
            }
        }

        private void UpdateOrderStatus6(string customer_id)
        {
            // Connection string for your MySQL database
            string connectionString = "server=localhost;user=root;password=;database=kyle";

            // Create a connection to the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Create an UPDATE query to set the order_status to 'IN-PROGRESS'
                    string updateQuery = "UPDATE tbl_order SET order_status = 'COMPLETED' WHERE customer_id = @CustomerId";
                    MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@CustomerId", customer_id);

                    // Execute the UPDATE query
                    updateCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while updating the order status: {ex.Message}");
                }
            }
        }

        private void LoadDataGridView7()
        {
            // Connection string for your MySQL database
            string connectionString = "server=localhost;user=root;password=;database=kyle";

            // Create a connection to the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Create a SQL command to select all records from tbl_order
                    string query = "SELECT customer_id,amount,name FROM tbl_order WHERE order_status = 'COMPLETED'";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    // Create a DataAdapter to fill a DataTable
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataTable to the DataGridView
                        dataGridView7.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void dataGridView7_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a cell in the customer_id column (assuming it's the first column) was clicked
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                // Get the value of the clicked cell (customer_id)
                string customer_id = dataGridView5.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                // Update the order status to 'IN-PROGRESS' for the clicked customer_id
                UpdateOrderStatus7(customer_id);

                // Reload the DataGridView to reflect the updated data
                LoadDataGridView7();
            }
        }

        private void UpdateOrderStatus7(string customer_id)
        {
            // Connection string for your MySQL database
            string connectionString = "server=localhost;user=root;password=;database=kyle";

            // Create a connection to the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Create an UPDATE query to set the order_status to 'IN-PROGRESS'
                    string updateQuery = "UPDATE tbl_order SET order_status = 'CLAIMED' WHERE customer_id = @CustomerId";
                    MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@CustomerId", customer_id);

                    // Execute the UPDATE query
                    updateCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while updating the order status: {ex.Message}");
                }
            }
        }

        private void LoadDataGridView8()
        {
            // Connection string for your MySQL database
            string connectionString = "server=localhost;user=root;password=;database=kyle";

            // Create a connection to the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Create a SQL command to select all records from tbl_order
                    string query = "SELECT customer_id,amount,name FROM tbl_order WHERE order_status = 'CLAIMED'";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    // Create a DataAdapter to fill a DataTable
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataTable to the DataGridView
                        dataGridView8.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            // Get the search criteria (e.g., product name) from a TextBox or other input control
            string searchCriteria = txtSearch.Text; // Replace with your actual input control

            string connectionString = "server=localhost;user=root;password=;database=kyle";

            // Create a connection to the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Create a SQL command to search for products based on the criteria
                    string query = "SELECT * FROM tbl_product WHERE name LIKE @SearchCriteria";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SearchCriteria", "%" + searchCriteria + "%"); // Use '%' for partial matching

                    // Create a DataAdapter to fill a DataTable
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataTable to dataGridView3
                        dataGridView3.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            string searchCriteria = ""; // Replace with your actual input control

            string connectionString = "server=localhost;user=root;password=;database=kyle";

            // Create a connection to the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                try
                {
                    connection.Open();

                    // Create a SQL command to search for products based on the criteria
                    string query = "SELECT * FROM tbl_product WHERE name LIKE @SearchCriteria";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SearchCriteria", "%" + searchCriteria + "%"); // Use '%' for partial matching

                    // Create a DataAdapter to fill a DataTable
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataTable to dataGridView3
                        dataGridView3.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }



        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedCustomerId5.HasValue)
            {
                // Update the order status to 'IN-PROGRESS' for the selected customer_id
                UpdateOrderStatus(selectedCustomerId5.Value);

                // Reload the DataGridView to reflect the updated data
                LoadDataGridView5();
            }
            else
            {
                MessageBox.Show("Please select a valid customer ID first.");
            }
        }



        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
}
