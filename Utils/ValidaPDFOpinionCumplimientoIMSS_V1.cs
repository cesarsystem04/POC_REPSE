using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using FluentValidation;

namespace POC_REPSE.Utils;
internal class ValidaPDFOpinionCumplimientoIMSS_V1
{
    private CumplimientoImss cumplimientoImss { get; set; }

    public ValidaPDFOpinionCumplimientoIMSS_V1(RequestPDF requestPDF)
    {
        TransformaRequest(requestPDF);
    }

    private void TransformaRequest(RequestPDF requestPDF)
    {
        cumplimientoImss = new CumplimientoImss();
        ParsearPDF(requestPDF.FileText);
        ParsearQR(requestPDF.QRReader);
    }

    private void ValidaOpinionCumplimientoIMSS()
    {

    }

    public ValidPDFResponse Validar()
    {
        ValidPDFResponse oValidPDFResponse = new ValidPDFResponse();
        List<BaseInputCompare> baseInputCompares = new List<BaseInputCompare>();

        //CumplimientoImssValidator validador = new CumplimientoImssValidator();
        //var result = validador.Validate(cumplimientoImss);

        BaseInputCompare baseInputCompare;

        baseInputCompare = new BaseInputCompare
        {
            Nombre = "Razon_Social",
            ValuePDF = cumplimientoImss.OpinionCumplimientoImss_PDF.Razon_Social,
            ValueQR = cumplimientoImss.OpinionCumplimientoImss_QR.Razon_Social,
            Result = cumplimientoImss.OpinionCumplimientoImss_PDF.Razon_Social == cumplimientoImss.OpinionCumplimientoImss_QR.Razon_Social
        };
        baseInputCompares.Add(baseInputCompare);


        baseInputCompare = new BaseInputCompare
        {
            Nombre = "RFC",
            ValuePDF = cumplimientoImss.OpinionCumplimientoImss_PDF.RFC,
            ValueQR = cumplimientoImss.OpinionCumplimientoImss_QR.RFC,
            Result = cumplimientoImss.OpinionCumplimientoImss_PDF.RFC == cumplimientoImss.OpinionCumplimientoImss_QR.RFC
        };
        baseInputCompares.Add(baseInputCompare);

        baseInputCompare = new BaseInputCompare
        {
            Nombre = "Folio",
            ValuePDF = cumplimientoImss.OpinionCumplimientoImss_PDF.Folio,
            ValueQR = cumplimientoImss.OpinionCumplimientoImss_QR.Folio,
            Result = cumplimientoImss.OpinionCumplimientoImss_PDF.Folio == cumplimientoImss.OpinionCumplimientoImss_QR.Folio
        };
        baseInputCompares.Add(baseInputCompare);


        baseInputCompare = new BaseInputCompare
        {
            Nombre = "Opinion",
            ValuePDF = cumplimientoImss.OpinionCumplimientoImss_PDF.Opinion,
            ValueQR = cumplimientoImss.OpinionCumplimientoImss_QR.Opinion,
            Result = cumplimientoImss.OpinionCumplimientoImss_PDF.Opinion.ToLower() == cumplimientoImss.OpinionCumplimientoImss_QR.Opinion.ToLower()
        };
        baseInputCompares.Add(baseInputCompare);

        baseInputCompare = new BaseInputCompare
        {
            Nombre = "Fecha",
            ValuePDF = cumplimientoImss.OpinionCumplimientoImss_PDF.Fecha,
            ValueQR = cumplimientoImss.OpinionCumplimientoImss_QR.Fecha,
            Result = cumplimientoImss.OpinionCumplimientoImss_PDF.Fecha == cumplimientoImss.OpinionCumplimientoImss_QR.Fecha
        };
        baseInputCompares.Add(baseInputCompare);

        baseInputCompare = new BaseInputCompare
        {
            Nombre = "Sentido",
            ValuePDF = cumplimientoImss.OpinionCumplimientoImss_PDF.Sentido,
            ValueQR = cumplimientoImss.OpinionCumplimientoImss_QR.Sentido,
            Result = cumplimientoImss.OpinionCumplimientoImss_PDF.Sentido == cumplimientoImss.OpinionCumplimientoImss_QR.Sentido
        };
        baseInputCompares.Add(baseInputCompare);

        oValidPDFResponse.BaseInputCompares = baseInputCompares;
        return oValidPDFResponse;
    }


