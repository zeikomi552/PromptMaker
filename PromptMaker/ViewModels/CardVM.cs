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

        #region カードデータ2用[CardData2]プロパティ
        /// <summary>
        /// カードデータ2用[CardData2]プロパティ用変数
        /// </summary>
        CardDataM _CardData2 = new CardDataM();
        /// <summary>
        /// カードデータ2用[CardData2]プロパティ
        /// </summary>
        public CardDataM CardData2
        {
            get
            {
                return _CardData2;
            }
            set
            {
                if (_CardData2 == null || !_CardData2.Equals(value))
                {
                    _CardData2 = value;
                    NotifyPropertyChanged("CardData2");
                }
            }
        }
        #endregion

        #region カードデータ3用[CardData3]プロパティ
        /// <summary>
        /// カードデータ3用[CardData3]プロパティ用変数
        /// </summary>
        CardDataM _CardData3 = new CardDataM();
        /// <summary>
        /// カードデータ3用[CardData3]プロパティ
        /// </summary>
        public CardDataM CardData3
        {
            get
            {
                return _CardData3;
            }
            set
            {
                if (_CardData3 == null || !_CardData3.Equals(value))
                {
                    _CardData3 = value;
                    NotifyPropertyChanged("CardData3");
                }
            }
        }
        #endregion

        #region タイトルが変更された際のイベント処理
        /// <summary>
        /// タイトルが変更された際のイベント処理
        /// </summary>
        public void TitleChange()
        {
            try
            {
                this.CardData2.Title = this.CardData3.Title = this.CardData1.Title;
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region タイプが変更された際のイベント処理
        /// <summary>
        /// タイプが変更された際のイベント処理
        /// </summary>
        public void TypeChange()
        {
            try
            {
                this.CardData2.TypeName = this.CardData3.TypeName = this.CardData1.TypeName;
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 説明が変更された際の処理
        /// <summary>
        /// 説明が変更された際の処理
        /// </summary>
        public void DescriptionChange()
        {
            try
            {
                this.CardData2.Description = this.CardData3.Description = this.CardData1.Description;
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
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
                    this.CardData2.Init(wnd.card_border2);
                    this.CardData3.Init(wnd.card_border3);
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
