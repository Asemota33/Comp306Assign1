using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Lab1Comp306MichaelAsemota
{
    /// <summary>
    /// Interaction logic for ObjectLevelOperations.xaml
    /// </summary>
    public partial class ObjectLevelOperations : Window
    {
        public ObjectLevelOperations()
        {
            InitializeComponent();
            populateDropDownBox();
            ObjectDataGrid.ItemsSource = objectsFromBucket;
        }

        ObservableCollection<S3Object> objectsFromBucket = new ObservableCollection<S3Object>();

        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;

        private void Bucket_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            populateObjectGrid();
        }

        private async void populateDropDownBox()
        {
            var s3Client = new AmazonS3Client(Constants.ACCESSKEY, Constants.SECRETKEY, bucketRegion);
            var buckets = await s3Client.ListBucketsAsync();
            foreach (var bucket in buckets.Buckets)
            {
                BucketDropDown.Items.Add(new ComboBoxItem().Content = bucket.BucketName);
            }
        }

        private static async Task UploadFileAsync(string filePath, string bucketName)
        {
            var s3Client = new AmazonS3Client(Constants.ACCESSKEY, Constants.SECRETKEY, bucketRegion);

            try
            {
                var fileTransferUtility = new TransferUtility(s3Client);
                await fileTransferUtility.UploadAsync(filePath, bucketName);
            } catch(AmazonS3Exception e)
            {
                MessageBox.Show(e.Message);

            } catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = GetFileDirectory();
            Nullable<bool> Result = openFileDialog.ShowDialog();
            if (Result == true)
            {
                this.NewObjectFileTextBox.Text = openFileDialog.FileName;
                this.UploadButton.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Please select a file.");
                return;
            }
        }

        private string GetFileDirectory()
        {
            string[] List = Environment.CurrentDirectory.Split(
                System.IO.Path.DirectorySeparatorChar);
            string currPath = string.Empty;

            for (int i = 0; i < List.Length - 2; i++)
            {
                currPath = currPath + List[i] + "\\";
            }

            return currPath + "Files\\";
        }

        private async void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string file = NewObjectFileTextBox.Text;
                await UploadFileAsync(file, BucketDropDown.SelectedItem.ToString());
                populateObjectGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while processing request " + ex.Message);
            }
        }

        private async void populateObjectGrid()
        {
            var s3Client = new AmazonS3Client(Constants.ACCESSKEY, Constants.SECRETKEY, bucketRegion);
            objectsFromBucket.Clear();

            if (BucketDropDown.SelectedIndex != -1)
            {
                var objects = await s3Client.ListObjectsAsync(BucketDropDown.SelectedItem.ToString());
                if (objects != null)
                {
                    var objectList = objects.S3Objects.Select(x => new S3Object { Name = x.Key, Size = x.Size });
                    foreach (var item in objectList)
                    {
                        objectsFromBucket.Add(item);
                    }
                }

            }
        }
    }
}
