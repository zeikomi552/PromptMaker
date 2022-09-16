using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromptMaker.Common
{
    public class GBLValues : ModelBase
    {
        private GBLValues()
        {

        }

        private static GBLValues _SingleInstance = new();

        #region インスタンス
        /// <summary>
        /// インスタンス
        /// </summary>
        /// <returns></returns>
        public static GBLValues GetInstance()
        {
            return _SingleInstance;
        }
        #endregion

    }
}
