//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dashboard.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CampaignMonth
    {
        public int ID { get; set; }
        public int CampaignID { get; set; }
        public int Month { get; set; }
    
        public virtual Campaign Campaign { get; set; }
    }
}
