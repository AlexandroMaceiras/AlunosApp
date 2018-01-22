using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlunosApp.cells
{
   public class notaCell : ViewCell
    {
        public notaCell()
        {
            var fotoEstudanteImagem = new Image
            {
                HeightRequest = 100,
                WidthRequest = 100,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
            };

            fotoEstudanteImagem.SetBinding(Image.SourceProperty, "Estudante.PhotoFullPath");


            var alunoNomeLabel = new Label
            {
                
                FontSize = 20,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
            };

            alunoNomeLabel.SetBinding(Label.TextProperty, "Estudante.NomeCompleto");


            var notaEntry = new Entry
            {
                Keyboard = Keyboard.Numeric,
                HorizontalTextAlignment = TextAlignment.End,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
            };

            notaEntry.SetBinding(Entry.TextProperty, "Nota");


            View = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    fotoEstudanteImagem,
                    alunoNomeLabel,
                    notaEntry
                },
            };

        }
    }
}
