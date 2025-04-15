using System.ComponentModel;

namespace Agentie_turism_transport_csharp;

partial class LoginForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        label1 = new System.Windows.Forms.Label();
        label2 = new System.Windows.Forms.Label();
        txtUsername = new System.Windows.Forms.TextBox();
        txtPassword = new System.Windows.Forms.TextBox();
        btnLogin = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // label1
        // 
        label1.Font = new System.Drawing.Font("Segoe UI", 12F);
        label1.Location = new System.Drawing.Point(190, 141);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(100, 23);
        label1.TabIndex = 0;
        label1.Text = "Username";
        // 
        // label2
        // 
        label2.Font = new System.Drawing.Font("Segoe UI", 12F);
        label2.Location = new System.Drawing.Point(190, 209);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(100, 23);
        label2.TabIndex = 1;
        label2.Text = "Password";
        // 
        // txtUsername
        // 
        txtUsername.Location = new System.Drawing.Point(323, 141);
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new System.Drawing.Size(142, 27);
        txtUsername.TabIndex = 2;
        // 
        // txtPassword
        // 
        txtPassword.Location = new System.Drawing.Point(323, 209);
        txtPassword.Name = "txtPassword";
        txtPassword.Size = new System.Drawing.Size(142, 27);
        txtPassword.TabIndex = 3;
        // 
        // btnLogin
        // 
        btnLogin.Font = new System.Drawing.Font("Segoe UI", 14F);
        btnLogin.Location = new System.Drawing.Point(274, 279);
        btnLogin.Name = "btnLogin";
        btnLogin.Size = new System.Drawing.Size(110, 54);
        btnLogin.TabIndex = 4;
        btnLogin.Text = "Login";
        btnLogin.UseVisualStyleBackColor = true;
        btnLogin.Click += btnLogin_Click;
        // 
        // LoginForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(679, 450);
        Controls.Add(btnLogin);
        Controls.Add(txtPassword);
        Controls.Add(txtUsername);
        Controls.Add(label2);
        Controls.Add(label1);
        Text = "LoginForm";
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Button btnLogin;

    #endregion
}