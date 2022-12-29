using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POC_REPSE.Utils;
internal class ValidPDFOpinionCumplimientoSATV1
{
    public ValidPDFResponse Validar(RequestPDF requestPDF)
    {
        ValidPDFResponse validPDFResponse = new ValidPDFResponse();
        List<BaseInputCompare> baseInputCompares = new List<BaseInputCompare>();
       OpinionCumplimientoSAT opinionCumplimientoSATPDF = new OpinionCumplimientoSAT();
        OpinionCumplimientoSAT opinionCumplimientoSATQR = new OpinionCumplimientoSAT();
        opinionCumplimientoSATPDF = ParsearPDF(requestPDF.FileText);
        opinionCumplimientoSATQR = ParsearQR(requestPDF.QRReader);
        BaseInputCompare baseInputCompare;

        StringBuilder Error = new StringBuilder();
        Error.Append(opinionCumplimientoSATQR.RFC != opinionCumplimientoSATPDF.RFC ? "El campo RFC no coincide\n" : "");
        Error.Append(opinionCumplimientoSATQR.Folio != opinionCumplimientoSATPDF.Folio ? "El campo Folio no coincide\n" : "");
        Error.Append(opinionCumplimientoSATQR.Sentido != opinionCumplimientoSATPDF.Sentido ? "El campo Sentido no coincide\n" : "");
        Error.Append(opinionCumplimientoSATQR.Fecha != opinionCumplimientoSATPDF.Fecha ? "El campo Fecha no coincide\n" : "");
        validPDFResponse.Message = Error.ToString();
        validPDFResponse.isSuccessful = Error.ToString().Length == 0;
        // RFC 
        baseInputCompare = new BaseInputCompare
        {
            Nombre = "RFC",
            ValuePDF = opinionCumplimientoSATPDF.RFC,
            ValueQR = opinionCumplimientoSATQR.RFC,
            Result = opinionCumplimientoSATQR.RFC == opinionCumplimientoSATPDF.RFC
        };
        baseInputCompares.Add(baseInputCompare);
        //Folio
        baseInputCompare = new BaseInputCompare
        {
            Nombre = "Folio",
            ValuePDF = opinionCumplimientoSATPDF.Folio,
            ValueQR = opinionCumplimientoSATQR.Folio,
            Result = opinionCumplimientoSATQR.Folio == opinionCumplimientoSATPDF.Folio
        };
        baseInputCompares.Add(baseInputCompare);
        //Sentido
        baseInputCompare = new BaseInputCompare
        {
            Nombre = "Sentido",
            ValuePDF = opinionCumplimientoSATPDF.Sentido,
            ValueQR = opinionCumplimientoSATQR.Sentido,
            Result = opinionCumplimientoSATQR.Sentido == opinionCumplimientoSATPDF.Sentido
        };
        baseInputCompares.Add(baseInputCompare);
        //Fecha
        baseInputCompare = new BaseInputCompare
        {
            Nombre = "Fecha",
            ValuePDF = opinionCumplimientoSATPDF.Fecha,
            ValueQR = opinionCumplimientoSATQR.Fecha,
            Result = opinionCumplimientoSATQR.Fecha == opinionCumplimientoSATPDF.Fecha
        };
        baseInputCompares.Add(baseInputCompare);

        validPDFResponse.BaseInputCompares = baseInputCompares;
        return validPDFResponse;
    }
    public OpinionCumplimientoSAT ParsearPDF(string textPDF)
    {
        OpinionCumplimientoSAT opinionCumplimientoSAT =  new OpinionCumplimientoSAT();

        string pattern = @"R.F.C.\r\n\s*(.*?)\s*\r\nRespuesta";

        Match match = Regex.Match(textPDF, pattern);
        if (match.Success)
        {
            string[] subcadenas = match.Groups[1].Value.Split(' ');
            opinionCumplimientoSAT.Folio = subcadenas[0];
            opinionCumplimientoSAT.RFC = subcadenas[1];
        }
        else
        {
            Console.WriteLine("No se encontró la cadena buscada");
        }
        string cadena = textPDF;
        Match ultimaPalabra = Regex.Match(cadena, @"\bPOSITIVO\b");
        if (ultimaPalabra.Success)
        {
            opinionCumplimientoSAT.Sentido = "Positiva";
        }
        else
        {
            opinionCumplimientoSAT.Sentido = "";
            Console.WriteLine("No se encontró la cadena buscada");
        }

        string regxFecha = @"\d{2}-\d{2}-\d{4}"; // patrón para coincidir con el formato de fecha "dd-mm-aaaa"

        Match matchFecha = Regex.Match(cadena, regxFecha);
        if (matchFecha.Success)
        {
            opinionCumplimientoSAT.Fecha = matchFecha.Value; // extrae el valor del primer grupo de coincidencia encontrado
        }
        else
        {
            Console.WriteLine("No se encontró la cadena buscada");
        }
        return opinionCumplimientoSAT;
    }
    public OpinionCumplimientoSAT ParsearQR(string textQR)
    {
        OpinionCumplimientoSAT opinionCumplimientoSAT = new OpinionCumplimientoSAT();

        Uri uri = new Uri(textQR);
        NameValueCollection queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);
       
        string value = queryParams["D3"];

        string[] subcadenas = value.Split('_');
        opinionCumplimientoSAT.Folio = subcadenas[0];
        opinionCumplimientoSAT.RFC = subcadenas[1];
        opinionCumplimientoSAT.Fecha = subcadenas[2];
        opinionCumplimientoSAT.Sentido= subcadenas[3] == "P" ? "Positiva" : "" ;

        return opinionCumplimientoSAT;
    }
}

internal class OpinionCumplimientoSAT
{
    public string RFC { get; set; }
    public string Folio { get; set; }
    public string Sentido { get; set; }
    public string Fecha { get; set; }

}