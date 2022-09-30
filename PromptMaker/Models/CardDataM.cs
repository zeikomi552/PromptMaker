﻿using Microsoft.Win32;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using MVVMCore.Common.Wrapper;
using PromptMaker.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Xml.Serialization;
using System.Windows.Shapes;

namespace PromptMaker.Models
{
    public class CardDataM : ModelBase
    {
        #region ワードクラウド画像パス[ImagePath]プロパティ
        /// <summary>
        /// 画像パス[ImagePath]プロパティ用変数
        /// </summary>
        string _ImagePath = string.Empty;
        /// <summary>
        /// 画像パス[ImagePath]プロパティ
        /// </summary>
        public string ImagePath
        {
            get
            {
                return _ImagePath;
            }
            set
            {
                _ImagePath = value;
                NotifyPropertyChanged("ImagePath");
            }
        }
        #endregion

        #region 背景画像[BackgroundImage]プロパティ
        /// <summary>
        /// 背景画像[BackgroundImage]プロパティ用変数
        /// </summary>
        string _BackgroundImage = string.Empty;
        /// <summary>
        /// 背景画像[BackgroundImage]プロパティ
        /// </summary>
        public string BackgroundImage
        {
            get
            {
                return _BackgroundImage;
            }
            set
            {
                if (_BackgroundImage == null || !_BackgroundImage.Equals(value))
                {
                    _BackgroundImage = value;
                    NotifyPropertyChanged("BackgroundImage");
                }
            }
        }
        #endregion

        #region タイトル[Title]プロパティ
        /// <summary>
        /// タイトル[Title]プロパティ用変数
        /// </summary>
        string _Title = "タイトル";
        /// <summary>
        /// タイトル[Title]プロパティ
        /// </summary>
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                if (_Title == null || !_Title.Equals(value))
                {
                    _Title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }
        #endregion

        #region タイプ名[TypeName]プロパティ
        /// <summary>
        /// タイプ名[TypeName]プロパティ用変数
        /// </summary>
        string _TypeName = string.Empty;
        /// <summary>
        /// タイプ名[TypeName]プロパティ
        /// </summary>
        public string TypeName
        {
            get
            {
                return _TypeName;
            }
            set
            {
                if (_TypeName == null || !_TypeName.Equals(value))
                {
                    _TypeName = value;
                    NotifyPropertyChanged("TypeName");
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

        [XmlIgnore]
        System.Windows.Controls.Border _ImageArea = new System.Windows.Controls.Border();

        public void Init(System.Windows.Controls.Border border)
        {
            _ImageArea = border;
        }

        #region イメージファイルを開く
        /// <summary>
        /// イメージファイルを開く
        /// </summary>
        public void OpenImageFile()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "画像ファイル (*.png)|*.png";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    this.BackgroundImage = this.ImagePath = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 背景ファイルを開く処理
        /// <summary>
        /// 背景ファイルを開く処理
        /// </summary>
        public void OpenBackgroundImageFile()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "画像ファイル (*.png)|*.png";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    this.BackgroundImage = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 保存ボタン処理(.png)
        /// <summary>
        /// 保存ボタン処理(.png)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Save(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dlgSave = new Microsoft.Win32.SaveFileDialog();

                dlgSave.Filter = "PNGファイル(*.png)|*.png";
                dlgSave.AddExtension = true;

                if ((bool)dlgSave.ShowDialog()!)
                {
                    // レンダリング
                    var bmp = new RenderTargetBitmap(
                        (int)(_ImageArea.ActualWidth),
                        (int)(_ImageArea.ActualHeight),
                        96, 96, // DPI
                        PixelFormats.Pbgra32);
                    bmp.Render(_ImageArea);

                    // jpegで保存
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    using (var fs = File.Open(dlgSave.FileName, FileMode.Create))
                    {
                        encoder.Save(fs);
                    }


                    var tmp = this.ShallowCopy<CardDataM>();

                    var fileName = System.IO.Path.GetFileNameWithoutExtension(dlgSave.FileName);
                    string dirName = System.IO.Path.GetDirectoryName(dlgSave.FileName)!;


                    XMLUtil.Seialize(System.IO.Path.Combine(dirName, fileName + ".cardxml"), tmp);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region ロード処理
        /// <summary>
        /// ロード処理
        /// </summary>
        public void Load()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "カードファイル (*.cardxml)|*.cardxml";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    var tmp = XMLUtil.Deserialize<CardDataM>(dialog.FileName);

                    Clone<CardDataM>(tmp, this);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion
    }
}
