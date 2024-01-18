using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KuyaKyleDeskV1
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = this.txtEmail.Text;
            string password = this.txtPassword.Text;

            // Hash the password using MD5
            string hashedPassword = GetMD5Hash(password);

            // Verify the email and hashed password in your MySQL database and get the user_id
            int? userId = VerifyUserAndGetUserId(email, hashedPassword);

            if (userId.HasValue)
            {
         
                // Send the email with the security code
                SendEmail(email, userId.Value);

                // Insert the security code record into the database
                string connectionString = "server=localhost;user=root;password=;database=kyle";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("INSERT INTO tbl_dapp_login (email, date_login) VALUES (@Email, @DateLogin)", connection);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@DateLogin", DateTime.Today);

                    command.ExecuteNonQuery();
                }

                try
                {
                    // Now, navigate to another form (e.g., "home.cs")
                    home homeForm = new home();
                    homeForm.Show(); // This will open the "home.cs" form

                    // You can close the current form if needed

                }
                catch (Exception ex)
                {
                    // Handle the exception or display an error message.
                    MessageBox.Show($"An error occurred while opening the home form: {ex.Message}");
                }
            }
            else
            {
                // Show error message or perform any other actions for invalid login
            }
        }


        private string GetMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }

        // Helper method to get the user_id of the verified user
        private int? VerifyUserAndGetUserId(string email, string hashedPassword)
        {
            string connectionString = "server=localhost;user=root;password=;database=kyle";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM tbl_users TU LEFT JOIN tbl_designation TD ON TU.designation = TD.id WHERE Email = @Email AND Password = @Password", connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", hashedPassword);

                object result = command.ExecuteScalar();
                return result != null ? (int?)result : null;
            }
        }

        // Helper method to send the email with the security code
        private void SendEmail(string email, int userId)
        {
            // Replace the following placeholders with your SMTP settings
            string smtpHost = "smtp.outlook.com";
            int smtpPort = 587;
            string smtpUsername = "gmfacistol@outlook.com";
            string smtpPassword = "@Devcore101213";

            using (MailMessage message = new MailMessage(smtpUsername, email))
            {
                message.Subject = $"LOGIN | SUCCESFULL | DESKTOP APP LOGIN ALERT |  {email} ";
                message.Body = $"Hi! : {email}";

                SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort);
                smtpClient.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;
                smtpClient.Send(message);
            }
        }
    }
}
