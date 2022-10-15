using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using PromptMaker.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromptMaker.Models
{
    public class ShiftPicM : ModelBase
    {
        #region 親オブジェクト[Parent]プロパティ
        /// <summary>
        /// 親オブジェクト[Parent]プロパティ用変数
        /// </summary>
        ParameterM? _Parent = null;
        /// <summary>
        /// 親オブジェクト[Parent]プロパティ
        /// </summary>
        public ParameterM? Parent
        {
            get
            {
                return _Parent;
            }
            set
            {
                if (_Parent == null || !_Parent.Equals(value))
                {
                    _Parent = value;
                    NotifyPropertyChanged("Parent");
                }
            }
        }
        #endregion



        #region X方向に移動した量[MoveXPos]プロパティ
        /// <summary>
        /// X方向に移動した量[MoveXPos]プロパティ用変数
        /// </summary>
        int _MoveXPos = 0;
        /// <summary>
        /// X方向に移動した量[MoveXPos]プロパティ
        /// </summary>
        public int MoveXPos
        {
            get
            {
                return _MoveXPos;
            }
            set
            {
                if (!_MoveXPos.Equals(value))
                {
                    _MoveXPos = value;
                    NotifyPropertyChanged("MoveXPos");
                }
            }
        }
        #endregion

        #region Z方向に移動した量[MoveZPos]プロパティ
        /// <summary>
        /// Z方向に移動した量[MoveZPos]プロパティ用変数
        /// </summary>
        int _MoveZPos = 0;
        /// <summary>
        /// Z方向に移動した量[MoveZPos]プロパティ
        /// </summary>
        public int MoveZPos
        {
            get
            {
                return _MoveZPos;
            }
            set
            {
                if (!_MoveZPos.Equals(value))
                {
                    _MoveZPos = value;
                    NotifyPropertyChanged("MoveZPos");
                }
            }
        }
        #endregion



        #region Y方向に移動した量[MoveYPos]プロパティ
        /// <summary>
        /// Y方向に移動した量[MoveYPos]プロパティ用変数
        /// </summary>
        int _MoveYPos = 0;
        /// <summary>
        /// Y方向に移動した量[MoveYPos]プロパティ
        /// </summary>
        public int MoveYPos
        {
            get
            {
                return _MoveYPos;
            }
            set
            {
                if (!_MoveYPos.Equals(value))
                {
                    _MoveYPos = value;
                    NotifyPropertyChanged("MoveYPos");
                }
            }
        }
        #endregion

        #region 移動量[MovePx]プロパティ
        /// <summary>
        /// 移動量[MovePx]プロパティ用変数
        /// </summary>
        int _MovePx = 10;
        /// <summary>
        /// 移動量[MovePx]プロパティ
        /// </summary>
        public int MovePx
        {
            get
            {
                return _MovePx;
            }
            set
            {
                if (!_MovePx.Equals(value))
                {
                    _MovePx = value;
                    NotifyPropertyChanged("MovePx");
                }
            }
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="param">パラメータ情報</param>
        public void Initialize( ParameterM param)
        {
            this.Parent = param;
            this.MoveXPos = 0;
            this.MoveYPos = 0;
            this.MoveZPos = 0;
            this.MovePx = 10;
        }
        #endregion

        #region ずらし処理
        /// <summary>
        /// 右にずらし処理
        /// </summary>
        public void ShiftRight()
        {
            try
            {
                var outpath = Utilities.ShiftPos(this.Parent!.InputImageOrgPath, this.MoveXPos + this.MovePx, this.MoveYPos, this.MoveZPos);
                this.Parent.InitFilePath = String.Empty;    // 一度リセット
                this.Parent.InitFilePath = outpath;         // 再設定
                this.MoveXPos += this.MovePx;               // X方向の移動量を保持
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }

        /// <summary>
        /// 左にずらし処理
        /// </summary>
        public void ShiftLeft()
        {
            try
            {
                var outpath = Utilities.ShiftPos(this.Parent!.InputImageOrgPath, this.MoveXPos - this.MovePx, this.MoveYPos, this.MoveZPos);
                this.Parent.InitFilePath = String.Empty;    // 一度リセット
                this.Parent.InitFilePath = outpath;         // 再設定
                this.MoveXPos -= this.MovePx;               // X方向の移動量を保持
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        /// <summary>
        /// 上にずらし処理
        /// </summary>
        public void ShiftUp()
        {
            try
            {
                var outpath = Utilities.ShiftPos(this.Parent!.InputImageOrgPath, this.MoveXPos, this.MoveYPos - this.MovePx, this.MoveZPos);
                this.Parent.InitFilePath = String.Empty;    // 一度リセット
                this.Parent.InitFilePath = outpath;         // 再設定
                this.MoveYPos -= this.MovePx;               // X方向の移動量を保持
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }

        /// <summary>
        /// 下にずらし処理
        /// </summary>
        public void ShiftDown()
        {
            try
            {
                var outpath = Utilities.ShiftPos(this.Parent!.InputImageOrgPath, this.MoveXPos, this.MoveYPos + this.MovePx, this.MoveZPos);
                this.Parent.InitFilePath = String.Empty;    // 一度リセット
                this.Parent.InitFilePath = outpath;         // 再設定
                this.MoveYPos += this.MovePx;               // X方向の移動量を保持
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 前方へ移動する
        /// <summary>
        /// 前方へ移動する
        /// </summary>
        public void ShiftForward()
        {
            try
            {
                var outpath = Utilities.ShiftPos(this.Parent!.InputImageOrgPath, this.MoveXPos, this.MoveYPos, this.MoveZPos - this.MovePx);
                this.Parent.InitFilePath = String.Empty; // 一度リセット
                this.Parent.InitFilePath = outpath;      // 再設定
                this.MoveZPos -= this.MovePx;
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 後方へ移動する
        /// <summary>
        /// 後方へ移動する
        /// </summary>
        public void ShiftBackward()
        {
            try
            {
                var outpath = Utilities.ShiftPos(this.Parent!.InputImageOrgPath,this.MoveXPos, this.MoveYPos, this.MoveZPos + this.MovePx);
                this.Parent.InitFilePath = String.Empty; // 一度リセット
                this.Parent.InitFilePath = outpath;      // 再設定
                this.MoveZPos += this.MovePx;
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion
    }
}
