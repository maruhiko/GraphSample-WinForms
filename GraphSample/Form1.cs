using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScottPlot;

namespace GraphSample
{
    public partial class Form1 : Form
    {
        // データ数
        // ※データ数は10万（100ミリ秒ごとのデータなら2.7時間分）程度なら問題ないが、100万になると描画が重い
        static int n = 100;

        // データ(y:データの値、x:時間の想定)
        double[] y = DataGen.RandomWalk(n);
        double[] x = Enumerable.Range(0, n).Select(i => (double)i).ToArray();

        // 描画に必要な値
        double currentTime = -100;  // 現在の時間
        double delta = 30;       // 現在から+-どれだけの範囲を表示するかの値
        double minX;             // 現在よりどれだけ過去まで表示するかの値
        double maxX;             // 現在よりどれだけ未来まで表示するかの値 
        ScottPlot.Plottable.VLine line;  // 現在に時間を示すバー

        public Form1()
        {
            InitializeComponent();
            UpdateValues();
            formsPlot1.Plot.AddScatter(x, y);
            line = formsPlot1.Plot.AddVerticalLine(currentTime);
            formsPlot1.Plot.Render();
        }

        void UpdateValues()
        {
            // 時間の更新(最後のデータで止まるようにする)
            currentTime = Math.Min(currentTime+1, x.Last());
            // 描画範囲値の更新
            minX = currentTime - delta;
            maxX = currentTime + delta;

        }


        private void timer1_Tick_1(object sender, EventArgs e)
        {
            // タイマーにより更新処理（今回は100ミリ秒ごと）

            UpdateValues();
            // 描画範囲の設定
            formsPlot1.Plot.SetAxisLimitsX(minX, maxX);
            // バーの位置の更新
            line.X = currentTime;
            // 再描画
            formsPlot1.Refresh();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 拡大は描画範囲を狭めることで実現
            delta = Math.Max(delta - 5, 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 縮小は描画範囲を広げることで実現
            delta += 5;
        }
    }
}
