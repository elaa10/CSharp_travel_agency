
namespace clientWinFormApp
{
    partial class LoginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
            loginButton = new System.Windows.Forms.Button();
            helloLabel = new System.Windows.Forms.Label();
            userTextBox = new System.Windows.Forms.TextBox();
            passwordTextBox = new System.Windows.Forms.TextBox();
            SuspendLayout();
            // 
            // loginButton
            // 
            loginButton.Location = new System.Drawing.Point(141, 244);
            loginButton.Name = "loginButton";
            loginButton.Size = new System.Drawing.Size(75, 32);
            loginButton.TabIndex = 2;
            loginButton.Text = "Login";
            loginButton.UseVisualStyleBackColor = true;
            loginButton.Click += loginButton_Click;
            // 
            // helloLabel
            // 
            helloLabel.AutoSize = true;
            helloLabel.Font = new System.Drawing.Font("Segoe UI", 24F);
            helloLabel.Location = new System.Drawing.Point(6, 40);
            helloLabel.Name = "helloLabel";
            helloLabel.Size = new System.Drawing.Size(341, 54);
            helloLabel.TabIndex = 1;
            helloLabel.Text = "Agentie de turism";
            helloLabel.Click += helloLabel_Click;
            // 
            // userTextBox
            // 
            userTextBox.Location = new System.Drawing.Point(113, 137);
            userTextBox.Name = "userTextBox";
            userTextBox.PlaceholderText = "Username";
            userTextBox.Size = new System.Drawing.Size(134, 27);
            userTextBox.TabIndex = 0;
            // 
            // passwordTextBox
            // 
            passwordTextBox.Location = new System.Drawing.Point(113, 194);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PlaceholderText = "Password";
            passwordTextBox.Size = new System.Drawing.Size(134, 27);
            passwordTextBox.TabIndex = 1;
            passwordTextBox.UseSystemPasswordChar = true;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(348, 343);
            Controls.Add(passwordTextBox);
            Controls.Add(userTextBox);
            Controls.Add(helloLabel);
            Controls.Add(loginButton);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Text = "Hello!";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Label helloLabel;
        private System.Windows.Forms.TextBox userTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
    }
}