    public void ParsearPDF(string texto_Entrada)
    {
        OpinionCumplimientoImss opinionCumplimientoImss = new OpinionCumplimientoImss();
        //string patron = @"Estimado Patrón:.\w(?<Folio>[0-9]{22})|(?<ClaveRFC>[A-Z&Ñ]{3,4}[0-9]{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])[A-Z0-9]{2}[0-9A])|(n(?<RazonSocial>[a-zA-Z]+).\w+Clave de R.F.C.)|(se emite opinión\s(?<Opinion>\w+))|Revisión practicada el día\s(?<Fecha>[a-zA-Z0-9 ]+)|se encuentra\s(?<Sentido>[a-zA-z ]+.,)";

        Regex regex = new Regex("((?<RazonSocial>[a-zA-Z]+)\\r\\nClave de R.F.C.)", RegexOptions.IgnoreCase);
        if (regex.Match(texto_Entrada).Groups["RazonSocial"].Success)
        {
            opinionCumplimientoImss.Razon_Social = regex.Match(texto_Entrada).Groups["RazonSocial"].Value;
        }


        regex = new Regex(@"(?<ClaveRFC>[A-Z&Ñ]{3,4}[0-9]{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])[A-Z0-9]{2}[0-9A])", RegexOptions.IgnoreCase);
        if (regex.Match(texto_Entrada).Groups["ClaveRFC"].Success)
        {
            opinionCumplimientoImss.RFC = regex.Match(texto_Entrada).Groups["ClaveRFC"].Value;
        }

        regex = new Regex("Estimado Patrón:\r\n(?<Folio>[0-9]{22})", RegexOptions.IgnoreCase);
        if (regex.Match(texto_Entrada).Groups["Folio"].Success)
        {
            opinionCumplimientoImss.Folio = regex.Match(texto_Entrada).Groups["Folio"].Value;
        }

        regex = new Regex(@"(se emite opinión\s(?<Opinion>\w+))", RegexOptions.IgnoreCase);
        if (regex.Match(texto_Entrada).Groups["Opinion"].Success)
        {
            opinionCumplimientoImss.Opinion = regex.Match(texto_Entrada).Groups["Opinion"].Value;
        }

        regex = new Regex(@"Revisión practicada el día\s(?<Fecha>[a-zA-Z0-9 ]+)", RegexOptions.IgnoreCase);
        if (regex.Match(texto_Entrada).Groups["Fecha"].Success)
        {
            opinionCumplimientoImss.Fecha = regex.Match(texto_Entrada).Groups["Fecha"].Value;
        }

        regex = new Regex("se encuentra\\s(?<Sentido>al\r\n[a-zA-z ]+.,)", RegexOptions.IgnoreCase);
        if (regex.Match(texto_Entrada).Groups["Sentido"].Success)
        {
            opinionCumplimientoImss.Sentido = regex.Match(texto_Entrada).Groups["Sentido"].Value.Replace("\r\n", " ");
        }


        cumplimientoImss.OpinionCumplimientoImss_PDF = opinionCumplimientoImss;
    }

