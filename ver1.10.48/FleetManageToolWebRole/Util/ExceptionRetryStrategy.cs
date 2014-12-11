using Microsoft.ApplicationServer.Caching;
using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.Cache;
using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.SqlAzure;
using Microsoft.Practices.TransientFaultHandling;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Util
{
    class MyRetryStrategy : ITransientErrorDetectionStrategy
    {
        public bool IsTransient(Exception ex)
        {
            if (ex != null)
            {
                if (ex is DataCacheException)
                {
                    DebugLog.Exception("DataCacheException = " + ex);
                    return DoWithDataCacheException(ex);
                }
                else if (ex is SqlException)
                {
                    DebugLog.Exception("DataBaseException = " + ex);
                    return DoWithDataBaseException(ex);
                }
                else
                {
                    DebugLog.Exception("" + ex);
                }
            }

            // For all others, do not retry.
            return false;
        }

        private Boolean DoWithDataCacheException(Exception ex)
        {
            Boolean result = false;
            int errorCode = (ex as DataCacheException).ErrorCode;
            if (errorCode == DataCacheErrorCode.RetryLater || errorCode == DataCacheErrorCode.Timeout)
            {
                result = true;
            }

            return result;
        }

        private Boolean DoWithDataBaseException(Exception ex)
        {
            Boolean result = false;
            foreach (SqlError error in (ex as SqlException).Errors)
            {
                switch (error.Number)
                {
                    case 1205://"SQL Error: Deadlock condition. Retrying..."
                        {
                            result = true;
                            break;
                        }

                    case -2://"SQL Error: Timeout expired. Retrying..."
                        {
                            result = true;
                            break;
                        }

                    case 40501://"The service is currently busy. Retrying..."
                        {
                            result = true;
                            break;
                        }
                }
            }

            return result;
        }
    }

    public class ExceptionRetryStrategy
    {
        private static int retryStrategyTime = 5;

        private volatile static ExceptionRetryStrategy instance = null;
        private static readonly object lockInstance = new object();

        private static RetryPolicy retryPolicy = null;

        private ExceptionRetryStrategy() { }

        public static ExceptionRetryStrategy GetInstance()
        {
            if (instance == null)
            {
                lock (ExceptionRetryStrategy.lockInstance)
                {
                    if (instance == null)
                    {
                        instance = new ExceptionRetryStrategy();
                        instance.UpdateRetryTime();
                        ExceptionRetryStrategy.retryPolicy = instance.CreateRetryStrategy();
                    }
                }
            }

            return instance;
        }

        public RetryPolicy GetRetryPolicy()
        {
            return ExceptionRetryStrategy.retryPolicy;
        }

        private RetryPolicy CreateRetryStrategy()
        {
            // Define your retry strategy: retry 3 times, starting 1 second apart
            // and adding 5 seconds to the interval each retry.
            var retryStrategy = new Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(retryStrategyTime));

            // Define your retry policy using the retry strategy and the Windows Azure storage
            // transient fault detection strategy.
            var retryPolicy = new RetryPolicy<MyRetryStrategy>(retryStrategy);

            // Receive notifications about retries.
            retryPolicy.Retrying += (sender, args) =>
            {
                // Log details of the retry.
                var msg = String.Format("Retry - Count:{0}, Delay:{1}, Exception:{2}",
                    args.CurrentRetryCount, args.Delay, args.LastException);
                DebugLog.Exception("DataCacheException or DataBaseException = " + msg);
            };

            return retryPolicy;
        }

        private void UpdateRetryTime()
        {
            string strRetryStrategyTime = ConfigureManager.GetConfigure("RetryStrategyTime");
            if (null != strRetryStrategyTime)
            {
                try
                {
                    int istrRetryStrategyTime = Convert.ToInt32(strRetryStrategyTime);
                    retryStrategyTime = istrRetryStrategyTime;
                }
                catch (Exception exception)
                {
                    DebugLog.Exception(exception.Message);
                }
            }
        }
    }

    public class DBExceptionRetryStrategy
    {
        private static int retryStrategyTimeDB = 5;

        private volatile static DBExceptionRetryStrategy instance = null;
        private static readonly object lockInstance = new object();

        private static RetryPolicy retryPolicy = null;

        private DBExceptionRetryStrategy() { }

        public static DBExceptionRetryStrategy GetInstance()
        {
            if (instance == null)
            {
                lock (DBExceptionRetryStrategy.lockInstance)
                {
                    if (instance == null)
                    {
                        instance = new DBExceptionRetryStrategy();
                        instance.UpdateRetryTime();
                        DBExceptionRetryStrategy.retryPolicy = instance.CreateRetryStrategy();
                    }
                }
            }

            return instance;
        }

        public RetryPolicy GetRetryPolicy()
        {
            return DBExceptionRetryStrategy.retryPolicy;
        }

        private RetryPolicy CreateRetryStrategy()
        {
            // Define your retry strategy: retry 3 times, starting 1 second apart
            // and adding 5 seconds to the interval each retry.
            var retryStrategy = new Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(retryStrategyTimeDB));

            // Define your retry policy using the retry strategy and the Windows Azure storage
            // transient fault detection strategy.
            var retryPolicy = new RetryPolicy<SqlAzureTransientErrorDetectionStrategy>(retryStrategy);

            // Receive notifications about retries.
            retryPolicy.Retrying += (sender, args) =>
            {
                // Log details of the retry.
                var msg = String.Format("Retry - Count:{0}, Delay:{1}, Exception:{2}",
                    args.CurrentRetryCount, args.Delay, args.LastException);
                DebugLog.Exception("DataBaseException : " + msg);
            };

            return retryPolicy;
        }

        private void UpdateRetryTime()
        {
            string strRetryStrategyTime = ConfigureManager.GetConfigure("RetryStrategyTime");
            if (null != strRetryStrategyTime)
            {
                try
                {
                    int istrRetryStrategyTime = Convert.ToInt32(strRetryStrategyTime);
                    retryStrategyTimeDB = istrRetryStrategyTime;
                }
                catch (Exception exception)
                {
                    DebugLog.Exception(exception.Message);
                }
            }
        }
    }

    public class CacheExceptionRetryStrategy
    {
        private static int retryStrategyTimeCache = 5;

        private volatile static CacheExceptionRetryStrategy instance = null;
        private static readonly object lockInstance = new object();

        private static RetryPolicy retryPolicy = null;

        private CacheExceptionRetryStrategy() { }

        public static CacheExceptionRetryStrategy GetInstance()
        {
            if (instance == null)
            {
                lock (CacheExceptionRetryStrategy.lockInstance)
                {
                    if (instance == null)
                    {
                        instance = new CacheExceptionRetryStrategy();
                        instance.UpdateRetryTime();
                        CacheExceptionRetryStrategy.retryPolicy = instance.CreateRetryStrategy();
                    }
                }
            }

            return instance;
        }

        public RetryPolicy GetRetryPolicy()
        {
            return CacheExceptionRetryStrategy.retryPolicy;
        }

        private RetryPolicy CreateRetryStrategy()
        {
            // Define your retry strategy: retry 3 times, starting 1 second apart
            // and adding 5 seconds to the interval each retry.
            var retryStrategy = new Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(retryStrategyTimeCache));

            // Define your retry policy using the retry strategy and the Windows Azure storage
            // transient fault detection strategy.
            var retryPolicy = new RetryPolicy<CacheTransientErrorDetectionStrategy>(retryStrategy);

            // Receive notifications about retries.
            retryPolicy.Retrying += (sender, args) =>
            {
                // Log details of the retry.
                var msg = String.Format("Retry - Count:{0}, Delay:{1}, Exception:{2}",
                    args.CurrentRetryCount, args.Delay, args.LastException);
                DebugLog.Exception("DataCacheException : " + msg);
            };

            return retryPolicy;
        }

        private void UpdateRetryTime()
        {
            string strRetryStrategyTime = ConfigureManager.GetConfigure("RetryStrategyTime");
            if (null != strRetryStrategyTime)
            {
                try
                {
                    int istrRetryStrategyTime = Convert.ToInt32(strRetryStrategyTime);
                    retryStrategyTimeCache = istrRetryStrategyTime;
                }
                catch (Exception exception)
                {
                    DebugLog.Exception(exception.Message);
                }
            }
        }
    }
}