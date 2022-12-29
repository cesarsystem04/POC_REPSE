using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_REPSE.Utils;
internal class RequestPDF
{
    public byte[] FileBase64 { get; set; }
    public int TypeFile { get; set; }
    public string FileName { get; set; }
    public string FileText { get; set; }
    public string QRReader { get; set; }
}


public class BaseResponse
{
    public bool isSuccessful { get; set; }
    public string Message { get; set; }
}

public class BaseInputCompare
{
    public string Nombre { get; set; }
    public string ValuePDF { get; set; }
    public string ValueQR { get; set; }
    public bool Result { get; set; }
}
public class ValidPDFResponse : BaseResponse
{
   public List<BaseInputCompare> BaseInputCompares { get; set; }
}