using FleetManageToolWebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using FleetManageToolWebRole.BusinessLayer;

namespace FleetManageToolWebRole.Util
{
    public class SetAlertConfigInfoThread
    {
        private string companyid;
        private List<AlertConfiguration> alertConfiguration;
        private int speedThreshold;
        private int rpmThreshold;
        private int rpmTimeThreshold;
        private string motionThreshold;
        private bool flag;
        private Thread thread; 

        public SetAlertConfigInfoThread(string companyid, List<AlertConfiguration> alertConfiguration, int speedThreshold, int rpmThreshold, int rpmTimeThreshold, string motionThreshold, bool flag) 
        {
            this.companyid = companyid;
            this.alertConfiguration = alertConfiguration;
            this.speedThreshold = speedThreshold;
            this.rpmThreshold = rpmThreshold;
            this.rpmTimeThreshold = rpmTimeThreshold;
            this.motionThreshold = motionThreshold;
            this.flag = flag;
            thread = new Thread(new ThreadStart(Run)); 
        } 
    
        public void Start() 
        { 
            if (thread != null) 
            {
                thread.Start(); 
            }
        } 
    
        private void Run() 
        {
            AlertFetcher alertFetcher = new AlertFetcher();
            alertFetcher.SetAlertConfigInfo(companyid, alertConfiguration, speedThreshold, rpmThreshold, rpmTimeThreshold, motionThreshold, flag);
        }

    }
}