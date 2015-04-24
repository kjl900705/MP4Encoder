using MP4Encoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AVEncoderTester
{
    public class EncoderHelper
    {
        string OrigVideo { get; set; }
        string Output { get; set; }
        string OutputDirectory { get; set; }
        int BitrateQuality { get; set; }
        int AudioQuality { get; set; }

        string _audioTmpFile;
        string _videoTmpFile;
        string _aviSynthTmpFile;


        public EncoderHelper(string origVideo, string output, int videoQuality = 0, int audioQuality = 0) {
            this.OrigVideo = origVideo;
            this.Output = output;
            this.BitrateQuality = videoQuality > 5000 ? 5000 : videoQuality;
            this.AudioQuality = audioQuality > 500 ? 500 : audioQuality;
            this.OutputDirectory = Path.GetDirectoryName(this.Output);

            this._aviSynthTmpFile = Path.Combine(this.OutputDirectory, "__tmp_aviSynth.avs");
            this._audioTmpFile = Path.Combine(this.OutputDirectory, "__tmp_audio.mp4");
            this._videoTmpFile = Path.Combine(this.OutputDirectory, "__tmp_video.mp4");
            CreateAviSynthFile();
        }


        public void Encode() {
            bool success = X246FaacEncode();
            if (!success) {
                DeleteTmpFiles();
                Console.WriteLine("using x246 + faac encode video failed, we try to using ffmpeg.");
                var job = new ffmpegJob(new ffmpegSetting(this.OrigVideo, this.Output, "255", "100", "20"));
                var encoder = new MP4Encoder.Encoder(job) { OnInfoOutput = new InfoOutputDele((o, e) => { Console.WriteLine(e); }) };
                encoder.Start().Join();
                success = encoder.Success;
            }
            if (!success) {
                Console.WriteLine("complete, encoded success!");
            } else {
                Console.WriteLine("encoded failed!");
            }
        }

        bool X246FaacEncode() {
            var jobs = new List<IJob>{
                new AudioJob(new AudioCodecSetting(this._aviSynthTmpFile, this._audioTmpFile, this.AudioQuality)),
                new x264Job(new x264Setting(this._aviSynthTmpFile,this._videoTmpFile)),
                new MuxJob(new MuxSetting(this._videoTmpFile,this._audioTmpFile,this.Output))
            };

            foreach (var x in jobs) {
                Console.WriteLine("Begin to encode " + x.GetType().Name);
                var encoder = new MP4Encoder.Encoder(x) {
                    OnInfoOutput = new InfoOutputDele((o, e) => {
                        Console.WriteLine(e);
                    })
                };
                encoder.Start().Join();
                if (!encoder.Success) {
                    return false;
                }
                Console.WriteLine("End encoding " + x.GetType().Name);
            }
            return true;
        }

        void DeleteTmpFiles() {
            // delete temp file
            var tmpFiles = new List<string>{
                this._aviSynthTmpFile,
                this._audioTmpFile,
                this._videoTmpFile
            };
            tmpFiles.ForEach(x => {
                try {
                    if (File.Exists(x))
                        File.Delete(x);
                } catch { }
            });


        }

        void CreateAviSynthFile() {
            StreamWriter sr = new StreamWriter(this._aviSynthTmpFile, false);
            sr.WriteLine("DirectShowSource(\"{0}\", fps=23.976, convertfps=true)", this.OrigVideo);
            sr.WriteLine();
            sr.WriteLine("ConvertToYV12()");
            sr.Close();
            sr.Dispose();
        }
    }
}
