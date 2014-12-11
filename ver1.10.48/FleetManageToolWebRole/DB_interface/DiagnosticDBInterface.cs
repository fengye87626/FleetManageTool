using FleetManageToolWebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.DB_interface
{
    public class DiagnosticDBInterface
    {
        public long AddProgressDiagnostic(long vehicleId, Diagnostic diagnostic)
        {
           try
           {
               var dbContext = new FleetManageToolDBContext();
               IEnumerable<Diagnostic> dtcDB = from diagnosticDB in dbContext.Diagnostic
                                               where diagnosticDB.vehicleId == vehicleId && (diagnosticDB.status.Trim() == "InProgress" || diagnosticDB.status.Trim() == "Cleaning")
                                               select diagnosticDB;

               foreach (Diagnostic dtcTemp in dtcDB)
               {
                   dbContext.Diagnostic.Remove(dtcTemp);
               }
               Diagnostic result = dbContext.Diagnostic.Add(diagnostic);
               dbContext.SaveChanges();
               return result.pkid;
           }
           catch (Exception e)
           {
               throw new DBException("添加Diagnostic失败," + e.Message);
           }
        }
        
        public void SetDiagnosticOfVehicle(long vehicleId, List<Diagnostic> diagnostics)
        {
           try
           {
               var dbContext = new FleetManageToolDBContext();
               dbContext.Configuration.ProxyCreationEnabled = false;
               dbContext.Configuration.LazyLoadingEnabled = false;
               IEnumerable<Diagnostic> dtcDB = from diagnosticDB in dbContext.Diagnostic
                                               where diagnosticDB.vehicleId == vehicleId
                                               select diagnosticDB;
               foreach (Diagnostic dtcTemp in dtcDB)
               {
                   dbContext.Diagnostic.Remove(dtcTemp);
               }
               
               foreach (Diagnostic dtcTemp in diagnostics)
               {
                   if (null == dtcTemp.message)
                   {
                       IEnumerable<DTC> dtcMessageDB = from diagnosticDB in dbContext.DTC
                                                       where diagnosticDB.DTCCode == dtcTemp.code
                                                       select diagnosticDB;
                       foreach (DTC dtcMessageTemp in dtcMessageDB)
                       {
                           dtcTemp.message = dtcMessageTemp.Title_ZH;
                       }
                   }
                   dbContext.Diagnostic.Add(dtcTemp);
               }
               dbContext.SaveChanges();
           }
           catch (Exception e)
           {
               throw new DBException("添加Diagnostics失败," + e.Message);
           }
        }

        public List<Diagnostic> GetDiagnosticByVehicleId(long vehicleId)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;

                IEnumerable<Diagnostic> dtcDB = from diagnosticDB in dbContext.Diagnostic
                                                where diagnosticDB.vehicleId == vehicleId
                                                orderby diagnosticDB.lastReadDate descending
                                                select diagnosticDB;
                List<Diagnostic> result = new List<Diagnostic> ();
                foreach (Diagnostic dtcTemp in dtcDB)
                {
                    result.Add(dtcTemp);
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("查询Diagnostic失败," + e.Message);
            }
        }

        public bool DeleteDiagnosticByVehicleId(long vehicleId)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;

                IEnumerable<Diagnostic> dtcDB = from diagnosticDB in dbContext.Diagnostic
                                                where diagnosticDB.vehicleId == vehicleId
                                                select diagnosticDB;
                foreach (Diagnostic dtcTemp in dtcDB)
                {
                    dbContext.Diagnostic.Remove(dtcTemp);
                }
                dbContext.SaveChanges();
                
                return true;
            }
            catch (Exception e)
            {
                throw new DBException("删除Diagnostic失败," + e.Message);
            }
        }

        public long getProgressDiagnosticNum(long vehicleId)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                IEnumerable<Diagnostic> dtcDB = from diagnosticDB in dbContext.Diagnostic
                                                where diagnosticDB.vehicleId == vehicleId && diagnosticDB.status.Trim() == "InProgress"
                                                select diagnosticDB;
                return dtcDB.Count();
            }
            catch (Exception e)
            {
                throw new DBException("getProgressDiagnosticNum失败," + e.Message);
            }
        }

    }
}