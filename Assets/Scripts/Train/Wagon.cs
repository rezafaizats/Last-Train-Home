using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{
    public class Wagon
    {
        private List<Person> passengers = new List<Person>();
        public List<Person> Passengers => passengers;
        private int capacity;
        public int Capacity => capacity;

        public void InitializeWagon(int capacity) {
            this.capacity = capacity;
            passengers.Clear();

            passengers = new List<Person>();
        }

        public void BoardPassenger(Person onboardingPassenger) {
            if(passengers.Count >= capacity)
                Debug.LogWarning($"Onboarding passengers are exceeding capacity! Maximum capacity : {capacity}");

            passengers.Add(onboardingPassenger);             
        }

        public bool IsPassengerOnboard(Person person) {
            return passengers.Contains(person);
        }

        public void DisembarkPassengers(List<Person> disembarkingPassengers) {
            foreach (var disembarkingPassenger in disembarkingPassengers)
            {
                if(!passengers.Contains(disembarkingPassenger)) continue;

                passengers.Remove(disembarkingPassenger);
            }
        }

        public bool IsFullCapacity() {
            return passengers.Count >= capacity;
        }

    }
}