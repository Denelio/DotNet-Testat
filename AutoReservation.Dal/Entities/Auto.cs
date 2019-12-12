using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoReservation.Dal.Entities
{
    public abstract class Auto
    {
        [Key]
        public int Id { get; set; }
     
        public string Marke { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
      
        public int Tagestarif { get; set; }

        public ICollection<Reservation> Reservationen { get; set; }

    }

}