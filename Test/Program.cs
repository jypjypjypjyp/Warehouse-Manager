using System;

namespace Test
{
    public static class CurrencyToChineseConverter
    {
        private static readonly string[] numberP = new string[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖",//9
            "", "拾", "佰", "仟",//13
            "", "万", "亿"};//16
        private static readonly string[] unitP = new string[] { "整", "元", "角", "分" };
        public static object Convert(object value)
        {
            try
            {
                int[,] nums = new int[4, 4];
                bool isInteger;
                var x = (double)value;
                double step = 0.01;
                // Fractional part
                nums[0, 0] = (int)((long)(x / step) % 10);
                step *= 10;
                nums[0, 1] = (int)((long)(x / step) % 10);
                step *= 10;
                isInteger = nums[0, 0] == 0 && nums[0, 1] == 0;
                // Integral part
                for (int i = 1; i <= 3; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (step > x) goto Convert;
                        nums[i, j] = (int)((long)(x / step) % 10);
                        step *= 10;
                    }
                }
            Convert:
                // Fractional part
                string fraS = "";
                if (!isInteger)
                {
                    if (nums[0, 1] != 0)
                        fraS += numberP[nums[0, 1]] + unitP[2];
                    if (nums[0, 0] != 0)
                        fraS += numberP[nums[0, 0]] + unitP[3];
                }
                else
                {
                    fraS = unitP[0];
                }
                // Integral part
                string intS = "";
                for (int i = 3; i > 0; i--)
                {
                    bool hasNum = false;
                    bool needZero = false;
                    for (int j = 3; j >= 0; j--)
                    {
                        if (nums[i, j] != 0)
                        {
                            hasNum = true;
                            intS += (intS != "" && needZero ? numberP[0] : "") + numberP[nums[i, j]] + numberP[10 + j];
                            needZero = false;
                        }
                        else
                        {
                            needZero = true;
                        }
                    }
                    if (hasNum)
                        intS += numberP[13 + i];
                }
                return (intS == "" ? "" : intS) + unitP[1] + fraS;
            }
            catch
            {
                return "错误";
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var a = Console.ReadLine();
            string b = (string)CurrencyToChineseConverter.Convert(double.Parse(a));
            Console.WriteLine(b);
        }
    }
}
