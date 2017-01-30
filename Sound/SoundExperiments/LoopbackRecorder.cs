using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
//using StoppedEventArgs = CSCore.StoppedEventArgs;
//using WasapiLoopbackCapture = CSCore.SoundIn.WasapiLoopbackCapture;

namespace SoundExperiments
{
    /// <summary>
    /// A wrapper for the WasapiLoopbackCapture that will implement basic recording to a file that is overwrite only.
    /// </summary>
    public class LoopbackRecorder
    {
        private IWaveIn _waveIn;
        private WaveFileWriter _writer;
        private bool _isRecording = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public LoopbackRecorder()
        {
        }

        /// <summary>
        /// Starts the recording.
        /// </summary>
        /// <param name="fileName"></param>
        public void StartRecording(string fileName)
        {
            // If we are currently record then go ahead and exit out.
            if (_isRecording == true)
            {
                return;
            }
            _fileName = fileName;
            _waveIn = new WasapiLoopbackCapture();
            _writer = new WaveFileWriter(fileName, _waveIn.WaveFormat);
            _waveIn.DataAvailable += OnDataAvailable;
            _waveIn.RecordingStopped += OnRecordingStopped;
            _waveIn.StartRecording();
            _isRecording = true;
        }

        /// <summary>
        /// Stops the recording
        /// </summary>
        public void StopRecording()
        {
            if (_waveIn == null)
            {
                return;
            }
            _waveIn.StopRecording();
        }

        /// <summary>
        /// Event handled when recording is stopped.  We will clean up open objects here that are required to be 
        /// closed and/or disposed of.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            // Writer Close() needs to come first otherwise NAudio will lock up.
            if (_writer != null)
            {
                _writer.Close();
                _writer = null;
            }
            if (_waveIn != null)
            {
                _waveIn.Dispose();
                _waveIn = null;
            }
            _isRecording = false;
            if (e.Exception != null)
            {
                throw e.Exception;
            }
        } // end void OnRecordingStopped

        /// <summary>
        /// Event handled when data becomes available.  The data will be written out to disk at this point.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            _writer.Write(e.Buffer, 0, e.BytesRecorded);
            //int secondsRecorded = (int)(_writer.Length / _writer.WaveFormat.AverageBytesPerSecond);
        }

        private string _fileName = "";
        /// <summary>
        /// The name of the file that was set when StartRecording was called.  E.g. the current file being written to.
        /// </summary>
        public string FileName
        {
            get
            {
                return _fileName;
            }
        }
    }
}
