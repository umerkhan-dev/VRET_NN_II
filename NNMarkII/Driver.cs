using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace NNMarkII
{
    class Driver
    {
        /// <summary>
        /// WavFiles - Array to store the wav txt file names.
        /// </summary>
        internal string[] WavTxtFiles;
        /// <summary>
        /// ByteArrays - jagged array to hold the data clip of wav files
        /// </summary>
        private double[][] ByteArrays;
        /// nNinterface - instance of NNInterface class
        /// </summary>
        NNInterface nNinterface = new NNInterface(25, 115, 115, 115, 115, 115, 115, 115, 115, 115, 115, 1);
        AudioController audioController = new AudioController();

        //---------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// BrowseForFiles - get the wav file names from the given directory.
        /// </summary>
        /// <param name="path"> path of the directory </param>
        /// <param name="filter"> file filter used </param>
        public void BrowseForFiles(string path, string filter)
        {
            string[] files = Directory.GetFiles(path, filter);
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileName(files[i]);
            }
            System.Random rnd = new System.Random();
            WavTxtFiles = files.OrderBy(x => rnd.Next()).ToArray();
            WavTxtFiles = WavTxtFiles.OrderBy(x => rnd.Next()).ToArray(); 
            //WavTxtFiles = files;

            this.FetchByteDataIntoArray(path);
        }

        //--------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// FetchByteDataIntoArray - populate the byte array with the data read from files
        /// </summary>
        private void FetchByteDataIntoArray(string path)
        {
            ByteArrays = new double[WavTxtFiles.Length][];
            for (int i = 0; i < ByteArrays.Length; i++)
            {
                List<double> temp = new List<double>();
                StreamReader reader = new StreamReader(path + WavTxtFiles[i]);
                while (!reader.EndOfStream)
                {
                    temp.Add(double.Parse(reader.ReadLine()));
                }

                ByteArrays[i] = temp.ToArray();                
            }
        }


        //---------------------------------------------------------------------------------------------------------------------
        internal float ExtractCategory(string fileName)
        {
            if(fileName[7] == '1')
            {
                return 0.1f;
            }
            else if (fileName[7] == '2')
            {
                return 0.2f;
            }
            //else if (fileName[7] == '3')
            //{
            //    return 0.3f;
            //}
            //else if (fileName[7] == '4')
            //{
            //    return 0.4f;
            //}
            //else if (fileName[7] == '5')
            //{
            //    return 0.5f;
            //}
            else if (fileName[7] == '6')
            {
                return 0.3f;
            }
            //else if (fileName[7] == '7')
            //{
            //    return 0.7f;
            //}
            //else if (fileName[7] == '8')
            //{
            //    return 0.8f;
            //}
            //else if (fileName[7] == 'n')
            //{
            //    return -1.0f;
            //}
            return 0.0f;
        }

        //---------------------------------------------------------------------------------------------------------------------
        internal void WriteWeights()
        {
            nNinterface.WriteWeights();
        }
        //---------------------------------------------------------------------------------------------------------------------
        internal void ReadWeights()
        {
            nNinterface.ReadWeights();
        }
        //---------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// DirectCompute - Handles the categraization at runtime. (Runtime version of test function)
        /// </summary>
        /// <param name="clipData"></param>
        internal void DirectCompute(float[] clipData)
        {
            double[] channel1 = CutArray( audioController.ConvertToFloat(clipData));
            this.NormalizeWavData(channel1);
            //StreamWriter writer = new StreamWriter(@"C:\Users\Abdul Wahab\Desktop\array.txt");
            //foreach (var item in channel1)
            //{
            //    writer.WriteLine(item);
            //}
            //writer.Close();
            audioController.ExtractFeatures(channel1);
            audioController.NormalizeExtractedFeatures(channel1.Length);

            Console.WriteLine("\nPredicted Output : " + nNinterface.TestNetwork(audioController.PositiveCounts,
                    audioController.NegativeCounts, audioController.SteepAscends, audioController.SteepDescends,
                    audioController.Pauses));
        }


        //---------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// CutArray - Trims the clipdata to non zero array
        /// </summary>
        /// <param name="channel1"></param>
        /// <returns></returns>
        private double[] CutArray(double[] channel1)
        {
            int nonZeroSize = channel1.Length - 1;
            for ( ; nonZeroSize > 0; nonZeroSize--)
            {
                if (channel1[nonZeroSize] != 0)
                {
                    nonZeroSize++;
                    break;
                }    
            }
            double[] nonZeroArray = new double[nonZeroSize];
            for (int i = 0; i < nonZeroSize; i++)
            {
                nonZeroArray[i] = channel1[i];
            }
            return nonZeroArray;
        }


        //---------------------------------------------------------------------------------------------------------------------
        /// NormalizeWavData - Normalize the data of wave points between a range of 1 and -1
        /// </summary>
        /// <param name="channel1"></param>
        private void NormalizeWavData(double[] channel1)
        {
            double maxInArray = channel1[0];
            // getting the max value
            for (int i = 0; i < channel1.Length; i++)
            {
                if (Math.Abs(channel1[i]) > maxInArray)
                {
                    maxInArray = Math.Abs(channel1[i]);
                }
            }
            double offset = 1 - maxInArray;
            // applying offset to the data values with respective to max value obtained
            for (int i = 0; i < channel1.Length; i++)
            {
                channel1[i] = channel1[i] / maxInArray;
            }
        }
 



        //private void NormalizeWavData(double[] channel1)
        //{
        //    double maxInArray = channel1[0];
        //    // getting the max value
        //    for (int i = 0; i < channel1.Length; i++)
        //    {
        //        if(Math.Abs(channel1[i]) > maxInArray)
        //        {
        //            maxInArray = Math.Abs(channel1[i]);
        //        }
        //    }
        //    double offset = 1 - maxInArray;
        //    // applying offset to the data values with respective to max value obtained
        //    for (int i = 0; i < channel1.Length; i++)
        //    {
        //        if(channel1[i] > 0)
        //        {
        //            channel1[i] += offset;
        //            if (channel1[i] < 0) { channel1[i] = 0; }
        //        }
        //        else if (channel1[i] < 0)
        //        {
        //            channel1[i] -= offset;
        //            if (channel1[i] > 0) { channel1[i] = 0; }
        //        }
        //    }
        //}

        //---------------------------------------------------------------------------------------------------------------------
        internal void StartTestingProcess(double[] clipData, string fileName)
        {
            nNinterface.ReadWeights();
            audioController.ExtractFeatures(clipData);
            audioController.NormalizeExtractedFeatures(clipData.Length);
            Console.WriteLine(fileName + "'s Predicted Output : " + nNinterface.TestNetwork(audioController.PositiveCounts, audioController.NegativeCounts,
                              audioController.SteepAscends, audioController.SteepDescends,
                              audioController.Pauses) + ", Orignal Value : " + this.ExtractCategory(fileName));

        }

        //---------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// StartProcess - starts the training process of the NN 
        /// </summary>
        /// <param name="directory"></param>
        internal void StartTrainingProcess()
        {
            for (int i = 0; i < this.WavTxtFiles.Length; i++)
            {
                audioController.ExtractFeatures(ByteArrays[i]);
                audioController.NormalizeExtractedFeatures(ByteArrays[i].Length);
                nNinterface.TrainNetwork(audioController.PositiveCounts, audioController.NegativeCounts,
                                     audioController.SteepAscends, audioController.SteepDescends,
                                     audioController.Pauses, this.ExtractCategory(WavTxtFiles[i]));
            }
        }

        //----------------------------------------------------------------------------------
        /*public void TestPlotter(string directory)
        {
            audioController.ExtractFeatures(ByteArrays[5]);
            audioController.NormalizeExtractedFeatures(ByteArrays[5].Length);
            


            Console.WriteLine("Pauses : " + audioController.Pauses);

            Console.WriteLine("NegativeCounts" + audioController.NegativeCounts);
            Console.WriteLine("SteepAscends" + audioController.SteepAscends);
            Console.WriteLine("SteepAscends" + audioController.SteepDescends);

            Console.WriteLine("PositiveCounts----------------------------");
            foreach (var item in audioController.PositiveCounts)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("NegativeCounts----------------------------");
            foreach (var item in audioController.NegativeCounts)
            {
                Console.WriteLine(item);
            }
        }
         
        */


        //----------------------------------------------------------------------------------

        internal void StartAutoTestingProcess()
        {
            //nNinterface.ReadWeights();
            string fileName = "";
            double difference = 0;
            double totaldiff = 0;

            for (int i = 0; i < this.WavTxtFiles.Length; i++)
            {
                
                fileName = this.WavTxtFiles[i];
                audioController.ExtractFeatures(ByteArrays[i]);
                audioController.NormalizeExtractedFeatures(ByteArrays[i].Length);
                double predicticed = Math.Round(nNinterface.TestNetwork(audioController.PositiveCounts, audioController.NegativeCounts,
                              audioController.SteepAscends, audioController.SteepDescends,
                              audioController.Pauses), 2);
                double actual = this.ExtractCategory(fileName);
                difference = predicticed - actual;
                Console.WriteLine(fileName + "'s Predicted Output : " + predicticed + ", Orignal Value : " +actual);
                Console.WriteLine("difference = " + (difference) );
            }
            totaldiff = totaldiff + difference;
            Console.WriteLine("Total Difference in prediction = "+totaldiff+"\n");
            
        }

        internal void displayCharcterset()
        {
            for (int i = 0; i < this.WavTxtFiles.Length; i++)
            {
                audioController.ExtractFeatures(ByteArrays[i]);
                audioController.NormalizeExtractedFeatures(ByteArrays[i].Length);
                Console.WriteLine("PC : " + audioController.PositiveCounts + " NC : " + audioController.NegativeCounts  + " SA : " +
                                     audioController.SteepAscends + " SD : " + audioController.SteepDescends + " P : " +
                                     audioController.Pauses + " CAT : " + this.ExtractCategory(WavTxtFiles[i]));
            }
        
        }
    }
}
