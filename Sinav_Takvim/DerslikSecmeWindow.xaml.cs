using System.IO;
using System.Windows;
using System.Windows.Controls;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Sinav_Takvim;

public partial class DerslikSecmeWindow
{
    private Sinav sinav;

    private List<Ogrenci> ogrenciList = [];
    
    private Dictionary<int, (List<Ogrenci>, Derslik)> ogrenciDic = new();
    
    private OgrenciDersDAO ogrenciDersDAO = new();
    
    public DerslikSecmeWindow(Sinav sinav, List<Ogrenci> ogrenciList)
    {
        this.sinav = sinav;
        this.ogrenciList = ogrenciList;
        InitializeComponent();
        BaslikTextBlock.Text = sinav.GetDers().GetAd() + " Dersinin Sınavı";
        DerslikButonlarCiz();
    }

    private void DerslikButonlarCiz()
    {
        var derslikListesi = sinav.GetDerslikler();
        DerslikButtonGrid.Columns = derslikListesi.Count;
            
        int a = 0;
        foreach (var derslik in derslikListesi)
        {
            var button = new Button
            {
                Content = derslik.GetAd(),
                Height = 100,
                Width = 200,
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Center,
                Tag = a
            };
            button.Click += DerslikButton_Click;
            
            DerslikButtonGrid.Children.Add(button);

            List<Ogrenci> list = [];
            
            int donguSayisi = ogrenciList.Count < derslik.GetKapasite() ? ogrenciList.Count : derslik.GetKapasite();

            for (var i = 0; i < donguSayisi; i++)
            {
                list.Add(ogrenciList[i]);
            }
            ogrenciList.RemoveAll(x => list.Contains(x));

            ogrenciDic[a] = (list, derslik);
            
            a++;
        }
        
        a = 0;
        foreach (var derslik in derslikListesi)
        {
            var button = new Button
            {
                Content = derslik.GetAd() + " için\nPDF Oluştur",
                Height = 100,
                Width = 200,
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Center,
                Tag = a
            };
            button.Click += pdfOlusturButton_Click;
            
            DerslikButtonGrid.Children.Add(button);
            
            a++;
        }
    }
    
    private void DerslikButton_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;

        var derslikNumara = int.Parse(button.Tag.ToString() ?? throw new InvalidOperationException());
        
        var list = ogrenciDic[derslikNumara].Item1;
        
        var derslik = ogrenciDic[derslikNumara].Item2;

        var window = new DerslikGoruntuleWindow(derslik, list);
        window.Show();
    }

    private void pdfOlusturButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var button = (Button)sender;

            var derslikNumara = int.Parse(button.Tag.ToString() ?? throw new InvalidOperationException());

            var derslikOgrenciList = ogrenciDic[derslikNumara].Item1;

            var derslik = ogrenciDic[derslikNumara].Item2;

            string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
                ?.Parent?.Parent?.Parent?.FullName ?? string.Empty;

            string folderPath = Path.Combine(projectRoot, "files");
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, $"{sinav.GetDers().GetKod()}_{derslik.GetAd()}.pdf");

            var doc = new Document(PageSize.A4.Rotate());
            var writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

            doc.Open();

            var cb = writer.DirectContent;

            float startX = 20;
            float startY = 500;

            var satir = derslik.GetSatir();
            var sutun = derslik.GetSutun();
            var sira_yapisi = derslik.GetSira_yapisi();
            var gercek_sutun = sutun * sira_yapisi + (sutun - 1);

            var rectWidth = (float)(815.0 / gercek_sutun);
            var rectHeight = (float)50;

            int siraNumber = 0;

            int a = 0;

            int atlamaRakam = sira_yapisi == 5 ? 2 : sira_yapisi / 2 + 1;

            var fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            var bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            var trFont = new Font(bf, 8);

            for (var i = 0; i < satir; i++)
            {
                for (var j = 0; j < gercek_sutun; j++)
                {

                    float x = startX + j * rectWidth;
                    float y = startY - i * rectHeight;


                    if ((j + 1) % (sira_yapisi + 1) != 0)
                    {
                        //Rectangle çiz
                        cb.Rectangle(x, y, rectWidth, rectHeight);
                        cb.Stroke();

                        if (siraNumber % atlamaRakam == 0 && !(a >= derslikOgrenciList.Count))
                        {
                            //Sıraya öğrenci oturacaksa üstüne yazı yaz
                            string text = "\n" + derslikOgrenciList[a].GetAd();

                            ColumnText ct = new ColumnText(cb);
                            ct.SetSimpleColumn(
                                new Phrase(text, trFont),
                                x, y,
                                x + rectWidth, y + rectHeight,
                                12,
                                Element.ALIGN_CENTER
                            );
                            ct.Go();

                            a++;
                        }

                        siraNumber++;
                    }
                    else
                    {
                        //Sütunlar arası boşluk
                        siraNumber = 0;
                    }
                }

                siraNumber = 0;
            }

            doc.Close();
            
            MessageBox.Show(
                $"PDF oluşturma başarılı! Dosya konumu:\n{filePath}",
                "Uyarı",
                MessageBoxButton.OK);
        }
        catch (IOException ex)
        {
            MessageBox.Show(
                "Dosya başka bir program tarafından kullanılıyor!",
                "Uyarı",
                MessageBoxButton.OK);
        }
    }
}