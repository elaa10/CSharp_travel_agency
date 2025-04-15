using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace TravelAgency
{
    public partial class MainForm : Form
    {
        private IService service;
        private User currentUser;
        private List<Trip> allTrips;
        private List<Trip> searchResults;

        public MainForm(IService service, User user)
        {
            this.service = service;
            this.currentUser = user;
            InitializeComponent();
            LoadTrips();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Search Section
            this.lblTouristAttraction = new Label();
            this.txtTouristAttraction = new TextBox();
            this.lblStartHour = new Label();
            this.txtStartHour = new TextBox();
            this.lblEndHour = new Label();
            this.txtEndHour = new TextBox();
            this.lblDate = new Label();
            this.datePicker = new DateTimePicker();
            this.btnSearch = new Button();

            // Search Results Table
            this.searchResultsGrid = new DataGridView();
            this.searchResultsGrid.AutoGenerateColumns = false;
            this.searchResultsGrid.Columns.AddRange(new DataGridViewColumn[] {
                new DataGridViewTextBoxColumn { Name = "TouristAttraction", HeaderText = "Tourist Attraction", DataPropertyName = "TouristAttraction" },
                new DataGridViewTextBoxColumn { Name = "TransportCompany", HeaderText = "Transport Company", DataPropertyName = "TransportCompany" },
                new DataGridViewTextBoxColumn { Name = "DepartureTime", HeaderText = "Departure Time", DataPropertyName = "DepartureTime" },
                new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "Price", DataPropertyName = "Price" },
                new DataGridViewTextBoxColumn { Name = "AvailableSeats", HeaderText = "Available Seats", DataPropertyName = "AvailableSeats" }
            });

            // All Trips Table
            this.tripsGrid = new DataGridView();
            this.tripsGrid.AutoGenerateColumns = false;
            this.tripsGrid.Columns.AddRange(new DataGridViewColumn[] {
                new DataGridViewTextBoxColumn { Name = "TouristAttraction", HeaderText = "Tourist Attraction", DataPropertyName = "TouristAttraction" },
                new DataGridViewTextBoxColumn { Name = "TransportCompany", HeaderText = "Transport Company", DataPropertyName = "TransportCompany" },
                new DataGridViewTextBoxColumn { Name = "DepartureTime", HeaderText = "Departure Time", DataPropertyName = "DepartureTime" },
                new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "Price", DataPropertyName = "Price" },
                new DataGridViewTextBoxColumn { Name = "AvailableSeats", HeaderText = "Available Seats", DataPropertyName = "AvailableSeats" }
            });

            // Reservation Section
            this.lblClientName = new Label();
            this.txtClientName = new TextBox();
            this.lblClientPhone = new Label();
            this.txtClientPhone = new TextBox();
            this.lblTicketCount = new Label();
            this.txtTicketCount = new TextBox();
            this.btnReserve = new Button();
            this.btnLogout = new Button();

            // Set properties and add controls
            this.Controls.AddRange(new Control[] {
                lblTouristAttraction, txtTouristAttraction,
                lblStartHour, txtStartHour,
                lblEndHour, txtEndHour,
                lblDate, datePicker,
                btnSearch,
                searchResultsGrid,
                tripsGrid,
                lblClientName, txtClientName,
                lblClientPhone, txtClientPhone,
                lblTicketCount, txtTicketCount,
                btnReserve,
                btnLogout
            });

            // Set positions and sizes
            // ... (set all the positions and sizes similar to your JavaFX layout)

            this.Text = $"Welcome, {currentUser.Username}";
            this.Size = new System.Drawing.Size(1200, 800);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private async void LoadTrips()
        {
            try
            {
                allTrips = await service.GetAllTripsAsync();
                tripsGrid.DataSource = allTrips;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading trips: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtStartHour.Text, out int startHour) || 
                !int.TryParse(txtEndHour.Text, out int endHour))
            {
                MessageBox.Show("Please enter valid start and end hours.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                searchResults = await service.SearchTripsAsync(
                    txtTouristAttraction.Text,
                    datePicker.Value,
                    startHour,
                    endHour
                );
                searchResultsGrid.DataSource = searchResults;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching trips: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnReserve_Click(object sender, EventArgs e)
        {
            var selectedTrip = tripsGrid.SelectedRows[0]?.DataBoundItem as Trip;
            if (selectedTrip == null)
            {
                MessageBox.Show("Please select a trip to reserve.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtTicketCount.Text, out int ticketCount))
            {
                MessageBox.Show("Please enter a valid number of tickets.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                await service.MakeReservationAsync(
                    txtClientName.Text,
                    txtClientPhone.Text,
                    ticketCount,
                    selectedTrip
                );
                MessageBox.Show("Reservation successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTrips();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error making reservation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                await service.LogoutAsync(currentUser);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during logout: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Form controls
        private Label lblTouristAttraction;
        private TextBox txtTouristAttraction;
        private Label lblStartHour;
        private TextBox txtStartHour;
        private Label lblEndHour;
        private TextBox txtEndHour;
        private Label lblDate;
        private DateTimePicker datePicker;
        private Button btnSearch;
        private DataGridView searchResultsGrid;
        private DataGridView tripsGrid;
        private Label lblClientName;
        private TextBox txtClientName;
        private Label lblClientPhone;
        private TextBox txtClientPhone;
        private Label lblTicketCount;
        private TextBox txtTicketCount;
        private Button btnReserve;
        private Button btnLogout;
    }
} 