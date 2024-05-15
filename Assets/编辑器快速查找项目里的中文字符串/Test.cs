using UnityEngine;

namespace 编辑器快速查找项目里的中文字符串
{
    /// <summary>
    /// https://www.cnblogs.com/turnip/p/11087837.html
    /// https://blog.csdn.net/qq_38397338/article/details/90726658
    /// </summary>
    public class Test : MonoBehaviour
    {
        //1.ctrl + shift + f 打卡全局查找
        //2.输入(".*[\u4E00-\u9FA5]+)|([\u4E00-\u9FA5]+.*") 即搜索文档中代码中文字符串
        //(\".*([^\x00-\xff]).*\")|(\'.*([^\x00-\xff]).*x\')  即搜索" "双引号或"单引号括起来的包含中文的字符串；
        //3.打开 find option 勾选全部条件（或指定文档中搜索，在”查找以下文件类型中设置“）
    }
}