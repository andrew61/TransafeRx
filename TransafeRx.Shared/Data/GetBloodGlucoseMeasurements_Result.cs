//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TransafeRx.Shared.Data
{
    using System;
    
    public partial class GetBloodGlucoseMeasurements_Result
    {
        public int BloodGlucoseId { get; set; }
        public string UserId { get; set; }
        public int GlucoseLevel { get; set; }
        public System.DateTime ReadingDate { get; set; }
        public string Model { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> TransferDateUTC { get; set; }
    }
}
