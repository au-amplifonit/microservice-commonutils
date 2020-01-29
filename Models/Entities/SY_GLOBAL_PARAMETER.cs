using System;
using System.Collections.Generic;

namespace Fox.Microservices.CommonUtils.Models.Entities
{
    public partial class SY_GLOBAL_PARAMETER
    {
        public string COMPANY_CODE { get; set; }
        public string DIVISION_CODE { get; set; }
        public string SHOP_CODE { get; set; }
        public string PARAMETER_NAME { get; set; }
        public string PARAMETER_VALUE { get; set; }
        public string CONTROLGROUP_CODE { get; set; }
        public string PARAMETER_DESCR { get; set; }
        public DateTime? DT_START { get; set; }
        public DateTime? DT_END { get; set; }
        public DateTime? DT_INSERT { get; set; }
        public string USERINSERT { get; set; }
        public DateTime? DT_UPDATE { get; set; }
        public string USERUPDATE { get; set; }
        public Guid ROWGUID { get; set; }
    }
}
