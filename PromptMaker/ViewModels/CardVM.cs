using Microsoft.Win32;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using PromptMaker.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using MVVMCore.Common.Wrapper;
using PromptMaker.Models;

namespace PromptMaker.ViewModels
{
    public class CardVM : ViewModelBase
    {
        #region カードデータ1用[CardData1]プロパティ
        /// <summary>
        /// カードデータ1用[CardData1]プロパティ用変数
        /// </summary>
        CardDataM _CardData1 = new CardDataM();
        /// <summary>
        /// カードデータ1用[CardData1]プロパティ
        /// </summary>
        public CardDataM CardData1
        {
            get
            {
                return _CardData1;
            }
            set
            {
                if (_CardData1 == null || !_CardData1.Equals(value))
                {
                    _CardData1 = value;
                    NotifyPropertyChanged("CardData1");
                }
            }
        }
        #endregion


        public override void Init(object sender, EventArgs e)
        {
            try
            {
                var wnd = sender as CardV;

                if (wnd != null)
                {
                    this.CardData1.Init(wnd.card_border);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }


        public override void Close(object sender, EventArgs e)
        {
        }
    }
}
