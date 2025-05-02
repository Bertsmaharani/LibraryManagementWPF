using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementWPF
{
    class Person
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronym { get; set; }
        public DateTime GiveOutDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int IdBook { get; set; }
        public int QuantityBook { get; set; }
    }
}
