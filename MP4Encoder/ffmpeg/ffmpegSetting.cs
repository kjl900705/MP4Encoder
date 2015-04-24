using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MP4Encoder
{
    public class ffmpegSetting
    {
        public ffmpegSetting(string input, string output, string quality = "0", string vBitrate = "200", string aBitrate = "96") {
            this.Input = input;
            this.Output = output;
            this.VBitrate = vBitrate;
            this.ABitrate = aBitrate;
            this.Quality = quality;
            this.Duration = "0";
            this.Size = "0*0";
        }

        public string Input { get; set; }
        public string Output { get; set; }
        public string Quality { get; set; }
        public string VBitrate { get; set; }
        public string ABitrate { get; set; }
        public string Duration { get; set; }
        public string Size { get; set; }
    }
}
