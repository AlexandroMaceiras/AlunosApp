using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlunosApp.cells
{
    public class alunosMatriculadosCell : ViewCell
    {
        public alunosMatriculadosCell()
        {
            var fotoProfImagem = new Image
            {
                HeightRequest = 150,
                WidthRequest = 150,
            };

            fotoProfImagem.SetBinding(Image.SourceProperty, "professor.PhotoFullPath");

            var materiasNomeLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
            };

            materiasNomeLabel.SetBinding(Label.TextProperty, "Descricao");

            var ProfNomeLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                
            };

            ProfNomeLabel.SetBinding(Label.TextProperty, "professor.NomeCompleto");



            var NotaLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontAttributes = FontAttributes.Bold,
            };

            NotaLabel.SetBinding(Label.TextProperty, "Nota", stringFormat: "Final: {0:N2}");



            var esq = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    fotoProfImagem
                },
            };

            var dir = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    materiasNomeLabel, ProfNomeLabel, NotaLabel
                },
            };

            View = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    esq, dir
                },
            };

        }


    }
}
