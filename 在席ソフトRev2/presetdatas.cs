using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 在席ソフトRev2
{
    /// <summary>
    /// プリセットのデータ形式
    /// </summary>
    public class presetdatas
    {
        /// <summary>
        /// プリセットの名前
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 備考
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 配信者or記録者
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 配信・記録者名
        /// </summary>
        public string broname { get; set; }
        /// <summary>
        /// 配信地点名
        /// </summary>
        public string pointname { get; set; }
        /// <summary>
        /// 配信局・記録局名
        /// </summary>
        public string brostaname { get; set; }
        /// <summary>
        /// ユーザアイコンを表示するか
        /// </summary>
        public bool isnicon { get; set; }
        /// <summary>
        /// 配信地点(下段)アイコンを表示するか
        /// </summary>
        public bool ispicn { get; set; }
        /// <summary>
        /// 配信地点(下段)アイコンのパス
        /// </summary>
        public string pointpath { get; set; }
        /// <summary>
        /// 配信者(上段)アイコンのパス
        /// </summary>
        public string broiconpath { get; set; }
    }
}
