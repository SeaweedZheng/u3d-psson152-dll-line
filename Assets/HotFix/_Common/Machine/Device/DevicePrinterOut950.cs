using CryPrinter;
using GameMaker;
using UnityEngine;

public class DevicePrinterOut950 : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onSelectPrinter(EventData res)
    {
        string printerName = ((string)res.value).ToLower();
        if (printerName.Contains("950") && printerName.Contains("Epic") ) //JCM
        {
            string port = Application.isEditor ? "COM4" : "/dev/ttyS1";
            Epic950Printer printer = new Epic950Printer(port, false);//"/dev/ttyS1") ;
        }
    }

    public void TestPrinterJCM950(string barcode, double money)
    {
        //1.如果接了JCM的950打印机:

        string port = Application.isEditor ? "COM4" : "/dev/ttyS1";
        Epic950Printer printer = new Epic950Printer(port, false);//"/dev/ttyS1") ;
        printer.PrintTicket("011058314280279645", 512.32, "Crown International Club", "CYTECH", "NORTH", "10600001", 10, "1", System.DateTime.Now);
    }

    public void TestPrinterTransactEpic950()
    {
        
        //2.如果接了TRANSACT的950打印机:
        string port = Application.isEditor ? "COM4" : "/dev/ttyS1";
        Epic950Printer printer = new Epic950Printer(port, true);//"/dev/ttyS1") ;
        printer.PrintTicket("011058314280279645", 512.32, "Crown International Club", "CYTECH", "NORTH", "10600001", 10, "1", System.DateTime.Now);

    }


}
