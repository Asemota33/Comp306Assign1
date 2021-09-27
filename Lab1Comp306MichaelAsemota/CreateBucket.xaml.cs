using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lab1Comp306MichaelAsemota
{
    /// <summary>
    /// Interaction logic for CreateBucket.xaml
    /// </summary>
    public partial class CreateBucket : Window
    {
        public CreateBucket()
        {
            InitializeComponent();
            populateTable();
            BucketsGrid.ItemsSource = bucketsFromS3;
        }
        ObservableCollection<Amazon.S3.Model.S3Bucket> bucketsFromS3 = new ObservableCollection<Amazon.S3.Model.S3Bucket>();

        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.mainWindow.Show();
            this.Hide();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var s3Client = new AmazonS3Client(Constants.ACCESSKEY, Constants.SECRETKEY, bucketRegion);
                var response = await s3Client.PutBucketAsync(new PutBucketRequest
                {
                    BucketName = bucketNameInput.Text
                });
                populateTable();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private async void populateTable()
        {
            var s3Client = new AmazonS3Client(Constants.ACCESSKEY, Constants.SECRETKEY, bucketRegion);
            var buckets = await s3Client.ListBucketsAsync();
            foreach (var bucket in buckets.Buckets)
            {
                if (bucketsFromS3.Count(b => b.BucketName == bucket.BucketName) == 0)
                {
                    bucketsFromS3.Add(bucket);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
