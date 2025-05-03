using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class Patient
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string CI { get; set; }  // Documento de identidad único
        public string BloodGroup { get; set; }

        public Patient() { }

        public Patient(string name, string lastName, string ci, string bloodGroup)
        {
            Name = name;
            LastName = lastName;
            CI = ci;
            BloodGroup = bloodGroup;
        }

        public override string ToString()
        {
            return $"{Name},{LastName},{CI},{BloodGroup}";
        }

        public static Patient FromString(string data)
        {
            var parts = data.Split(',');
            if (parts.Length != 4) return null;

            return new Patient(parts[0], parts[1], parts[2], parts[3]);
        }
    }
}
