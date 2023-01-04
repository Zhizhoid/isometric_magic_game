using System;

public struct Int2
{
    public int x;
    public int y;

    public Int2(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public static Int2 operator +(Int2 a, Int2 b) => new Int2(a.x + b.x, a.y + b.y);
    public static Int2 operator -(Int2 a, Int2 b) => new Int2(a.x - b.x, a.y - b.y);
    public static bool operator ==(Int2 a, Int2 b) => a.x == b.x && a.y == b.y;
    public static bool operator !=(Int2 a, Int2 b) => a.x != b.x || a.y != b.y;

    public override bool Equals(object o)
    {
        if (o == null || this.GetType() != o.GetType())
        {
            return false;
        }
        else
        {
            Int2 oInt2 = (Int2)o;
            return this == oInt2;
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }

    public override string ToString()
    {
        return "(" + this.x + ", " + this.y + ")";
    }
}
