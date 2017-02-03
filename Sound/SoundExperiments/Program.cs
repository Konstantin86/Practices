using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSCore;
using CSCore.Codecs.WAV;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using CSCore.Streams;
using CUETools.Codecs;
using CUETools.Codecs.FLAKE;
using Google.Apis.Auth.OAuth2;
using Google.Apis.CloudSpeechAPI.v1beta1;
using Google.Apis.Services;
using Google.Cloud.Speech.V1Beta1;
using NAudio.Wave;
using RecognitionConfig = Google.Apis.CloudSpeechAPI.v1beta1.Data.RecognitionConfig;
using WasapiLoopbackCapture = CSCore.SoundIn.WasapiLoopbackCapture;
using WaveFormat = CSCore.WaveFormat;

namespace SoundExperiments
{
    class Program
    {
        static bool completed;
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            if (RecordWav())
            {
                return;
            }

            ResampleToMono();

            ConvertToFlac();

            //RecognizeWithGoogle();

//            AsyncRecognizeWithGoogle();

            var asyncStreamingRecognizeResult = AsyncStreamingRecognizeWithGoogle("out_resampled.wav").Result;
            // $1.44 per 1 hour

            //RecognizeViaMicrosoft();

            // TODO Translate to russian...
            //https://cloud.google.com/translate/ - 20$ per 1 million of characters
        }

