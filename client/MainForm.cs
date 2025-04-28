using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Agentie_turism_transport_csharp;
using model;
using services;


namespace client
{
    public partial class MainForm : Form
    {
        private ProjectClientCtrl ctrl;
        private List<Trip> displayedTrips;
        private Trip displayedTrip;

        public MainForm(ProjectClientCtrl ctrl)
        {
            InitializeComponent();
            this.ctrl = ctrl;
            displayedTrips = ctrl.getAllTrips();
            tripsTable.DataSource = displayedTrips;

            ctrl.updateEvent += userUpdate;
        }
        
        public void userUpdate(object sender, ProjectEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, ProjectEventArgs>(userUpdate), sender, e);
                return;
            }

            try
            {
                displayedTrips = ctrl.getAllTrips();
                tripsTable.DataSource = null;
                tripsTable.DataSource = displayedTrips;
                tripsTable.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating trips: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public delegate void UpdateFlightsCallback();

        private void LoadTrips()
        {
            displayedTrips = ctrl.getAllTrips();
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
                searchResultsTable.DataSource = ctrl.filterTrips(objective, date, startHour, endHour);
            }
            catch (MyException ex)
            {
                MessageBox.Show($"Error while searching for trips: {ex.Message}", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                ctrl.makeReservation(clientName, clientPhone, ticketCount, selectedTrip);
                MessageBox.Show("Reservation successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            ctrl.logout();
            // this.Close();
            // LoginForm loginForm = new LoginForm(_service);
            // loginForm.Show();
        }
    }

}
