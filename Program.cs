using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace EDI850ParseRepeatingREF_PER {
   public class PurchaseOrder850 {
    public string PONum;
    public DateTime PODate;
    public string PODateText;
    public string POType;
    public string VendorNumber;
    public string BuyerName;
    public string BuyerTelephone;

    }


    class Program {
        static void Main(string[] args) {
            string ediFilename = @"C:\Users\Sheryl.Acree\Desktop\Sample_850_01_Orig.edi";
            //insert any file name above
            string ediFileContents = File.ReadAllText(ediFilename);

            string currentRef01 = " ";
            string currentPer01 = " ";
            //temporary variables for parsing
            Console.WriteLine(ediFileContents);

            string elementSeperator = ediFileContents.Substring(103, 1);
            //add the position # of the file's element seperator here, this one is an asterisk, for C# it will be position # minus 1            
            string lineSeperator = ediFileContents.Substring(105, 1);
            //add the position # of the file's line/segment seperator here, this one is a tilde,  for C# it will be position # minus 1  

            ediFileContents = ediFileContents.Replace("\r", "").Replace("\n", "");//replace the carriage return and line feeds with nothing


            Console.WriteLine("elementSeperator = " + elementSeperator);
            Console.WriteLine("lineSeperator = " + lineSeperator);

            PurchaseOrder850 po850 = new PurchaseOrder850();


            string[] lines = ediFileContents.Split(char.Parse(lineSeperator));
            Console.WriteLine("Number of lines = " + lines.Length);


            foreach (string line in lines) {

                Console.WriteLine(line);
                string[] elements = line.Split(char.Parse(elementSeperator));
                int loopCounter = 0;
                string segment = "";
                string elNum = "";
                string elName = ""; 

                foreach (string el in elements) {
                    if (loopCounter == 0) {
                        segment = el;
                    } else {
                        elNum = loopCounter.ToString("D2");
                        elName = segment + elNum;
                        Console.WriteLine(elName + " = " + el);

                        switch (elName)
                        {
                            case "BEG03":
                                po850.PONum = el;
                                break;
                            case "BEG05":
                                po850.PODateText = el;
                                po850.PODate = DateTime.ParseExact(
                                    el, "yyyyMMdd", CultureInfo.InvariantCulture);
                                break;
                            case "BEG02":
                                po850.POType= el;
                                break;
                            case "REF01":
                                currentRef01 = el;
                                break;
                            case "REF02":
                                if (currentRef01 == "VR") {
                                    po850.VendorNumber = el;
                                }
                                break;
                            case "PER01":
                                currentPer01 = el;
                                break;
                            case "PER02":
                                if (currentPer01 == "BD") {
                                    po850.BuyerName = el;
                                }
                                break;
                            case "PER04":
                                if (currentPer01 == "BD") {
                                    po850.BuyerTelephone = el;
                                }
                                break;
                        }

                    }
                    loopCounter++;
                }
                Console.WriteLine("*** PONum = " + po850.PONum + " PO Date = " + po850.PODateText + " PO Type = " + po850.POType);
                Console.WriteLine("*** Vendor = " + po850.VendorNumber + " Buyer Name : " + po850.BuyerName + " Buyer Telephone : " + po850.BuyerTelephone);
                
            }
            Console.WriteLine("\n\n Press enter to end;");//to keep the data on screen
            Console.ReadLine();
        }

    }

}

