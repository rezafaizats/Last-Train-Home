using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{
    public class Wagon
    {
        private List<Person> passengers;
        private int capacity;

        public void InitializeWagon() {
            passengers.Clear();

            passengers = new List<Person>(capacity);
            for (int i = 0; i < capacity; i++)
                passengers.Add(null);           
        }

        public void BoardPassengers(List<Person> onboardingPassenger) {
            if(onboardingPassenger.Count > capacity)
                Debug.LogWarning($"Onboarding passengers are exceeding capacity! Current onboard passenger : {onboardingPassenger.Count}, Maximum capacity {capacity}");
            
        }

    }
}