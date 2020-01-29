using System;
using System.Collections.Generic;

namespace Fox.Microservices.CommonUtils.Models.Entities
{
    public partial class CM_B_COUNTER
    {
        public string COMPANY_CODE { get; set; }
        public string DIVISION_CODE { get; set; }
        public string SHOP_CODE { get; set; }
        public string LAPTOP_CODE { get; set; }
        public string TABLE_NAME { get; set; }
        public string FIELD_NAME { get; set; }
        public int? VALUE { get; set; }
        public int? MIN_VALUE { get; set; }
        public int? MAX_VALUE { get; set; }
        public string FLG_BASE36 { get; set; }
        public string FLG_AUTOMOVENEXT { get; set; }
        public string DATEFIELDFORCHECK { get; set; }
        public string COUNTER_DESCR { get; set; }
        public string FLG_MANUAL_RESET { get; set; }
        public DateTime? DT_START { get; set; }
        public DateTime? DT_END { get; set; }
        public DateTime? DT_INSERT { get; set; }
        public string USERINSERT { get; set; }
        public DateTime? DT_UPDATE { get; set; }
        public string USERUPDATE { get; set; }
        public Guid ROWGUID { get; set; }
    }
}
