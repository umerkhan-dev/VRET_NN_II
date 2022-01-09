using System;
namespace NNMarkII
{
    class NNInterface
    {

        /// <summary>
        /// nNetwork - instance of the NeuralNetwork class.
        /// </summary>
        NeuralNetwork nNetwork;

        //------------------------------------------------------------------
        /// <summary>
        /// NNInterface - constructor. used to initialize the nNetwork instance.
        /// </summary>
        /// <param name="inputs"> number of inputs in nNetwork</param>
        /// <param name="neuronL1"> number of neurons in first layer </param>
        /// <param name="neuronsL2"> number of neurons in second layer </param>
        /// <param name="neuronsL3"> number of neurons in third layer </param>
        ///             .........................
        ///             .........................
        ///             .........................
        ///             .........................        
        /// <param name="neuronsL22"> number of neurons in eleventh layer</param>
        /// <param name="output"> number of outputs of nNetwork </param>


        public NNInterface(int inputs, int neuronL1, int neuronsL2, int neuronsL3, int neuronsL4, int neuronsL5,
                           int neuronsL6, int neuronsL7, int neuronsL8, int neuronsL9, int neuronsL10, int output)
        {
            this.nNetwork = new NeuralNetwork(new int[] { inputs, neuronL1, neuronsL2, neuronsL3, neuronsL4,
                                                          neuronsL5, neuronsL5, neuronsL6, neuronsL7, neuronsL8, neuronsL9, neuronsL10, output});
        }


        //public NNInterface(int inputs, int neuronL1, int neuronsL2, int neuronsL3, int neuronsL4, int neuronsL5, 
        //                   int neuronsL6, int neuronsL7, int neuronsL8, int neuronsL9, int neuronsL10, int neuronsL11,
        //                   int neuronsL12, int neuronsL13, int neuronsL14, int neuronsL15, int neuronsL16, int neuronsL17,
        //                   int neuronsL18, int neuronsL19, int neuronsL20, int neuronsL21, int neuronsL22, int output)
        //{
        //    this.nNetwork = new NeuralNetwork(new int[] { inputs, neuronL1, neuronsL2, neuronsL3, neuronsL4,
        //                                                  neuronsL5, neuronsL5, neuronsL6, neuronsL7, neuronsL8, neuronsL9, neuronsL10, neuronsL11,
        //                                                  neuronsL12, neuronsL13, neuronsL14, neuronsL15, neuronsL16, neuronsL17, neuronsL18, 
        //                                                  neuronsL19, neuronsL20, neuronsL21, neuronsL22, output});
        //}
        //------------------------------------------------------------------
        public void TrainNetwork (float[] positiveCounts, float[] negativeCounts, float steepAscends, float steepDescends, float pause, float category)
        {
            this.nNetwork.FeedForward(new float[] { 
                positiveCounts[0], positiveCounts[1], positiveCounts[2], positiveCounts[3],
                positiveCounts[4], positiveCounts[5], positiveCounts[6], positiveCounts[7],
                positiveCounts[8], positiveCounts[9], positiveCounts[10],
                negativeCounts[0], negativeCounts[1], negativeCounts[2], negativeCounts[3],
                negativeCounts[4], negativeCounts[5], negativeCounts[6], negativeCounts[7],
                negativeCounts[8], negativeCounts[9], negativeCounts[10], 
                steepAscends, steepDescends, pause
            });
            this.nNetwork.BackProp(new float[] { category });
        }

        //--------------------------------------------------------------------
        public void WriteWeights()
        {
            foreach (var item in nNetwork.layers)
            {
                item.WriteWeights();
            }
        }  

        //---------------------------------------------------------------------

        public void ReadWeights()
        {

            Platform.W0 w0 = new Platform.W0();
            Platform.W1 w1 = new Platform.W1();
            Platform.W2 w2 = new Platform.W2();
            Platform.W3 w3 = new Platform.W3();
            Platform.W4 w4 = new Platform.W4();
            Platform.W5 w5 = new Platform.W5();
            Platform.W6 w6 = new Platform.W6();
            Platform.W7 w7 = new Platform.W7();
            Platform.W8 w8 = new Platform.W8();
            Platform.W9 w9 = new Platform.W9();
            Platform.W10 w10 = new Platform.W10();
            Platform.W11 w11 = new Platform.W11();


            nNetwork.layers[0]._ReadWeights(w0.weightArray);
            nNetwork.layers[1]._ReadWeights(w1.weightArray);
            nNetwork.layers[2]._ReadWeights(w2.weightArray);
            nNetwork.layers[3]._ReadWeights(w3.weightArray);
            nNetwork.layers[4]._ReadWeights(w4.weightArray);
            nNetwork.layers[5]._ReadWeights(w5.weightArray);
            nNetwork.layers[6]._ReadWeights(w6.weightArray);
            nNetwork.layers[7]._ReadWeights(w7.weightArray);
            nNetwork.layers[8]._ReadWeights(w8.weightArray);
            nNetwork.layers[9]._ReadWeights(w9.weightArray);
            nNetwork.layers[10]._ReadWeights(w10.weightArray);
            nNetwork.layers[11]._ReadWeights(w11.weightArray);


        }

        //------------------------------------------------------------------
        public float TestNetwork(float[] positiveCounts, float[] negativeCounts, float steepAscends, float steepDescends, float pause)
        {
            return ( this.nNetwork.FeedForward(new float[] { 
                positiveCounts[0], positiveCounts[1], positiveCounts[2], positiveCounts[3],
                positiveCounts[4], positiveCounts[5], positiveCounts[6], positiveCounts[7],
                positiveCounts[8], positiveCounts[9], positiveCounts[10],
                negativeCounts[0], negativeCounts[1], negativeCounts[2], negativeCounts[3],
                negativeCounts[4], negativeCounts[5], negativeCounts[6], negativeCounts[7],
                negativeCounts[8], negativeCounts[9], negativeCounts[10], 
                steepAscends, steepDescends, pause
            })[0]);
        }
        //-------------------------------------------------------------------
    }
}
