using System;
using System.Collections.Generic;
using System.IO;

namespace NNMarkII
{
    class AudioController
    {

        /// <summary>
        /// positiveCounts - array for storing number of positive peaks for different value levels
        /// </summary>
        float[] positiveCounts = new float[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// negativeCounts - array for storing number of negative peaks for different value levels
        /// </summary>
        float[] negativeCounts = new float[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// steepAscends - variable for storing steep ascends in the waveform based on a threshold
        /// </summary>
        float steepAscends = 0;
        /// <summary>
        /// steepDescends - variable for storing steep descends in the waveform based on a threshold
        /// </summary>
        float steepDescends = 0;
        /// <summary>
        /// pauses - variable for storing number of pauses in the waveform based on a threshold
        /// </summary>
        float pauses = 0;
        
        //----------------------------------------------------------------------------
        public float[] PositiveCounts
        {
            set { positiveCounts = value; }
            get { return positiveCounts; }
        }
        
        //-----------------------------------------------------------------------------
        /// <summary>
        /// NegativeCounts - setter/getter property for negativeCounts
        /// </summary>
        public float[] NegativeCounts
        {
            set { negativeCounts = value; }
            get { return negativeCounts; }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// SteepAscends - setter/getter property for steepAscends
        /// </summary>
        public float SteepAscends
        {
            set { steepAscends = value; }
            get { return steepAscends; }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// SteepDescends - setter/getter property for steepDescends
        /// </summary>
        public float SteepDescends
        {
            set { steepDescends = value; }
            get { return steepDescends; }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Pauses - setter/getter property for pauses
        /// </summary>
        public float Pauses
        {
            set { pauses = value; }
            get { return pauses; }
        }

        //------------------------------------------------------------------------------
        /// <summary>
        /// calculateThreshold - Extract the features such as positiveCounts, negativeCounts from voice byte values.
        /// </summary>
        /// <param name="audioByteArray"></param>
        public void ExtractFeatures(double[] audioByteArray)
        {
            double previous = 0.0, next = 0.0;
            double recentPositivePeak = 0.0, recentNegativePeak = 0.0;
            bool localAscend = false, localDescend = false;
            int pauseLength = 0;

            // looping through all the points in the waveform calculating the pauses based on threshold in the waveform.
            // in this case threshold length being 813 and amplitude being |0.03|
            for (int i = 0; i < audioByteArray.Length; i++)
            {
                pauseLength = 0;
                while (i < audioByteArray.Length && Math.Abs(audioByteArray[i]) <= 0.03)
                {
                    pauseLength++;
                    i++;
                }
                if (pauseLength >= 813) { pauses++; }
            }

            // looping through all the points in the waveform
            for (int i = 0; i < audioByteArray.Length; i++)
            {
                localAscend = false;
                localDescend = false;
                // selecting the previous point and next point of the waveform
                if (i == 0) { previous = 0; }
                else if (i == audioByteArray.Length - 1) { next = 0; }
                else
                {
                    previous = audioByteArray[i - 1];
                    next = audioByteArray[i + 1];
                }
                // checking if the point lies in the positive region of the waveform
                if (audioByteArray[i] >= 0)
                {
                    // checking for peaks
                    if (previous < audioByteArray[i] && next < audioByteArray[i])
                    {
                        // checking for the steep ascends and descends in positive region
                        if (audioByteArray[i] - recentPositivePeak >= 0.3)
                        {
                            localAscend = true;
                        }
                        else if (recentPositivePeak - audioByteArray[i] >= 0.3)
                        {
                            localDescend = true;
                        }
                        // incrementing the respective array index according to peak value
                        recentPositivePeak = Math.Round(audioByteArray[i], 1);
                        int index = (int)(recentPositivePeak * 10);
                        this.positiveCounts[index]++;
                    }
                }
                // checking if the point lies in the negative region of the waveform
                else if (audioByteArray[i] < 0)
                {

                    // checking for peaks
                    if (previous > audioByteArray[i] && next > audioByteArray[i])
                    {
                        // checking for the steep ascends and descends in negative region
                        if ((Math.Abs(audioByteArray[i]) - Math.Abs(recentNegativePeak)) >= 0.3)
                        {
                            localAscend = true;
                        }
                        else if (Math.Abs(recentNegativePeak) - (Math.Abs(audioByteArray[i])) >= 0.3)
                        {
                            localDescend = true;
                        }
                        // incrementing the respective array index according to peak value
                        recentNegativePeak = Math.Round(audioByteArray[i], 1);
                        int index = Math.Abs((int)(recentNegativePeak * 10));
                        this.negativeCounts[index]++;
                    }
                }
                if (localAscend) { steepAscends++; }
                if (localDescend) { steepDescends++; }
            }
        }

        //----------------------------------------------------------------------------------------
        /// <summary>
        /// openWav - Returns left and right double arrays. 'right' will be null if sound is mono.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public void OpenWav(string filename, out double[] left, out double[] right)
        {
            left = new double[] { 0.0, 0.0 };
            right = new double[] { 0.0, 0.0 };
            try
            {
                byte[] wav = File.ReadAllBytes(filename);
                // Determine if mono or stereo
                int channels = wav[22];     // Forget byte 23 as 99.999% of WAVs are 1 or 2 channels

                // Get past all the other sub chunks to get to the data subchunk:
                int pos = 12;   // First Subchunk ID from 12 to 16

                // Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))
                while (!(wav[pos] == 100 && wav[pos + 1] == 97 && wav[pos + 2] == 116 && wav[pos + 3] == 97))
                {
                    pos += 4;
                    int chunkSize = wav[pos] + wav[pos + 1] * 256 + wav[pos + 2] * 65536 + wav[pos + 3] * 16777216;
                    pos += 4 + chunkSize;
                }
                pos += 8;

                // Pos is now positioned to start of actual sound data.
                int samples = (wav.Length - pos) / 2;     // 2 bytes per sample (16 bit sound mono)
                if (channels == 2) samples /= 2;        // 4 bytes per sample (16 bit stereo)

                // Allocate memory (right will be null if only mono sound)
                left = new double[samples];
                if (channels == 2) right = new double[samples];
                else right = null;

                // Write to double array/s:
                int i = 0;
                while (pos < wav.Length)
                {
                    left[i] = BytesToDouble(wav[pos], wav[pos + 1]);
                    pos += 2;
                    if (channels == 2)
                    {
                        right[i] = BytesToDouble(wav[pos], wav[pos + 1]);
                        pos += 2;
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "! Please Try again with another Wave file.");
            }
        }

        //------------------------------------------------------------------------
        /// <summary>
        /// bytesToDouble - convert two bytes to one double in the range -1 to 1
        /// </summary>
        /// <param name="firstByte"></param>
        /// <param name="secondByte"></param>
        /// <returns></returns>
        private double BytesToDouble(byte firstByte, byte secondByte)
        {
            // convert two bytes to one short (little endian)
            short s = (short)((secondByte << 8) | firstByte);
            // convert to range from -1 to (just below) 1
            return s / 32768.0;
        }

        //--------------------------------------------------------------------------
        /// <summary>
        /// NormalizeData - normalize the characteristic data to percentage form
        /// </summary>
        /// <param name="waveSize">waveSize - total size of the wave (number of indexes in the audio channel array) </param>
        public void NormalizeExtractedFeatures(int waveSize)
        {
            for (int i = 0; i < this.positiveCounts.Length; i++)
            {
                this.positiveCounts[i] = this.positiveCounts[i] * 100 / waveSize;
            }
            for (int i = 0; i < this.negativeCounts.Length; i++)
            {
                this.negativeCounts[i] = this.negativeCounts[i] * 100 / waveSize;
            }
            this.steepAscends = this.steepAscends * 100 / waveSize;
            this.steepDescends = this.steepDescends * 100 / waveSize;
            this.pauses = this.pauses * 100 / waveSize;
        }

        //--------------------------------------------------------------------------
        /// <summary>
        /// ConvertToFloat - convert double data to float type
        /// </summary>
        /// <param name="channel1"></param>
        internal double[] ConvertToFloat(float[] channel1)
        {
            List<double> clipdata = new List<double>();
            foreach (var item in channel1)
            {
                clipdata.Add((float)item);
            }
            return clipdata.ToArray();
        }

    }
}
