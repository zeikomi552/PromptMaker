using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using PromptMaker.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromptMaker.ViewModels
{
    public class OutputImageVM : ViewModelBase
    {

        public override void Init(object sender, EventArgs e)
        {
            try
            {
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
