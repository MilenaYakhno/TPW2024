using System;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Model
{
    internal class ModelBall : IBall, IDisposable
    {
        private double _scaleFactor = 1.0;

        public ModelBall()
        {
            Top = 0;
            Left = 0;
            Diameter = 0;
        }

        public double Top
        {
            get { return TopBackingField; }
            private set
            {
                if (TopBackingField == value)
                    return;
                TopBackingField = value * _scaleFactor;
                RaisePropertyChanged();
            }
        }

        public double Left
        {
            get { return LeftBackingField; }
            private set
            {
                if (LeftBackingField == value)
                    return;
                LeftBackingField = value * _scaleFactor;
                RaisePropertyChanged();
            }
        }

        public double Diameter
        {
            get { return DiameterBackingField; }
            internal set
            {
                if (DiameterBackingField == value)
                    return;
                DiameterBackingField = value * _scaleFactor;
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
        }

        private double TopBackingField;
        private double LeftBackingField;
        private double DiameterBackingField;

        public void UpdatePosition(Vector2 newPosition)
        {
            Top = newPosition.Y - (Diameter / 2);
            Left = newPosition.X - (Diameter / 2);
        }

        public void SetScaleFactor(double scaleFactor)
        {
            _scaleFactor = scaleFactor;
            RaisePropertyChanged(nameof(Top));
            RaisePropertyChanged(nameof(Left));
            RaisePropertyChanged(nameof(Diameter));
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
