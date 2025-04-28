using System.ComponentModel;
using model;
using services;


namespace clientWinFormApp;

public partial class LoginForm : Form
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    private readonly IServices server;
    public LoginForm(IServices server)
    {
        InitializeComponent();
        this.server = server;
    }
    
    private void loginButton_Click(object sender, EventArgs e)
    {
        if(userTextBox.Text == "" || passwordTextBox.Text == "")
        {
            MessageBox.Show("Please fill in all fields");
            return;
        }
        string usernameText = userTextBox.Text;
        string password = passwordTextBox.Text;
        
        SoftUser softUser = new SoftUser(usernameText, password);

        try
        {   
            Console.WriteLine("Creez MainView");
            MainForm mainView = new MainForm(softUser, server);
            Console.WriteLine("Acum login");
            server.Login(softUser, mainView);
            mainView.loadTrips(); 
            mainView.Show();
            this.Hide();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            userTextBox.Clear();
            passwordTextBox.Clear();
            return;
        }
    }
}