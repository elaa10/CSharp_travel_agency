using System;
using System.Windows.Forms;

namespace TravelAgency
{
    public partial class LoginForm : Form
    {
        private IService service;
        private MainForm mainForm;

        public LoginForm()
        {
            InitializeComponent();
            InitializeService();
        }

        private void InitializeService()
        {
            // Initialize your service here
            // This would be similar to your Java implementation
            service = new ServiceProxy("localhost", 55555);
        }

        private void InitializeComponent()
        {
            this.txtUsername = new TextBox();
            this.txtPassword = new TextBox();
            this.btnLogin = new Button();
            this.SuspendLayout();

            // txtUsername
            this.txtUsername.Location = new System.Drawing.Point(50, 38);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.PlaceholderText = "Username";
            this.txtUsername.Size = new System.Drawing.Size(142, 23);

            // txtPassword
            this.txtPassword.Location = new System.Drawing.Point(50, 82);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.PlaceholderText = "Password";
            this.txtPassword.Size = new System.Drawing.Size(142, 23);

            // btnLogin
            this.btnLogin.Location = new System.Drawing.Point(102, 128);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.Text = "Login";
            this.btnLogin.Click += new EventHandler(btnLogin_Click);

            // LoginForm
            this.ClientSize = new System.Drawing.Size(242, 168);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Name = "LoginForm";
            this.Text = "Authentication";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtPassword.Text))
            {
                try
                {
                    var user = await service.LoginAsync(txtUsername.Text, txtPassword.Text);
                    if (user != null)
                    {
                        mainForm = new MainForm(service, user);
                        mainForm.FormClosed += (s, args) => this.Close();
                        this.Hide();
                        mainForm.Show();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsername.Clear();
                    txtPassword.Clear();
                }
            }
            else
            {
                MessageBox.Show("Please fill in both fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
} 