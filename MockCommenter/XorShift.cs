using System;
using System.Diagnostics.Contracts;

namespace Project.Common
{
    /// <summary>乱数生成クラス(XorShift)</summary>
    /// <remarks>
    /// MicroSoft標準の乱数と同様に
    /// 扱えるように System.Random クラスから派生
    /// </remarks>
    public class XorShift : Random
    {
        /// <summary>整数を実数に変換 (1 / Int32.MaxValue)</summary>
        private const Double CONVERT_DOUBLE = 0.000000000465661287524579;
        /// <summary>内部変数 初期値</summary>
        private readonly UInt32 X_SEED = 521288629u;

        private UInt32 x;
        private UInt32 y;
        private UInt32 z;
        private UInt32 w;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public XorShift() : this((UInt64)DateTime.Now.Ticks) { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="seed">乱数シード値</param>
        /// <remarks>
        /// 念のためベースの内部変数を初期化しておく
        /// 特に必要でないので [: base(Environment.TickCount)]は
        /// 削除しても良い
        /// </remarks>
        public XorShift(UInt64 seed) : base(Environment.TickCount)
        {
            SetSeed(seed);
        }

        /// <summary>
        /// 乱数のシード値を設定
        /// </summary>
        /// <param name="seed">シード値</param>
        private void SetSeed(UInt64 seed)
        {
            // x,y,z,wがすべて0にならないようにする
            this.x = this.X_SEED;
            this.y = (UInt32)(seed >> 32) & 0xFFFFFFFF;
            this.z = (UInt32)(seed & 0xFFFFFFFF);
            this.w = this.x ^ this.z;
        }

        /// <summary>
        /// 乱数の生成(実数)
        /// </summary>
        /// <returns>乱数</returns>
        protected override Double Sample()
        {
            return (Math.Abs((Int32)XorShiftSample()) * CONVERT_DOUBLE);
        }

        /// <summary>
        /// 乱数の生成(符号無し整数)
        /// </summary>
        /// <returns>乱数</returns>
        private UInt32 XorShiftSample()
        {
            var t = this.x ^ (this.x << 11);
            this.x = this.y;
            this.y = this.z;
            this.z = this.w;
            this.w = (this.w ^ (this.w >> 19)) ^ (t ^ (t >> 8));
            return this.w;
        }

        /// <summary>
        /// 乱数を取得
        /// </summary>
        /// <returns>乱数</returns> 
        public override Int32 Next()
        {
            return Math.Abs((Int32)XorShiftSample());
        }

        /// <summary>
        /// 0　～ 最大値までの乱数を取得
        /// </summary>
        /// <param name="maxValue">最大値</param>
        /// <returns>乱数</returns> 
        public override Int32 Next(Int32 maxValue)
        {
            return Next(0, maxValue);
        }

        /// <summary>
        /// 指定範囲内の乱数を取得
        /// </summary>
        /// <param name="minValue">最小値</param>
        /// <param name="maxValue">最大値</param>
        /// <returns>乱数</returns>
        public Int32 Next(UInt32 minValue, UInt32 maxValue)
        {
            return (Int32)(minValue + Next() % ((maxValue - minValue)));
        }

        /// <summary>
        /// 指定範囲内の乱数を取得
        /// </summary>
        /// <param name="minValue">最小値</param>
        /// <param name="maxValue">最大値</param>
        /// <returns>乱数</returns> 
        public override Int32 Next(Int32 minValue, Int32 maxValue)
        {
            return (minValue + Next() % ((maxValue - minValue)));
        }

        /// <summary>
        /// Fills the byte array with random bytes [0..0x7f].  The entire array is filled.
        /// </summary>
        /// <param name="buffer">the array to be filled</param>
        /// <remarks>MSDNソースまま。乱数生成のみ変更</remarks>
        public override void NextBytes(Byte[] buffer)
        {
            if (buffer == null) { throw new ArgumentNullException(nameof(buffer)); }
            Contract.EndContractBlock();
            for (var i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (Byte)(XorShiftSample() % (Byte.MaxValue + 1));
            }
        }

        /// <summary>
        /// 乱数を取得
        /// </summary>
        /// <returns>乱数</returns> 
        public override Double NextDouble()
        {
            return Sample();
        }

        /// <summary>指定範囲内の乱数を取得</summary>
        /// <param name="minValue">最小値</param>
        /// <param name="maxValue">最大値</param>
        /// <returns>乱数</returns>
        public Double NextDouble(Double minValue, Double maxValue)
        {
            //( 最大値 - 最小値 ) * 乱数(0～1) + 最小値
            return (maxValue - minValue) * NextDouble() + minValue;
        }
    }
}
