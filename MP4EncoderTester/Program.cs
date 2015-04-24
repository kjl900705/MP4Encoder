﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVEncoderTester
{
    class Program
    {
        static void Main(string[] args) {
            string input = @"E:\movie\thief.rmvb";
            string output = @"C:\Users\Canie\Desktop\testVideo\encoded\encoded.mp4";
            EncoderHelper encoderHelper = new EncoderHelper(input, output);
            encoderHelper.Encode();
        }
    }
}
