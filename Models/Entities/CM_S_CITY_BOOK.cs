using System;
using System.Collections.Generic;

namespace Fox.Microservices.CommonUtils.Models.Entities
{
    public partial class CM_S_CITY_BOOK
    {
        public string COUNTRY_CODE { get; set; }
        public string AREA_CODE { get; set; }
        public string ZIP_CODE { get; set; }
        public short CITY_COUNTER { get; set; }
        public string CITY_NAME { get; set; }
        public string ZIP_CODE_UNIQUE_ID { get; set; }
        public DateTime? DT_INSERT { get; set; }
        public string USERINSERT { get; set; }
        public DateTime? DT_UPDATE { get; set; }
        public string USERUPDATE { get; set; }
        public Guid ROWGUID { get; set; }

        public virtual CM_S_AREA_BOOK CM_S_AREA_BOOK { get; set; }
    }
}
