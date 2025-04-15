using client;
using services;

namespace Agentie_turism_transport_csharp;

public partial class LoginForm : Form
{
    private readonly IServices _service;

    public LoginForm(IServices service)
    {
        InitializeComponent();
        _service = service;
    }

    private void btnLogin_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text;
        string password = txtPassword.Text;

        var mainForm = new MainForm(_service); 
        var user = _service.Login(username, password, mainForm);
        if (user != null)
        {
            // MessageBox.Show("Login successful!");
            
            mainForm.Show();
            this.Hide(); 
        }
        else
        {
            MessageBox.Show("Invalid username or password.");
        }
    }
    
}