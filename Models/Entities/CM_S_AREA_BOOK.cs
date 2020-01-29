using System;
using System.Collections.Generic;

namespace Fox.Microservices.CommonUtils.Models.Entities
{
    public partial class CM_S_AREA_BOOK
    {
        public CM_S_AREA_BOOK()
        {
            CM_S_CITY_BOOK = new HashSet<CM_S_CITY_BOOK>();
        }

        public string COUNTRY_CODE { get; set; }
        public string AREA_CODE { get; set; }
        public string AREA_DESCR { get; set; }
        public string REGION_CODE { get; set; }
        public DateTime? DT_START { get; set; }
        public DateTime? DT_END { get; set; }
        public DateTime? DT_INSERT { get; set; }
        public string USERINSERT { get; set; }
        public DateTime? DT_UPDATE { get; set; }
        public string USERUPDATE { get; set; }
        public Guid ROWGUID { get; set; }

        public virtual ICollection<CM_S_CITY_BOOK> CM_S_CITY_BOOK { get; set; }
    }
}
