using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Threading.Tasks;
using FleetManageToolWebRole.Models.API;

namespace FleetManageTool.WebAPI
{
    public class TastString
    {
        public const string TaskName = "TaskName";

        public const string FindVehiclesByIdTaskProcess = "FindVehiclesByIdTaskProcess";
    }

    public class TaskEngine<T> where T : class
    {

        private string taskName { get; set; }

        public IHalClient client { get; set; }

        public TaskEngine()
        {
            client = HalClient.GetInstance();
        }

        private Dictionary<string, object> parameters = new Dictionary<string, object>();

        //添加参数
        public void AddPara(string key, object value)
        {
            parameters.Add(key, value);
        }

        //创建Task
        public Task<TaskModel> CreateTask()
        {
            var link = new HalLink { Href = URI.TASK, IsTemplated = false };
            return client.Post<TaskModel>(link, parameters);
        }

        //获取TaskID的Task
        public Task<TaskModel> GetTaskIDTask(HalLink link)
        {
            Task<TaskModel> createTask = client.Get<TaskModel>(link, null);
            return createTask;
        }

        public Task<T> execute()
        {
            //创建Task
            Task<TaskModel> task = CreateTask();
            TaskModel taskModel = task.Result;

            //获取自己的TaskID
            var selfLink = new HalLink { Href = taskModel.Links.FirstOrDefault(l => l.Rel == URI.LINK_SELE).Href, IsTemplated = false };
            Task<TaskModel> selfTask = GetTaskIDTask(selfLink);
            TaskModel taskResult = selfTask.Result;
            while ("Pending".Equals(taskResult.Status))
            {
                System.Threading.Thread.Sleep(2000);
                selfTask = GetTaskIDTask(selfLink);
                taskResult = selfTask.Result;
            }
            if ("Failed".Equals(taskResult.Status))
            {
                return null;
            }
            //获取结果
            var resultLink = new HalLink { Href = taskResult.Links.FirstOrDefault(l => l.Rel == URI.LINK_TASKRESULT).Href, IsTemplated = true };
            this.AddPara("ID", taskResult.Id);
            return client.Get<T>(resultLink, parameters);
        }

    }
}