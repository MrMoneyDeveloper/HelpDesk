using C8.eServices.Mvc.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C8.eServices.Mvc.Helpers
{
    public static class IncidentHelper
    {
        public static string response = string.Empty;
        public static string GetTicketReference(string ErrorCode, eServicesDbContext cxt)
        {
            try
            {
                string prefix = "XH";
                //var AppSettings = cxt.AppSettings;
                //var Customers = cxt.Customers;
                //var Statuses = cxt.Status;
                //var query = AppSettings.FirstOrDefault(o => o.Key == AppSettingKeys.hsd_daily_sequence_counter);
                //var SeqLimit = AppSettings.FirstOrDefault(o => o.Key == AppSettingKeys.hsd_daily_sequence_limiter);

                //var BatchCounter = query.Value;
                //int limiter = Convert.ToInt16(SeqLimit.Value);
                //if (query.ModifiedDateTime.Value.Date == DateTime.Now.Date)
                //{
                //    var lastRef = query.Value;
                //    BatchCounter = lastRef.ToString();
                //    int nextSeq = Convert.ToInt16(query.Value) + 1;
                //    string nextVal = nextSeq.ToString().PadLeft(limiter, '0');
                //    query.Value = nextVal;
                //    cxt.Entry(query).State = EntityState.Modified;
                //    cxt.SaveChanges();
                //}
                //else
                //{
                //    int nextSeq = 1;
                //    string nextVal = nextSeq.ToString().PadLeft(limiter, '0');
                //    BatchCounter = nextVal;
                //    nextSeq = 2;
                //    nextVal = nextSeq.ToString().PadLeft(limiter, '0');
                //    query.Value = nextVal;
                //    query.ModifiedDateTime = DateTime.Now.Date;
                //    cxt.Entry(query).State = EntityState.Modified;
                //    cxt.SaveChanges();
                //}
                //var RefNum = string.Format("HSD{0}{1}", DateTime.Now.ToString("yyyyMMdd"), BatchCounter);

                response = string.Format("{0}{1}{2}", prefix, ErrorCode, DateTime.Now.ToString("yyyyMMdd"));
            }
            catch
            {
                response = null;
            }
            return response;
        }
    }

}