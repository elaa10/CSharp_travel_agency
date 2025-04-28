
using Microsoft.VisualBasic.ApplicationServices;
using model;
using services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    public class ProjectClientCtrl : IObserver
    {
        public event EventHandler<ProjectEventArgs> updateEvent; //ctrl calls it when it has received an update
        private readonly IServices server;
        private SoftUser currentUser;
        public ProjectClientCtrl(IServices server)
        {
            this.server = server;
            currentUser = null;
        }

        public SoftUser login(String userId, String pass)
        {
            SoftUser user = server.Login(userId, pass, this);
            Console.WriteLine("Login succeeded ....");
            currentUser = user;
            Console.WriteLine("Current user {0}", user);
            return user;
        }

        public void logout()
        {
            Console.WriteLine("Logout...");
            server.Logout(currentUser, this);
            currentUser = null; 
        }

        public List<Trip> getAllTrips()
        {
            return server.GetAllTrips().ToList();
        } 

        public List<Trip> filterTrips(string objective, DateTime date, int startHour, int endHour)
        {
            return server.SearchTripsByObjectiveAndTime(objective, date, startHour, endHour).ToList();
        }


        public void makeReservation(string clientName, string clientPhone, int ticketCount, Trip selectedTrip)
        {
            server.MakeReservation(clientName, clientPhone, ticketCount, selectedTrip);
        }
        
        protected virtual void OnUserEvent(ProjectEventArgs e)
        {
            if (updateEvent == null) return;
            updateEvent(this, e);
            Console.WriteLine("Update Event called");
        }

        public void ReservationMade(Reservation reservation)
        {
            ProjectEventArgs projectEvent = new ProjectEventArgs(ProjectEvent.ReservationMade, reservation);
            OnUserEvent(projectEvent);
        }
    }
}
