using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Voice_rec_program
{
    public class fileControl
    {
        public enum ClickPoints { reset, numberTwo, numberThree};
        public ClickPoints clickPoint;

        /// <summary>
        /// turns the AppDomain basdirectory.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public string cleanDirectory (string directory)
        {
            string[] parts = directory.Split('\\');
            string fixDir = parts[0];
            for (int x = 1; x < (parts.Length - 1); x++)
            {
                fixDir = fixDir + '\\' + parts[x];
            }
            return fixDir;
        }

        /// <summary>
        /// Writes textToWrite to a file at saveFileDirectory.
        /// </summary>
        /// <param name="textToWrite"></param>
        /// <param name="saveFileDirectory"></param>
        /// <returns></returns>
        public bool writeToFile (string textToWrite, string saveFileDirectory)
        {
            if (clickPoint == ClickPoints.reset)
            {
                if (File.Exists(saveFileDirectory))
                {
                    File.WriteAllText(saveFileDirectory, textToWrite);
                    clickPoint = ClickPoints.numberTwo;
                    return true;
                }
                else
                {
                    string directoryPath = cleanDirectory(saveFileDirectory);
                    bool directoryExists;
                    if (directoryExists = Directory.Exists(directoryPath))
                    {
                        File.WriteAllText(saveFileDirectory, textToWrite);
                        clickPoint = ClickPoints.numberTwo;
                        return true;
                    }
                    else
                    {
                        Directory.CreateDirectory(directoryPath);
                        File.WriteAllText(saveFileDirectory, textToWrite);
                        clickPoint = ClickPoints.numberTwo;
                        return true;
                    }
                    
                }
            }
            else if (clickPoint != ClickPoints.reset)
            {
                using (StreamWriter sW = File.AppendText(saveFileDirectory))
                {
                    sW.Write("/" + textToWrite);
                    return true;
                }
            }
            return false;
        }

        public string[] readScreenPointFile(string fileDirectory)
        {
            string textFileContentStr = File.ReadAllText(fileDirectory);
            if (textFileContentStr == null)
            {
                return null;
            }
            string[] fileContentStrArry = textFileContentStr.Split('/');
            return fileContentStrArry;
        }

        public int[] parsPointIntxy (string screenPointString)
        {
            string[] screenPoints = screenPointString.Split('*');
            int[] xYPoint = new int[2];
            try
            {
                xYPoint[0] = Int32.Parse(screenPoints[0]);
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            try
            {
                xYPoint[1] = Int32.Parse(screenPoints[1]);
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            return xYPoint;
        }
    }
}
