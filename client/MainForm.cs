using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Agentie_turism_transport_csharp;
using model;
using services;


namespace client
{
    public partial class MainForm : Form, IObserver
    {
        private IServices _service;
        
        public MainForm(IServices service)
        {
            InitializeComponent();
            _service = service;
        }
        

        private void LoadTrips()
        {
            var trips = _service.GetAllTrips();
            tripsTable.DataSource = trips.ToList();
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            string objective = txtAttraction.Text;
            if (!int.TryParse(txtStrat.Text, out int startHour) || !int.TryParse(txtEnd.Text, out int endHour))
            {
                MessageBox.Show("Please enter valid start and end hours.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime? date = dateTimePicker1.Value;
            if (date == null)
            {
                MessageBox.Show("Please select a date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var results = _service.SearchTripsByObjectiveAndTime(objective, date.Value, startHour, endHour);
            searchResultsTable.DataSource = results.ToList();
        }


        private void btnReserve_Click(object sender, EventArgs e)
        {
            if (tripsTable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a trip to reserve.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Trip selectedTrip = (Trip)tripsTable.SelectedRows[0].DataBoundItem;
            string clientName = txtName.Text;
            string clientPhone = txtPhone.Text;

            if (!int.TryParse(txtNr.Text, out int ticketCount))
            {
                MessageBox.Show("Please enter a valid number of tickets.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                _service.MakeReservation(clientName, clientPhone, ticketCount, selectedTrip);
                LoadTrips(); // Reload trips after reservation
                MessageBox.Show("Reservation successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginForm loginForm = new LoginForm(_service);
            loginForm.Show();
        }

        public void ReservationMade(Reservation reservation)
        {
            if (InvokeRequired)
            {
                BeginInvoke(() => ReservationMade(reservation));
                return;
            }

            // Evită apeluri blocante aici!
            LoadTrips(); 
            }
        }

    }
