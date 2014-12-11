using FleetManageToolWebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Util
{
    public class AlertComparer : IEqualityComparer<Alert>
    {
        #region IEqualityComparer<Alert>
        public bool Equals(Alert x, Alert y)
        {

            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.guid == y.guid;
        }

        public int GetHashCode(Alert obj)
        {
            int hashAlertGUID = obj.guid == null ? 0 : obj.guid.GetHashCode();
            return hashAlertGUID;
        }
        #endregion
    }
}