using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoReservation.Dal.Entities
{
    public class Kunde
    {
        public DateTime Geburtsdatum { get; set; }
        [Key]
        public int Id { get; set; }
        public string Nachname { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string Vorname { get; set; }
        public ICollection<Reservation> Reservationen { get; set; }
    }

}