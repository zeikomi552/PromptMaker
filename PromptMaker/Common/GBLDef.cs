using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromptMaker.Common
{
    public enum ScriptTypeEnum
    {
        [Description("Txt2Img")]
        Txt2Img,
        [Description("Img2Img")]
        Img2Img,
        [Description("Inpaint")]
        Inpaint
    }


}
