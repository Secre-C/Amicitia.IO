﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using Amicitia.IO.Binary.Utilities;

namespace Amicitia.IO.Binary
{
    public static class BinaryOperations<T> 
        where T : unmanaged
    {
        private static readonly TypeBinaryReverseMethod<T> sReverseMethod;

        static BinaryOperations()
        {
            if ( !TypeTraits<T>.IsPrimitiveType() )
                sReverseMethod = TypeBinaryReverseMethodGenerator.Generate<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Reverse( ref T value )
        {
            if ( typeof( T ) == typeof( byte ) || typeof( T ) == typeof( sbyte ) )
                return;
            else if ( typeof( T ) == typeof( short ) || typeof( T ) == typeof( ushort ) )
            {
                var reversedValue = BinaryPrimitives.ReverseEndianness( Unsafe.As<T, ushort>( ref value ) );
                value = Unsafe.As<ushort, T>( ref reversedValue );
            }
            else if ( typeof( T ) == typeof( int ) || typeof( T ) == typeof( uint ) || typeof( T ) == typeof( float ) )
            {
                var reversedValue = BinaryPrimitives.ReverseEndianness( Unsafe.As<T, uint>( ref value ) );
                value = Unsafe.As<uint, T>( ref reversedValue );
            }
            else if ( typeof( T ) == typeof( long ) || typeof( T ) == typeof( ulong ) || typeof( T ) == typeof( double ) )
            {
                var reversedValue = BinaryPrimitives.ReverseEndianness( Unsafe.As<T, ulong>( ref value ) );
                value = Unsafe.As<ulong, T>( ref reversedValue );
            }
            else
            {
                sReverseMethod( ref value );
            }
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T Reverse( T value )
        {
            Reverse( ref value );
            return value;
        }
    }
}
