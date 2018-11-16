using MessagingToolkit.QRCode.Codec.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using Emgu.CV;
using Emgu.CV.Structure;


namespace QRcode
{
    public partial class Form1 : Form
    {
        

        //Frame
        Image<Bgr, byte> current;
        //Webcam
        VideoCapture webcam = new VideoCapture();

        public Form1()
        {
            InitializeComponent();
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnEncode_Click(object sender, EventArgs e)
        {
            using(SaveFileDialog sfd = new SaveFileDialog() { Filter = "JPEG|*.jpg", ValidateNames = true })
            {
                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    MessagingToolkit.QRCode.Codec.QRCodeEncoder encoder = new MessagingToolkit.QRCode.Codec.QRCodeEncoder();
                    encoder.QRCodeScale = 8;
                    Bitmap bmp = encoder.Encode(txtEncode.Text);
                    pictureBox2.Image = bmp;
                    bmp.Save(sfd.FileName, ImageFormat.Jpeg);
                }
            }
        }

        private void btnDecode_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG|*.jpg", ValidateNames=true, Multiselect=false })
            {
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox2.Image = Image.FromFile(ofd.FileName);
                    MessagingToolkit.QRCode.Codec.QRCodeDecoder decoder = new MessagingToolkit.QRCode.Codec.QRCodeDecoder();
    
                    try
                    {
                        txtDecode.Text = decoder.Decode(new QRCodeBitmapImage(pictureBox2.Image as Bitmap));
                    }
                    catch (Exception ex)
                    {
                        txtDecode.Text = "Text Pattern not found. NOT A QR CODE (?)";
                    }
                    
                }
            }
        }
        private void processCamera(object sender, EventArgs e)
        {
            current = webcam.QuerySmallFrame().ToImage<Bgr, Byte>();

            // Flip because forecam is MIRRORED
            //current = current.Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);
            pictureBox1.Image = current.Bitmap;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void camStart_Click(object sender, EventArgs e)
        {
            //run until close
            Application.Idle += new EventHandler(processCamera);
        }

        private void capture_Click(object sender, EventArgs e)
        {
            //Get the current frame
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Image = pictureBox1.Image;

            MessagingToolkit.QRCode.Codec.QRCodeDecoder decoder = new MessagingToolkit.QRCode.Codec.QRCodeDecoder();
            try
            {
                txtDecode.Text = decoder.Decode(new QRCodeBitmapImage(pictureBox2.Image as Bitmap));
            }
            catch (Exception ex)
            {
                txtDecode.Text = "Text Pattern not found. NOT A QR CODE (?)";
            }
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}
