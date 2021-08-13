using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Lab02_04
{
    class KhachHang
    {
        private long stk;
        private string hoTen;
        private string diaChi;
        private double soTien;

        public long Stk { get => stk; set => stk = value; }
        public string HoTen { get => hoTen; set => hoTen = value; }
        public string DiaChi { get => diaChi; set => diaChi = value; }
        public double SoTien { get => soTien; set => soTien = value; }

        public KhachHang()
        {
        }

        public KhachHang(long stk, string hoTen, string diaChi, double soTien)
        {
            this.stk = stk;
            this.hoTen = hoTen;
            this.diaChi = diaChi;
            this.soTien = soTien;
        }

        public string GetCurrencyFormat()
        {
            return GetCurrencyFormat(SoTien);
        }
        public static string GetCurrencyFormat(double soTien)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            return double.Parse(soTien.ToString()).ToString("#,###", cul.NumberFormat) + "đ";
        }
    }
}
