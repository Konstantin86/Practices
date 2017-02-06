using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace CloudSpeech.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var stt = new SpeechToText();
            //using (var stream = new FileStream(@"C:\git\Practices\Sound\josephcooney-cloudspeech-8619cf699541\this_is_a_test.wav", FileMode.Open))
            using (var stream = new FileStream(@"C:\git\Practices\Sound\josephcooney-cloudspeech-8619cf699541\out resampled 16kHz.wav", FileMode.Open))
            {
                var response = stt.Recognize(stream);
            }
        }
    }
}
