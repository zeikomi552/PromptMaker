using MVVMCore.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromptMaker.Models
{
    public class PromptConsistM : ModelBase
    {
        #region 有効フラグ[IsEnable]プロパティ
        /// <summary>
        /// 有効フラグ[IsEnable]プロパティ用変数
        /// </summary>
        bool _IsEnable = false;
        /// <summary>
        /// 有効フラグ[IsEnable]プロパティ
        /// </summary>
        public bool IsEnable
        {
            get
            {
                return _IsEnable;
            }
            set
            {
                if (!_IsEnable.Equals(value))
                {
                    _IsEnable = value;
                    NotifyPropertyChanged("IsEnable");
                }
            }
        }
        #endregion

        #region プロンプト[Prompt]プロパティ
        /// <summary>
        /// プロンプト[Prompt]プロパティ用変数
        /// </summary>
        string _Prompt = string.Empty;
        /// <summary>
        /// プロンプト[Prompt]プロパティ
        /// </summary>
        public string Prompt
        {
            get
            {
                return _Prompt;
            }
            set
            {
                if (_Prompt == null || !_Prompt.Equals(value))
                {
                    _Prompt = value;
                    NotifyPropertyChanged("Prompt");
                }
            }
        }
        #endregion

        #region 説明[Description]プロパティ
        /// <summary>
        /// 説明[Description]プロパティ用変数
        /// </summary>
        string _Description = string.Empty;
        /// <summary>
        /// 説明[Description]プロパティ
        /// </summary>
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (_Description == null || !_Description.Equals(value))
                {
                    _Description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }
        #endregion

        #region URL[URL]プロパティ
        /// <summary>
        /// URL[URL]プロパティ用変数
        /// </summary>
        string _URL = string.Empty;
        /// <summary>
        /// URL[URL]プロパティ
        /// </summary>
        public string URL
        {
            get
            {
                return _URL;
            }
            set
            {
                if (_URL == null || !_URL.Equals(value))
                {
                    _URL = value;
                    NotifyPropertyChanged("URL");
                }
            }
        }
        #endregion


    }
}
