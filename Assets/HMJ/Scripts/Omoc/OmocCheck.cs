using MJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmocCheck : MonoBehaviour
{
    static public bool OmocWin(ROCK[,] _rockDatas, int row, int col)
    { // 3 4
        if (row < 0 || col < 0 || row >= MJ.InputRocks.ROCK_ROW || col >= MJ.InputRocks.ROCK_COLUMN)
            return false;
        int horizonN = 0;
        return HorizontalOmoc(_rockDatas, row, col, horizonN) ||
               VerticalOmoc(_rockDatas, row, col, horizonN) ||
               RightDiagonal(_rockDatas, row, col, horizonN) ||
               LeftDiagonal(_rockDatas, row, col, horizonN);
    }

    /// <summary>
    /// 수평 오목 체크
    /// </summary>
    /// <param name="_rockDatas"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    static public bool HorizontalOmoc(ROCK[,] _rockDatas, int row, int col, int n)
    {
        int leftHorizon = LeftHorizontalOmoc(_rockDatas, row, col - 1, 0, _rockDatas[row, col].GetColor());
        int rightHorizon = RightHorizontalOmoc(_rockDatas, row, col + 1, 0, _rockDatas[row, col].GetColor());

        return (leftHorizon + rightHorizon + 1) >= 5 ? true : false;
    }

    static public int LeftHorizontalOmoc(ROCK[ , ] _rockDatas, int row, int col, int n, ROCK.ROCKCOLOR color)
    {
        if (col < 0) return n;

        if (_rockDatas[row, col].GetColor() == color) n++;
        else return n;

        return LeftHorizontalOmoc(_rockDatas, row, --col, n, color);
    }

    static public int RightHorizontalOmoc(ROCK[ , ] _rockDatas, int row, int col, int n, ROCK.ROCKCOLOR color)
    {
        if (col >= MJ.InputRocks.ROCK_COLUMN) return n;

        if (_rockDatas[row, col].GetColor() == color) n++;
        else return n;
        return RightHorizontalOmoc(_rockDatas, row, ++col, n, color);
    }

    /// <summary>
    /// 수직 오목 체크
    /// </summary>
    /// <param name="_rockDatas"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="n"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    /// 
    static public bool VerticalOmoc(ROCK[,] _rockDatas, int row, int col, int n)
    {
        int leftVertical = UpVerticalOmoc(_rockDatas, row - 1, col, 0, _rockDatas[row, col].GetColor());
        int rightVertical = DownVerticalOmoc(_rockDatas, row + 1, col, 0, _rockDatas[row, col].GetColor());

        return (leftVertical + rightVertical + 1) >= 5 ? true : false;
    }

    static public int UpVerticalOmoc(ROCK[,] _rockDatas, int row, int col, int n, ROCK.ROCKCOLOR color)
    {
        if (row < 0) return n;

        if (_rockDatas[row, col].GetColor() == color) n++;
        else return n;

        return UpVerticalOmoc(_rockDatas, --row, col, n, color);
    }

    static public int DownVerticalOmoc(ROCK[,] _rockDatas, int row, int col, int n, ROCK.ROCKCOLOR color)
    {
        if (row >= MJ.InputRocks.ROCK_ROW) return n;

        if (_rockDatas[row, col].GetColor() == color) n++;
        else return n;
        return DownVerticalOmoc(_rockDatas, ++row, col, n, color);
    }

    /// <summary>
    /// 오른쪽 대각선 체크
    /// </summary>
    /// <param name="_rockDatas"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="n"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    /// 

    static public bool RightDiagonal(ROCK[,] _rockDatas, int row, int col, int n)
    {
        int upRightDiagonal = UPRightDiagonal(_rockDatas, row - 1, col + 1, 0, _rockDatas[row, col].GetColor());
        int downRightDiagonal = DownRightDiagonal(_rockDatas, row + 1, col - 1, 0, _rockDatas[row, col].GetColor());

        return (upRightDiagonal + downRightDiagonal + 1) >= 5 ? true : false;
    }

    static public int UPRightDiagonal(ROCK[,] _rockDatas, int row, int col, int n, ROCK.ROCKCOLOR color)
    {
        if (row < 0 || col >= MJ.InputRocks.ROCK_COLUMN) return n;

        if (_rockDatas[row, col].GetColor() == color) n++;
        else return n;

        return UPRightDiagonal(_rockDatas, --row, ++col, n, color);
    }

    static public int DownRightDiagonal(ROCK[,] _rockDatas, int row, int col, int n, ROCK.ROCKCOLOR color)
    {
        if (row >= MJ.InputRocks.ROCK_ROW || col < 0) return n;

        if (_rockDatas[row, col].GetColor() == color) n++;
        else return n;

        return DownRightDiagonal(_rockDatas, ++row, --col, n, color);
    }

    /// <summary>
    /// 왼쪽 대각선 체크
    /// </summary>
    /// <param name="_rockDatas"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="n"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    /// 

    static public bool LeftDiagonal(ROCK[,] _rockDatas, int row, int col, int n)
    {
        int upRightDiagonal = UpLeftDiagonal(_rockDatas, row - 1, col - 1, 0, _rockDatas[row, col].GetColor());
        int downRightDiagonal = DownLeftDiagonal(_rockDatas, row + 1, col + 1, 0, _rockDatas[row, col].GetColor());

        return (upRightDiagonal + downRightDiagonal + 1) >= 5 ? true : false;
    }

    static public int UpLeftDiagonal(ROCK[,] _rockDatas, int row, int col, int n, ROCK.ROCKCOLOR color)
    {
        if (row < 0 || col < 0) return n;

        if (_rockDatas[row, col].GetColor() == color) n++;
        else return n;

        return UpLeftDiagonal(_rockDatas, --row, --col, n, color);
    }

    static public int DownLeftDiagonal(ROCK[,] _rockDatas, int row, int col, int n, ROCK.ROCKCOLOR color)
    {
        if (row >= MJ.InputRocks.ROCK_ROW || col >= MJ.InputRocks.ROCK_COLUMN) return n;

        if (_rockDatas[row, col].GetColor() == color) n++;
        else return n;
        return DownLeftDiagonal(_rockDatas, ++row, ++col, n, color);
    }
}
