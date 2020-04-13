using System.Collections.Generic;

namespace SDisco.Affiliate.Dto
{
    public class CreatePartnerInputDto
    {
        public  string Name { get; set; }
        public int PlaceId { get; set; }
        public  string WebsiteUrl { get; set; }
        public List<int> Languages { get; set; }
        public  string SkypeId { get; set; }
        public  string Comment { get; set; }
        public bool AlreadyBecomeSdiscoPartner { get; set; }
        public bool HasDriverLicense { get; set; }
    }
}