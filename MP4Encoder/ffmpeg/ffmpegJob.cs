using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MP4Encoder
{
    public class ffmpegJob : IJob
    {
        ffmpegSetting _setting;

        public ffmpegJob(ffmpegSetting setting) {
            this._setting = setting;
        }

        public string EncoderFileFullName {
            get { return Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "ffmpeg/ffmpeg.exe"); }
        }

        public string GetCommandLine() {
            StringBuilder cmdSb = new StringBuilder();
            cmdSb.AppendFormat("-i {0} -y ", _setting.Input);

            //-- qscale doesn't work in converting to mp4, but work perfectly in converting to flv
            string originalQscale = "0";
            if (_setting.Quality == originalQscale)
                cmdSb.AppendFormat("-sameq ");
            else
                cmdSb.AppendFormat("-qscale {0} ", _setting.Quality);

            //-- b:v effection like sqscale, so it can instead of qscale when in converting to mp4

            string origVideoBitrate = "0";
            if (_setting.VBitrate != origVideoBitrate)
                cmdSb.AppendFormat("-b:v {0}k ", _setting.VBitrate);


            string origAudioBitrate = "0";
            if (_setting.ABitrate != origAudioBitrate)
                cmdSb.AppendFormat("-b:a {0}k ", _setting.ABitrate);

            ////cmdSb.AppendFormat("-b:v 1400k -r 29.97 ");
            ////cmdSb.AppendFormat("-b:a 128k ");

            string compelte = "0";
            if (_setting.Duration != compelte)
                cmdSb.AppendFormat("-t {0} ", _setting.Duration);
            string originalSize = "0*0";
            if (_setting.Size != originalSize)
                cmdSb.AppendFormat("-s {0} ", _setting.Size);

            cmdSb.AppendFormat("{0}", _setting.Output);

            return cmdSb.ToString();
        }

        public ExecuteProcessDele ExecuteInProcess {
            get {
                return null;
            }
        }
    }
}
