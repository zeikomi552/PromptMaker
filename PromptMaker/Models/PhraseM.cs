using MVVMCore.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromptMaker.Models
{
    public class PhraseM : ModelBase
    {
        #region フレーズ[Phrase]プロパティ
        /// <summary>
        /// フレーズ[Phrase]プロパティ用変数
        /// </summary>
        string _Phrase = string.Empty;
        /// <summary>
        /// フレーズ[Phrase]プロパティ
        /// </summary>
        public string Phrase
        {
            get
            {
                return _Phrase;
            }
            set
            {
                if (_Phrase == null || !_Phrase.Equals(value))
                {
                    _Phrase = value;
                    NotifyPropertyChanged("Phrase");
                    NotifyPropertyChanged("NumberOfPhrase");
                }
            }
        }
        #endregion

        #region 頻度[Frequency]プロパティ
        /// <summary>
        /// 頻度[Frequency]プロパティ用変数
        /// </summary>
        int _Frequency = 0;
        /// <summary>
        /// 頻度[Frequency]プロパティ
        /// </summary>
        public int Frequency
        {
            get
            {
                return _Frequency;
            }
            set
            {
                if (!_Frequency.Equals(value))
                {
                    _Frequency = value;
                    NotifyPropertyChanged("Frequency");
                }
            }
        }
        #endregion

        #region フレーズ語句数
        /// <summary>
        /// フレーズ語句数
        /// </summary>
        public int NumberOfPhrase
        {
            get
            {
                var tmp = this._Phrase.Split(' ');
                return tmp.Length;
            }
        }
        #endregion
    }
}
