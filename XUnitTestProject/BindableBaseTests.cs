using System;
using System.Collections.Generic;
using System.Text;

namespace SVCalendar.Tests
{
    using System.ComponentModel;

    using SVCalendar.WPF;

    using Xunit;

    public class BindableBaseTests
    {
        [Fact]
        public void ShouldRisePropertyChangedUsingSetProperty()
        {
            var mockClass = new MockViewModel();
            
            Assert.PropertyChanged(mockClass, nameof(mockClass.MockVariable1), () => mockClass.MockVariable1 = 1);
        }

        [Fact]
        public void ShouldNotRisePropertyChangedIfNewValueSameAsBeforeWhenUsingSetProperty()
        {
            var mockClass = new MockViewModel();
            mockClass.MockVariable1 = 1;

            var receivedEvents = new List<string>();

            mockClass.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
                {
                    receivedEvents.Add(e.PropertyName);
                };

            mockClass.MockVariable1 = 1;

            Assert.Empty(receivedEvents);
        }

        [Fact]
        public void ShouldRisePropertyChangedUsingOnPropertyChanged()
        {
            var mockClass = new MockViewModel { MockVariable2 = 1 };

            Assert.PropertyChanged(mockClass, nameof(mockClass.MockVariable2), () => mockClass.MockVariable2 = 33);
        }
    }

    internal class MockViewModel : BindableBase
    {
        private int mockVariable1;

        private int mockVariable2;

        public int MockVariable1
        {
            get => mockVariable1;
            set => SetProperty(ref mockVariable1, value);
        }

        public int MockVariable2
        {
            get => mockVariable2;
            set
            {
                mockVariable2 = value;
                OnPropertyChanged();
            }
        }
    }
}
