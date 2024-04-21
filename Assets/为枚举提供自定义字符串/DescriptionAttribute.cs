using System;

namespace 为枚举提供自定义字符串
{
    /// <summary>
    /// 为枚举提供自定义字符串,结合Utility.Enum.ToCustomStrs使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class DescriptionAttribute : Attribute
    {
        public string str;

        public DescriptionAttribute(string str)
        {
            this.str = str;
        }
    }
    
    /// <summary>
    /// 时间单位类型
    /// </summary>
    public enum TimeUnitType
    {
        [Description("ms")]     Millisecond,
        [Description("s")]      Second,
        [Description("m")]      Min,
        [Description("h")]      Hour,
    }
}