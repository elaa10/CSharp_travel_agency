using client;
using services;

namespace client;

public partial class LoginForm : Form
{
    private ProjectClientCtrl ctrl;
    public LoginForm(ProjectClientCtrl ctrl)
    {
        InitializeComponent();
        this.ctrl = ctrl;
    }

    private void loginButton_Click(object sender, EventArgs e)
    {
        string username = userTextBox.Text;
        string password = passwordTextBox.Text;

        try
        {
            ctrl.login(username, password);
            MainForm chatWin = new MainForm(ctrl);
            chatWin.Text = "Chat window for " + username;
            chatWin.Show();
            this.Hide();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "Login Error " + ex.Message/*+ex.StackTrace*/, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }
    
}