using System.ComponentModel;

namespace client;

partial class MainForm
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
        label3 = new System.Windows.Forms.Label();
        label4 = new System.Windows.Forms.Label();
        txtAttraction = new System.Windows.Forms.TextBox();
        txtStrat = new System.Windows.Forms.TextBox();
        txtEnd = new System.Windows.Forms.TextBox();
        dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
        searchResultsTable = new System.Windows.Forms.DataGridView();
        label5 = new System.Windows.Forms.Label();
        label6 = new System.Windows.Forms.Label();
        label7 = new System.Windows.Forms.Label();
        label8 = new System.Windows.Forms.Label();
        txtName = new System.Windows.Forms.TextBox();
        txtPhone = new System.Windows.Forms.TextBox();
        txtNr = new System.Windows.Forms.TextBox();
        btnSearch = new System.Windows.Forms.Button();
        btnReserve = new System.Windows.Forms.Button();
        tripsTable = new System.Windows.Forms.DataGridView();
        btnLogout = new System.Windows.Forms.Button();
        ((System.ComponentModel.ISupportInitialize)searchResultsTable).BeginInit();
        ((System.ComponentModel.ISupportInitialize)tripsTable).BeginInit();
        SuspendLayout();
        // 
        // label1
        // 
        label1.Location = new System.Drawing.Point(31, 31);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(126, 23);
        label1.TabIndex = 0;
        label1.Text = "TouristAttraction";
        // 
        // label2
        // 
        label2.Location = new System.Drawing.Point(31, 74);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(100, 23);
        label2.TabIndex = 1;
        label2.Text = "StartHour";
        // 
        // label3
        // 
        label3.Location = new System.Drawing.Point(31, 116);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(100, 23);
        label3.TabIndex = 2;
        label3.Text = "EndHour";
        // 
        // label4
        // 
        label4.Location = new System.Drawing.Point(31, 155);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(100, 23);
        label4.TabIndex = 3;
        label4.Text = "Date";
        // 
        // txtAttraction
        // 
        txtAttraction.Location = new System.Drawing.Point(180, 27);
        txtAttraction.Name = "txtAttraction";
        txtAttraction.Size = new System.Drawing.Size(157, 27);
        txtAttraction.TabIndex = 4;
        // 
        // txtStrat
        // 
        txtStrat.Location = new System.Drawing.Point(180, 70);
        txtStrat.Name = "txtStrat";
        txtStrat.Size = new System.Drawing.Size(157, 27);
        txtStrat.TabIndex = 5;
        // 
        // txtEnd
        // 
        txtEnd.Location = new System.Drawing.Point(180, 116);
        txtEnd.Name = "txtEnd";
        txtEnd.Size = new System.Drawing.Size(157, 27);
        txtEnd.TabIndex = 6;
        // 
        // dateTimePicker1
        // 
        dateTimePicker1.Location = new System.Drawing.Point(137, 155);
        dateTimePicker1.Name = "dateTimePicker1";
        dateTimePicker1.Size = new System.Drawing.Size(200, 27);
        dateTimePicker1.TabIndex = 7;
        // 
        // searchResultsTable
        // 
        searchResultsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        searchResultsTable.Location = new System.Drawing.Point(417, 27);
        searchResultsTable.Name = "searchResultsTable";
        searchResultsTable.RowHeadersWidth = 51;
        searchResultsTable.Size = new System.Drawing.Size(566, 218);
        searchResultsTable.TabIndex = 8;
        searchResultsTable.Text = "dataGridView1";
        // 
        // label5
        // 
        label5.Location = new System.Drawing.Point(31, 283);
        label5.Name = "label5";
        label5.Size = new System.Drawing.Size(100, 23);
        label5.TabIndex = 9;
        label5.Text = "ClientName";
        // 
        // label6
        // 
        label6.Location = new System.Drawing.Point(31, 330);
        label6.Name = "label6";
        label6.Size = new System.Drawing.Size(100, 23);
        label6.TabIndex = 10;
        label6.Text = "ClientPhone";
        // 
        // label7
        // 
        label7.Location = new System.Drawing.Point(50, 353);
        label7.Name = "label7";
        label7.Size = new System.Drawing.Size(8, 8);
        label7.TabIndex = 11;
        label7.Text = "NumberOfTickets";
        // 
        // label8
        // 
        label8.AutoSize = true;
        label8.Location = new System.Drawing.Point(31, 368);
        label8.Name = "label8";
        label8.Size = new System.Drawing.Size(124, 20);
        label8.TabIndex = 12;
        label8.Text = "NumberOfTickets";
        // 
        // txtName
        // 
        txtName.Location = new System.Drawing.Point(180, 283);
        txtName.Name = "txtName";
        txtName.Size = new System.Drawing.Size(125, 27);
        txtName.TabIndex = 13;
        // 
        // txtPhone
        // 
        txtPhone.Location = new System.Drawing.Point(180, 326);
        txtPhone.Name = "txtPhone";
        txtPhone.Size = new System.Drawing.Size(125, 27);
        txtPhone.TabIndex = 14;
        // 
        // txtNr
        // 
        txtNr.Location = new System.Drawing.Point(180, 368);
        txtNr.Name = "txtNr";
        txtNr.Size = new System.Drawing.Size(125, 27);
        txtNr.TabIndex = 15;
        // 
        // btnSearch
        // 
        btnSearch.Font = new System.Drawing.Font("Segoe UI", 12F);
        btnSearch.Location = new System.Drawing.Point(86, 203);
        btnSearch.Name = "btnSearch";
        btnSearch.Size = new System.Drawing.Size(133, 42);
        btnSearch.TabIndex = 16;
        btnSearch.Text = "SearchTrips";
        btnSearch.UseVisualStyleBackColor = true;
        btnSearch.Click += btnSearch_Click;
        // 
        // btnReserve
        // 
        btnReserve.Font = new System.Drawing.Font("Segoe UI", 12F);
        btnReserve.Location = new System.Drawing.Point(125, 414);
        btnReserve.Name = "btnReserve";
        btnReserve.Size = new System.Drawing.Size(94, 48);
        btnReserve.TabIndex = 17;
        btnReserve.Text = "Reserve";
        btnReserve.UseVisualStyleBackColor = true;
        btnReserve.Click += btnReserve_Click;
        // 
        // tripsTable
        // 
        tripsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        tripsTable.Location = new System.Drawing.Point(417, 283);
        tripsTable.Name = "tripsTable";
        tripsTable.RowHeadersWidth = 51;
        tripsTable.Size = new System.Drawing.Size(566, 242);
        tripsTable.TabIndex = 18;
        // 
        // btnLogout
        // 
        btnLogout.Font = new System.Drawing.Font("Segoe UI", 12F);
        btnLogout.Location = new System.Drawing.Point(11, 472);
        btnLogout.Name = "btnLogout";
        btnLogout.Size = new System.Drawing.Size(94, 40);
        btnLogout.TabIndex = 19;
        btnLogout.Text = "Logout";
        btnLogout.UseVisualStyleBackColor = true;
        btnLogout.Click += btnLogout_Click;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1000, 531);
        Controls.Add(btnLogout);
        Controls.Add(tripsTable);
        Controls.Add(btnReserve);
        Controls.Add(btnSearch);
        Controls.Add(txtNr);
        Controls.Add(txtPhone);
        Controls.Add(txtName);
        Controls.Add(label8);
        Controls.Add(label7);
        Controls.Add(label6);
        Controls.Add(label5);
        Controls.Add(searchResultsTable);
        Controls.Add(dateTimePicker1);
        Controls.Add(txtEnd);
        Controls.Add(txtStrat);
        Controls.Add(txtAttraction);
        Controls.Add(label4);
        Controls.Add(label3);
        Controls.Add(label2);
        Controls.Add(label1);
        Text = "Incercare";
        //Load += MainForm_Load;
        ((System.ComponentModel.ISupportInitialize)searchResultsTable).EndInit();
        ((System.ComponentModel.ISupportInitialize)tripsTable).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;

    private System.Windows.Forms.TextBox txtAttraction;
    private System.Windows.Forms.TextBox txtStrat;
    private System.Windows.Forms.TextBox txtEnd;
    private System.Windows.Forms.DateTimePicker dateTimePicker1;
    private System.Windows.Forms.DataGridView searchResultsTable;

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;

    #endregion

    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.TextBox txtName;
    private System.Windows.Forms.TextBox txtPhone;
    private System.Windows.Forms.TextBox txtNr;
    private System.Windows.Forms.Button btnSearch;
    private System.Windows.Forms.Button btnReserve;
    private System.Windows.Forms.DataGridView tripsTable;
    private System.Windows.Forms.Button btnLogout;
}