using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using PromptMaker.Models;
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

        #region 設定ファイルオブジェクト[SettingConf]プロパティ
        /// <summary>
        /// 設定ファイルオブジェクト[SettingConf]プロパティ用変数
        /// </summary>
        ConfigManager<SettingConfM> _SettingConf = new ConfigManager<SettingConfM>("Config", "Setting.conf", new SettingConfM());
        /// <summary>
        /// 設定ファイルオブジェクト[SettingConf]プロパティ
        /// </summary>
        public ConfigManager<SettingConfM> SettingConf
        {
            get
            {
                return _SettingConf;
            }
            set
            {
                if (_SettingConf == null || !_SettingConf.Equals(value))
                {
                    _SettingConf = value;
                    NotifyPropertyChanged("SettingConf");
                }
            }
        }
        #endregion
    }
}
