using System.Collections;

namespace Framework.Core
{
    public interface ICore
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public void Init();

        /// <summary>
        /// 异步初始化
        /// </summary>
        /// <returns></returns>
        public IEnumerator AsyncEnter();

        /// <summary>
        /// 退出
        /// </summary>
        public IEnumerator Exit();
    }
}