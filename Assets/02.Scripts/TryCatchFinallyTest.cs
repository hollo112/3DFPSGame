using System;
using UnityEngine;

public class TryCatchFinallyTest : MonoBehaviour
{
    public int age; // 사람 나이
    
    // 예외: 런타임중에 발생하는 오류 (참조, 나누기, 인덱스 범위 벗어나기 등등)
    
    // try-catch 문법 : 예외를 처리하는 기본 문법

    private void Start()
    {
        if (age < 0)
        {
            Debug.LogError("사람나이는 8살보다 적을 수 없습니다");
            throw new Exception("사람나이는 8살보다 적을 수 없습니다");
        }
        // 아래 문법은 인덱스 범위를 벗어나므로 오류가 일어난다
        // 다른 컴포넌트나 게임 오브젝트에도 영향을 줌으로써 프로그램이 정상적으로 동작 안할 수 있다
        
        // 베스트 : 알고리즘을 잘 처리하는 것
        int[] numbers = new int[32];
        try
        {
            // 예외가 발생할만한 코드 작성
            int index = 75;
            numbers[index] = 1;

        }
        catch (Exception e)
        {
            Debug.LogException(e);
            
            // 예외가 발생했을때 처리해야할 코드 작성
            int index = numbers.Length - 1;
            numbers[index] = 2;
            Debug.Log("IndexOutofRange 오류 일어남");
        }
        finally
        {
            // (옵션: 정상이든 오류이든 실행할 코드 작성)작
        }
        
        // try-catch 구문은 되도록이면 안쓰는 것이 좋다
        // - 성능 저하 (제일 크다)
        // - 잘못된 알고리즘
        
        // 써야 하는 경우 : 내가 제어할 수 없을 때
        // 1. 네트워크 접근
        //  - 로그인, 로그아웃 / 서버 / DB 아이템 저장 / 불러오기
        // 2. 파일 접근
        //  - 용량 충분?, 파일명 권한?...
        // 3. DB에 접근
    }
}
