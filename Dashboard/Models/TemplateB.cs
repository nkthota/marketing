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
    using System.Web.Mvc;

    public partial class TemplateB
    {
        public int ID { get; set; }
        public Nullable<int> CampaignID { get; set; }
        public string HeadLine { get; set; }
        public string SubHeadLine { get; set; }
        public string KeyBannerImage { get; set; }
        [AllowHtml]
        public string IntroductionMessage { get; set; }
        public string CTAText { get; set; }
        public string CTALink { get; set; }
        public string SecondaryCaption { get; set; }
        public string Column1Image { get; set; }
        public string Column1Title { get; set; }
        public string Column1Message { get; set; }
        public string Column1CTAText { get; set; }
        public string Column1CTALink { get; set; }
    
        public virtual Campaign Campaign { get; set; }
    }
}
