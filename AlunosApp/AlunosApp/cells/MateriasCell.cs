using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlunosApp.cells
{
    public class MateriasCell : ViewCell
    {
        public MateriasCell()
        {
            var materiasNomeLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
            };

            materiasNomeLabel.SetBinding(Label.TextProperty, "Descricao");

            View = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    materiasNomeLabel
                },
            };
        }
    }
}
