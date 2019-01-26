﻿//------------------------------------------------------------------------------
//	<auto-generated>
//		This code was generated from a template.
//		Manual changes to this file will be overwritten if the code is regenerated.
//	</auto-generated>
//------------------------------------------------------------------------------

#if USE_INTRINSICS
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Runtime.CompilerServices;

namespace SauceControl.Blake2Fast
{
	unsafe internal partial struct Blake2sContext
	{
		private static readonly byte[] rormask = new byte[] {
			1, 2, 3, 0, 5, 6, 7, 4, 9, 10, 11, 8, 13, 14, 15, 12, //r8
			2, 3, 0, 1, 6, 7, 4, 5, 10, 11, 8, 9, 14, 15, 12, 13 //r16
		};

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> ror32_12(Vector128<uint> x) =>
			Sse2.Xor(Sse2.ShiftRightLogical(x, 12), Sse2.ShiftLeftLogical(x, 20));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> ror32_7(Vector128<uint> x) =>
			Sse2.Xor(Sse2.ShiftRightLogical(x, 7), Sse2.ShiftLeftLogical(x, 25));

#if OLD_INTRINSICS
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> ror32_shuffle(Vector128<uint> x, Vector128<sbyte> y) =>
			Sse.StaticCast<sbyte, uint>(Ssse3.Shuffle(Sse.StaticCast<uint, sbyte>(x), y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> blend_uint(Vector128<uint> x, Vector128<uint> y, byte m) =>
			Sse.StaticCast<ushort, uint>(Sse41.Blend(Sse.StaticCast<uint, ushort>(x), Sse.StaticCast<uint, ushort>(y), m));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> unpackhigh64_uint(Vector128<uint> x, Vector128<uint> y) =>
			Sse.StaticCast<ulong, uint>(Sse2.UnpackHigh(Sse.StaticCast<uint, ulong>(x), Sse.StaticCast<uint, ulong>(y)));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> unpacklow64_uint(Vector128<uint> x, Vector128<uint> y) =>
			Sse.StaticCast<ulong, uint>(Sse2.UnpackLow(Sse.StaticCast<uint, ulong>(x), Sse.StaticCast<uint, ulong>(y)));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> shuffle2_uint(Vector128<uint> x, Vector128<uint> y, byte m) =>
			Sse.StaticCast<float, uint>(Sse.Shuffle(Sse.StaticCast<uint, float>(x), Sse.StaticCast<uint, float>(y), m));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> shufflehigh_uint(Vector128<uint> x, byte m) =>
			Sse.StaticCast<ushort, uint>(Sse2.ShuffleHigh(Sse.StaticCast<uint, ushort>(x), m));
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> ror32_shuffle(Vector128<uint> x, Vector128<sbyte> y) =>
			Ssse3.Shuffle(x.AsSByte(), y).AsUInt32();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> blend_uint(Vector128<uint> x, Vector128<uint> y, byte m) =>
			Sse41.Blend(x.AsUInt16(), y.AsUInt16(), m).AsUInt32();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> unpackhigh64_uint(Vector128<uint> x, Vector128<uint> y) =>
			Sse2.UnpackHigh(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> unpacklow64_uint(Vector128<uint> x, Vector128<uint> y) =>
			Sse2.UnpackLow(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> shuffle2_uint(Vector128<uint> x, Vector128<uint> y, byte m) =>
			Sse.Shuffle(x.AsSingle(), y.AsSingle(), m).AsUInt32();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> shufflehigh_uint(Vector128<uint> x, byte m) =>
			Sse2.ShuffleHigh(x.AsUInt16(), m).AsUInt32();
#endif

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void g1(ref Vector128<uint> row1, ref Vector128<uint> row2, ref Vector128<uint> row3, ref Vector128<uint> row4, in Vector128<uint> b0, in Vector128<sbyte> r16)
		{
			row1 = Sse2.Add(Sse2.Add(row1, b0), row2);
			row4 = Sse2.Xor(row4, row1);
			row4 = ror32_shuffle(row4, r16);
			row3 = Sse2.Add(row3, row4);
			row2 = Sse2.Xor(row2, row3);
			row2 = ror32_12(row2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void g2(ref Vector128<uint> row1, ref Vector128<uint> row2, ref Vector128<uint> row3, ref Vector128<uint> row4, in Vector128<uint> b0, in Vector128<sbyte> r8)
		{
			row1 = Sse2.Add(Sse2.Add(row1, b0), row2);
			row4 = Sse2.Xor(row4, row1);
			row4 = ror32_shuffle(row4, r8);
			row3 = Sse2.Add(row3, row4);
			row2 = Sse2.Xor(row2, row3);
			row2 = ror32_7(row2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void diagonalize(ref Vector128<uint> row1, ref Vector128<uint> row2, ref Vector128<uint> row3, ref Vector128<uint> row4)
		{
			row4 = Sse2.Shuffle(row4, 0b_10_01_00_11);
			row3 = Sse2.Shuffle(row3, 0b_01_00_11_10);
			row2 = Sse2.Shuffle(row2, 0b_00_11_10_01);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void undiagonalize(ref Vector128<uint> row1, ref Vector128<uint> row2, ref Vector128<uint> row3, ref Vector128<uint> row4)
		{
			row4 = Sse2.Shuffle(row4, 0b_00_11_10_01);
			row3 = Sse2.Shuffle(row3, 0b_01_00_11_10);
			row2 = Sse2.Shuffle(row2, 0b_10_01_00_11);
		}

		unsafe private static void mixSse41(Blake2sContext* s, uint* m)
		{
			var row1 = Sse2.LoadVector128(s->h);
			var row2 = Sse2.LoadVector128(s->h + 4);

			var row3 = Sse2.LoadVector128(s->viv);
			var row4 = Sse2.LoadVector128(s->viv + 4);

			row4 = Sse2.Xor(row4, Sse2.LoadVector128(s->t)); // reads into f[] as well

			var r8 = Sse2.LoadVector128((sbyte*)s->vrm);
			var r16 = Sse2.LoadVector128((sbyte*)s->vrm + 16);

			var m0 = Sse2.LoadVector128(m);
			var m1 = Sse2.LoadVector128(m + 4);
			var m2 = Sse2.LoadVector128(m + 8);
			var m3 = Sse2.LoadVector128(m + 12);

			//ROUND 1
			var b0 = shuffle2_uint(m0, m1, 0b_10_00_10_00);;

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			b0 = shuffle2_uint(m0, m1, 0b_11_01_11_01);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			diagonalize(ref row1, ref row2, ref row3, ref row4);

			b0 = shuffle2_uint(m2, m3, 0b_10_00_10_00);;

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			b0 = shuffle2_uint(m2, m3, 0b_11_01_11_01);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			undiagonalize(ref row1, ref row2, ref row3, ref row4);

			//ROUND 2
			var t0 = blend_uint(m1, m2, 0b_00_00_11_00);
			var t1 = Sse2.ShiftLeftLogical128BitLane(m3, 4);
			var t2 = blend_uint(t0, t1, 0b_11_11_00_00);
			b0 = Sse2.Shuffle(t2, 0b_10_01_00_11);

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			t0 = Sse2.Shuffle(m2, 0b_00_00_10_00);
			t1 = blend_uint(m1, m3, 0b_11_00_00_00);
			t2 = blend_uint(t0, t1, 0b_11_11_00_00);
			b0 = Sse2.Shuffle(t2, 0b_10_11_00_01);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			diagonalize(ref row1, ref row2, ref row3, ref row4);

			t0 = Sse2.ShiftLeftLogical128BitLane(m1, 4);
			t1 = blend_uint(m2, t0, 0b_00_11_00_00);
			t2 = blend_uint(m0, t1, 0b_11_11_00_00);
			b0 = Sse2.Shuffle(t2, 0b_10_11_00_01);

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			t0 = Sse2.UnpackHigh(m0, m1);
			t1 = Sse2.ShiftLeftLogical128BitLane(m3, 4);
			t2 = blend_uint(t0, t1, 0b_00_00_11_00);
			b0 = Sse2.Shuffle(t2, 0b_10_11_00_01);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			undiagonalize(ref row1, ref row2, ref row3, ref row4);

			//ROUND 3
			t0 = Sse2.UnpackHigh(m2, m3);
			t1 = blend_uint(m3, m1, 0b_00_00_11_00);
			t2 = blend_uint(t0, t1, 0b_00_00_11_11);
			b0 = Sse2.Shuffle(t2, 0b_11_01_00_10);

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			t0 = Sse2.UnpackLow(m2, m0);
			t1 = blend_uint(t0, m0, 0b_11_11_00_00);
			t2 = Sse2.ShiftLeftLogical128BitLane(m3, 8);
			b0 = blend_uint(t1, t2, 0b_11_00_00_00);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			diagonalize(ref row1, ref row2, ref row3, ref row4);

			t0 = blend_uint(m0, m2, 0b_00_11_11_00);
			t1 = Sse2.ShiftRightLogical128BitLane(m1, 12);
			t2 = blend_uint(t0, t1, 0b_00_00_00_11);
			b0 = Sse2.Shuffle(t2, 0b_01_00_11_10);

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			t0 = Sse2.ShiftLeftLogical128BitLane(m3, 4);
			t1 = blend_uint(m0, m1, 0b_00_11_00_11);
			t2 = blend_uint(t1, t0, 0b_11_00_00_00);
			b0 = Sse2.Shuffle(t2, 0b_00_01_10_11);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			undiagonalize(ref row1, ref row2, ref row3, ref row4);

			//ROUND 4
			t0 = Sse2.UnpackHigh(m0, m1);
			t1 = Sse2.UnpackHigh(t0, m2);
			t2 = blend_uint(t1, m3, 0b_00_00_11_00);
			b0 = Sse2.Shuffle(t2, 0b_11_01_00_10);

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			t0 = Sse2.ShiftLeftLogical128BitLane(m2, 8);
			t1 = blend_uint(m3, m0, 0b_00_00_11_00);
			t2 = blend_uint(t1, t0, 0b_11_00_00_00);
			b0 = Sse2.Shuffle(t2, 0b_10_00_01_11);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			diagonalize(ref row1, ref row2, ref row3, ref row4);

			t0 = blend_uint(m0, m1, 0b_00_00_11_11);
			t1 = blend_uint(t0, m3, 0b_11_00_00_00);
			b0 = Sse2.Shuffle(t1, 0b_11_00_01_10);

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			t0 = Sse2.UnpackLow(m0, m2);
			t1 = Sse2.UnpackHigh(m1, m2);
			b0 = unpacklow64_uint(t1, t0);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			undiagonalize(ref row1, ref row2, ref row3, ref row4);

			//ROUND 5
			t0 = unpacklow64_uint(m1, m2);
			t1 = unpackhigh64_uint(m0, m2);
			t2 = blend_uint(t0, t1, 0b_00_11_00_11);
			b0 = Sse2.Shuffle(t2, 0b_10_00_01_11);

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			t0 = unpackhigh64_uint(m1, m3);
			t1 = unpacklow64_uint(m0, m1);
			b0 = blend_uint(t0, t1, 0b_00_11_00_11);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			diagonalize(ref row1, ref row2, ref row3, ref row4);

			t0 = unpackhigh64_uint(m3, m1);
			t1 = unpackhigh64_uint(m2, m0);
			b0 = blend_uint(t1, t0, 0b_00_11_00_11);

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			t0 = blend_uint(m0, m2, 0b_00_00_00_11);
			t1 = Sse2.ShiftLeftLogical128BitLane(t0, 8);
			t2 = blend_uint(t1, m3, 0b_00_00_11_11);
			b0 = Sse2.Shuffle(t2, 0b_01_10_00_11);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			undiagonalize(ref row1, ref row2, ref row3, ref row4);

			//ROUND 6
			t0 = Sse2.UnpackHigh(m0, m1);
			t1 = Sse2.UnpackLow(m0, m2);
			b0 = unpacklow64_uint(t0, t1);

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			t0 = Sse2.ShiftRightLogical128BitLane(m2, 4);
			t1 = blend_uint(m0, m3, 0b_00_00_00_11);
			b0 = blend_uint(t1, t0, 0b_00_11_11_00);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			diagonalize(ref row1, ref row2, ref row3, ref row4);

			t0 = blend_uint(m1, m0, 0b_00_00_11_00);
			t1 = Sse2.ShiftRightLogical128BitLane(m3, 4);
			t2 = blend_uint(t0, t1, 0b_00_11_00_00);
			b0 = Sse2.Shuffle(t2, 0b_01_10_11_00);

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			t0 = unpacklow64_uint(m1, m2);
			t1= Sse2.Shuffle(m3, 0b_00_10_00_01);
			b0 = blend_uint(t0, t1, 0b_00_11_00_11);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			undiagonalize(ref row1, ref row2, ref row3, ref row4);

			//ROUND 7
			t0 = Sse2.ShiftLeftLogical128BitLane(m1, 12);
			t1 = blend_uint(m0, m3, 0b_00_11_00_11);
			b0 = blend_uint(t1, t0, 0b_11_00_00_00);

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			t0 = blend_uint(m3, m2, 0b_00_11_00_00);
			t1 = Sse2.ShiftRightLogical128BitLane(m1, 4);
			t2 = blend_uint(t0, t1, 0b_00_00_00_11);
			b0 = Sse2.Shuffle(t2, 0b_10_01_11_00);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			diagonalize(ref row1, ref row2, ref row3, ref row4);

			t0 = unpacklow64_uint(m0, m2);
			t1 = Sse2.ShiftRightLogical128BitLane(m1, 4);
			b0 = Sse2.Shuffle(blend_uint(t0, t1, 0b_00_00_11_00), 0b_10_11_01_00);

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			t0 = Sse2.UnpackHigh(m1, m2);
			t1 = unpackhigh64_uint(m0, t0);
			b0 = Sse2.Shuffle(t1, 0b_11_00_01_10);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			undiagonalize(ref row1, ref row2, ref row3, ref row4);

			//ROUND 8
			t0 = Sse2.UnpackHigh(m0, m1);
			t1 = blend_uint(t0, m3, 0b_00_00_11_11);
			b0 = Sse2.Shuffle(t1, 0b_10_00_11_01);

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			t0 = blend_uint(m2, m3, 0b_00_11_00_00);
			t1 = Sse2.ShiftRightLogical128BitLane(m0, 4);
			t2 = blend_uint(t0, t1, 0b_00_00_00_11);
			b0 = Sse2.Shuffle(t2, 0b_01_00_10_11);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			diagonalize(ref row1, ref row2, ref row3, ref row4);

			t0 = unpackhigh64_uint(m0, m3);
			t1 = unpacklow64_uint(m1, m2);
			t2 = blend_uint(t0, t1, 0b_00_11_11_00);
			b0 = Sse2.Shuffle(t2, 0b_00_10_11_01);

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			t0 = Sse2.UnpackLow(m0, m1);
			t1 = Sse2.UnpackHigh(m1, m2);
			b0 = unpacklow64_uint(t0, t1);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			undiagonalize(ref row1, ref row2, ref row3, ref row4);

			//ROUND 9
			t0 = Sse2.UnpackHigh(m1, m3);
			t1 = unpacklow64_uint(t0, m0);
			t2 = blend_uint(t1, m2, 0b_11_00_00_00);
			b0 = shufflehigh_uint(t2, 0b_01_00_11_10);

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			t0 = Sse2.UnpackHigh(m0, m3);
			t1 = blend_uint(m2, t0, 0b_11_11_00_00);
			b0 = Sse2.Shuffle(t1, 0b_00_10_01_11);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			diagonalize(ref row1, ref row2, ref row3, ref row4);

			t0 = blend_uint(m2, m0, 0b_00_00_11_00);
			t1 = Sse2.ShiftLeftLogical128BitLane(t0, 4);
			b0 = blend_uint(t1, m3, 0b_00_00_11_11);

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			t0 = blend_uint(m1, m0, 0b_00_11_00_00);
			b0 = Sse2.Shuffle(t0, 0b_01_00_11_10);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			undiagonalize(ref row1, ref row2, ref row3, ref row4);

			//ROUND 10
			t0 = blend_uint(m0, m2, 0b_00_00_00_11);
			t1 = blend_uint(m1, m2, 0b_00_11_00_00);
			t2 = blend_uint(t1, t0, 0b_00_00_11_11);
			b0 = Sse2.Shuffle(t2, 0b_01_11_00_10);

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			t0 = Sse2.ShiftLeftLogical128BitLane(m0, 4);
			t1 = blend_uint(m1, t0, 0b_11_00_00_00);
			b0 = Sse2.Shuffle(t1, 0b_01_10_00_11);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			diagonalize(ref row1, ref row2, ref row3, ref row4);

			t0 = Sse2.UnpackHigh(m0, m3);
			t1 = Sse2.UnpackLow(m2, m3);
			t2 = unpackhigh64_uint(t0, t1);
			b0 = Sse2.Shuffle(t2, 0b_11_00_10_01);

			g1(ref row1, ref row2, ref row3, ref row4, b0, r16);

			t0 = blend_uint(m3, m2, 0b_11_00_00_00);
			t1 = Sse2.UnpackLow(m0, m3);
			t2 = blend_uint(t0, t1, 0b_00_00_11_11);
			b0 = Sse2.Shuffle(t2, 0b_00_01_10_11);

			g2(ref row1, ref row2, ref row3, ref row4, b0, r8);
			undiagonalize(ref row1, ref row2, ref row3, ref row4);

			row1 = Sse2.Xor(row1, row3);
			row2 = Sse2.Xor(row2, row4);
			row1 = Sse2.Xor(row1, Sse2.LoadVector128(s->h));
			row2 = Sse2.Xor(row2, Sse2.LoadVector128(s->h + 4));
			Sse2.Store(s->h, row1);
			Sse2.Store(s->h + 4, row2);
		}
	}
}
#endif