    public void ParsearQR(string texto_Entrada)
    {
        OpinionCumplimientoImss opinionCumplimientoImss = new OpinionCumplimientoImss();
        //string patronCadenaQR = @"Nombre o Razon Social:(?<RazonSocial>[a-zA-Z]+)\w|RFC:(?<RFC>[A-Z&Ñ]{3,4}[0-9]{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])[A-Z0-9]{2}[0-9A])|Folio:(?<Folio>[1-9]+)|Opinion:(?<Opinion>[a-zA-Z]+)|Fecha:(?<Fecha>[a-zA-Z0-9 ]+)";

        Regex regex = new Regex("Social:(?<RazonSocial>[a-zA-Z]+)\\w", RegexOptions.IgnoreCase);
        if (regex.Match(texto_Entrada).Groups["RazonSocial"].Success)
        {
            opinionCumplimientoImss.Razon_Social = regex.Match(texto_Entrada).Groups["RazonSocial"].Value;
        }

        regex = new Regex("RFC:(?<RFC>[A-Z&Ñ]{3,4}[0-9]{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])[A-Z0-9]{2}[0-9A])", RegexOptions.IgnoreCase);
        if (regex.Match(texto_Entrada).Groups["RFC"].Success)
        {
            opinionCumplimientoImss.RFC = regex.Match(texto_Entrada).Groups["RFC"].Value;
        }

        regex = new Regex("Folio:(?<Folio>[1-9]+)", RegexOptions.IgnoreCase);
        if (regex.Match(texto_Entrada).Groups["Folio"].Success)
        {
            opinionCumplimientoImss.Folio = regex.Match(texto_Entrada).Groups["Folio"].Value;
        }

        regex = new Regex("Opinion:(?<Opinion>[a-zA-Z]+)", RegexOptions.IgnoreCase);
        if (regex.Match(texto_Entrada).Groups["Opinion"].Success)
        {
            opinionCumplimientoImss.Opinion = regex.Match(texto_Entrada).Groups["Opinion"].Value;
        }

        regex = new Regex("Fecha:(?<Fecha>[a-zA-Z0-9 ]+)", RegexOptions.IgnoreCase);
        if (regex.Match(texto_Entrada).Groups["Fecha"].Success)
        {
            opinionCumplimientoImss.Fecha = regex.Match(texto_Entrada).Groups["Fecha"].Value;
        }


        cumplimientoImss.OpinionCumplimientoImss_QR = opinionCumplimientoImss;



    }


    public class CumplimientoImssValidator : AbstractValidator<CumplimientoImss>
    {
        public CumplimientoImssValidator()
        {
            //RuleFor(x => x.OpinionCumplimientoImss_QR.Folio == x.OpinionCumplimientoImss_QR.Folio).WithMessage("");
            RuleFor(x => x.OpinionCumplimientoImss_QR.Razon_Social == x.OpinionCumplimientoImss_PDF.Fecha).Must(y => y == true).WithMessage("La razón social no es igual");
            RuleFor(x => x.OpinionCumplimientoImss_QR.Folio == x.OpinionCumplimientoImss_PDF.Folio).Must(y => y == true).WithMessage("El Folio no es igual");
            RuleFor(x => x.OpinionCumplimientoImss_QR.Fecha == x.OpinionCumplimientoImss_PDF.Fecha).Must(y => y == true).WithMessage("La Fecha no es igual");
        }

    }


    internal class OpinionCumplimientoImss
    {
        public string Razon_Social { get; set; }
        public string RFC { get; set; }
        public string Folio { get; set; }
        public string Opinion { get; set; }
        public string Fecha { get; set; }
        public string Sentido { get; set; }

        public OpinionCumplimientoImss()
        {
            this.Razon_Social = String.Empty;
            this.RFC = String.Empty;
            this.Folio = String.Empty;
            this.Opinion = String.Empty;
            this.Fecha = String.Empty;
            this.Sentido = String.Empty;
        }
    }


    internal class CumplimientoImss
    {
        public OpinionCumplimientoImss OpinionCumplimientoImss_PDF { get; set; }
        public OpinionCumplimientoImss OpinionCumplimientoImss_QR { get; set; }
    }


}





