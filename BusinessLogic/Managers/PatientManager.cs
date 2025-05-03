using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Models;

namespace BusinessLogic.Managers
{
    public class PatientManager
    {
        private List<Patient> _patients;
        private readonly string _filePath;
        private static readonly string[] _bloodGroups = { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
        private readonly Random _random = new Random();

        public PatientManager(string filePath)
        {
            _filePath = filePath;
            _patients = new List<Patient>();
            LoadFromFile();
        }

        public void AddPatient(Patient patient)
        {
            if (_patients.Any(p => p.CI == patient.CI))
                throw new InvalidOperationException("Patient with this CI already exists.");

            patient.BloodGroup = AssignRandomBloodGroup();
            _patients.Add(patient);
            SaveToFile();
        }

        public List<Patient> GetAllPatients()
        {
            return _patients;
        }

        public Patient GetPatientByCI(string ci)
        {
            return _patients.FirstOrDefault(p => p.CI == ci);
        }

        public bool UpdatePatient(string ci, string newName, string newLastName)
        {
            var patient = _patients.FirstOrDefault(p => p.CI == ci);
            if (patient == null)
                return false;

            patient.Name = newName;
            patient.LastName = newLastName;
            SaveToFile();
            return true;
        }

        public bool DeletePatient(string ci)
        {
            var patient = _patients.FirstOrDefault(p => p.CI == ci);
            if (patient == null)
                return false;

            _patients.Remove(patient);
            SaveToFile();
            return true;
        }

        private void LoadFromFile()
        {
            if (!File.Exists(_filePath))
                return;

            var lines = File.ReadAllLines(_filePath);
            _patients = lines
                .Select(line => Patient.FromString(line))
                .Where(p => p != null)
                .ToList();
        }

        private void SaveToFile()
        {
            var lines = _patients.Select(p => p.ToString());
            File.WriteAllLines(_filePath, lines);
        }

        private string AssignRandomBloodGroup()
        {
            return _bloodGroups[_random.Next(_bloodGroups.Length)];
        }
    }
}