        /// <summary>
        /// Stream the content of the file to the API in 32kb chunks.
        /// </summary>
        // [START speech_streaming_recognize]
        static async Task<object> AsyncStreamingRecognizeWithGoogle(string filePath)
        {
            var speech = SpeechClient.Create();
            var streamingCall = speech.GrpcClient.StreamingRecognize();
            // Write the initial request with the config.
            await streamingCall.RequestStream.WriteAsync(
                new StreamingRecognizeRequest()
                {
                    StreamingConfig = new StreamingRecognitionConfig()
                    {
                        Config = new Google.Cloud.Speech.V1Beta1.RecognitionConfig()
                        {
                            Encoding =
                            Google.Cloud.Speech.V1Beta1.RecognitionConfig.Types.AudioEncoding.Linear16,
                            SampleRate = 16000,
                            //LanguageCode = "ru-RU"
                        },
                        InterimResults = true,
                    }
                });
            // Print responses as they arrive.
            Task printResponses = Task.Run(async () =>
            {
                while (await streamingCall.ResponseStream.MoveNext(
                    default(CancellationToken)))
                {
                    foreach (var result in streamingCall.ResponseStream
                        .Current.Results)
                    {
                        foreach (var alternative in result.Alternatives)
                        {
                            Console.WriteLine(alternative.Transcript);
                        }
                    }
                }
            });
            // Stream the file content to the API.  Write 2 32kb chunks per 
            // second.
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                var buffer = new byte[32 * 1024];
                int bytesRead;
                while ((bytesRead = await fileStream.ReadAsync(
                    buffer, 0, buffer.Length)) > 0)
                {
                    await streamingCall.RequestStream.WriteAsync(
                        new StreamingRecognizeRequest()
                        {
                            AudioContent = Google.Protobuf.ByteString
                            .CopyFrom(buffer, 0, bytesRead),
                        });
                    await Task.Delay(500);
                };
            }
            await streamingCall.RequestStream.CompleteAsync();
            await printResponses;
            return 0;
        }

        private static void AsyncRecognizeWithGoogle()
        {
            var service = CreateAuthorizedClient();

            // Async recognize can work only with wav
            string audio_file_path = "out_resampled.wav";

            // [END run_application]
            // [START construct_request]
            var request = new Google.Apis.CloudSpeechAPI.v1beta1.Data.AsyncRecognizeRequest()
            {
                Config = new Google.Apis.CloudSpeechAPI.v1beta1.Data.RecognitionConfig()
                {
                    Encoding = "LINEAR16",
                    SampleRate = 16000,
                    //LanguageCode = "en-US"
                    LanguageCode = "ru-RU"
                },
                Audio = new Google.Apis.CloudSpeechAPI.v1beta1.Data.RecognitionAudio()
                {
                    Content = Convert.ToBase64String(File.ReadAllBytes(audio_file_path))
                }
            };
            // [END construct_request]
            // [START send_request]
            var asyncResponse = service.Speech.Asyncrecognize(request).Execute();
            var name = asyncResponse.Name;
            Google.Apis.CloudSpeechAPI.v1beta1.Data.Operation op;
            do
            {
                Console.WriteLine("Waiting for server processing...");
                Thread.Sleep(1000);
                op = service.Operations.Get(name).Execute();
            } while (!(op.Done.HasValue && op.Done.Value));
            dynamic results = op.Response["results"];

            string output = string.Empty;

            foreach (var result in results)
            {
                foreach (var alternative in result.alternatives)
                {
                    Console.WriteLine(alternative.transcript);
                    output += alternative.transcript;
                }
            }
        }

        private static void RecognizeWithGoogle()
        {
            var service = CreateAuthorizedClient();

            string audio_file_path = "testtest.flac";

            var request = new Google.Apis.CloudSpeechAPI.v1beta1.Data.SyncRecognizeRequest()
            {
                Config = new Google.Apis.CloudSpeechAPI.v1beta1.Data.RecognitionConfig()
                {
                    Encoding = "FLAC",
                    SampleRate = 16000,
                    LanguageCode = "en-US"
                },
                Audio = new Google.Apis.CloudSpeechAPI.v1beta1.Data.RecognitionAudio()
                {
                    Content = Convert.ToBase64String(File.ReadAllBytes(audio_file_path))
                }
            };
            var response = service.Speech.Syncrecognize(request).Execute();
            foreach (var result in response.Results)
            {
                foreach (var alternative in result.Alternatives)
                    Console.WriteLine(alternative.Transcript);
            }
        }

        private static CloudSpeechAPIService CreateAuthorizedClient()
        {
            GoogleCredential credential =
                GoogleCredential.GetApplicationDefaultAsync().Result;
            if (credential.IsCreateScopedRequired)
            {
                credential = credential.CreateScoped(new[]
                {
                    CloudSpeechAPIService.Scope.CloudPlatform
                });
            }
            return new CloudSpeechAPIService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "DotNet Google Cloud Platform Speech Sample",
            });
        }

        private static void ResampleToMono()
        {
            using (var reader = new MediaFoundationReader("out.wav"))
            {
                using (var resampler = new MediaFoundationResampler(reader, CreateOutputFormat(reader.WaveFormat)))
                {
                    WaveFileWriter.CreateWaveFile("out_resampled.wav", resampler);
                }
            }
        }

        private static NAudio.Wave.WaveFormat CreateOutputFormat(NAudio.Wave.WaveFormat inputFormat)
        {
            var waveFormat = new NAudio.Wave.WaveFormat(inputFormat.SampleRate, inputFormat.BitsPerSample, 1);
            return waveFormat;
        }

        private static bool RecordWav()
        {
            //choose the capture mode
            Console.WriteLine("Select capturing mode:");
            Console.WriteLine("- 1: Capture");
            Console.WriteLine("- 2: LoopbackCapture");

            CaptureMode captureMode = (CaptureMode)ReadInteger(1, 2);
            DataFlow dataFlow = captureMode == CaptureMode.Capture ? DataFlow.Capture : DataFlow.Render;

            //---

            //select the device:
            var devices = CSCore.CoreAudioAPI.MMDeviceEnumerator.EnumerateDevices(dataFlow, DeviceState.Active);
            if (!devices.Any())
            {
                Console.WriteLine("No devices found.");
                return true;
            }

            Console.WriteLine("Select device:");
            for (int i = 0; i < devices.Count; i++)
            {
                Console.WriteLine("- {0:#00}: {1}", i, devices[i].FriendlyName);
            }
            int selectedDeviceIndex = ReadInteger(Enumerable.Range(0, devices.Count).ToArray());
            var device = devices[selectedDeviceIndex];

            //--- choose format
            Console.WriteLine("Enter sample rate:");
            int sampleRate;
            do
            {
                sampleRate = ReadInteger();
                if (sampleRate >= 100 && sampleRate <= 200000)
                    break;
                Console.WriteLine("Must be between 1kHz and 200kHz.");
            } while (true);

            Console.WriteLine("Choose bits per sample (8, 16, 24 or 32):");
            int bitsPerSample = ReadInteger(8, 16, 24, 32);

            //note: this sample does not support multi channel formats like surround 5.1,...
            //if this is required, the DmoChannelResampler class can be used
            Console.WriteLine("Choose number of channels (1, 2):");
            int channels = ReadInteger(1, 2);

            //---

            //start recording

            //create a new soundIn instance
            using (WasapiCapture soundIn = captureMode == CaptureMode.Capture
                ? new WasapiCapture()
                : new WasapiLoopbackCapture())
            {
                //optional: set some properties 
                soundIn.Device = device;
                //...
                //initialize the soundIn instance
                soundIn.Initialize();

                //create a SoundSource around the the soundIn instance
                //this SoundSource will provide data, captured by the soundIn instance
                SoundInSource soundInSource = new SoundInSource(soundIn) { FillWithZeros = false };

                //create a source, that converts the data provided by the
                //soundInSource to any other format
                //in this case the "Fluent"-extension methods are being used
                IWaveSource convertedSource = soundInSource
                    .ChangeSampleRate(sampleRate) // sample rate
                    .ToSampleSource()
                    .ToWaveSource(bitsPerSample); //bits per sample

                //channels...
                using (convertedSource = channels == 1 ? convertedSource.ToMono() : convertedSource.ToStereo())
                {
                    //convertedSource.WaveFormat = new WaveFormat(16000, 16, 1, );
                    //create a new wavefile
                    using (WaveWriter waveWriter = new WaveWriter("out.wav", convertedSource.WaveFormat))
                    {
                        //register an event handler for the DataAvailable event of 
                        //the soundInSource
                        //Important: use the DataAvailable of the SoundInSource
                        //If you use the DataAvailable event of the ISoundIn itself
                        //the data recorded by that event might won't be available at the
                        //soundInSource yet
                        soundInSource.DataAvailable += (s, e) =>
                        {
                            //read data from the converedSource
                            //important: don't use the e.Data here
                            //the e.Data contains the raw data provided by the 
                            //soundInSource which won't have your target format
                            byte[] buffer = new byte[convertedSource.WaveFormat.BytesPerSecond / 2];
                            int read;

                            //keep reading as long as we still get some data
                            //if you're using such a loop, make sure that soundInSource.FillWithZeros is set to false
                            while ((read = convertedSource.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                //write the read data to a file
                                // ReSharper disable once AccessToDisposedClosure
                                waveWriter.Write(buffer, 0, read);
                            }
                        };

                        //we've set everything we need -> start capturing data
                        soundIn.Start();

                        Console.WriteLine("Capturing started ... press any key to stop.");
                        Console.ReadKey();

                        soundIn.Stop();
                    }
                }
            }
            return false;
        }

        private static void ConvertToFlac()
        {
            using (Stream io = new FileStream("out_resampled.wav", FileMode.Open, FileAccess.Read))
            {
                using (var outStream = new FileStream("testtest.flac", FileMode.Create, FileAccess.ReadWrite))
                {
                    ConvertToFlac(io, outStream);
                }
            }
        }

        private static void RecognizeViaMicrosoft()
        {
            using (SpeechRecognitionEngine recognizer =
                new SpeechRecognitionEngine())
            {
                // Create and load a grammar.
                Grammar dictation = new DictationGrammar();
                dictation.Name = "Dictation Grammar";

                recognizer.LoadGrammar(dictation);

                // Configure the input to the recognizer.
                recognizer.SetInputToWaveFile("out.wav");

                // Attach event handlers for the results of recognition.
                recognizer.SpeechRecognized +=
                    new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
                recognizer.RecognizeCompleted +=
                    new EventHandler<RecognizeCompletedEventArgs>(recognizer_RecognizeCompleted);

                // Perform recognition on the entire file.
                Console.WriteLine("Starting asynchronous recognition...");
                completed = false;
                recognizer.Recognize();

                // Keep the console window open.
                while (!completed)
                {
                    Console.ReadLine();
                }
                Console.WriteLine("Done.");
            }
        }

        // Handle the SpeechRecognized event.
        static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result != null && e.Result.Text != null)
            {
                Console.WriteLine("  Recognized text =  {0}", e.Result.Text);
            }
            else
            {
                Console.WriteLine("  Recognized text not available.");
            }
        }

        private static void ConvertToFlac(Stream sourceStream, Stream destinationStream)
        {
            var audioSource = new WAVReader(null, sourceStream);

            try
            {
                if (audioSource.PCM.SampleRate != 16000)
                {
                    throw new InvalidOperationException("Incorrect frequency - WAV file must be at 16 KHz.");
                }
                var buff = new AudioBuffer(audioSource, 0x10000);

                var flakeWriter = new FlakeWriter(null, destinationStream, audioSource.PCM);
                //                flakeWriter.CompressionLevel = 8;

                while (audioSource.Read(buff, -1) != 0)
                {
                    flakeWriter.Write(buff);
                }
                flakeWriter.Close();
            }
            finally
            {
                audioSource.Close();
            }
        }

        // Handle the RecognizeCompleted event.
        static void recognizer_RecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Console.WriteLine("  Error encountered, {0}: {1}",
                e.Error.GetType().Name, e.Error.Message);
            }
            if (e.Cancelled)
            {
                Console.WriteLine("  Operation cancelled.");
            }
            if (e.InputStreamEnded)
            {
                Console.WriteLine("  End of stream encountered.");
            }
            completed = true;
        }

        private static int ReadInteger(params int[] validValues)
        {
            int value;

            do
            {
                value = ReadInteger();
                if (validValues == null || validValues.Any(x => x == value))
                    return value;
                Console.WriteLine("Invalid value");
            } while (true);
        }

        private static int ReadInteger()
        {
            int value;
            while (!Int32.TryParse(Console.ReadLine(), out value))
            {
                Console.WriteLine("Invalid value");
            }
            return value;
        }

        enum CaptureMode
        {
            Capture = 1,
            // ReSharper disable once UnusedMember.Local
            LoopbackCapture = 2
        }
    }
}
