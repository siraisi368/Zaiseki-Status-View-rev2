using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace 在席ソフトRev2
{
    public class preset{
        public void save(List<PresetData> presets)
        {
            var json = JsonConvert.SerializeObject(presets);
            using (StreamWriter sw = new StreamWriter(@"Settings/PresetSettings.json", false, Encoding.UTF8))
            {
                sw.Write(json);
            }
        }
        public List<PresetData> load()
        {
            List<PresetData> respData = new List<PresetData>();
            using (StreamReader sr = new StreamReader(@"Settings/PresetSettings.json", Encoding.UTF8))
            {
                var json = sr.ReadToEnd();
                if(json == "")
                {
                    respData = new List<PresetData>();
                    return respData;
                }
                respData = JsonConvert.DeserializeObject<List<PresetData>>(json);
            }
            return respData;
        }
    }
    public class lastdata
    {
        public void save(ViewDatas lastdata)
        {
            var json = JsonConvert.SerializeObject(lastdata);
            using (StreamWriter sw = new StreamWriter(@"Settings/LastData.json", false, Encoding.UTF8))
            {
                sw.Write(json);
            }
        }
        public ViewDatas load()
        {
            ViewDatas respData = new ViewDatas();
            using (StreamReader sr = new StreamReader(@"Settings/LastData.json", Encoding.UTF8))
            {
                var json = sr.ReadToEnd();
                if (json == "")
                {
                    respData = new ViewDatas()
                    {
                        stateColors = new StateColors() { 
                            stateBackColor = Color.White,
                            stateForeColor = Color.Black,
                        },
                        iconDatas = new IconDatas(),
                    };
                    return respData;
                }
                respData = JsonConvert.DeserializeObject<ViewDatas>(json);
            }
            return respData;
        }
    }
}
