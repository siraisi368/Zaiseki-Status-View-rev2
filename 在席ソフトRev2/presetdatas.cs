using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace 在席ソフトRev2
{
    /// <summary>
    /// プリセットのデータ形式
    /// </summary>
    public class IconDatas
    {
        public int prefectureIcon { get; set; }
        public bool isViewAuthorIcon { get; set; }
        public string authorIconPath { get; set; }
    }

    public class PresetData
    {
        public string presetName { get; set; }
        public string presetDescription { get; set; }
        public ViewDatas viewDatas { get; set; } = new ViewDatas();
    }

    public class StateColors
    {
        public Color stateForeColor { get; set; } = Color.Black;
        public Color stateBackColor { get; set; } = Color.White;
    }

    public class ViewDatas
    {
        public string authorName { get; set; }
        public string authorMemo { get; set; }
        public string castStaName { get; set; }
        public string authorState { get; set; }
        public bool isRecorder { get; set; }
        public StateColors stateColors { get; set; } = new StateColors();
        public IconDatas iconDatas { get; set; } = new IconDatas();
    }


}
