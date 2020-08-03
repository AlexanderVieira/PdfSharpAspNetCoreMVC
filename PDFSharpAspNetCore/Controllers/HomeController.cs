using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PDFSharpAspNetCore.Models;
using PdfSharpCore.Drawing;

namespace PDFSharpAspNetCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public FileResult GerarRelatorio()
        {
            using (var doc = new PdfSharpCore.Pdf.PdfDocument())
            {
                var page = doc.AddPage();
                page.Size = PdfSharpCore.PageSize.A4;
                page.Orientation = PdfSharpCore.PageOrientation.Portrait;
                var graphic = PdfSharpCore.Drawing.XGraphics.FromPdfPage(page);
                var corFonte = PdfSharpCore.Drawing.XBrushes.Black;

                var textFormatter = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphic);
                var fonteOrganizacao = new PdfSharpCore.Drawing.XFont("Arial", 10);
                var fonteDescricao = new PdfSharpCore.Drawing.XFont("Arial", 8, PdfSharpCore.Drawing.XFontStyle.BoldItalic);
                var tituloDetalhes = new PdfSharpCore.Drawing.XFont("Arial", 14, PdfSharpCore.Drawing.XFontStyle.Bold);
                var fonteDetalhesDescricao = new PdfSharpCore.Drawing.XFont("Arial", 7);

                var logo = @"C:\Users\Alexander\source\repos\PDFSharpAspNetCore\PDFSharpAspNetCore\wwwroot\images\transferir.jpg";

                var qtdPaginas = doc.PageCount;
                textFormatter.DrawString(qtdPaginas.ToString(), new PdfSharpCore.Drawing.XFont("Arial", 10), corFonte, new PdfSharpCore.Drawing.XRect(578, 825, page.Width, page.Height));

                //Impressão do Logo
                var imagem = XImage.FromFile(logo);
                graphic.DrawImage(imagem, 20, 5, 300, 50);

                // Titulo Exibição
                textFormatter.DrawString("Nome: ", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(20, 75, page.Width, page.Height));
                textFormatter.DrawString("Alexander Silva ", fonteOrganizacao, corFonte, new PdfSharpCore.Drawing.XRect(80, 75, page.Width, page.Height));

                textFormatter.DrawString("Profissão :", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(20, 95, page.Width, page.Height));
                textFormatter.DrawString("Desenvolvedor de Software", fonteOrganizacao, corFonte, new PdfSharpCore.Drawing.XRect(80, 95, page.Width, page.Height));

                textFormatter.DrawString("Tempo :", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(20, 115, page.Width, page.Height));
                textFormatter.DrawString("01 ano", fonteOrganizacao, corFonte, new PdfSharpCore.Drawing.XRect(80, 115, page.Width, page.Height));

                // Titulo Maior
                var tituloDetalhesMaior = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphic);
                tituloDetalhesMaior.Alignment = PdfSharpCore.Drawing.Layout.XParagraphAlignment.Center;
                tituloDetalhesMaior.DrawString("RELATÓRIO ", tituloDetalhes, corFonte, new PdfSharpCore.Drawing.XRect(0, 120, page.Width, page.Height));

                // Titulo Colunas
                var alturaTituloDetalhesY = 140;
                var detalhes = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphic);

                detalhes.DrawString("Descrição ", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(20, alturaTituloDetalhesY, page.Width, page.Height));
                detalhes.DrawString("Atendimento", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(144, alturaTituloDetalhesY, page.Width, page.Height));
                detalhes.DrawString("Operação", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(220, alturaTituloDetalhesY, page.Width, page.Height));
                detalhes.DrawString("Quantidade", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(290, alturaTituloDetalhesY, page.Width, page.Height));
                detalhes.DrawString("Status", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(337, alturaTituloDetalhesY, page.Width, page.Height));
                detalhes.DrawString("Data", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(400, alturaTituloDetalhesY, page.Width, page.Height));

                // Dados Relatório
                var alturaDetalhesItens = 160;
                for (int i = 1; i < 30; i++)
                {
                    textFormatter.DrawString("Descrição" + ": " + i.ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(21, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString("Atendimento" + " : " + i.ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(145, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString("Operação" + " : " + i.ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(215, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString("Quantidade" + " : " + i.ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(290, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString("Status" + " : " + i.ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(332, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString(DateTime.Now.ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(400, alturaDetalhesItens, page.Width, page.Height));

                    alturaDetalhesItens += 20;                    
                }

                using (var stream = new MemoryStream())
                {
                    var contentType = "application/pdf";
                    doc.Save(stream, false);
                    var nomeArquivo = "Relatorio.pdf";
                    return File(stream.ToArray(), contentType, nomeArquivo);
                }
            }
        }
    }
}
