//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FleetManageToolWebRole.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Customer
    {
        public Customer()
        {
            this.Vehicle = new HashSet<Vehicle>();
        }
    
        public long pkid { get; set; }
        public string guid { get; set; }
        public long tenantid { get; set; }
        public long obuid { get; set; }
    
        public virtual Obu Obu { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual ICollection<Vehicle> Vehicle { get; set; }
    }
}
