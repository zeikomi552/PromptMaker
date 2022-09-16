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
        #region キーワード[Keyword]プロパティ
        /// <summary>
        /// キーワード[Keyword]プロパティ用変数
        /// </summary>
        string _Keyword = string.Empty;
        /// <summary>
        /// キーワード[Keyword]プロパティ
        /// </summary>
        public string Keyword
        {
            get
            {
                return _Keyword;
            }
            set
            {
                if (_Keyword == null || !_Keyword.Equals(value))
                {
                    _Keyword = value;
                    NotifyPropertyChanged("Keyword");
                }
            }
        }
        #endregion
    }
}
