using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{
    public class Wagon
    {
        private List<Person> passengers = new List<Person>();
        private int capacity;
        public int Capacity => capacity;

        public void InitializeWagon(int capacity) {
            this.capacity = capacity;
            passengers.Clear();

            passengers = new List<Person>(capacity);
            for (int i = 0; i < capacity; i++)
                passengers.Add(null);
        }

        public void BoardPassenger(Person onboardingPassenger) {
            if(passengers.Count >= capacity)
                Debug.LogWarning($"Onboarding passengers are exceeding capacity! Maximum capacity : {capacity}");

            passengers.Add(onboardingPassenger);             
        }

        public bool IsPassengerOnboard(Person person) {
            return passengers.Contains(person);
        }

        public bool IsFullCapacity() {
            return passengers.Count >= capacity;
        }

    }
}