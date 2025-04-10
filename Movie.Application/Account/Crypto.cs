﻿using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Movie.Application.Account;

public static class Crypto
{
    private const int PBKDF2IterCount = 1000; // default for Rfc2898DeriveBytes
    private const int PBKDF2SubkeyLength = 256 / 8; // 256 bits
    private const int SaltSize = 128 / 8; // 128 bits

    /* =======================
     * HASHED PASSWORD FORMATS
     * =======================
     *
     * Version 0:
     * PBKDF2 with HMAC-SHA1, 128-bit salt, 256-bit subkey, 1000 iterations.
     * (See also: SDL crypto guidelines v5.1, Part III)
     * Format: { 0x00, salt, subkey }
     */

    public static string HashPassword(string password)
    {
        if (password == null) throw new ArgumentNullException("password");

        // Produce a version 0 (see comment above) text hash.
        byte[] salt;
        byte[] subkey;
        using (var deriveBytes = new Rfc2898DeriveBytes(password, SaltSize, PBKDF2IterCount))
        {
            salt = deriveBytes.Salt;
            subkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
        }

        var outputBytes = new byte[1 + SaltSize + PBKDF2SubkeyLength];
        Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
        Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, PBKDF2SubkeyLength);
        return Convert.ToBase64String(outputBytes);
    }

    // hashedPassword must be of the format of HashWithPassword (salt + Hash(salt+input)
    public static bool VerifyHashedPassword(string hashedPassword, string password)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword)) throw new ArgumentNullException(nameof(hashedPassword));
        if (string.IsNullOrWhiteSpace(hashedPassword)) throw new ArgumentNullException(nameof(password));

        var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);

        // Verify a version 0 (see comment above) text hash.

        if (hashedPasswordBytes.Length != 1 + SaltSize + PBKDF2SubkeyLength || hashedPasswordBytes[0] != 0x00)
            // Wrong length or version header.
            return false;

        var salt = new byte[SaltSize];
        Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, SaltSize);
        var storedSubkey = new byte[PBKDF2SubkeyLength];
        Buffer.BlockCopy(hashedPasswordBytes, 1 + SaltSize, storedSubkey, 0, PBKDF2SubkeyLength);

        byte[] generatedSubkey;
        using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, PBKDF2IterCount))
        {
            generatedSubkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
        }

        return ByteArraysEqual(storedSubkey, generatedSubkey);
    }

    // Compares two byte arrays for equality. The method is specifically written so that the loop is not optimized.
    [MethodImpl(MethodImplOptions.NoOptimization)]
    private static bool ByteArraysEqual(byte[] a, byte[] b)
    {
        if (ReferenceEquals(a, b)) return true;

        if (a == null || b == null || a.Length != b.Length) return false;

        var areSame = true;
        for (var i = 0; i < a.Length; i++) areSame &= a[i] == b[i];

        return areSame;
    }
}