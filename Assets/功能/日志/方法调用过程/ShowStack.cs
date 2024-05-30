using System;
using System.Diagnostics;
using System.Text;

namespace 日志.方法调用过程
{
    public class ShowStack
    {
        /// <summary>
        /// 消息显示
        /// </summary>
        public string message;
        
        public ShowStack(string messageValue)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{messageValue} 调用时间:{DateTime.Now.TimeOfDay}");
            var stack = new StackTrace(true);//如果为 true，则捕获文件名、行号和列号；否则为 false。
            //调用GetFrame得到栈空间
            var stackFrames = stack.GetFrames();
            for (var i = 1; i < stackFrames?.Length; i++)
            {
                //参数index 表示栈空间的级别，0表示当前栈空间，1表示上一级的栈空间，依次类推
                var method = stackFrames[i].GetMethod();
                //堆栈信息  类名.方法名; 文件名.行数
                //sb.AppendLine($"[CALL STACK][{i}]: 【Method:{method?.DeclaringType?.FullName}.{method?.Name}】; 【File:{stackFrames[i].GetFileName()}.{stackFrames[i].GetFileLineNumber()}】");
                sb.AppendLine($"[调用堆栈][{i}]: 【方法:{method?.DeclaringType?.FullName}.{method?.Name}】: 【行:{stackFrames[i].GetFileLineNumber()}】");
                //sb.Append($"【文件:{stackFrames[i].GetFileName()}】 ");
            }

            message = sb.ToString();
        }
    }
}