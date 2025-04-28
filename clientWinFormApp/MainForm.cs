using System.ComponentModel;
using model;
using services;


namespace clientWinFormApp;

public partial class MainForm : Form, IObserver{

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    private readonly IServices server;

    public SoftUser user;
    
    
    public MainForm(SoftUser softUser, IServices server)
    {
        Console.WriteLine("setez server");
        this.server = server;
        Console.WriteLine("setez angajat");
        this.user = softUser;  
        Console.WriteLine("setez component");
        InitializeComponent();
        Console.WriteLine("setez trips");
    }
    
    
    public void loadTrips()
    {
        List<Trip> displayedTrips = server.GetAllTrips().ToList();
        tripsTable.DataSource = displayedTrips;
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
        string objective = txtAttraction.Text;
        if (!int.TryParse(txtStrat.Text, out int startHour) || !int.TryParse(txtEnd.Text, out int endHour))
        {
            MessageBox.Show("Please enter valid start and end hours.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        try
        {
            DateTime date = dateTimePicker1.Value;
            searchResultsTable.DataSource = server.SearchTripsByObjectiveAndTime(objective, date, startHour, endHour);
        }
        catch (MyException ex)
        {
            MessageBox.Show($"Error while searching for trips: {ex.Message}", "Search Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Unexpected error: {ex.Message}", "Unexpected Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnReserve_Click(object sender, EventArgs e)
        {
            if (tripsTable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a trip to reserve.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Trip selectedTrip = (Trip)tripsTable.SelectedRows[0].DataBoundItem;
            string clientName = txtName.Text;
            string clientPhone = txtPhone.Text;

            if (!int.TryParse(txtNr.Text, out int ticketCount))
            {
                MessageBox.Show("Please enter a valid number of tickets.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            try
            {
                server.MakeReservation( clientName, clientPhone, ticketCount, selectedTrip);
                MessageBox.Show("Reservation successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    private void btnLogout_Click(object sender, EventArgs e)
    {
        server.Logout(user, this);
        LoginForm login = new LoginForm(server);
        login.Show();
        this.Hide();
    }

    public void ReservationMade(Reservation reservation)
    {
        if (this.InvokeRequired)
        {
            this.BeginInvoke((MethodInvoker)(() => ReservationMade(reservation)));
            return;
        }
        try
        {
            if (this.IsDisposed || !this.IsHandleCreated)
                return;

            loadTrips();
            
            if (!string.IsNullOrWhiteSpace(txtAttraction.Text) 
                && int.TryParse(txtStrat.Text, out int startHour) 
                && int.TryParse(txtEnd.Text, out int endHour))
            {
                DateTime date = dateTimePicker1.Value;
                var filteredTrips = server.SearchTripsByObjectiveAndTime(
                    txtAttraction.Text, date, startHour, endHour
                );
                searchResultsTable.DataSource = filteredTrips;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in BiletCumparat: " + ex.Message);
        }
    }
}