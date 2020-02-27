using System;
using System.Collections.Generic;

namespace DaysOff.Models.LeelaBack
{
    public partial class Contacts
    {
        public int CtId { get; set; }
        public string UsedName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LlinkCode { get; set; }
        public string CtAddress { get; set; }
        public string CtHomeTel { get; set; }
        public string CtWorkTel { get; set; }
        public string CtMobile { get; set; }
        public string CtEmail { get; set; }
        public short? SendMailings { get; set; }
        public bool SendEmailings { get; set; }
        public bool SendTexts { get; set; }
        public short? CtGender { get; set; }
        public DateTime? CtDob { get; set; }
        public string CtNationality { get; set; }
        public int? HeardId { get; set; }
        public string Ttref { get; set; }
        public string EmergencyContact { get; set; }
        public DateTime? CtCreated { get; set; }
        public bool CtMarked { get; set; }
        public string CtMedical { get; set; }
        public string CtNotes { get; set; }
        public bool Dbuser { get; set; }
        public string CtAlertNotes { get; set; }
    }
}
