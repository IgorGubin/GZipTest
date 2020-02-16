using System;
using System.Collections.Generic;

namespace GZipTest.Model.Modes.Сonverting
{
    public enum OutputStateEnum
    {
        Default,
        Debug
    }

    internal class WorkItemContext
    {
        public Dictionary<string, dynamic> Params { get; } = new Dictionary<string, dynamic>();

        public double FileInLength
        {
            get
            {
                double res = 0;
                if (Params.TryGetValue("FileInLength", out var tmpValue))
                {
                    res = tmpValue;
                }
                return res;
            }
            set
            {
                Params["FileInLength"] = value;
            }
        }

        public DateTime? PipelineStart
        {
            get
            {
                DateTime? res = null;
                if (Params.TryGetValue("PipelineStart", out var tmpValue))
                {
                    res = tmpValue;
                }
                return res;
            }
            set
            {
                Params["PipelineStart"] = value;
            }
        }

        public TimeSpan? ConsumedTime => PipelineStart.HasValue ? DateTime.Now - PipelineStart.Value : (TimeSpan?)null;

        public TimeSpan? ExpectedTime(double progress)
        {
            TimeSpan? res = null;
            if (ConsumedTime.HasValue && progress > 0)
            {
                res = TimeSpan.FromSeconds(ConsumedTime.Value.TotalSeconds / progress);
            }
            return res;
        }

        public OutputStateEnum ConsoleOutputMode
        {
            get
            {
                OutputStateEnum res = OutputStateEnum.Default;
                if (Params.TryGetValue("ConsoleOutputMode", out var tmpValue))
                {
                    try
                    {
                        res = tmpValue;
                    }
                    catch
                    {
                        res = OutputStateEnum.Default;
                    }
                }
                return res;
            }
            set
            {
                Params["ConsoleOutputMode"] = value;
            }
        }

        public void SetMax<T>(string key, T curVal) where T : IComparable
        {
            if (Params.TryGetValue(key, out var curMax))
            {
                if (curVal.CompareTo(curMax) > 0)
                    Params[key] = curVal;
            }
            else
            {
                Params[key] = curVal;
            }
        }
    }
}
