using MJ;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class OmocCheck : MonoBehaviour
{
    public bool OmocWin(ROCK[,] _rockDatas, int row, int col)
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
    public bool HorizontalOmoc(ROCK[,] _rockDatas, int row, int col, int n)
    {
        int leftHorizon = LeftHorizontalOmoc(_rockDatas, row, col - 1, 0, _rockDatas[row, col].GetColor());
        int rightHorizon = RightHorizontalOmoc(_rockDatas, row, col + 1, 0, _rockDatas[row, col].GetColor());

        if ((leftHorizon + rightHorizon + 1) >= 5)
        {
            StartCoroutine(ResetOmoc(_rockDatas, new Vector2Int(col - leftHorizon, row), new Vector2Int(1, 0), leftHorizon + rightHorizon + 1, 3.0f));
            return true;
        }
        else
            return false;
    }

    public int LeftHorizontalOmoc(ROCK[ , ] _rockDatas, int row, int col, int n, ROCK.ROCKCOLOR color)
    {
        if (col < 0) return n;

        if (_rockDatas[row, col].GetColor() == color) n++;
        else return n;

        return LeftHorizontalOmoc(_rockDatas, row, --col, n, color);
    }

    public int RightHorizontalOmoc(ROCK[ , ] _rockDatas, int row, int col, int n, ROCK.ROCKCOLOR color)
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
    public bool VerticalOmoc(ROCK[,] _rockDatas, int row, int col, int n)
    {
        int leftVertical = UpVerticalOmoc(_rockDatas, row - 1, col, 0, _rockDatas[row, col].GetColor());
        int rightVertical = DownVerticalOmoc(_rockDatas, row + 1, col, 0, _rockDatas[row, col].GetColor());

        if ((leftVertical + rightVertical + 1) >= 5)
        {
            StartCoroutine(ResetOmoc(_rockDatas, new Vector2Int(col, row - leftVertical), new Vector2Int(0, 1), leftVertical + rightVertical + 1, 3.0f));
            return true;
        }
        else
            return false;

    }

    public int UpVerticalOmoc(ROCK[,] _rockDatas, int row, int col, int n, ROCK.ROCKCOLOR color)
    {
        if (row < 0) return n;

        if (_rockDatas[row, col].GetColor() == color) n++;
        else return n;

        return UpVerticalOmoc(_rockDatas, --row, col, n, color);
    }

    public int DownVerticalOmoc(ROCK[,] _rockDatas, int row, int col, int n, ROCK.ROCKCOLOR color)
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

    public bool RightDiagonal(ROCK[,] _rockDatas, int row, int col, int n)
    {
        int upRightDiagonal = UPRightDiagonal(_rockDatas, row - 1, col + 1, 0, _rockDatas[row, col].GetColor());
        int downRightDiagonal = DownRightDiagonal(_rockDatas, row + 1, col - 1, 0, _rockDatas[row, col].GetColor());

        if ((upRightDiagonal + downRightDiagonal + 1) >= 5)
        {
            StartCoroutine(ResetOmoc(_rockDatas, new Vector2Int(col + upRightDiagonal, row - upRightDiagonal), new Vector2Int(-1, 1), upRightDiagonal + downRightDiagonal + 1, 3.0f));
            return true;
        }
        else
            return false;
    }

    public int UPRightDiagonal(ROCK[,] _rockDatas, int row, int col, int n, ROCK.ROCKCOLOR color)
    {
        if (row < 0 || col >= MJ.InputRocks.ROCK_COLUMN) return n;

        if (_rockDatas[row, col].GetColor() == color) n++;
        else return n;

        return UPRightDiagonal(_rockDatas, --row, ++col, n, color);
    }

    public int DownRightDiagonal(ROCK[,] _rockDatas, int row, int col, int n, ROCK.ROCKCOLOR color)
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

    public bool LeftDiagonal(ROCK[,] _rockDatas, int row, int col, int n)
    {
        int upRightDiagonal = UpLeftDiagonal(_rockDatas, row - 1, col - 1, 0, _rockDatas[row, col].GetColor());
        int downRightDiagonal = DownLeftDiagonal(_rockDatas, row + 1, col + 1, 0, _rockDatas[row, col].GetColor());

        if ((upRightDiagonal + downRightDiagonal + 1) >= 5)
        {
            StartCoroutine(ResetOmoc(_rockDatas, new Vector2Int(col - upRightDiagonal, row - upRightDiagonal), new Vector2Int(1, 1), upRightDiagonal + downRightDiagonal + 1, 3.0f));
            return true;
        }
        else
            return false;
    }

    public int UpLeftDiagonal(ROCK[,] _rockDatas, int row, int col, int n, ROCK.ROCKCOLOR color)
    {
        if (row < 0 || col < 0) return n;

        if (_rockDatas[row, col].GetColor() == color) n++;
        else return n;

        return UpLeftDiagonal(_rockDatas, --row, --col, n, color);
    }

    public int DownLeftDiagonal(ROCK[,] _rockDatas, int row, int col, int n, ROCK.ROCKCOLOR color)
    {
        if (row >= MJ.InputRocks.ROCK_ROW || col >= MJ.InputRocks.ROCK_COLUMN) return n;

        if (_rockDatas[row, col].GetColor() == color) n++;
        else return n;
        return DownLeftDiagonal(_rockDatas, ++row, ++col, n, color);
    }

    // 시작 위치, 누적할 위치XY, 총 개수
    //private IEnumerator ResetSoccerPosition(float delayTime)
    IEnumerator ResetOmoc(ROCK[,] _rockDatas, Vector2Int firstXY, Vector2Int GapXY, int N, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        for (int i = 0; i < N; i++)
            _rockDatas[firstXY.y + GapXY.y * i, firstXY.x + GapXY.x * i].SetColor(ROCK.ROCKCOLOR.NONE);

        yield return null;
    }
}
