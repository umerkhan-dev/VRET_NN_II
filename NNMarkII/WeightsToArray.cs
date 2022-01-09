/*starting with the holy name of Allah Almighty, Who is the most Beneficent and the Merciful*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NNMarkII
{
    class WeightsToArray
    {

        public int layerNumber;
        List<float> temp = new List<float>();


        //------------------------------------------------------------------------------
        public void WriteWeightFileHeader(int arrSize)
        {
            StreamWriter writer = new StreamWriter(Environment.CurrentDirectory + @"\Weights.cs");
            writer.Write(@"using System;");
            writer.Write(@"class Weights");
            writer.Write(@"{");
            writer.Write(@"    public float[][] weightArray = new float[" + arrSize + @"][];");
            writer.Write(@"    public Weights()");
            writer.Write(@"    {");



            writer.Close();
        }

        //------------------------------------------------------------------------------------------
        public void WriteWeightFileDeclaration(string path)
        {
            temp = new List<float>();
            StreamReader reader = new StreamReader(path);
            while (!reader.EndOfStream)
            {
                temp.Add(float.Parse(reader.ReadLine()));
            }
            reader.Close();

            StreamWriter writer = new StreamWriter(Environment.CurrentDirectory + @"\Weights.cs" , true);
            writer.Write(@"        weightArray[" + this.layerNumber + @"] = new float[]{");
            for (int i = 0; i < temp.Count; i++)
            {
                writer.Write(temp[i]);
                if (i != temp.Count - 1)
                {
                    writer.Write(@", ");
                }
            }
            writer.Write("};\n\n");
            writer.Close();
        }
        //-----------------------------------------------------------------------------------------
        public void WriteWeightFileFooter()
        {
            StreamWriter writer = new StreamWriter(Environment.CurrentDirectory + @"\Weights.cs", true);
            writer.Write("    }\n");
            writer.Write("}\n");
            writer.Close();
        }

    }
}
